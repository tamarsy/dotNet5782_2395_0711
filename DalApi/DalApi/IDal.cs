using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface IDal
    {
        #region Add
        void AddStation(Station newStation);
        void AddDrone(Drone newDrone);
        void AddCustomer(Customer newCustomer);
        void AddParcel(Parcel newpParcel);
        #endregion
        #region DroneAndParcel
        void ParcelToDrone(int percelChoose, int droneChoose);
        void PickParcel(int percelChoose);
        void Destination(int percelChoose);
        DateTime StartChargeTime(int droneId);
        #endregion
        #region Get
        Station GetStation(int id);
        Drone GetDrone(int id);
        Customer GetCustomer(int id);
        Parcel GetParcel(int id);
        int GetStationIdOfDroneCharge(int droneId);
        #endregion
        #region Update
        void UpdateStation(Station id);
        void UpdateDrone(Drone id);
        void UpdateCustomer(Customer id);
        #endregion
        #region Delete
        void DeleteStation(int id);
        void DeleteDrone(int id);
        void DeleteCustomer(int id);
        void DeleteParcel(int id);
        #endregion
        #region GetIEnumerable
        IEnumerable<Station> StationList(Predicate<bool> selectList = default);
        IEnumerable<Drone> DroneList();
        IEnumerable<Customer> CustomerList();
        IEnumerable<Parcel> ParcelList(Predicate<int?> selectList = default);
        #endregion
        #region Charging
        void DeleteDroneCharge(int droneId);
        void AddDroneCharge(int droneId, int baseStationId);
        void StationDroneIn(int baseStationId);
        void StationDroneOut(int baseStationId);
        void ChargeOn(int droenId, int stationId);
        void ChargeOf(int droenId);
        #endregion
        double[] PowerConsumptionRequest();
    }
}

