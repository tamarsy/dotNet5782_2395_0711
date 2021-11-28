using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using DalObject;
using IBL.BO;

namespace IBL
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BL : IBL
    {
        private IDal.IDal dalObject;
        private double vacent;
        private double lightWeightCarrier;
        private double mediumWeightCarrier;
        private double heavyWeightCarrier;
        private double skimmerLoadingRate;
        private List<DroneToList> drones;
        private static Random rand = new Random();

        /// <summary>
        /// BL constructor reset the drones arry and battry details in BL class
        /// </summary>
        public BL()
        {
            dalObject = new DalObject.DalObject();
            double[] PowerConsumption = dalObject.PowerConsumptionRequest();
            vacent = PowerConsumption[0];
            lightWeightCarrier = PowerConsumption[1];
            mediumWeightCarrier = PowerConsumption[2];
            heavyWeightCarrier = PowerConsumption[3];
            skimmerLoadingRate = PowerConsumption[4];
            drones = new List<DroneToList>();
            initializeDronesList();
        }



        /// <summary>
        /// initialize the DroneStatuses, CurrentLocation, BatteryStatuses of evry drone in drones list
        /// </summary>
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
                    newDrone.DroneStatuses = (DroneStatuses)rand.Next(-1, 1);
                    newDrone.CurrentLocation = FindLocation(newDrone);
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
            catch (ObjectNotAvailableForActionException)
            {
                throw;
            }
            drones[i].DroneStatuses = DroneStatuses.sending;
            choosenParcel.DroneDelivery = new DroneDelivery()
            {
                BatteryStatuses = drones[i].BatteryStatuses,
                CurrentLocation = drones[i].CurrentLocation,
                Id = drones[i].Id
            };
            choosenParcel.AssignmentTime = DateTime.Now;
        }


        /// <summary>
        /// find the best available parcel for the drone
        /// </summary>
        /// <param name="drone">the specific drone</param>
        /// <returns>Parcel</returns>
        private Parcel FindParcelToCollecting(DroneToList drone)
        {
            //only parcel with possible route for drone and weight less then max weight of drone can be choosen
            List<ParcelToList> availableList = ParcelsList().Where(p => p.Weight < drone.MaxWeight && checkAvailableDelivery(drone, p)).ToList();
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
            Drone drone = GetDrone(droneToList.Id);
            return 0 < drone.BatteryStatuses - (FindMinPowerForDistance(drone.Parcel.Distance) +
               FindMinPowerForDistance(drone.Parcel.DeliveryDestination.Distance(FindCloseStationWithChargeSlot(drone.Parcel.DeliveryDestination))));
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
            drones[i].BatteryStatuses -= drone.Distance(drone.Parcel.Collecting);
            drones[i].CurrentLocation = drone.Parcel.Collecting;
            dalObject.PickParcel(drone.Parcel.Id);
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
            drones[i].BatteryStatuses -= drone.Parcel.Distance;
            drones[i].CurrentLocation = drone.Parcel.DeliveryDestination;
            drones[i].DroneStatuses = DroneStatuses.vacant;
            try
            {
                dalObject.Destination(drone.Parcel.Id);
            }
            catch (ObjectNotAvailableForActionException)
            {
                throw;
            }
            catch (ObjectNotExistException)
            {
                throw;
            }
        }
    }
}