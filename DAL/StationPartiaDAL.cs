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
        /// the function add a new station to the arry
        /// </summary>
        /// <param name="station">the new station to add</param>
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
        public Station GetStation(int id)
        {
            foreach (var item in DataSource.StationsArr)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            throw new ObjectNotExistException("not found a station with id = " + id);
        }


        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Station> StationList(Predicate<bool> selectList = default)
            => DataSource.StationsArr.Where((c) => selectList != null ? selectList(isEmptyChargeSlotInStation(c.Id, c.ChargeSlot)) : true);


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
        public void DeleteStation(int Id)
        {
            int i = DataSource.StationsArr.FindIndex(s => s.Id == Id);
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
    }
}
