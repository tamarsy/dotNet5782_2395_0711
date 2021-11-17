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
    public partial class BL: IBL
    {
        private IDal dalObject;
        private double[] PowerConsumptionRequest;
        private List<DroneToList> drones;
        private static Random rand = new Random();
        public BL()
        {
            dalObject = new DalObject.DalObject();
            PowerConsumptionRequest = new double[] { dalObject.PowerConsumptionRequest() };
            drones = new List<DroneToList>();
            initializeDronesList();
        }

        private void initializeDronesList()
        {
            //copy to drones from data
            foreach (var drone in dalObject.GetDrones())
            {
                drones.Add(new DroneToList
                (
                    drone.Id,
                    drone.Model,
                    (WeightCategories)drone.MaxWeight
                ));
            }
            
            foreach (var drone in drones)
            {
                drone.CurrentLocation = findLocation(drone);
            }
        }

        private Location findLocation(DroneToList drone)
        {
            Location newLoction = new Location();

            if (drone.DroneStatuses == DroneStatuses.sending)
            {
                IDAL.DO.Parcel parcel = dalObject.GetParcel(drone.NumOfParcel);
                if (parcel.PickedUp == null)
                {
                    return findClosetStationLocation(drone);
                }
                if (parcel.Delivered == null)
                {
                    return ViewCustomer(parcel.SenderId).CurrentLocation;
                }
            }


            if (drone.DroneStatuses == DroneStatuses.maintanance)
            {
                drone.BatteryStatuses = rand.Next(20);
                int stationId = rand.Next(dalObject.StationList().Count());
                IDAL.DO.Station Station = dalObject.GetStation(stationId);
                newLoction = new Location(Station.Lattitude, Station.Longitude);
                return newLoction;
            }


            if (drone.DroneStatuses == DroneStatuses.vacant)
            {
                //dalObject.
                drone.BatteryStatuses = rand.Next(81) + 20;
            }

            return newLoction;
        }

        private Location findClosetStationLocation(DroneToList drone)
        {
            List<Station> locations = new List<Station>();
            foreach (var Station in dalObject.GetStations())
            {
                locations.Add(new Station
                {
                    CurrentSiting = new Location
                    {
                        Latitude = Station.Latitude,
                        Longitude = Station.Longitude
                    }
                });
            }
            Location location = locations[0].Location;
            double distance = drone.Distance(locations[0]);
            for (int i = 1; i < locations.Count; i++)
            {
                if (drone.Distance(locations[i]) < distance)
                {
                    location = locations[i].Location;
                    distance = drone.Distance(locations[i]);
                }
            }
            return location;
        }

        public void AddStation(int id, string name, Location location, int chargeSlots)
        {
            throw new NotImplementedException();
        }

        public void AddDrone(int id, string model, WeightCategories maxWeight, int stationId)
        {
            throw new NotImplementedException();
        }

        public void AddCustomer(int id, string name, string phone, Location location)
        {
            throw new NotImplementedException();
        }

        public void AddParcel(int sid, int tid, WeightCategories weigth, Priorities priority)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(int id, string name, int numOfChargeSlot)
        {
            throw new NotImplementedException();
        }

        public void UpdateDrone(int id, string model)
        {
            throw new NotImplementedException();
        }

        public void UpdateCusomer(int id, string name, string phone)
        {
            throw new NotImplementedException();
        }

        public void ChargeOn(int id)
        {
            throw new NotImplementedException();
        }

        public void ChargeOf(int id, float timeInCharge)
        {
            throw new NotImplementedException();
        }

        public void ParcelToDrone(int id)
        {
            throw new NotImplementedException();
        }

        public void PickParcel(int id)
        {
            throw new NotImplementedException();
        }

        public void Destination(int id)
        {
            throw new NotImplementedException();
        }

        public Station GetStation(int requestedId)
        {
            throw new NotImplementedException();
        }

        public Drone GetDrone(int requestedId)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int requestedId)
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int requestedId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StationToList> GetStations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DroneToList> GetDrones()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetParcels()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> EmptyChangeSlotlList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> ParcesWithoutDronelList()
        {
            throw new NotImplementedException();
        }
    }
}
