using System;
using System.Collections.Generic;
using System.Text;
using BO;

namespace BlApi
{
    public interface IBL
    {
        #region Add
        void AddStation(int id, string name, Location location, int chargeSlots);
        void AddDrone(int id, string model, WeightCategories maxWeight, int stationId);
        void AddCustomer(int id, string name, string phone, Location location);
        void AddParcel(int sid, int tid, WeightCategories weigth, Priorities priority);
        #endregion
        #region UpDate
        void UpdateStation(int stationId, string name, int numOfChargeSlot);
        void UpdateDrone(int droneId, string model);
        void UpdateCusomer(int customerId, string name = default, string phone = default);
        void ChargeOn(int droneId);
        void ChargeOf(int droneId);
        #endregion
        #region DroneAndParcel
        void ParcelToDrone(int Droneid);
        void PickParcel(int Droneid);
        void Destination(int Droneid);
        #endregion
        #region Delete
        void DeleteStation(int id);
        void DeleteDrone(int id);
        void DeleteCustomer(int id);
        void DeleteParcel(int id);
        #endregion
        #region Get
        Station GetStation(int requestedId);
        Drone GetDrone(int requestedId);
        Customer GetCustomer(int requestedId);
        BO.Parcel GetParcel(int requestedId);
        #endregion
        #region GetIEnumerable
        IEnumerable<BO.StationToList> StationsList();
        IEnumerable<BO.DroneToList> DronesList();
        IEnumerable<CustomerToList> CustomersList();
        IEnumerable<ParcelToList> ParcelsList();
        IEnumerable<StationToList> EmptyChangeSlotlList();
        IEnumerable<ParcelToList> ParcesWithoutDronelList();
        #endregion
        void StartSimulator(int id, Action update, Func<bool> checkStop);
    }
}

