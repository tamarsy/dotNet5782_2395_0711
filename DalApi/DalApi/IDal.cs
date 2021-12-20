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
        void AddStation(Station newStation);
        void AddDrone(Drone newDrone);

        void AddCustomer(Customer newCustomer);
        void AddParcel(Parcel newpParcel);


        void ParcelToDrone(int percelChoose, int droneChoose);
        void PickParcel(int percelChoose);
        void Destination(int percelChoose);
        void ChargeOn(int droenId, int stationId);
        void ChargeOf(int droenId);
        DateTime StartChargeTime(int droneId);


        Station GetStation(int id);
        Drone GetDrone(int id);
        Customer GetCustomer(int id);
        Parcel GetParcel(int id);


        void UpdateStation(Station id);
        void UpdateDrone(Drone id);
        void UpdateCustomer(Customer id);


        IEnumerable<Station> StationList(Predicate<bool> selectList = default);
        IEnumerable<Drone> DroneList();
        IEnumerable<Customer> CustomerList();
        IEnumerable<Parcel> ParcelList(Predicate<int?> selectList = default);


        double[] PowerConsumptionRequest();
    }
}

