using System;
using System.Collections.Generic;
using System.Text;
using BO;

namespace BlApi
{
    public interface IBL
    {
        void AddStation(int id, string name, Location location, int chargeSlots);
        void AddDrone(int id, string model, WeightCategories maxWeight, int stationId);
        void AddCustomer(int id, string name, string phone, Location location);
        void AddParcel(int sid, int tid, WeightCategories weigth, Priorities priority);


        void UpdateStation(int stationId, string name, int numOfChargeSlot);
        void UpdateDrone(int droneId, string model);
        void UpdateCusomer(int customerId, string name = default, string phone = default);
        void ChargeOn(int droneId);
        void ChargeOf(int droneId);


        void ParcelToDrone(int Droneid);
        void PickParcel(int Droneid);
        void Destination(int Droneid);


        void DeleteStation(int id);
        void DeleteDrone(int id);
        void DeleteCustomer(int id);
        void DeleteParcel(int id);



        Station GetStation(int requestedId);
        Drone GetDrone(int requestedId);
        Customer GetCustomer(int requestedId);
        BO.Parcel GetParcel(int requestedId);


        IEnumerable<BO.StationToList> StationsList();
        IEnumerable<BO.DroneToList> DronesList();
        IEnumerable<CustomerToList> CustomersList();
        IEnumerable<ParcelToList> ParcelsList();
        IEnumerable<StationToList> EmptyChangeSlotlList();
        IEnumerable<ParcelToList> ParcesWithoutDronelList();
    }
}

