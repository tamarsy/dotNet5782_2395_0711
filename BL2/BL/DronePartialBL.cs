using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using DalApi;
using BO;
using System.Runtime.CompilerServices;

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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(int id, string model, WeightCategories maxWeight, int stationId)
        {
            DO.Drone newDrone = new DO.Drone() { Id = id, Model = model, MaxWeight = (DO.WeightCategories)maxWeight };
            try
            {
                lock (dalObject) { dalObject.AddDrone(newDrone); }
            }
            catch (DO.ObjectAlreadyExistException e)
            {
                throw new ObjectAlreadyExistException(e.Message);
            }
            lock (dalObject)
            {
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
        }




        /// <summary>
        /// update the model for specific drone
        /// </summary>
        /// <param name="id">the drone id</param>
        /// <param name="model"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int id, string model)
        {
            lock (dalObject)
            {
                DO.Drone drone = dalObject.GetDrone(id);
                if (model == default)
                    throw new NoChangesToUpdateException();

                drone.Model = model;
                dalObject.UpdateDrone(drone);
                drones[drones.FindIndex((d) => d.Id == id)].Model = model;
            }
        }



        /// <summary>
        /// return the specific drone details
        /// </summary>
        /// <param name="requestedId"> the requested drone id</param>
        /// <returns>Drone</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int requestedId)
        {
            DO.Drone drone;
            try
            {
                lock (dalObject) { drone = dalObject.GetDrone(requestedId); }
            }
            catch (DO.ObjectNotExistException e){ throw new ObjectNotExistException(e.Message); }
            if (drone.IsDelete) throw new ObjectNotExistException("drone deleted");
            return DalToBlDrone(drone);
        }





        /// <summary>
        /// return all drones
        /// </summary>
        /// <returns>IEnumerable of DroneToList</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> DronesList() => drones;



        /// <summary>
        /// start charging
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ChargeOn(int id)
        {
            int i = drones.FindIndex(d => d.Id == id);
            if (i < 0)
                throw new ObjectNotExistException($"Drone with id = {id} is not exsist");
            if (drones[i].DroneStatuses != DroneStatuses.vacant)
                throw new ObjectNotAvailableForActionException($"Drone with id = {id} is not vacant");

            Station closeStation = FindCloseStationWithChargeSlot(drones[i].CurrentLocation);
            double powerForDistance = FindMinPowerForDistance(drones[i].Distance(closeStation));
            if (drones[i].BatteryStatuses - powerForDistance < 0)
            {
                powerForDistance = FindMinPowerForDistance(drones[i].Distance(FindClosetStationLocation(drones[i])));
                if (drones[i].BatteryStatuses - powerForDistance < 0)
                    throw new ObjectNotAvailableForActionException($"Not enough power for distance in drone with id = {id}");
                else
                {
                    drones[i].BatteryStatuses = drones[i].BatteryStatuses - powerForDistance;
                    drones[i].CurrentLocation = closeStation.CurrentLocation;
                    throw new ObjectNotAvailableForActionException($"No empty charge slot in station");
                }
            }
            drones[i].BatteryStatuses = drones[i].BatteryStatuses - powerForDistance;
            drones[i].CurrentLocation = closeStation.CurrentLocation;
            drones[i].DroneStatuses = DroneStatuses.maintanance;
            lock (dalObject) { dalObject.ChargeOn(id, closeStation.Id); }
        }



        /// <summary>
        /// releas from charging
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timeInCharge"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ChargeOf(int id)
        {
            DateTime startTime = default;
            try
            {
                lock (dalObject)
                {
                    startTime = dalObject.StartChargeTime(id);
                    dalObject.ChargeOf(id);
                }
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
            drones[i].BatteryStatuses = Min(newBatteryStatuses, 100);
            drones[i].DroneStatuses = DroneStatuses.vacant;
        }



        /// <summary>
        /// Delete Drone
        /// </summary>
        /// <param name="id">drone id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int id)
        {
            lock (dalObject) { dalObject.DeleteDrone(id); }
            drones.RemoveAt(drones.FindIndex(d => d.Id == id));
        }
    }
}
