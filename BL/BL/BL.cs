using System;
using System.Collections.Generic;
using System.Linq;
using Singelton;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using BO;
using BlApi;
using System.Runtime.CompilerServices;

namespace BL
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class BL : Singleton<BL>, IBL
    {
        internal static Random rand;
        internal readonly DalApi.IDal dalObject;
        internal double vacent;
        internal double lightWeightCarrier;
        internal double mediumWeightCarrier;
        internal double heavyWeightCarrier;
        internal double skimmerLoadingRate;
        internal List<DroneToList> drones;
        static BL() { }
        /// <summary>
        /// BL constructor reset the drones arry and battry details in BL class
        /// </summary>
        private BL()
        {
            rand = new Random();
            dalObject = DalApi.FactoryDL.GetDL();
            double[] PowerConsumption = dalObject.PowerConsumptionRequest();
            vacent = PowerConsumption[0] / 100;
            lightWeightCarrier = PowerConsumption[1] / 100;
            mediumWeightCarrier = PowerConsumption[2] / 100;
            heavyWeightCarrier = PowerConsumption[3] / 100;
            skimmerLoadingRate = PowerConsumption[4] / 100;
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
        /// the function find the battery status for the initialize drones list
        /// </summary>
        /// <param name="drone"></param>
        /// <returns> rand number as battery</returns>
        private double FindBatteryStatusesForInitializeDronesList(DroneToList drone)
        {
            if (drone.DroneStatuses == DroneStatuses.maintanance)
            {
                return rand.NextDouble() * 20;//0 to 20
            }
            if (drone.DroneStatuses == DroneStatuses.sending)
            {
                lock (dalObject)
                {
                    DO.Parcel parcel = dalObject.GetParcel((int)drone.NumOfParcel);
                    Location getterLocation = GetCustomer(parcel.GetterId).CurrentLocation;
                    Location chargeSlotLocation = FindCloseStationWithChargeSlot(getterLocation).CurrentLocation;
                    double distanceWithParcel = drone.Distance(getterLocation);
                    double distanceWithOutParcel = getterLocation.Distance(chargeSlotLocation);
                    double MinPower = FindMinPowerForDistance(distanceWithParcel, (WeightCategories)parcel.Weight) + FindMinPowerForDistance(distanceWithOutParcel);
                    return Min(100, (rand.NextDouble() * (100 - MinPower)) + MinPower);//min to 100
                }
            }
            else
            {
                Location chargeSlotLocation = FindCloseStationWithChargeSlot(drone.CurrentLocation).CurrentLocation;
                double distance = drone.CurrentLocation.Distance(chargeSlotLocation);
                double MinPower = FindMinPowerForDistance(distance);
                return (MinPower > 100) ? 100 : (rand.NextDouble() * (100 - MinPower)) + MinPower;//min to 100
            }
        }



        /// <summary>
        /// find the best available parcel for the drone
        /// </summary>
        /// <param name="drone">the specific drone</param>
        /// <returns>Parcel</returns>
        internal Parcel FindParcelToCollecting(int droneId)
        {
            Drone drone = GetDrone(droneId);
            ParcelToList parcelToCollect = ParcesWithoutDronelList().Where((p) => checkAvailableDelivery(drone, p)).
             OrderByDescending(p => p?.Priority)
             .ThenByDescending(p => p?.Weight)
             .FirstOrDefault();
            return parcelToCollect != null? GetParcel(parcelToCollect.Id):null;
        }



        /// <summary>
        /// return true if the drone can make the delivery with this parcel
        /// </summary>
        /// <param name="droneToList"> the drone</param>
        /// <param name="parcel">the parcel</param>
        /// <returns>bool value</returns>
        private bool checkAvailableDelivery(Drone droneToList, ParcelToList parcel)
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickParcel(int droneld)
        {
            Drone drone = GetDrone(droneld);
            if (drone.DroneStatuses != DroneStatuses.sending || drone.Parcel.StatusParcel)
                throw new ObjectNotAvailableForActionException($"parcel already colleced or not Assignment to drone with id: {droneld}");
            int i = drones.FindIndex(d => d.Id == droneld);
            if (drones[i].BatteryStatuses - drone.Distance(drone.Parcel.Collecting) < 0)
                throw new ObjectNotAvailableForActionException("not enugh battery in drone");
            drones[i].BatteryStatuses = Max(drones[i].BatteryStatuses - FindMinPowerForDistance(drone.Parcel.Distance), 0);
            drones[i].CurrentLocation = drone.Parcel.Collecting;
            dalObject.PickParcel(drone.Parcel.Id);
        }



        /// <summary>
        /// find and conect parcel to the specific drone
        /// </summary>
        /// <param name="droneId">the specific drone id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ParcelToDrone(int droneId)
        {
            int i = drones.FindIndex(d => d.Id == droneId);
            if (i < 0)
                throw new ObjectNotExistException($"drone with id: {droneId} is not exist");
            if (drones[i].DroneStatuses != DroneStatuses.vacant)
                throw new ObjectNotAvailableForActionException($"drone with id: {droneId} is no vacant");
            Parcel choosenParcel = FindParcelToCollecting(drones[i].Id);
            if(choosenParcel == null)
                throw new ObjectNotAvailableForActionException("No available Parcel");
            drones[i].DroneStatuses = DroneStatuses.sending;
            drones[i].NumOfParcel = choosenParcel.Id;
            choosenParcel.DroneDelivery = new DroneDelivery()
            {
                BatteryStatuses = drones[i].BatteryStatuses,
                CurrentLocation = drones[i].CurrentLocation,
                Id = drones[i].Id
            };
            choosenParcel.AssignmentTime = DateTime.Now;
            lock (dalObject)
            {
                dalObject.ParcelToDrone(choosenParcel.Id, droneId);
            }
        }


        /// <summary>
        /// supply the parcel to the geeter customer from the drone
        /// </summary>
        /// <param name="droneld">the drone id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
            dalObject.Destination(drone.Parcel.Id);
        }

        public void StartSimulator(int id, Action update, Func<bool> checkStop) => new Simulator(this, id, update, checkStop);


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="drone"></param>
        ///// <returns></returns>
        //public Drone detailsDrone(Drone drone)
        //{
        //    DroneToList droneToList = drones.Find(item => item.Id == drone.Id);
        //    return new Drone()
        //    {
        //        Id = drone.Id,
        //        Model = drone.Model,
        //        MaxWeight = (WeightCategories)drone.MaxWeight,
        //        BatteryStatuses = droneToList.BatteryStatuses,
        //        DroneStatuses = droneToList.DroneStatuses,
        //        CurrentLocation = droneToList.CurrentLocation,
        //        //Parcel = droneToList.NumOfParcel != null ? throw("finish") : null
        //    };
        //}
    }
}