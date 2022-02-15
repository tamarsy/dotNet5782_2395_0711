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
        /// <param name="station"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int Id)
        {
            int i = DataSource.StationsArr.FindIndex(s => s.Id == Id && !s.IsDelete);
            if (i < 0)
                throw new ObjectNotExistException("not found a station with id = " + Id);
            DataSource.StationsArr[i] = new Station()
            {
                Id = DataSource.StationsArr[i].Id,
                Name = DataSource.StationsArr[i].Name,
                Lattitude = DataSource.StationsArr[i].Lattitude,
                Longitude = DataSource.StationsArr[i].Longitude,
                ChargeSlot = DataSource.StationsArr[i].ChargeSlot,
                IsDelete = true
            };
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetStationIdOfDroneCharge(int droneId) => DataSource.listOfChargeSlot.Find(dc => dc.DroneId == droneId).StationId;


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
