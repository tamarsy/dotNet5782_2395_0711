using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalObject
{
    public partial class DAL
    {

        /// <summary>
        /// the function add a new drone to the arry
        /// </summary>
        /// <param name="drone">the new drone to add</param>
        public void AddDrone(Drone newDrone)
        {
            if (DataSource.DronesArr.Exists(drone => drone.Id == newDrone.Id))
            {
                throw new ObjectAlreadyExistException("Can't add, There is already a drone with this ID");
            }
            DataSource.DronesArr.Add(newDrone);
        }


        /// <summary>
        /// the function find a ststion with empty charge slot and charge the dron
        /// </summary>
        /// <param name="droenId">the dron id</param>
        public void ChargeOn(int droenId, int stationId)
        {
            Station s;
            try
            {
                GetDrone(droenId); s = GetStation(stationId);
            }
            catch (ObjectNotExistException)
            {
                throw new ObjectNotExistException("no drone whith id: " + droenId);
            }
            if (!isEmptyChargeSlotInStation(stationId, s.ChargeSlot))
            {
                throw new ObjectNotAvailableForActionException($"no empty charge slot in station with id: {stationId}");
            }
            DataSource.listOfChargeSlot.Add(
                new DroneCharge()
                {
                    DroneId = droenId,
                    StationId = stationId,
                    StartTime = DateTime.Now
                });
        }



        /// <summary>
        /// the function charge of the dron from the charge slot
        /// </summary>
        /// <param name="droenId">the dron id</param>
        public void ChargeOf(int droenId)
        {
            int i = DataSource.DronesArr.FindIndex((d) => d.Id == droenId);
            if (i < 0)
            {
                throw new ObjectNotExistException("no drone whith id: " + droenId + "in charge slot");
            }
            //remove from the list Of Charge Slot in DataSource
            int f = DataSource.listOfChargeSlot.RemoveAll((dch)=> dch.DroneId == droenId);
            if (f < 0)
            {
                throw new ObjectNotAvailableForActionException($"no charge for drone: {droenId}");
            }
        }

        /// <summary>
        /// view the choosen drone
        /// </summary>
        /// <param name="id">the drone id</param>
        /// <returns></returns>
        public Drone GetDrone(int id)
        {
            int i = DataSource.DronesArr.FindIndex(item => item.Id == id);
            if (i < 0)
            {
                throw new ObjectNotExistException("id no exist");
            }
            return DataSource.DronesArr[i];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> DroneList() => DataSource.DronesArr;


        public void UpdateDrone(Drone drone)
        {
            int i = DataSource.DronesArr.FindIndex(s => s.Id == drone.Id);
            DataSource.DronesArr[i] = drone;
        }
    }
}
