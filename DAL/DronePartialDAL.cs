using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;

namespace DalObject
{
    public partial class DAL
    {

        /// <summary>
        /// Exception: ObjectAlreadyExistException, ArgumentNullException
        /// the function add a new drone to the arry
        /// </summary>
        /// <param name="drone">the new drone to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone newDrone)
        {
            if (newDrone.Equals(default(Drone)))
                throw new ArgumentNullException("Null argument");
            if (DataSource.DronesArr.Exists(drone => drone.Id == newDrone.Id))
                throw new ObjectAlreadyExistException("Can't add, There is already a drone with this ID");
            DataSource.DronesArr.Add(newDrone);
        }


        /// <summary>
        /// Exception: ObjectNotExistException, ObjectNotAvailableForActionException
        /// find a ststion with empty charge slot and charge the dron
        /// </summary>
        /// <param name="droenId">the dron id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ChargeOn(int droenId, int stationId)
        {
            Station s;
            if (!DataSource.DronesArr.Exists(d => d.Id == droenId))
                throw new ObjectNotExistException("no drone whith id: " + droenId);
            s = GetStation(stationId);
            if (!isEmptyChargeSlotInStation(stationId, s.ChargeSlot))
                throw new ObjectNotAvailableForActionException($"no empty charge slot in station with id: {stationId}");
            DataSource.listOfChargeSlot.Add(
                new DroneCharge()
                {
                    DroneId = droenId,
                    StationId = stationId,
                    StartTime = DateTime.Now
                });
        }



        /// <summary>
        /// Exception: ObjectNotExistException, ObjectNotAvailableForActionException
        /// charge of the dron from the charge slot
        /// </summary>
        /// <param name="droenId">the dron id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ChargeOf(int droenId)
        {
            int i = DataSource.DronesArr.FindIndex((d) => d.Id == droenId);
            if (i < 0)
                throw new ObjectNotExistException("no drone whith id: " + droenId + "in charge slot");
            //remove from the list Of Charge Slot in DataSource
            int f = DataSource.listOfChargeSlot.RemoveAll((dch)=> dch.DroneId == droenId);
            if (f < 0)
                throw new ObjectNotAvailableForActionException($"no charge for drone: {droenId}");
        }

        /// <summary>
        /// Exception: ObjectNotExistException
        /// return the choosen drone by id
        /// </summary>
        /// <param name="id">the drone id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int id)
        {
            int i = DataSource.DronesArr.FindIndex(item => item.Id == id);
            if (i < 0)
                throw new ObjectNotExistException("no drone whith id: " + id + "in charge slot");
            return DataSource.DronesArr[i];
        }

        /// <summary>
        /// return Drones List
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> DroneList() => DataSource.DronesArr.Where(c => !c.IsDelete);


        /// <summary>
        /// Exception: ObjectNotExistException, ArgumentNullException
        /// Update Drone details
        /// </summary>
        /// <param name="drone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone drone)
        {
            if(drone.Equals(default(Drone)))
                throw new ArgumentNullException("Null argument");
            int i = DataSource.DronesArr.FindIndex(s => s.Id == drone.Id && !s.IsDelete);
            if (i < 0)
                throw new ObjectNotExistException("no drone whith id: " + drone.Id + "in charge slot");
            DataSource.DronesArr[i] = drone;
        }



        /// <summary>
        /// Exception: ObjectNotExistException
        /// Delete Drone
        /// </summary>
        /// <param name="Drone id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int id)
        {
            int i = DataSource.DronesArr.FindIndex(d => d.Id == id && !d.IsDelete);
            if (i < 0)
                throw new ObjectNotExistException("not found a Drone with id = " + id);
            DataSource.DronesArr[i] = new Drone()
            {
                Id = DataSource.DronesArr[i].Id,
                Model = DataSource.DronesArr[i].Model,
                MaxWeight = DataSource.DronesArr[i].MaxWeight,
                IsDelete = true
            };
        }
    }
}
