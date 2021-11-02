using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject : IDal.IDal
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
        public void AddStation(Station newStation)
        {
            if (DataSource.StationsArr.Exists(station => station.Id == newStation.Id))
            {
                throw new ArgumentException("Can't add, There is already a station with this ID");
            }
            DataSource.StationsArr.Add(newStation);
        }
        /// <summary>
        /// the function add a new drone to the arry
        /// </summary>
        /// <param name="drone">the new drone to add</param>
        public void AddDrone(Drone newDrone)
        {
            if (DataSource.DronesArr.Exists(drone => drone.Id == newDrone.Id))
            {
                throw new ArgumentException("Can't add, There is already a drone with this ID");
            }
            DataSource.DronesArr.Add(newDrone);
        }
        /// <summary>
        /// the function add a new customer to the arry
        /// </summary>
        /// <param name="customer">the new customer to add</param>
        public void AddCustomer(Customer newCustomer)
        {
            if (DataSource.CustomerArr.Exists(drone => drone.Id == newCustomer.Id))
            {
                throw new ArgumentException("Can't add, There is already a customer with this ID");
            }
            DataSource.CustomerArr.Add(newCustomer);
        }
        /// <summary>
        /// the function add a new parcel to the arry
        /// </summary>
        /// <param name="parcel">the new parcel to add</param>
        public void AddParcel(Parcel newpParcel)
        {
            if (DataSource.CustomerArr.Exists(drone => drone.Id == newpParcel.Id))
            {
                throw new ArgumentException("Can't add, There is already a parcel with this ID");
            }
            DataSource.ParcelArr.Add(newpParcel);
        }
        /// <summary>
        /// the function contected beetwen parcel and drone
        /// </summary>
        /// <param name="percelChoose">the choosen percel</param>
        /// <param name="droneChoose">the drone percel</param>
        public void ParcelToDrone(int percelChoose, int droneChoose)
        {
            WeightCategories itemDroneWeight = 0;
            DroneStatuses itemDroneStatus = 0;
            bool check = false;
            foreach (var item in DataSource.DronesArr)
            {
                if (item.Id == droneChoose)
                {
                    check = true;
                    itemDroneWeight = item.MaxWeight;
                    itemDroneStatus = item.Status;
                    break;
                }
            }
            if (check == false)
                throw new ArgumentException("Error!! Ther is no drone with this id");

            int itemParcelDrone = 0;
            DateTime itemParcelSchedulet;
            check = false;
            foreach (var item in DataSource.ParcelArr)
            {
                if (item.Id == percelChoose)
                {
                    check = true;
                    itemParcelDrone = item.Droneld;
                    itemParcelSchedulet = item.Schedulet;
                    if (item.Weight > itemDroneWeight)
                        throw new ArgumentException("Error!! The drone cannot carry this weight");

                    if (itemDroneStatus != DroneStatuses.vacant)
                        throw new ArgumentException("We can't send this drone");

                    itemDroneStatus = DroneStatuses.sending;
                    itemParcelDrone = item.Id;
                    itemParcelSchedulet = DateTime.Now;
                    break;
                }
            }
            if (check == false)
                throw new ArgumentException("Error!! Ther is no percel with this id");
        }
        /// <summary>
        /// the function pick the percel from the dron
        /// </summary>
        /// <param name="percelChoose">the choosen percel</param>
        public void PickParcel(int percelChoose)
        {
            /*int i = 0;
            for (; i < DataSource.ParcelArr.Count; i++)
            {
                if (DataSource.ParcelArr[i].Id == percelChoose)
                {
                    //itemParcelPickedUp = DataSource.ParcelArr[i].PickedUp;
                    //itemParcelPickedUp = DataSource.ParcelArr[i].PickedUp;
                    DataSource.ParcelArr[i] = new Parcel(DataSource.ParcelArr[i].Id, DataSource.ParcelArr[i].SenderId,
                    DataSource.ParcelArr[i].TargilId, DataSource.ParcelArr[i].Weight, DataSource.ParcelArr[i].Priority,
                    DataSource.ParcelArr[i].ReQuested, DataSource.ParcelArr[i].Droneld, DataSource.ParcelArr[i].Schedulet,
                    DataSource.ParcelArr[i].PickedUp=DateTime.Now, DataSource.ParcelArr[i].Delivered);
                    break;
                    break;
                }
            }
            if (i == DataSource.Config.droneIndex)
                throw new ArgumentException("Error!! Ther is no drone with this id");*/
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="percelChoose"></param>
        public void Destination(int percelChoose)
        {
            bool check = false;
            for (int i = 0; i < DataSource.ParcelArr.Count; ++i)
            {
                if (DataSource.ParcelArr[i].Id == percelChoose)
                {
                    check = true;
                    DataSource.ParcelArr[i] = new Parcel(DataSource.ParcelArr[i].Id, DataSource.ParcelArr[i].SenderId, DataSource.ParcelArr[i].TargilId,
                        DataSource.ParcelArr[i].Weight, DataSource.ParcelArr[i].Priority, DataSource.ParcelArr[i].ReQuested, DataSource.ParcelArr[i].Droneld,
                        DataSource.ParcelArr[i].Schedulet, DataSource.ParcelArr[i].PickedUp, DateTime.Now);

                    for (int j = 0; j < DataSource.DronesArr.Count; ++j)
                    {
                        if (DataSource.DronesArr[j].Id == DataSource.ParcelArr[i].Droneld)
                            DataSource.DronesArr[j] = new Drone(DataSource.DronesArr[j].Id, DataSource.DronesArr[j].Model, DataSource.DronesArr[j].MaxWeight
                                , DroneStatuses.vacant, DataSource.DronesArr[j].Battery);
                    }
                    break;
                }
            }
            if (check == false)
                throw new ArgumentException("Error!! Ther is no drone with this id");
        }
        /// <summary>
        /// return if is empty charge slot in a station
        /// </summary>
        /// <param name="station">the choosen station</param>
        /// <returns></returns>
        private bool isEmptyChargeSlotInStation(int stationId, int stationChargeSlot)
        {
            int counter = 0;
            foreach (var ChargeSlot in DataSource.listOfChargeSlot)
            {
                if (ChargeSlot.StationId == stationId)
                {
                    ++counter;
                }
            }
            if (counter < stationChargeSlot)
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
                        if (isEmptyChargeSlotInStation(item.Id, item.ChargeSlot))
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
                if (isEmptyChargeSlotInStation(item.Id, item.ChargeSlot))
                {
                    stationWithEmptyChargeSlot.Add(item);
                }
            }
            return stationWithEmptyChargeSlot;
        }
        /// <summary>
        /// the function return an 5 doubls:
        /// 1 vacent
        /// 2 LightWeightCarrier
        /// 3 MediumWeightCarrier
        /// 4 heavyWeightCarrier
        /// 5 SkimmerLoadingRate
        /// </summary>
        /// <returns></returns>
        public double[] PowerConsumptionRequest()
        {
            return (new double[5]
            {
                DataSource.Config.vacent,
                DataSource.Config.LightWeightCarrier,
                DataSource.Config.MediumWeightCarrier,
                DataSource.Config.heavyWeightCarrier,
                DataSource.Config.SkimmerLoadingRate
            });
        }
    }
}