using System;
using System.Collections.Generic;
using System.Text;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        public void AddStation(Station newStation);
        public void AddDrone(Drone newDrone);
        public void AddCustomer(Customer newCustomer);
        public void AddParcel(Parcel newpParcel);
        public void ParcelToDrone(int percelChoose, int droneChoose);
        public void PickParcel(int percelChoose);
        public void Destination(int percelChoose);
        public void ChargeOn(int droenId);
        public void ChargeOf(int droenId);
        public Station ViewStation(int id);
        public Drone ViewDrone(int id);
        public Customer ViewCustomer(int id);
        public Parcel ViewParcel(int id);
        public IEnumerable<Station> StationList();
        public IEnumerable<Drone> DroneList();
        public IEnumerable<Customer> CustomerList();
        public IEnumerable<Parcel> ParcelList();
        public IEnumerable<Parcel> ParcesWithoutDronelList();
        public IEnumerable<Station> EmptyChangeSlotlList();
        public double[] PowerConsumptionRequest();
    }
}


