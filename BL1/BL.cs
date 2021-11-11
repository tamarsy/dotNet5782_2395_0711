using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using DalObject;
using IBL.BO;

namespace BL
{
    partial class BL: IBL.IBL
    {
        private IDal dalObject;
        private List<DroneToList> drones;
        private static Random rand = new Random();
        public BL()
        {
            dalObject = new DalObject.DalObject();
            drones = new List<DroneToList>();
            initializeDronesList();
        }

        private void initializeDronesList()
        {
            foreach (var drone in dalObject.GetDrones())
            {
                drones.Add(new DroneToList
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (WeightCategories)drone.MaxWeight
                });
            }

            foreach (var drone in drones)
            {
                drone.CurrentLocation = findLocation(drone);
            }
        }

        private location findLocation(Drone drone)
        {
            if (drone.DroneStatuses == DroneStatuses.Maintenance)
            {
                int stationId = rand.Next(dalObject.GetBaseStations().Count());
                IDAL.DO.BaseStation baseStation = dalObject.GetBaseStation(stationId);
                return new Location { Latitude = baseStation.Latitude, Longitude = baseStation.Longitude };
            }

            if (drone.Status == DroneStatuses.Delivery)
            {
                IDAL.DO.Parcel parcel = dalObject.GetParcel(drone.DeliveryId);
                if (parcel.PickedUp == null)
                {
                    return findClosetBaseStationLocation(drone);
                }
                if (parcel.Delivered == null)
                {
                    return GetCustomer(parcel.SenderId).Location;
                }
            }
            if (drone.Status == DroneStatuses.Available)
            {
                //TODO: find customer location
            }
            return new Location();
        }

        private Location findClosetBaseStationLocation(DroneForList drone)
        {
            List<Station> locations = new List<Station>();
            foreach (var baseStation in dalObject.GetStations())
            {
                locations.Add(new Station
                {
                    CurrentSiting = new Location
                    {
                        Latitude = baseStation.Latitude,
                        Longitude = baseStation.Longitude
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
        public void AddStation(Station station)
        {

        }
        public void AddDrone(Drone newDrone)
        {

        }
        public void AddCustomer(Customer newCustomer)
        {

        }
        public void AddParcel(Parcel newpParcel)
        {

        }
        public void ParcelToDrone(int percelChoose, int droneChoose)
        {

        }
        public void PickParcel(int percelChoose)
        {

        }
        public void Destination(int percelChoose)
        {

        }
        public void ChargeOn(int droenId)
        {

        }
        public void ChargeOf(int droenId)
        {

        }
        public Station ViewStation(int id)
        {

        }
        public Drone ViewDrone(int id)
        {

        }
        public Customer ViewCustomer(int id)
        {

        }
        public Parcel ViewParcel(int id)
        {

        }
        public IEnumerable<Station> StationList()
        {

        }
        public IEnumerable<Drone> DroneList()
        {

        }
        public IEnumerable<Customer> CustomerList()
        {

        }
        public IEnumerable<Parcel> ParcelList()
        {

        }
        public IEnumerable<Parcel> ParcesWithoutDronelList()
        {

        }
        public IEnumerable<Station> EmptyChangeSlotlList()
        {

        }
        public double[] PowerConsumptionRequest()
        {

        }

        public void AddStation(Station newStation)
        {
            throw new NotImplementedException();
        }
    }
}
