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
        /// <summary>
        /// add new ststion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <param name="chargeSlots"></param>
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

        /// <summary>
        /// update a station after change that done.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="numOfChargeSlot"></param>
        public void UpdateStation(int id, string name, int numOfChargeSlot)
        {
            IDAL.DO.Station station;
            try
            {
                station = dalObject.GetStation(id);
            }
            catch (DalObject.ObjectNotExistException e)
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
            dalObject.UpdateStation(station);
        }

        /// <summary>
        /// function that get station
        /// </summary>
        /// <param name="requestedId"></param>
        /// <returns>DalToBlStation</returns>
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

        /// <summary>
        /// change the things in dal to bl
        /// </summary>
        /// <param name="station"></param>
        /// <returns>newStation</returns>
        Station DalToBlStation(IDAL.DO.Station station)
        {
            Station newStation = new Station()
            {
                Id = station.Id,
                ChargeSlot = station.ChargeSlot,
                CurrentLocation = new Location { Latitude = station.Lattitude, Longitude = station.Longitude },
                Name = station.Name,
                DronesInCharge = drones.Where(d => d.DroneStatuses == DroneStatuses.maintanance && d.CurrentLocation == new Location { Latitude = station.Lattitude, Longitude = station.Longitude })
                .Select(d => DroneToListToDrone(d)).ToList()
            };
            return newStation;
        }

        /// <summary>
        /// function that create a list of drones to the drone 
        /// </summary>
        /// <param name="drone"></param>
        /// <returns>Drone</returns>
        private Drone DroneToListToDrone(DroneToList drone)
        {
            Parcel parcel = GetParcel((int)drone.NumOfParcel);
            Customer CustomerSet = GetCustomer(parcel.SenderId.Id);
            Customer CustomerGet = GetCustomer(parcel.GetterId.Id);

            return new Drone()
            { 
                Id = drone.Id, 
                BatteryStatuses= drone.BatteryStatuses, 
                CurrentLocation= drone.CurrentLocation, 
                DroneStatuses= drone.DroneStatuses, 
                MaxWeight= drone.MaxWeight, 
                Model= drone.Model, 
                Parcel= new ParcelDelivery()
                {
                    Id = parcel.Id,
                    Weight = parcel.Weight,
                    Priority = parcel.Priority,
                    StatusParcel = !parcel.PickUpTime.Equals(default),
                    Collecting = CustomerSet.CurrentLocation,
                    DeliveryDestination = CustomerGet.CurrentLocation,
                    Distance = CustomerSet.CurrentLocation.Distance(CustomerGet),
                    SenderId = parcel.SenderId,
                    GetterId = parcel.GetterId
                }
            };
        }
        /// <summary>
        /// function that create list of slot stations
        /// </summary>
        /// <returns>stationList</returns>
        public IEnumerable<StationToList> StationsList()
        {
            List<StationToList> stationList = new List<StationToList>();
            foreach (var s in dalObject.StationList())
            {
                int NumOfCatchChargeSlots = drones.Count(d => d.DroneStatuses == DroneStatuses.maintanance && d.CurrentLocation == new Location(s.Lattitude, s.Longitude));

                int numOfChargeSlot = s.ChargeSlot;
                stationList.Add(new StationToList()
                {
                    Id = s.Id,
                    Name = s.Name,
                    NumOfCatchChargeSlots = NumOfCatchChargeSlots,
                    NumOfEmptyChargeSlots = s.ChargeSlot - NumOfCatchChargeSlots
                });
            }

            return stationList;
        }


        /// <summary>
        /// function that create a list of all the empty change slot ststion list
        /// </summary>
        /// <returns>stationWithEmptyChangeSlotl</returns>
        public IEnumerable<StationToList> EmptyChangeSlotlList()
        {
            List<StationToList> stationWithEmptyChangeSlotl = new List<StationToList>();

            foreach (var s in dalObject.EmptyChangeSlotlList())
            {
                int NumOfCatchChargeSlots = drones.Count(d => d.DroneStatuses == DroneStatuses.maintanance && d.CurrentLocation == new Location(s.Lattitude, s.Longitude));

                int numOfAllChargeSlot = s.ChargeSlot;
                stationWithEmptyChangeSlotl.Add(new StationToList()
                {
                    Id = s.Id,
                    Name = s.Name,
                    NumOfCatchChargeSlots = NumOfCatchChargeSlots,
                    NumOfEmptyChargeSlots = s.ChargeSlot - NumOfCatchChargeSlots
                });
            }

            return stationWithEmptyChangeSlotl;
        }

    }
}
