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
    /// Drone BL class
    /// </summary>
    partial class BL
    {
        /// <summary>
        /// AddDrone
        /// Exception: ObjectAlreadyExistException
        /// </summary>
        /// <param name="id">id of the new drone</param>
        /// <param name="model">model of the new drone</param>
        /// <param name="maxWeight">max Weight of the new drone</param>
        /// <param name="stationId">stationId for the new drone first charging</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(int id, string model, WeightCategories maxWeight, int stationId)
        {
            DO.Drone newDrone = new DO.Drone() { Id = id, Model = model, MaxWeight = (DO.WeightCategories)maxWeight };
            try { lock (dalObject) { dalObject.AddDrone(newDrone); } }
            catch (DO.ObjectAlreadyExistException e) { throw new ObjectAlreadyExistException(e.Message); }
            drones.Add(DroneToDroneToList(newDrone, stationId));
            dalObject.ChargeOn(id, stationId);
        }


        /// <summary>
        /// UpdateDrone
        /// Exception: NoChangesToUpdateException
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
        /// GetDrone
        /// Exception: ObjectNotExistException
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
            catch (DO.ObjectNotExistException e) { throw new ObjectNotExistException(e.Message); }
            if (drone.IsDelete) throw new ObjectNotExistException("drone deleted");
            return DlToBlDrone(drone);
        }


        /// <summary>
        /// Delete Drone
        /// Exception: ObjectNotExistException
        /// </summary>
        /// <param name="id">drone id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int id)
        {
            lock (dalObject) { dalObject.DeleteDrone(id); }
            drones.RemoveAt(drones.FindIndex(d => d.Id == id));
        }


        /// <summary>
        /// DronesList
        /// return all drones
        /// </summary>
        /// <returns>IEnumerable of DroneToList</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> DronesList() => drones;

        #region DroneCharge
        /// <summary>
        /// ChargeOn
        /// Exception: ObjectNotExistException, ObjectNotAvailableForActionException
        /// Start charging
        /// </summary>
        /// <param name="droneId">drone Id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ChargeOn(int droneId)
        {
            int i = drones.FindIndex(d => d.Id == droneId);
            if (i < 0)
                throw new ObjectNotExistException($"Drone with id = {droneId} is not exsist");
            Station closeStation = FindCloseStationWithChargeSlot(drones[i].CurrentLocation);
            double powerForDistance = FindMinPowerForDistance(drones[i].Distance(closeStation));
            if (drones[i].BatteryStatuses - powerForDistance < 0)
                throw new ObjectNotAvailableForActionException($"Not enough power for distance in drone with id = {droneId}");

            StartChargeing(droneId, closeStation.Id);
            drones[i].BatteryStatuses = drones[i].BatteryStatuses - powerForDistance;
            drones[i].CurrentLocation = closeStation.CurrentLocation;
        }


        /// <summary>
        /// Start Chargeing
        /// Exception: ObjectNotExistException, ObjectNotAvailableForActionException
        /// </summary>
        /// <param name="droneId">droneId</param>
        /// <param name="closeStationId">close Station Id</param>
        internal void StartChargeing(int droneId, int closeStationId)
        {
            int i = drones.FindIndex(d => d.Id == droneId);
            if (i < 0)
                throw new ObjectNotExistException($"Drone with id = {droneId} is not exsist");

            if (drones[i].DroneStatuses != DroneStatuses.vacant)
                throw new ObjectNotAvailableForActionException($"Drone with id = {droneId} is not vacant");

            drones[i].DroneStatuses = DroneStatuses.maintanance;
            lock (dalObject) { dalObject.ChargeOn(droneId, closeStationId); }
        }


        /// <summary>
        /// Charge Step
        /// Exception: ObjectNotExistException
        /// </summary>
        /// <param name="droneId">droneId</param>
        /// <param name="timeStep">timeStep</param>
        internal void ChargeStep(int droneId, double timeStep)
        {
            int i = drones.FindIndex(d => d.Id == droneId);
            if (i < 0)
                throw new ObjectNotExistException("drone with id: " + droneId);
            drones[i].BatteryStatuses = Min(1.0, drones[i].BatteryStatuses + skimmerLoadingRate * timeStep);
        }


        /// <summary>
        /// ChargeOf
        /// Exception: ObjectNotExistException, ObjectNotAvailableForActionException
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
            double newBatteryStatuses = drones[i].BatteryStatuses + skimmerLoadingRate * (DateTime.Now - startTime).TotalSeconds / 10;
            drones[i].BatteryStatuses = Min(newBatteryStatuses, 1.0);
            drones[i].DroneStatuses = DroneStatuses.vacant;
        }
        #endregion


        /// <summary>
        /// DronStepTo
        /// Exception: ObjectNotExistException
        /// Dron Step To
        /// </summary>
        /// <param name="dronId">dron Id</param>
        /// <param name="currentDistance">current Distance</param>
        /// <param name="distance">distance</param>
        internal void DronStepTo(int dronId, double currentDistance, double distance, Ilocatable destination)
        {
            int i = drones.FindIndex(d => d.Id == dronId);
            if (i < 0)
                throw new ObjectNotExistException("dronId");
            WeightCategories? parcelWeight = drones[i].NumOfParcel != null ? GetParcel((int)drones[i].NumOfParcel).Weight : default(WeightCategories?);
            double propor = currentDistance == 0 ? 0 : currentDistance / distance;
            double powerForStep = FindMinPowerForDistance(currentDistance, parcelWeight);
            drones[i].BatteryStatuses = Max(0.0, drones[i].BatteryStatuses - powerForStep);
            drones[i].CurrentLocation = new Location()
            {
                Latitude = drones[i].CurrentLocation.Latitude + (destination.CurrentLocation.Latitude - drones[i].CurrentLocation.Latitude) * propor,
                Longitude = drones[i].CurrentLocation.Longitude + (destination.CurrentLocation.Longitude - drones[i].CurrentLocation.Longitude) * propor
            };
        }
    }
}
