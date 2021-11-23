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
        public void AddDrone(int id, string model, WeightCategories maxWeight, int stationId)
        {
            IDAL.DO.Drone newDrone = new IDAL.DO.Drone() { Id = id, Model = model, MaxWeight = (IDAL.DO.WeightCategories)maxWeight };
            try
            {
                dalObject.AddDrone(newDrone);
            }
            catch (DalObject.ObjectAlreadyExistException e)
            {
                throw new ObjectAlreadyExistException(e.Message);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            drones.Add(new DroneToList()
            {
                Id = id,
                Model = model,
                MaxWeight = maxWeight,
                BatteryStatuses = (rand.NextDouble()*20) + 20,
                CurrentLocation = GetStation(stationId).CurrentLocation,
                DroneStatuses = DroneStatuses.maintanance
            });
            dalObject.ChargeOn(id);
        }





        public void UpdateDrone(int id, string model)
        {
            IDAL.DO.Drone drone = dalObject.GetDrone(id);
            if (model == default)
                throw new NoChangesToUpdateException();

            drone.Model = model;
            //או למחוק ולהכניס חדש או לעדכן
        }




        public Drone GetDrone(int requestedId)
        {
            IDAL.DO.Drone drone;
            try
            {
                drone = dalObject.GetDrone(requestedId);
            }
            catch (DalObject.ObjectNotExistException e)
            {
                throw new ObjectNotExistException(e.Message);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return DalToBlDrone(drone);
        }




        private Drone DalToBlDrone(IDAL.DO.Drone drone)
        {
            DroneToList droneFromList = drones.Find(d => d.Id == drone.Id);
            Parcel parcel = GetParcel((int)droneFromList.NumOfParcel);
            Customer sender = GetCustomer(parcel.SenderId.Id);
            Customer Getter = GetCustomer(parcel.GetterId.Id);
            return new Drone()
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (WeightCategories)drone.MaxWeight,
                CurrentLocation = droneFromList.CurrentLocation,
                BatteryStatuses = droneFromList.BatteryStatuses,
                DroneStatuses = droneFromList.DroneStatuses,
                Parcel = droneFromList.NumOfParcel == default ? default : new ParcelDelivery()
                {
                    Id = parcel.Id,
                    Weight = parcel.Weight,
                    Priority = parcel.Priority,
                    StatusParcel = !parcel.PickUpTime.Equals(default),
                    Collecting = sender.CurrentLocation,
                    DeliveryDestination = Getter.CurrentLocation,
                    Distance = sender.CurrentLocation.Distance(Getter),
                    SenderId = new DeliveryCustomer() { Id = sender.Id, Name = sender.Name },
                    GetterId = new DeliveryCustomer() { Id = Getter.Id, Name = Getter.Name }
                }
            };
        }


        public IEnumerable<DroneToList> DronesList() => drones;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void ChargeOn(int id)
        {
            int i = drones.FindIndex(d => d.Id == id);
            if (drones[i].DroneStatuses != DroneStatuses.vacant)
            {
                throw new ObjectNotAvailableForActionException($"drone with id = {id} is not vacant");
            }
            Station closeStation = FindCloseStationWithChargeSlot(drones[i].CurrentLocation);
            double powerForDistance = MinPowerForDistance(drones[i].Distance(closeStation));
            if (drones[i].BatteryStatuses - powerForDistance < 0)
                throw new ObjectNotAvailableForActionException($"not enough power for distance in drone with id = {id}");
            drones[i].BatteryStatuses = drones[i].BatteryStatuses - powerForDistance;
            drones[i].CurrentLocation = closeStation.CurrentLocation;
            drones[i].DroneStatuses = DroneStatuses.maintanance;
            //in station saved only count of all charge slots
            try
            {
                dalObject.ChargeOn(id);
            }
            catch (DalObject.ObjectNotExistException)
            {
                throw;
            }
        }




        public void ChargeOf(int id, float timeInCharge)
        {
            int i = drones.FindIndex(d => d.Id == id);
            if (drones[i].DroneStatuses != DroneStatuses.maintanance)
            {
                throw new ObjectNotAvailableForActionException($"drone with id = {id} is not in charging now");
            }
            drones[i].BatteryStatuses += skimmerLoadingRate * timeInCharge;
            drones[i].DroneStatuses = DroneStatuses.vacant;
            try
            {
                dalObject.ChargeOf(id);
            }
            catch (DalObject.ObjectNotExistException)
            {
                throw;
            }
        }
    }
}
