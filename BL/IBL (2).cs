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



        //void PickedupParcel(int parcelId);
        //void SendDroneToRecharge(int droneId, int baseStationId);
        //void ReleaseDroneFromRecharge(int droneId);
        Station GetStation(int requestedId);
        Drone GetDrone(int requestedId);
        Customer GetCustomer(int requestedId);
        BO.Parcel GetParcel(int requestedId);
        IEnumerable<BO.StationToList> GetStations();
        IEnumerable<DroneToList> GetDrones();
        IEnumerable<Customer> GetCustomers();
        IEnumerable<Parcel> GetParcels();
        //IEnumerable<Parcel> UnAssignmentParcels();
        //IEnumerable<Station> AvailableChargingStations();
    }
}


