using System;
using System.Collections.Generic;
using System.Linq;
using Singelton;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;

namespace BL
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class BL : Singleton<BL>, IBL
    {
        private static Random rand;
        private readonly DalApi.IDal dalObject;
        private double vacent;
        private double lightWeightCarrier;
        private double mediumWeightCarrier;
        private double heavyWeightCarrier;
        private double skimmerLoadingRate;
        private List<DroneToList> drones;
        static BL() { }
        /// <summary>
        /// BL constructor reset the drones arry and battry details in BL class
        /// </summary>
        private BL()
        {
            rand = new Random();
            dalObject = DalApi.FactoryDL.GetDL();
            double[] PowerConsumption = dalObject.PowerConsumptionRequest();
            vacent = PowerConsumption[0];
            lightWeightCarrier = PowerConsumption[1];
            mediumWeightCarrier = PowerConsumption[2];
            heavyWeightCarrier = PowerConsumption[3];
            skimmerLoadingRate = PowerConsumption[4];
            drones = new List<DroneToList>();
            initializeDronesList();
        }
        private void initializeDronesList()
        {
            foreach (var drone in dalObject.DroneList())
            {
                DroneToList newDrone = new DroneToList()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (WeightCategories)drone.MaxWeight
                };
                foreach (var parcel in dalObject.ParcelList())
                {
                    if (parcel.Droneld == drone.Id && parcel.Delivered != null)
                    {
                        newDrone.NumOfParcel = parcel.Id;
                        break;
                    }
                }
                if (newDrone.NumOfParcel != null)
                {
                    newDrone.DroneStatuses = DroneStatuses.sending;
                    newDrone.CurrentLocation = FindLocation(newDrone);
                    //to fill BatteryStatuses
                    newDrone.BatteryStatuses = FindBatteryStatusesForInitializeDronesList(newDrone);
                }
                else
                {
                    newDrone.DroneStatuses = (DroneStatuses)rand.Next(0, 2);
                    newDrone.CurrentLocation = FindLocation(newDrone);
                    if (newDrone.DroneStatuses == DroneStatuses.maintanance)
                        dalObject.ChargeOn(drone.Id, FindCloseStationWithChargeSlot(newDrone.CurrentLocation).Id);
                    //to fill BatteryStatuses
                    newDrone.BatteryStatuses = FindBatteryStatusesForInitializeDronesList(newDrone);
                }
                drones.Add(newDrone);
            }
        }


        /// <summary>
        /// find and conect parcel to the specific drone
        /// </summary>
        /// <param name="droneId">the specific drone id</param>
        public void ParcelToDrone(int droneId)
        {
            int i = drones.FindIndex(d => d.Id == droneId);
            if (drones[i].DroneStatuses != DroneStatuses.vacant)
                throw new ObjectNotAvailableForActionException($"drone with id: {droneId} is no vacant");
            Parcel choosenParcel;
            try
            {
                choosenParcel = FindParcelToCollecting(drones[i]);
            }
            catch (ObjectNotAvailableForActionException e)
            {
                throw new ObjectNotAvailableForActionException(e.Message);
            }
            drones[i].DroneStatuses = DroneStatuses.sending;
            drones[i].NumOfParcel = choosenParcel.Id;
            choosenParcel.DroneDelivery = new DroneDelivery()
            {
                BatteryStatuses = drones[i].BatteryStatuses,
                CurrentLocation = drones[i].CurrentLocation,
                Id = drones[i].Id
            };
            choosenParcel.AssignmentTime = DateTime.Now;
            dalObject.ParcelToDrone(choosenParcel.Id, droneId);
        }


        /// <summary>
        /// find the best available parcel for the drone
        /// </summary>
        /// <param name="drone">the specific drone</param>
        /// <returns>Parcel</returns>
        private Parcel FindParcelToCollecting(DroneToList drone)
        {
            //only parcel with possible route for drone and weight less then max weight of drone can be choosen
            List<ParcelToList> availableList = ParcesWithoutDronelList().Where((p) => checkAvailableDelivery(drone, p)).ToList();
            if (availableList.Count() < 1)
            {
                throw new ObjectNotAvailableForActionException("no available parcel to collect");
            }

            List<ParcelToList> parcelListOfChosenParcel = availableList
            //only parcel with max priority can be choosen
                .Where(p => p.Priority == (availableList.Exists(p1 => p1.Priority == Priorities.emergancy) ? Priorities.emergancy : availableList.Exists(p2 => p2.Priority == Priorities.fast) ? Priorities.fast : Priorities.regular))
            //only parcel with max weight can be choosen
                .Where(p => p.Weight == (availableList.Exists(p1 => p1.Weight == WeightCategories.heavy) ? WeightCategories.heavy : availableList.Exists(p2 => p2.Weight == WeightCategories.medium) ? WeightCategories.medium : WeightCategories.easy)).ToList();
            //return the first parcel with max weight from all parcels with max priority from all available parcels
            return GetParcel(parcelListOfChosenParcel[0].Id);
        }



        /// <summary>
        /// return true if the drone can make the delivery with this parcel
        /// </summary>
        /// <param name="droneToList"> the drone</param>
        /// <param name="parcel">the parcel</param>
        /// <returns>bool value</returns>
        private bool checkAvailableDelivery(DroneToList droneToList, ParcelToList parcel)
        {
            Location getterLocation = GetCustomer(parcel.GetterId).CurrentLocation;
            Location senderLocation = GetCustomer(parcel.SenderId).CurrentLocation;
            return parcel.Weight <= droneToList.MaxWeight && 0 < droneToList.BatteryStatuses - (FindMinPowerForDistance(
                droneToList.CurrentLocation.Distance(senderLocation) +
                getterLocation.Distance(FindCloseStationWithChargeSlot(getterLocation))
                ) + FindMinPowerForDistance(senderLocation.Distance(getterLocation), parcel.Weight)
                );
        }



        /// <summary>
        /// colleced the parcel from customer to the drone
        /// </summary>
        /// <param name="droneld">the drone id</param>
        public void PickParcel(int droneld)
        {
            Drone drone = GetDrone(droneld);
            if (drone.DroneStatuses != DroneStatuses.sending || drone.Parcel.StatusParcel)
            {
                throw new ObjectNotAvailableForActionException($"parcel already colleced or not Assignment to drone with id: {droneld}");
            }
            int i = drones.FindIndex(d => d.Id == droneld);
            if (drones[i].BatteryStatuses - drone.Distance(drone.Parcel.Collecting) < 0)
            {
                throw new ObjectNotAvailableForActionException("not enugh battery in drone");
            }
            drones[i].BatteryStatuses -= drone.Distance(drone.Parcel.Collecting);
            drones[i].CurrentLocation = drone.Parcel.Collecting;
            //update time collecting in parcel
        }



        /// <summary>
        /// supply the parcel to the geeter customer from the drone
        /// </summary>
        /// <param name="droneld">the drone id</param>
        public void Destination(int droneld)
        {
            Drone drone = GetDrone(droneld);
            if (!drone.Parcel.StatusParcel)
            {
                throw new ObjectNotAvailableForActionException($"parcel already colleced or not pick up by drone with id: {droneld}");
            }
            int i = drones.FindIndex(d => d.Id == droneld);
            drones[i].BatteryStatuses -= FindMinPowerForDistance(drone.Parcel.Distance, drone.Parcel.Weight);
            drones[i].CurrentLocation = drone.Parcel.DeliveryDestination;
            drones[i].DroneStatuses = DroneStatuses.vacant;
            drones[i].NumOfParcel = null;
            //update time suplied in parcel
        }


        /// <returns></returns>
        public Drone detailsDrone(Drone drone)
        {
            DroneToList droneToList = drones.Find(item => item.Id == drone.Id);
            return new Drone()
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (WeightCategories)drone.MaxWeight,
                BatteryStatuses = droneToList.BatteryStatuses,
                DroneStatuses = droneToList.DroneStatuses,
                CurrentLocation = droneToList.CurrentLocation,
                //Parcel = droneToList.NumOfParcel != null ? throw("finish") : null
            };
        }
    }
}