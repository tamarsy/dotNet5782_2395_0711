using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
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
            throw new ArgumentException("not found a station with id = " + id);
        }


        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Station> StationList()
        {
            List<Station> StationList = new List<Station>();
            foreach (var item in DataSource.StationsArr)
            {
                StationList.Add(item);
            }
            return StationList;
        }



        public IEnumerable<Station> EmptyChangeSlotlList()
        {
            List<Station> stationWithEmptyChargeSlot = new List<Station>();
            foreach (var item in DataSource.StationsArr)
            {
                if (isEmptyChargeSlotInStation(item.Id, item.ChargeSlot))
                {
                    stationWithEmptyChargeSlot.Add(item);
                }
            }
            return stationWithEmptyChargeSlot;
        }
    }
}
