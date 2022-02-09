using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using BO;

namespace BL
{
    /// <summary>
    /// all the function in BL class that conction to drone
    /// </summary>
    partial class BL
    {
        /// <summary>
        /// creat a new drone with the details:
        /// </summary>
        /// <param name="id">id of the new drone</param>
        /// <param name="model">model of the new drone</param>
        /// <param name="maxWeight">max Weight of the new drone</param>
        /// <param name="stationId">stationId for the new drone first charging</param>
        public void AddDrone(int id, string model, WeightCategories maxWeight, int stationId)
        {
            DO.Drone newDrone = new DO.Drone() { Id = id, Model = model, MaxWeight = (DO.WeightCategories)maxWeight };
            try
            {
                dalObject.AddDrone(newDrone);
            }
            catch (DO.ObjectAlreadyExistException e)
            {
                throw new ObjectAlreadyExistException(e.Message);
            }

            DO.Station station = dalObject.GetStation(stationId);
            drones.Add(new DroneToList()
            {
                Id = id,
                Model = model,
                MaxWeight = maxWeight,
                BatteryStatuses = (rand.NextDouble() * 20) + 20,
                CurrentLocation = new Location(station.Lattitude, station.Longitude),
                DroneStatuses = DroneStatuses.maintanance
            });
            dalObject.ChargeOn(id, stationId);
        }




        /// <summary>
        /// update the model for specific drone
        /// </summary>
        /// <param name="id">the drone id</param>
        /// <param name="model"></param>
        public void UpdateDrone(int id, string model)
        {
            DO.Drone drone = dalObject.GetDrone(id);
            if (model == default)
                throw new NoChangesToUpdateException();

            drone.Model = model;
            dalObject.UpdateDrone(drone);
            drones[drones.FindIndex((d) => d.Id == id)].Model = model;
        }



        /// <summary>
        /// return the specific drone details
        /// </summary>
        /// <param name="requestedId"> the requested drone id</param>
        /// <returns>Drone</returns>
        public Drone GetDrone(int requestedId)
        {
            DO.Drone drone;
            try
            {
                drone = dalObject.GetDrone(requestedId);
            }
            catch (DO.ObjectNotExistException e)
            {
                throw new ObjectNotExistException(e.Message);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return DalToBlDrone(drone);
        }



        /// <summary>
        /// convert from drone i dal to drone in bl
        /// </summary>
        /// <param name="drone">the specific drone id</param>
        /// <returns>Drone</returns>
        private Drone DalToBlDrone(DO.Drone drone)
        {
            DroneToList droneFromList = drones.Find(d => d.Id == drone.Id);
            DO.Parcel parcel = default;
            Customer sender = default;
            Customer Getter = default;
            if (droneFromList.NumOfParcel != default)
            {
                parcel = dalObject.GetParcel((int)droneFromList.NumOfParcel);
                sender = GetCustomer(parcel.SenderId);
                Getter = GetCustomer(parcel.GetterId);
            }

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
                    Weight = (WeightCategories)parcel.Weight,
                    Priority = (Priorities)parcel.Priority,
                    StatusParcel = !parcel.PickedUp.Equals(default),
                    Collecting = sender.CurrentLocation,
                    DeliveryDestination = Getter.CurrentLocation,
                    Distance = sender.CurrentLocation.Distance(Getter),
                    SenderId = new DeliveryCustomer() { Id = sender.Id, Name = sender.Name },
                    GetterId = new DeliveryCustomer() { Id = Getter.Id, Name = Getter.Name }
                }
            };
        }


        /// <summary>
        /// return all drones
        /// </summary>
        /// <returns>IEnumerable of DroneToList</returns>
        public IEnumerable<DroneToList> DronesList() => drones;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void ChargeOn(int id)
        {
            int i = drones.FindIndex(d => d.Id == id);
            if (i < 0)
            {
                throw new ObjectNotExistException($"drone with id = {id} is not exsist");
            }
            if (drones[i].DroneStatuses != DroneStatuses.vacant)
            {
                throw new ObjectNotAvailableForActionException($"drone with id = {id} is not vacant");
            }
            Station closeStation = FindCloseStationWithChargeSlot(drones[i].CurrentLocation);
            double powerForDistance = FindMinPowerForDistance(drones[i].Distance(closeStation));
            if (drones[i].BatteryStatuses - powerForDistance < 0)
                throw new ObjectNotAvailableForActionException($"not enough power for distance in drone with id = {id}");
            drones[i].BatteryStatuses = drones[i].BatteryStatuses - powerForDistance;
            drones[i].CurrentLocation = closeStation.CurrentLocation;
            drones[i].DroneStatuses = DroneStatuses.maintanance;
            dalObject.ChargeOn(id, closeStation.Id);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeInCharge"></param>
        public void ChargeOf(int id)
        {
            DateTime startTime = default;
            try
            {
                startTime = dalObject.StartChargeTime(id);
                dalObject.ChargeOf(id);
            }
            catch (DO.ObjectNotExistException e)
            {
                throw new ObjectNotExistException(e.Message);
            }
            int i = drones.FindIndex(d => d.Id == id);
            if (drones[i].DroneStatuses != DroneStatuses.maintanance)
            {
                throw new ObjectNotAvailableForActionException($"drone with id = {id} is not in charging now");
            }
            double newBatteryStatuses = drones[i].BatteryStatuses + skimmerLoadingRate * (DateTime.Now - startTime).TotalSeconds;
            drones[i].BatteryStatuses = newBatteryStatuses > 100 ?100 : newBatteryStatuses;
            drones[i].DroneStatuses = DroneStatuses.vacant;
        }



        /// <summary>
        /// Delete Drone
        /// </summary>
        /// <param name="id">drone id</param>
        public void DeleteDrone(int id) => dalObject.DeleteDrone(id);
    }
}
