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
        /// the function add a new station to the arry
        /// </summary>
        /// <param name="station">the new station to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station newStation)
        {
            if (DataSource.StationsArr.Exists(station => station.Id == newStation.Id))
            {
                throw new ObjectAlreadyExistException("Can't add, There is already a station with this ID");
            }
            DataSource.StationsArr.Add(newStation);
        }


        /// <summary>
        /// update station
        /// </summary>
        /// <param name="station"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station station)
        {
            int i = DataSource.StationsArr.FindIndex(s => s.Id == station.Id);
            DataSource.StationsArr[i] = station;
        }


        /// <summary>
        /// Exception: ObjectNotExistException
        /// Delete Station
        /// </summary>
        /// <param name="statioId">station id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int statioId)
        {
            int i = DataSource.StationsArr.FindIndex(s => s.Id == statioId && !s.IsDelete);
            if (i < 0)
                throw new ObjectNotExistException("not found a station with id = " + statioId);
            if (DataSource.listOfChargeSlot.Exists(cs => cs.StationId == statioId))
                throw new ObjectNotAvailableForActionException("Exist drone in station" + statioId);
            Station station = DataSource.StationsArr[i];
            station.IsDelete = true;
            DataSource.StationsArr[i] = station;
        }


        /// <summary>
        /// view the choosen station
        /// </summary>
        /// <param name="id">the station id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int id)
        {
            int i = DataSource.StationsArr.FindIndex((s) => s.Id == id);
            if (i < 0)
                throw new ObjectNotExistException("not found a station with id = " + id);

            return DataSource.StationsArr[i];
        }


        /// <summary>
        /// return StationList
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> StationList(Predicate<bool> selectList = default)
            => DataSource.StationsArr.Where((c) => !c.IsDelete && (selectList == null || selectList(isEmptyChargeSlotInStation(c.Id, c.ChargeSlot))));


        /// <summary>
        /// Get Station Id Of DroneCharge
        /// </summary>
        /// <param name="droneId">droneId</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetStationIdOfDroneCharge(int droneId) => DataSource.listOfChargeSlot.Find(dc => dc.DroneId == droneId).StationId;


        /// <summary>
        /// Add Drone Charge
        /// </summary>
        /// <param name="droneId">droneId</param>
        /// <param name="StationId">StationId</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(int droneId, int StationId)
        {
            if (DataSource.listOfChargeSlot.Exists(d => d.DroneId == droneId))
                throw new ObjectAlreadyExistException("This drone is already being charged");
            DataSource.listOfChargeSlot.Add(
                new DroneCharge()
                {
                    DroneId = droneId,
                    StationId = StationId,
                    StartTime = DateTime.Now
                });
        }


        /// <summary>
        /// Delete Drone Charge
        /// </summary>
        /// <param name="droneId">droneId</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDroneCharge(int droneId) =>
            DataSource.listOfChargeSlot.RemoveAll(d => d.DroneId == droneId);


        /// <summary>
        /// Station Drone In
        /// </summary>
        /// <param name="stationId">station Id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StationDroneIn(int stationId)
        {
            int i = DataSource.StationsArr.FindIndex(s => s.Id == stationId);
            if (i < 0)
                throw new ObjectNotExistException("Station not exist");
            Station station = DataSource.StationsArr[i];
            --station.ChargeSlot;
            DataSource.StationsArr[i] = station;
        }


        /// <summary>
        /// Station Drone out
        /// </summary>
        /// <param name="stationId">station Id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StationDroneOut(int StationId)
        {
            int i = DataSource.StationsArr.FindIndex(s => s.Id == StationId);
            if (i == -1)
                throw new ObjectNotExistException("station not exist");
            Station station = DataSource.StationsArr[i];
            ++station.ChargeSlot;
            DataSource.StationsArr[i] = station;
        }


        /// <summary>
        /// return if is empty charge slot in a station
        /// </summary>
        /// <param name="station">the choosen station</param>
        /// <returns></returns>
        private bool isEmptyChargeSlotInStation(int stationId, int stationChargeSlot)
        {
            int counter = 0;
            foreach (var ChargeSlot in DataSource.listOfChargeSlot)
            {
                if (ChargeSlot.StationId == stationId)
                {
                    ++counter;
                }
            }
            if (counter < stationChargeSlot)
            {
                return true;
            }
            return false;
        }
    }
}