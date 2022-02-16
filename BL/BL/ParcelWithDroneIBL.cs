using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Math;
using DalApi;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// find the best available parcel for the drone
        /// </summary>
        /// <param name="drone">the specific drone</param>
        /// <returns>Parcel</returns>
        internal Parcel FindParcelToCollecting(int droneId)
        {
            Drone drone = GetDrone(droneId);
            ParcelToList parcelToCollect = ParcesWithoutDronelList().Where((p) => checkAvailableDelivery(drone, p)).
             OrderByDescending(p => p?.Priority)
             .ThenByDescending(p => p?.Weight)
             .FirstOrDefault();
            return parcelToCollect != null ? GetParcel(parcelToCollect.Id) : null;
        }


        /// <summary>
        /// return true if the drone can make the delivery with this parcel
        /// </summary>
        /// <param name="droneToList"> the drone</param>
        /// <param name="parcel">the parcel</param>
        /// <returns>bool value can make the delivery</returns>
        internal bool checkAvailableDelivery(Drone droneToList, ParcelToList parcel)
        {
            Location getterLocation = GetCustomer(parcel.GetterId).CurrentLocation;
            Location senderLocation = GetCustomer(parcel.SenderId).CurrentLocation;
            return parcel.Weight <= droneToList.MaxWeight && 0 < droneToList.BatteryStatuses - (FindMinPowerForDistance(
                droneToList.CurrentLocation.Distance(senderLocation) +
                getterLocation.Distance(FindCloseStationWithChargeSlot(getterLocation))
                )
                + FindMinPowerForDistance(senderLocation.Distance(getterLocation), parcel.Weight)
                );
        }


        /// <summary>
        /// find and conect parcel to the specific drone
        /// </summary>
        /// <param name="droneId">the specific drone id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ParcelToDrone(int droneId)
        {
            int i = drones.FindIndex(d => d.Id == droneId);
            if (i < 0)
                throw new ObjectNotExistException($"drone with id: {droneId} is not exist");
            if (drones[i].DroneStatuses != DroneStatuses.vacant)
                throw new ObjectNotAvailableForActionException($"drone with id: {droneId} is no vacant");
            Parcel choosenParcel = FindParcelToCollecting(drones[i].Id);
            if (choosenParcel == null)
                throw new ObjectNotAvailableForActionException("No available Parcel");
            drones[i].DroneStatuses = DroneStatuses.sending;
            drones[i].NumOfParcel = choosenParcel.Id;
            choosenParcel.DroneDelivery = new DroneDelivery()
            {
                BatteryStatuses = drones[i].BatteryStatuses,
                CurrentLocation = drones[i].CurrentLocation,
                Id = drones[i].Id
            };
            choosenParcel.AssignmentTime = DateTime.Now;
            lock (dalObject)
            {
                dalObject.ParcelToDrone(choosenParcel.Id, droneId);
            }
        }


        /// <summary>
        /// colleced the parcel from customer to the drone
        /// </summary>
        /// <param name="droneld">the drone id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickParcel(int droneld)
        {
            Drone drone = GetDrone(droneld);
            if (drone.DroneStatuses != DroneStatuses.sending || drone.Parcel.StatusParcel)
                throw new ObjectNotAvailableForActionException($"parcel already colleced or not Assignment to drone with id: {droneld}");
            int i = drones.FindIndex(d => d.Id == droneld);
            if (drones[i].BatteryStatuses - FindMinPowerForDistance(drone.Parcel.Distance) < 0)
                throw new ObjectNotAvailableForActionException("not enugh battery in drone");
            drones[i].BatteryStatuses = Max(drones[i].BatteryStatuses - FindMinPowerForDistance(drone.Parcel.Distance), 0);
            drones[i].CurrentLocation = drone.Parcel.Collecting;
            dalObject.PickParcel(drone.Parcel.Id);
        }


        /// <summary>
        /// Supply the parcel to the geeter customer from the drone
        /// </summary>
        /// <param name="droneld">the drone id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Destination(int droneld)
        {
            Drone drone = GetDrone(droneld);
            if (!drone.Parcel.StatusParcel)
                throw new ObjectNotAvailableForActionException($"parcel already colleced or not pick up by drone with id: {droneld}");
            DestinationEnd(droneld);
            int i = drones.FindIndex(d => d.Id == droneld);
            drones[i].BatteryStatuses -= FindMinPowerForDistance(drone.Parcel.Distance, drone.Parcel.Weight);
            drones[i].CurrentLocation = drone.Parcel.DeliveryDestination;
        }


        /// <summary>
        /// Destination End
        /// </summary>
        /// <param name="droneld">drone ld</param>
        internal void DestinationEnd(int droneld)
        {
            int i = drones.FindIndex(d => d.Id == droneld);
            if (i < 0)
                throw new ObjectNotExistException("drone Not Exist" + droneld);
            if (drones[i].NumOfParcel == null)
                throw new ObjectNotAvailableForActionException("drone not Pick a Parcel" + droneld);
            drones[i].DroneStatuses = DroneStatuses.vacant;
            dalObject.Destination((int)drones[i].NumOfParcel);
            drones[i].NumOfParcel = null;
        }
    }
}
