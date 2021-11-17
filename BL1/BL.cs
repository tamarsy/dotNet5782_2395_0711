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
            //PowerConsumptionRequest = new double { dalObject.PowerConsumptionRequest() };
            drones = new List<DroneToList>();
            initializeDronesList();
        }

        private void initializeDronesList()
        {
            /*//TODO : DeliveryId
            foreach (var drone in drones)
            {
                drone.NumOfParcel = 0;
            }
            //TODO : Battery & Status
            foreach (var drone in drones)
            {
                drone.Battery = 1;
                drone.Status = DroneStatuses.Maintenance;
            }
            */
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
            if (drone.DroneStatuses == DroneStatuses.Maintenance)
            {
                int stationId = rand.Next(dalObject.GetStations().Count());
                IDAL.DO.Station Station = dalObject.GetStation(stationId);
                Location l = new Location( Station.Lattitude, Station.Longitude );
                return l;
            }

            if (drone.DroneStatuses == DroneStatuses.Delivery)
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
            if (drone.DroneStatuses == DroneStatuses.Available)
            {
                //TODO: find customer location
            }
            return new Location();
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
        

        public void AddStation(string stationName, int positions)
        {
            
        }

        public int AddDrone()
        {
            throw new NotImplementedException();
        }

        public void AssignmentParcelToDrone(int parcelId, int droneId)
        {
            throw new NotImplementedException();
        }

        public void PickedupParcel(int parcelId)
        {
            throw new NotImplementedException();
        }

        public void SendDroneToRecharge(int droneId, int baseStationId)
        {
            throw new NotImplementedException();
        }

        public void ReleaseDroneFromRecharge(int droneId)
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

        public IEnumerable<Parcel> UnAssignmentParcels()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> AvailableChargingStations()
        {
            throw new NotImplementedException();
        }
    }
}
