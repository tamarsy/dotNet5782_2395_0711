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
    /// bl class
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


        /// <summary>
        /// initialize Drones List for bl constructor
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
        /// find the location for Bl Cotor
        /// </summary>
        /// <param name="drone"></param>
        /// <returns>GetCustomer</returns>
        internal Location FindLocation(DroneToList drone)
        {
            if (drone.DroneStatuses == DroneStatuses.sending)
            {
                lock (dalObject)
                {
                    DO.Parcel parcel = dalObject.GetParcel((int)drone.NumOfParcel);
                    if (parcel.PickedUp == null)
                    {
                        return FindClosetStationLocation(drone.CurrentLocation).CurrentLocation;
                    }
                    if (parcel.Delivered == null)
                    {
                        DO.Customer customer = dalObject.GetCustomer(parcel.SenderId);
                        return new Location(customer.Lattitude, customer.Longitude);
                    }
                }
            }
            if (drone.DroneStatuses == DroneStatuses.maintanance)
            {
                lock (dalObject)
                {
                    int stationId = rand.Next(dalObject.StationList().Count());
                    return GetStation(stationId).CurrentLocation;
                }
            }
            List<int> idOfCustomers = CustomersList().Where(customer => customer.NumOfParcelsSupplied > 0).Select(customer => customer.Id).ToList();
            if (idOfCustomers.Count == 0)
            {
                return GetCustomer(CustomersList().ToList()[0].Id).CurrentLocation;
            }
            int customerId = rand.Next(idOfCustomers.Count());
            return GetCustomer(idOfCustomers[customerId]).CurrentLocation;
        }


        /// <summary>
        /// find the battery status for the initialize drones list
        /// </summary>
        /// <param name="drone"></param>
        /// <returns> rand number as battery</returns>
        private double FindBatteryStatusesForInitializeDronesList(DroneToList drone)
        {
            if (drone.DroneStatuses == DroneStatuses.maintanance)
            {
                return rand.NextDouble() * 20/100;//0 to 20
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
                    return Min(100, (rand.NextDouble() * (100 - MinPower)) + MinPower)/100;//min to 100
                }
            }
            else
            {
                Location chargeSlotLocation = FindCloseStationWithChargeSlot(drone.CurrentLocation).CurrentLocation;
                double distance = drone.CurrentLocation.Distance(chargeSlotLocation);
                double MinPower = FindMinPowerForDistance(distance);
                return ((MinPower > 100) ? 100 : (rand.NextDouble() * (100 - MinPower)) + MinPower)/100;//min to 100
            }
        }


        /// <summary>
        /// Start Simulator in bl
        /// </summary>
        /// <param name="id">drone id</param>
        /// <param name="update">to update window</param>
        /// <param name="checkStop">to stop simulator</param>
        public void StartSimulator(int id, Action update, Func<bool> checkStop) => new Simulator(this, id, update, checkStop);
    }
}