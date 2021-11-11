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
        IDal dalObject;
        List<Drone> dones;
        public BL()
        {
            dalObject = new DalObject();
            dones = new List<DroneForList>();
            initializeDrones();
        }

        private void initializeDrones()
        {
            foreach (var drone in dal.GetDrones())
            {
                drones.Add(new DroneForList
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (WeightCategories)drone.MaxWeight
                });
            }
            //TODO : DeliveryId
            foreach (var drone in drones)
            {
                drone.DeliveryId = 0;
            }
            //TODO : Battery & Status
            foreach (var drone in drones)
            {
                drone.Battery = 1;
                drone.Status = DroneStatuses.Maintenance;
            }

            foreach (var drone in drones)
            {
                drone.Location = findDroneLocation(drone);
            }
        }
        public void AddStation(int id, string name, int longitude, int lattitude)
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
    }
}
