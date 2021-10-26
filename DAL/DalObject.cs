using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        /// <summary>
        /// coter that play the Initialize in DataSoutce
        /// </summary>
        public DalObject()
        {
            DataSource.Config.Initialize();
        }
        /// <summary>
        /// the function add a new station to the arry
        /// </summary>
        /// <param name="station">the new station to add</param>
        public void AddStation(Station station)
        {
            /*if (DataSource.Config.stationIndex > DataSource.StationsArr.Count() - 1)
            {
                throw new ArgumentException("station's place are full! you can't add a new station!");
            }*/
            DataSource.StationsArr.Add(station);
        }
        /// <summary>
        /// the function add a new drone to the arry
        /// </summary>
        /// <param name="drone">the new drone to add</param>
        public void AddDrone(Drone drone)
        {
            /*if (DataSource.Config.stationIndex > DataSource.StationsArr.Count() - 1)
            {
                throw new ArgumentException("drone's place are full! you can't add a new drone!");
            }*/
            DataSource.DronesArr.Add(drone);
        }
        /// <summary>
        /// the function add a new customer to the arry
        /// </summary>
        /// <param name="customer">the new customer to add</param>
        public void AddCustomer(Customer customer)
        {
            /*if (DataSource.Config.stationIndex > DataSource.StationsArr.Count() - 1)
            {
                throw new ArgumentException("customer's place are full! you can't add a new customer!");
            }*/
            DataSource.CustomerArr.Add(customer);
        }
        /// <summary>
        /// the function add a new parcel to the arry
        /// </summary>
        /// <param name="parcel">the new parcel to add</param>
        public void AddParcel(Parcel parcel)
        {
            /*if (DataSource.Config.stationIndex > DataSource.StationsArr.Count() - 1)
            {
                throw new ArgumentException("parcel's place are full! you can't add a new parcel!");
            }*/
            DataSource.ParcelArr.Add(parcel);
        }
        /// <summary>
        /// the function contected beetwen parcel and drone
        /// </summary>
        /// <param name="percelChoose">the choosen percel</param>
        /// <param name="droneChoose">the drone percel</param>
        public void ParcelToDrone(int percelChoose, int droneChoose)
        {
            int i = 0;
            for (; i < DataSource.DronesArr.Count; i++)
            {
                if (DataSource.DronesArr[i].Id == droneChoose)
                {
                    break;
                }
            }
            if (i == DataSource.Config.droneIndex)
                throw new ArgumentException("Error!! Ther is no drone with this id");
            int j = 0;
            for (; j < DataSource.Config.parcelIndex; j++)
            {
                if (DataSource.ParcelArr[j].Id == percelChoose)
                {
                    if (DataSource.ParcelArr[j].Weight > DataSource.DronesArr[i].MaxWeight)
                        throw new ArgumentException("Error!! The drone cannot carry this weight");

                    if (DataSource.DronesArr[i].Status != DroneStatuses.vacant)
                        throw new ArgumentException("We can't send this drone");

                    DataSource.DronesArr[i].Status = DroneStatuses.sending;
                    DataSource.ParcelArr[j].Droneld = DataSource.DronesArr[i].Id;
                    DataSource.ParcelArr[j].Schedulet = DateTime.Now;
                    break;
                }
            }
            if (j == DataSource.Config.droneIndex)
                throw new ArgumentException("Error!! Ther is no drone with this id");
        }
        /// <summary>
        /// the function pick the percel from the dron
        /// </summary>
        /// <param name="percelChoose">the choosen percel</param>
        public void PickParcel(int percelChoose)
        {
            int i = 0;
            for (; i < DataSource.Config.parcelIndex; i++)
            {
                if (DataSource.ParcelArr[i].Id == percelChoose)
                {
                    DataSource.ParcelArr[i].PickedUp = DateTime.Now;
                    break;
                }
            }
            if (i == DataSource.Config.droneIndex)
                throw new ArgumentException("Error!! Ther is no drone with this id");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="percelChoose"></param>
        public void Destination(int percelChoose)
        {
            int i = 0;
            for (; i < DataSource.Config.parcelIndex; i++)
            {
                if (DataSource.ParcelArr[i].Id == percelChoose)
                {
                    DataSource.ParcelArr[i].Delivered = DateTime.Now;
                    int j = 0;
                    for (; j < DataSource.Config.droneIndex; j++)
                    {
                        if (DataSource.DronesArr[j].Id == DataSource.ParcelArr[i].Droneld)
                            DataSource.DronesArr[j].Status = DroneStatuses.vacant;
                    }
                    break;
                }
            }
            if (i == DataSource.Config.droneIndex)
                throw new ArgumentException("Error!! Ther is no drone with this id");
        }
        /// <summary>
        /// return if is empty charge slot in a station
        /// </summary>
        /// <param name="station">the choosen station</param>
        /// <returns></returns>
        private bool isEmptyChargeSlotInStation(Station station)
        {
            int counter = 0;
            foreach (var ChargeSlot in DataSource.listOfChargeSlot)
            {
                if (ChargeSlot.StationId == station.Id)
                {
                    ++counter;
                }
            }
            if (counter < station.ChargeSlot)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// the function find a ststion with empty charge slot and charge the dron
        /// </summary>
        /// <param name="droenId">the dron id</param>
        public void ChargeOn(int droenId)
        {
            for (int i = 0; i < DataSource.DronesArr.Count; ++i)
            {
                if (droenId == DataSource.DronesArr[i].Id)
                {
                    if (DataSource.DronesArr[i].Status == DroneStatuses.sending)
                    {
                        throw new ArgumentException("cant charge on this drone is sending");
                    }
                    foreach (var item in DataSource.StationsArr)
                    {
                        if (isEmptyChargeSlotInStation(item))
                        {
                            DroneCharge d = new DroneCharge(droenId, DataSource.StationsArr[i].Id);
                            DataSource.DronesArr[i] = new Drone(DataSource.DronesArr[i].Id, DataSource.DronesArr[i].Model, DataSource.DronesArr[i].MaxWeight,
                                DroneStatuses.maintanance, 99);
                            return;
                        }
                    }
                    throw new ArgumentException("no station with empty charge slot");
                }
            }
            throw new ArgumentException("no drone whith id: " + droenId);
        }
        /// <summary>
        /// the function charge of the dron from the charge slot
        /// </summary>
        /// <param name="droenId">the dron id</param>
        public void ChargeOf(int droenId)
        {
            for (int i = 0; i < DataSource.DronesArr.Count(); ++i)
            {
                if (droenId == DataSource.DronesArr[i].Id)
                {
                    if (DataSource.DronesArr[i].Status != DroneStatuses.maintanance)
                    {
                        throw new ArgumentException("cant charge of this drone is not charging now");
                    }
                    DataSource.DronesArr[i] = new Drone(DataSource.DronesArr[i].Id, DataSource.DronesArr[i].Model, DataSource.DronesArr[i].MaxWeight,
                        DroneStatuses.vacant, DataSource.DronesArr[i].Battery);
                    //remove from the list Of Charge Slot in DataSource
                    for (int j = 0; j < DataSource.listOfChargeSlot.Count(); ++j)
                    {
                        if (DataSource.listOfChargeSlot[j].DroneId == DataSource.DronesArr[i].Id)
                        {
                            DataSource.listOfChargeSlot.RemoveAt(j);
                        }
                    }
                }
            }
            throw new ArgumentException("no drone whith id: " + droenId + "in charge slot");
        }
        /// <summary>
        /// view the choosen station
        /// </summary>
        /// <param name="id">the station id</param>
        /// <returns></returns>
        public Station ViewStation(int id)
        {
            foreach (var item in DataSource.StationsArr)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            throw new ArgumentException("not found a station with id = " + id);
        }
        /// <summary>
        /// view the choosen drone
        /// </summary>
        /// <param name="id">the drone id</param>
        /// <returns></returns>
        public Drone ViewDrone(int id)
        {
            foreach (var item in DataSource.DronesArr)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            throw new ArgumentException("not found a drone with id = " + id);
        }
        /// <summary>
        /// view the choosen customer
        /// </summary>
        /// <param name="id">the customer id</param>
        /// <returns></returns>
        public Customer ViewCustomer(int id)
        {
            foreach (var item in DataSource.CustomerArr)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            throw new ArgumentException("not found a customer with id = " + id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">the parcel id</param>
        /// <returns></returns>
        public Parcel ViewParcel(int id)
        {
            foreach (var item in DataSource.ParcelArr)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            throw new ArgumentException("not found a parcel with id = " + id);
        }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Station> StationList()
        {
            List<Station> StationList = new List<Station>();
            foreach (var item in DataSource.StationsArr)
            {
                StationList.Add(item);
            }
            return StationList;
        }
        public IEnumerable<Drone> DroneList()
        {
            List<Drone> DroneList = new List<Drone>();
            foreach (var item in DataSource.DronesArr)
            {
                DroneList.Add(item);
            }
            return DroneList;
        }
        public IEnumerable<Customer> CustomerList()
        {
            List<Customer> CustomerList = new List<Customer>();
            foreach (var item in DataSource.CustomerArr)
            {
                CustomerList.Add(item);
            }
            return CustomerList;
        }
        public IEnumerable<Parcel> ParcelList()
        {
            List<Parcel> ParcelList = new List<Parcel>();
            foreach (var item in DataSource.ParcelArr)
            {
                ParcelList.Add(item);
            }
            return ParcelList;
        }
        public IEnumerable<Parcel> ParcesWithoutDronelList()
        {
            List<Parcel> ParcesWithoutDronelList = new List<Parcel>();
            foreach (var item in DataSource.ParcelArr)
            {
                if (item.Droneld != -1)
                {
                    ParcesWithoutDronelList.Add(item);
                }
            }
            return ParcesWithoutDronelList;
        }
        public IEnumerable<Station> EmptyChangeSlotlList()
        {
            List<Station> stationWithEmptyChargeSlot = new List<Station>();
            foreach (var item in DataSource.StationsArr)
            {
                if (isEmptyChargeSlotInStation(item))
                {
                    stationWithEmptyChargeSlot.Add(item);
                }
            }
            return stationWithEmptyChargeSlot;
        }
    }
}