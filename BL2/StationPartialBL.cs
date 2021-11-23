using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using DalObject;
using IBL.BO;

namespace IBL
{
    partial class BL
    {
        public void AddStation(int id, string name, Location location, int chargeSlots)
        {
            IDAL.DO.Station newStation = new IDAL.DO.Station()
            {
                Id = id,
                Name = name,
                Lattitude = location.Latitude,
                Longitude = location.Longitude,
                ChargeSlot = chargeSlots
            };
            try
            {
                dalObject.AddStation(newStation);
            }
            catch (DalObject.ObjectAlreadyExistException e)
            {
                throw new ObjectAlreadyExistException(e.Message);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public void UpdateStation(int id, string name, int numOfChargeSlot)
        {
            throw new NotImplementedException();
        }

        public Station GetStation(int requestedId)
        {
            IDAL.DO.Station station;
            try
            {
                station = dalObject.GetStation(requestedId);

            }
            catch (DalObject.ObjectNotExistException e)
            {
                throw new ObjectNotExistException(e.Message);
            }
            catch (Exception)
            {
                throw new Exception();
            }

            return DalToBlStation(station);
        }


        Station DalToBlStation(IDAL.DO.Station station)
        {
            Station newStation = new Station()
            {
                Id = station.Id
            };
            /*foreach (var item in dalObject.)
            {
                Drone drone = new Drone();
                station.DronesInCharge.Add();
            }*/
            return newStation;
        }

        public IEnumerable<StationToList> StationsList()
        {
            List<StationToList> stationList = new List<StationToList>();

            foreach (var s in dalObject.StationList())
            {
                //לשנות מספר תחנות ריקות מלאות
                int numOfChargeSlot = s.ChargeSlot;
                StationToList newStation = new StationToList(s.Id, s.Name, numOfChargeSlot, numOfChargeSlot);
                stationList.Add(newStation);
            }

            return stationList;
        }


        public IEnumerable<StationToList> EmptyChangeSlotlList()
        {
            List<StationToList> stationWithEmptyChangeSlotl = new List<StationToList>();

            foreach (var s in dalObject.EmptyChangeSlotlList())
            {
                //לשנות את מספר העמדות טעינה שיכילו את המספר הנכון
                int numOfAllChargeSlot = s.ChargeSlot;
                StationToList newStation = new StationToList(s.Id, s.Name, s.ChargeSlot, s.ChargeSlot);
                stationWithEmptyChangeSlotl.Add(newStation);
            }

            return stationWithEmptyChangeSlotl;
        }

    }
}
