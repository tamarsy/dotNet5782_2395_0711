using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        void AddStation(int id, string name, Location location, int chargeSlots);
        void AddDrone(int id, string model, WeightCategories maxWeight, int stationId);
        void AddCustomer(int id, string name, string phone, Location location);
        void AddParcel(int sid, int tid, WeightCategories weigth, Priorities priority);


        void UpdateStation(int id, string name, int numOfChargeSlot);
        void UpdateDrone(int id, string model);
        void UpdateCusomer(int id, string name, string phone);
        void ChargeOn(int id);
        void ChargeOf(int id, float timeInCharge);
        void ParcelToDrone(int id);
        void PickParcel(int id);
        void Destination(int id);



        Station ViewStation(int requestedId);
        Drone ViewDrone(int requestedId);
        Customer ViewCustomer(int requestedId);
        BO.Parcel ViewParcel(int requestedId);
        IEnumerable<BO.StationToList> StationsList();
        IEnumerable<BO.DroneToList> DronesList();
        IEnumerable<CustomerToList> CustomersList();
        IEnumerable<ParcelToList> ParcelsList();
        IEnumerable<StationToList> EmptyChangeSlotlList();
        IEnumerable<ParcelToList> ParcesWithoutDronelList();
    }
}

