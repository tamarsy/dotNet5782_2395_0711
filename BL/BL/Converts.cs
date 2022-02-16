using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL
{
    /// <summary>
    /// convert types in bl
    /// </summary>
    partial class BL
    {
        #region DlToBl

        /// <summary>
        /// Convert from drone i dal to drone in bl
        /// </summary>
        /// <param name="drone">do drone</param>
        /// <returns>Drone</returns>
        internal Drone DlToBlDrone(DO.Drone drone)
        {
            DroneToList droneFromList = drones.Find(d => d.Id == drone.Id);
            DO.Parcel parcel = default;
            if (droneFromList.NumOfParcel != default)
                parcel = dalObject.GetParcel((int)droneFromList.NumOfParcel);
            return new Drone()
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (WeightCategories)drone.MaxWeight,
                CurrentLocation = droneFromList.CurrentLocation,
                BatteryStatuses = droneFromList.BatteryStatuses,
                DroneStatuses = droneFromList.DroneStatuses,
                Parcel = droneFromList.NumOfParcel == default ? default : ParcelToParcelDelivery(parcel, droneFromList)
            };
        }


        /// <summary>
        /// the function change the "Idal" details to "bl" details
        /// </summary>
        /// <param name="parcel">parcel</param>
        /// <returns>Parecel</returns>
        internal Parcel DlToBlParcel(DO.Parcel parcel)
        {
            Drone drone = (parcel.Droneld == default) ? default : GetDrone((int)parcel.Droneld);
            return new Parcel()
            {
                Id = parcel.Id,
                AssignmentTime = parcel.Requested,
                DeliveryTime = parcel.Delivered,
                DroneDelivery = drone == null ? default : new DroneDelivery() { Id = drone.Id, BatteryStatuses = drone.BatteryStatuses, CurrentLocation = drone.CurrentLocation },
                PickUpTime = parcel.PickedUp,
                Priority = (Priorities)parcel.Priority,
                SenderId = new DeliveryCustomer() { Id = parcel.SenderId, Name = GetCustomer(parcel.SenderId).Name },
                GetterId = new DeliveryCustomer() { Id = parcel.GetterId, Name = GetCustomer(parcel.GetterId).Name },
                SupplyTime = parcel.Schedulet,
                Weight = (WeightCategories)parcel.Weight
            };
        }


        /// <summary>
        /// change the things in dal to bl
        /// </summary>
        /// <param name="station"></param>
        /// <returns>newStation</returns>
        internal Station DalToBlStation(DO.Station station)
        {
            Station newStation = new Station()
            {
                Id = station.Id,
                ChargeSlot = station.ChargeSlot,
                CurrentLocation = new Location { Latitude = station.Lattitude, Longitude = station.Longitude },
                Name = station.Name,
                DronesInCharge = (from s in drones
                                  let lo = station.Longitude
                                  let la = station.Lattitude
                                  where s.DroneStatuses == DroneStatuses.maintanance && s.CurrentLocation.Longitude.Equals(lo) && s.CurrentLocation.Latitude.Equals(la)
                                  select DroneToListToDroneCharge(s)).ToList()
            };
            return newStation;
        }
        #endregion
        #region ListToNot

        /// <summary>
        /// Exceptions: ObjectNotExistException
        /// convert from DroneToList to drone 
        /// </summary>
        /// <param name="drone"></param>
        /// <returns>Drone</returns>
        internal Drone DroneToListToDrone(DroneToList drone)
        {
            DO.Parcel? parcel = (drone.NumOfParcel == null) ? null : (DO.Parcel?)dalObject.GetParcel((int)drone.NumOfParcel);

            return new Drone()
            {
                Id = drone.Id,
                BatteryStatuses = drone.BatteryStatuses,
                CurrentLocation = drone.CurrentLocation,
                DroneStatuses = drone.DroneStatuses,
                MaxWeight = drone.MaxWeight,
                Model = drone.Model,
                Parcel = parcel == null ? null : ParcelToParcelDelivery((DO.Parcel)parcel, drone)
            };
        }


        /// <summary>
        /// DroneToDroneToList
        /// Exception: ObjectNotExistException
        /// </summary>
        /// <param name="drone">do drone</param>
        /// <param name="stationId">station Id</param>
        /// <returns>DroneToList</returns>
        internal DroneToList DroneToDroneToList(DO.Drone drone, int stationId)
        {
            lock (dalObject)
            {
                try
                {
                    DO.Station station = dalObject.GetStation(stationId);
                    return new DroneToList()
                    {
                        Id = drone.Id,
                        Model = drone.Model,
                        MaxWeight = (WeightCategories)drone.MaxWeight,
                        BatteryStatuses = (rand.NextDouble() * 20) + 20,
                        CurrentLocation = new Location(station.Lattitude, station.Longitude),
                        DroneStatuses = DroneStatuses.maintanance
                    };
                }
                catch (DO.ObjectNotExistException e) { throw new ObjectNotExistException(e.Message); }
            }
        }


        /// <summary>
        /// change bl parcel to list 
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="parcelStatuses"></param>
        /// <returns>ParcelToList</returns>
        internal ParcelToList DlToBlParcelToList(DO.Parcel parcel, ParcelStatuses parcelStatuses = default)
        {
            return new ParcelToList()
            {
                Id = parcel.Id,
                SenderId = parcel.SenderId,
                GetterId = parcel.GetterId,
                Weight = (WeightCategories)parcel.Weight,
                Priority = (Priorities)parcel.Priority,
                ParcelStatuses = parcelStatuses != default ? ParcelStatuses.defined : FindParcelStatuses(parcel)
            };
        }


        /// <summary>
        /// Convert To StationToList
        /// </summary>
        /// <param name="station">station</param>
        /// <returns></returns>
        internal StationToList ConvertToStationToList(DO.Station station)
        {
            int NumOfCatchChargeSlots = drones.Count(d => d.DroneStatuses == DroneStatuses.maintanance && d.CurrentLocation.Latitude == station.Lattitude && d.CurrentLocation.Longitude == station.Longitude);
            return new StationToList()
            {
                Id = station.Id,
                Name = station.Name,
                NumOfCatchChargeSlots = NumOfCatchChargeSlots,
                NumOfEmptyChargeSlots = station.ChargeSlot - NumOfCatchChargeSlots
            };
        }
        #endregion

        /// <summary>
        /// Exceptions: ObjectNotExistException
        /// creat a new CustomerDelivery by the customer and parcel 
        /// </summary>
        /// <param name="parcel">the parcel</param>
        /// <param name="id">the customer id</param>
        /// <returns>CustomerDelivery</returns>
        internal CustomerDelivery CustomerAndParcelToCustomerDelivery(int parcelId, int customerId)
        {

            ParcelToList parcel = DlToBlParcelToList(dalObject.GetParcel(parcelId));
            DO.Customer geterCustomer = dalObject.GetCustomer(customerId);

            return new CustomerDelivery()
            {
                Id = parcel.Id,
                Weight = parcel.Weight,
                Priority = parcel.Priority,
                Status = parcel.ParcelStatuses,
                Customer = new DeliveryCustomer()
                {
                    Id = geterCustomer.Id,
                    Name = geterCustomer.Name
                }
            };
        }


        /// <summary>
        /// Convert Parcel To ParcelDelivery
        /// </summary>
        /// <param name="parcel">parcel</param>
        /// <param name="droneLocation">drone Location</param>
        /// <returns></returns>
        internal ParcelDelivery ParcelToParcelDelivery(DO.Parcel parcel, Ilocatable droneLocation)
        {
            Customer sender;
            Customer getter;
            sender = GetCustomer(parcel.SenderId);
            getter = GetCustomer(parcel.GetterId);
            return new ParcelDelivery()
            {
                Id = parcel.Id,
                Weight = (WeightCategories)parcel.Weight,
                Priority = (Priorities)parcel.Priority,
                StatusParcel = !parcel.PickedUp.Equals(null),
                Collecting = sender.CurrentLocation,
                DeliveryDestination = getter.CurrentLocation,
                Distance = droneLocation.Distance(!parcel.PickedUp.Equals(null) ? getter : sender),
                Sender = new DeliveryCustomer() { Id = sender.Id, Name = sender.Name },
                Getter = new DeliveryCustomer() { Id = getter.Id, Name = getter.Name }
            };
        }


        /// <summary>
        /// convert from DroneToList to DroneCharge 
        /// </summary>
        /// <param name="drone">drone</param>
        /// <returns>Drone Charge</returns>
        internal DroneCharge DroneToListToDroneCharge(DroneToList drone)
        {
            return new DroneCharge()
            {
                Id = drone.Id,
                BatteryStatuses = drone.BatteryStatuses
            };
        }
    }
}
