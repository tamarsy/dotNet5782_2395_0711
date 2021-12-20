using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using BO;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// the function find the battery status for the initialize drones list
        /// </summary>
        /// <param name="drone"></param>
        /// <returns> (rand.NextDouble() * (100 - MinPower)) + MinPower</returns>
        private double FindBatteryStatusesForInitializeDronesList(DroneToList drone)
        {
            if (drone.DroneStatuses == DroneStatuses.maintanance)
            {
                return rand.NextDouble() * 20;//0 to 20
            }
            if (drone.DroneStatuses == DroneStatuses.sending)
            {
                DO.Parcel parcel = dalObject.GetParcel((int)drone.NumOfParcel);
                Location getterLocation = GetCustomer(parcel.Getter).CurrentLocation;
                Location chargeSlotLocation = FindCloseStationWithChargeSlot(getterLocation).CurrentLocation;
                double distanceWithParcel = drone.Distance(getterLocation);
                double distanceWithOutParcel = getterLocation.Distance(chargeSlotLocation); 
                double MinPower = FindMinPowerForDistance(distanceWithParcel, (WeightCategories)parcel.Weight) + FindMinPowerForDistance(distanceWithOutParcel);
                return (MinPower > 100) ? 100:(rand.NextDouble() * (100 - MinPower)) + MinPower;//min to 100
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
        /// a function to find the distance between the route
        /// </summary>
        /// <param name="Locations"></param>
        /// <returns>sumOfRoute</returns>
        private double FindDistanceOfRoute(params Location[] Locations)
        {
            double sumOfRoute = 0;
            Location lastLocation = Locations[0];
            foreach (var Location in Locations)
            {
                sumOfRoute += Location.Distance(lastLocation);
            }
            return sumOfRoute;
        }



        /// <summary>
        /// a function to find minimum power for distance
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="weight"></param>
        /// <returns>GetCustomer</returns>
        private double FindMinPowerForDistance(double distance, WeightCategories? weight = null)
        {
            if (weight == WeightCategories.easy)
                return lightWeightCarrier * distance;
            if (weight == WeightCategories.medium)
                return mediumWeightCarrier * distance;
            if (weight == WeightCategories.heavy)
                return heavyWeightCarrier * distance;
            return vacent * distance;
        }

        /// <summary>
        /// the function find the location
        /// </summary>
        /// <param name="drone"></param>
        /// <returns>GetCustomer</returns>
        private Location FindLocation(DroneToList drone)
        {
            if (drone.DroneStatuses == DroneStatuses.sending)
            {
                DO.Parcel parcel = dalObject.GetParcel((int)drone.NumOfParcel);
                if (parcel.PickedUp == null)
                {
                    return FindClosetStationLocation(drone.CurrentLocation);
                }
                if (parcel.Delivered == null)
                {
                    DO.Customer customer = dalObject.GetCustomer(parcel.SenderId);
                    return new Location(customer.Lattitude, customer.Longitude);
                }
            }
            if (drone.DroneStatuses == DroneStatuses.maintanance)
            {
                int stationId = rand.Next(dalObject.StationList().Count());
                return GetStation(stationId).CurrentLocation;
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
        /// a function that find close ststion with charge slot
        /// </summary>
        /// <param name="location"></param>
        /// <returns>mostCloseLocation</returns>
        private Station FindCloseStationWithChargeSlot(Ilocatable location)
        {
            double minDistance = double.MaxValue;
            Station mostCloseLocation = default;
            foreach (var station in dalObject.StationList((bool b)=>b))
            {
                Location ChargeSlotLocation = new Location(station.Lattitude, station.Longitude);
                double newDistance = location.Distance(ChargeSlotLocation);
                if (newDistance < minDistance)
                {
                    minDistance = newDistance;
                    mostCloseLocation = DalToBlStation(station);
                }
            }
            return mostCloseLocation;
        }



        /// <summary>
        /// a function to find closet station location
        /// </summary>
        /// <param name="currentLoction"></param>
        /// <returns>mostCloseStationLoction</returns>
        private Location FindClosetStationLocation(Ilocatable currentLoction)
        {
            double minDistance = double.MaxValue;
            Location mostCloseStationLoction = default;
            foreach (var station in dalObject.StationList())
            {
                Location stationLocation = new Location()
                {
                    Latitude = station.Lattitude,
                    Longitude = station.Longitude
                };
                double currentDistance = currentLoction.Distance(stationLocation);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    mostCloseStationLoction = stationLocation;
                }
            }
            return mostCloseStationLoction;
        }
    }
}
