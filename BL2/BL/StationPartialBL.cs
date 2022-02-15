using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// add new ststion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <param name="chargeSlots"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(int id, string name, Location location, int chargeSlots)
        {
            DO.Station newStation = new DO.Station()
            {
                Id = id,
                Name = name,
                Lattitude = location.Latitude,
                Longitude = location.Longitude,
                ChargeSlot = chargeSlots
            };
            try
            {
                lock (dalObject)
                {
                    dalObject.AddStation(newStation);
                }
            }
            catch (DO.ObjectAlreadyExistException e)
            {
                throw new ObjectAlreadyExistException(e.Message);
            }
        }

        /// <summary>
        /// update a station after change that done.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="numOfChargeSlot"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(int id, string name, int numOfChargeSlot)
        {
            DO.Station station;
            try
            {
                lock (dalObject)
                {
                    station = dalObject.GetStation(id);
                }
            }
            catch (DO.ObjectNotExistException e)
            {
                throw new ObjectNotExistException(e.Message);
            }
            if (name == default && numOfChargeSlot == default)
            {
                throw new NoChangesToUpdateException("No Changes To Update");
            }
            if (numOfChargeSlot != default)
                station.ChargeSlot = numOfChargeSlot;
            if (name != default)
                station.Name = name;
            lock (dalObject)
            {
                dalObject.UpdateStation(station);
            }
        }

        /// <summary>
        /// function that get station
        /// </summary>
        /// <param name="requestedId"></param>
        /// <returns>DalToBlStation</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int requestedId)
        {
            DO.Station station;
            try
            {
                lock (dalObject) { station = dalObject.GetStation(requestedId); }

            }
            catch (DO.ObjectNotExistException e)
            {
                throw new ObjectNotExistException(e.Message);
            }
            if (station.IsDelete)
                throw new ObjectNotExistException($"Station with id {requestedId}");
            return DalToBlStation(station);
        }







        /// <summary>
        /// return Stations List
        /// </summary>
        /// <returns>stationList</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> StationsList()
        {
            List<StationToList> stationList = new List<StationToList>();
            lock (dalObject)
            {
                foreach (DO.Station s in dalObject.StationList())
                {
                    stationList.Add(ConvertToStationToList(s));
                }

                return stationList;
            }
        }


        /// <summary>
        /// function that create a list of all the empty change slot ststion list
        /// </summary>
        /// <returns>stationWithEmptyChangeSlotl</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> EmptyChangeSlotlList()
        {
            List<StationToList> stationWithEmptyChangeSlotl = new List<StationToList>();
            lock (dalObject)
            {
                foreach (var s in dalObject.StationList((bool b) => b))
                {
                    stationWithEmptyChangeSlotl.Add(ConvertToStationToList(s));
                }

                return stationWithEmptyChangeSlotl;
            }
        }


        /// <summary>
        /// Delete Station
        /// </summary>
        /// <param name="id">station id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int id) { lock (dalObject) { dalObject.DeleteStation(id); } }



    }
}
