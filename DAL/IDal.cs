using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IDAL.DO;

namespace IDal
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
        void ChargeOn(int droenId);
        void ChargeOf(int droenId);


        Station GetStation(int id);
        Drone GetDrone(int id);
        Customer GetCustomer(int id);
        Parcel GetParcel(int id);


        IEnumerable<Station> StationList();
        IEnumerable<Drone> DroneList();
        IEnumerable<Customer> CustomerList();
        IEnumerable<Parcel> ParcelList();
        IEnumerable<Parcel> ParcesWithoutDronelList();
        IEnumerable<Station> EmptyChangeSlotlList();


        double[] PowerConsumptionRequest();
    }
}

