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
    partial class BL
    {
        private double FindBatteryStatusesForInitializeDronesList(DroneToList drone)
        {
            if (drone.DroneStatuses == DroneStatuses.maintanance)
            {
                return rand.NextDouble() * 20;//0 to 20
            }
            if (drone.DroneStatuses == DroneStatuses.sending)
            {
                IDAL.DO.Parcel parcel = dalObject.GetParcel((int)drone.NumOfParcel);
                Location targilIdLocation = GetCustomer(parcel.Getter).CurrentLocation;
                Location chargeSlotLocation = FindCloseStationWithChargeSlot(targilIdLocation).CurrentLocation;
                double distance = FindDistanceOfRoute(drone.CurrentLocation, targilIdLocation, chargeSlotLocation);
                return (rand.NextDouble() * (100 - MinPowerForDistance(distance, (WeightCategories)parcel.Weight))) + MinPowerForDistance(distance, (WeightCategories)parcel.Weight);//min to 100
            }
            else
            {
                Location chargeSlotLocation = FindCloseStationWithChargeSlot(drone.CurrentLocation).CurrentLocation;
                double distance = drone.CurrentLocation.Distance(chargeSlotLocation);
                return (rand.NextDouble() * (100 - MinPowerForDistance(distance))) + MinPowerForDistance(distance);//min to 100
            }
        }




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




        private double MinPowerForDistance(double distance, WeightCategories? weight = null)
        {
            if (weight == WeightCategories.easy)
                return lightWeightCarrier * distance;
            if (weight == WeightCategories.medium)
                return mediumWeightCarrier * distance;
            if (weight == WeightCategories.heavy)
                return heavyWeightCarrier * distance;
            return vacent * distance;
        }

        private Location FindLocation(DroneToList drone)
        {
            if (drone.DroneStatuses == DroneStatuses.sending)
            {
                IDAL.DO.Parcel parcel = dalObject.GetParcel((int)drone.NumOfParcel);
                if (parcel.PickedUp == null)
                {
                    return FindClosetStationLocation(drone.CurrentLocation);
                }
                if (parcel.Delivered == null)
                {
                    IDAL.DO.Customer customer = dalObject.GetCustomer(parcel.SenderId);
                    return new Location(customer.Lattitude, customer.Longitude);
                }
            }
            if (drone.DroneStatuses == DroneStatuses.maintanance)
            {
                int stationId = rand.Next(dalObject.StationList().Count());
                return GetStation(stationId).CurrentLocation;
            }
            List<int> idOfCustomers = CustomersList().Where(customer => customer.NumOfParcelsSupplied > 0).Select(customer => customer.Id).ToList();
            int customerId = rand.Next(idOfCustomers.Count());
            return GetCustomer(idOfCustomers[customerId]).CurrentLocation;
        }




        private Station FindCloseStationWithChargeSlot(Location location)
        {
            double minDistance = double.MaxValue;
            Station mostCloseLocation = default;
            foreach (var station in dalObject.EmptyChangeSlotlList())
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




        private Location FindClosetStationLocation(Location currentLoction)
        {
            double minDistance = double.MaxValue;
            Location mostCloseStationLoction = default(Location);
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
