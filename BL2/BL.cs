using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using DalObject;
using IBL.BO;

namespace IBL
{
    public partial class BL : IBL
    {
        private IDal.IDal dalObject;
        private double[] PowerConsumptionRequest;
        private List<DroneToList> drones;
        private static Random rand = new Random();
        public BL()
        {
            dalObject = new DalObject.DalObject();
            PowerConsumptionRequest = new double[dalObject.PowerConsumptionRequest().Length];
            int i = 0;
            foreach (var item in dalObject.PowerConsumptionRequest())
            {
                PowerConsumptionRequest[i++] = item;
            }

            drones = new List<DroneToList>();
            initializeDronesList();
        }

        private void initializeDronesList()
        {
            foreach (var drone in dalObject.DroneList())
            {
                DroneToList newDrone = new()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (WeightCategories)drone.MaxWeight
                };

                foreach (var parcel in dalObject.ParcelList())
                {
                    if (parcel.Droneld == drone.Id && parcel.Delivered != null)
                    {
                        newDrone.DroneStatuses = DroneStatuses.sending;
                        newDrone.NumOfParcel = parcel.Id;
                        newDrone.CurrentLocation = FindLocation(newDrone);
                        double distance = FindLocationOfCloseChargeSlot((Ilocatable)ViewCustomer(parcel.TargilId).CurrentLocation);
                        newDrone.BatteryStatuses = rand.Next(MinPowerForDistance(distance), 100);
                    }
                }
            }
        }

        private double FindLocationOfCloseChargeSlot(Ilocatable location)
        {
            double minDistance = double.MaxValue;
            dalObject.EmptyChangeSlotlList().Min<station=>{ FindLocationOfCloseChargeSlot}>

                customer => customer.NumParcelReceived > 0

            foreach (var station in dalObject.EmptyChangeSlotlList())
            {
                double newDistance = location.Distance((Ilocatable)new Location(station.Lattitude, station.Longitude));
                if (newDistance < minDistance)
                {
                    minDistance = newDistance;
                }
            }
            return 0;
        }

        private int MinPowerForDistance(double distance)
        {

        }

        private Location FindLocation(DroneToList drone)
        {
            Location newLoction = new Location();

            if (drone.DroneStatuses == DroneStatuses.sending)
            {
                IDAL.DO.Parcel parcel = dalObject.ViewParcel((int)drone.NumOfParcel);
                if (parcel.PickedUp == null)
                {
                    return findClosetStationLocation(drone);
                }
                if (parcel.Delivered == null)
                {
                    return new Location(dalObject.ViewCustomer(parcel.SenderId).Lattitude, dalObject.ViewCustomer(parcel.SenderId).Longitude);
                }
            }


            if (drone.DroneStatuses == DroneStatuses.maintanance)
            {
                int stationId = rand.Next(dalObject.StationList().Count());
                IDAL.DO.Station Station = dalObject.ViewStation(stationId);
                newLoction = new Location(Station.Lattitude, Station.Longitude);
                return newLoction;
            }


            if (drone.DroneStatuses == DroneStatuses.vacant)
            {
                //dalObject.;
            }

            return newLoction;
        }

        private Location findClosetStationLocation(DroneToList drone)
        {
            List<Station> locations = new List<Station>();
            foreach (var Station in dalObject.StationList())
            {
                locations.Add(new Station
                (
                    0, 0, 0,
                    new Location
                    {
                        Latitude = Station.Lattitude,
                        Longitude = Station.Longitude
                    }
                ));
            }
            Location location = locations[0].CurrentLocation;
            double distance = drone.Distance(locations[0]);
            for (int i = 1; i < locations.Count; i++)
            {
                if (drone.Distance(locations[i]) < distance)
                {
                    location = locations[i].CurrentLocation;
                    distance = drone.Distance(locations[i]);
                }
            }
            return location;
        }

        public void AddStation(int id, string name, Location location, int chargeSlots)
        {
            IDAL.DO.Station newStation = new IDAL.DO.Station(id, name, location.Longitude, location.Latitude, chargeSlots);
            try
            {
                dalObject.AddStation(newStation);
            }
            catch (Exception)
            {

            }
        }

        public void AddDrone(int id, string model, WeightCategories maxWeight, int stationId)
        {
            IDAL.DO.Drone newDrone = new IDAL.DO.Drone(id, model, (IDAL.DO.WeightCategories)maxWeight);
            try
            {
                dalObject.AddDrone(newDrone);
            }
            catch (Exception)
            {

            }
        }

        public void AddCustomer(int id, string name, string phone, Location location)
        {
            IDAL.DO.Customer newCustomer = new IDAL.DO.Customer(id, name: name, phone: phone, longitude: location.Longitude, lattitude: location.Latitude);
            try
            {
                dalObject.AddCustomer(newCustomer);
            }
            catch (Exception)
            {

            }
        }

        public void AddParcel(int sid, int tid, WeightCategories weigth, Priorities priority)
        {
            IDAL.DO.Parcel newParcel = new IDAL.DO.Parcel(0, sid, tid, (IDAL.DO.WeightCategories)weigth, (IDAL.DO.Priorities)priority);
            try
            {
                dalObject.AddParcel(newParcel);
            }
            catch (Exception)
            {

            }
        }

        public void UpdateStation(int id, string name, int numOfChargeSlot)
        {
            throw new NotImplementedException();
        }

        public void UpdateDrone(int id, string model)
        {
            throw new NotImplementedException();
        }

        public void UpdateCusomer(int id, string name, string phone)
        {
            throw new NotImplementedException();
        }

        public void ChargeOn(int id)
        {
            throw new NotImplementedException();
        }

        public void ChargeOf(int id, float timeInCharge)
        {
            throw new NotImplementedException();
        }

        public void ParcelToDrone(int id)
        {
            throw new NotImplementedException();
        }

        public void PickParcel(int id)
        {
            throw new NotImplementedException();
        }

        public void Destination(int id)
        {
            throw new NotImplementedException();
        }

        public Station ViewStation(int requestedId)
        {
            IDAL.DO.Station temptation = dalObject.ViewStation(requestedId);
            Station station = new Station(temptation.Id, temptation.Name, temptation.ChargeSlot, new Location(temptation.Lattitude, temptation.Longitude));
            foreach (var item in dalObject.)
            {
                Drone drone = new Drone();
                station.DronesInCharge.Add();
            }
            return station;
        }

        //public BO.Drone ViewDrone(int requestedId)
        //{
        //    throw new NotImplementedException();
        //}

        public BO.Drone GetDrone(int requestedId)
        {

            throw new NotImplementedException();
        }

        public Customer ViewCustomer(int requestedId)
        {
            throw new NotImplementedException();
        }

        public Parcel ViewParcel(int requestedId)
        {
            IDAL.DO.Parcel tempParcel= dalObject.ViewParcel(requestedId);
            Parcel parcel = new Parcel(tempParcel.Id, tempParcel.SenderId, tempParcel.TargilId, tempParcel.Weight, tempParcel.Priority,
                );
            return parcel;
        }

        public IEnumerable<StationToList> StationsList()
        {
            List<StationToList> stationList = new List<StationToList>();

            foreach (var s in dalObject.StationList())
            {
                //לשנות מספר תחנות ריקות מלאות
                int numOfChargeSlot = s.ChargeSlot;
                StationToList newStation = new StationToList(s.Id, s.Name, numOfChargeSlot, numOfChargeSlot);
                stationList.Add(newStation);
            }

            return stationList;
        }

        public IEnumerable<DroneToList> DronesList()
        {
            List<DroneToList> droneList = new List<DroneToList>();

            foreach (var d in dalObject.DroneList())
            {
                DroneToList newDrone = new DroneToList(d.Id, d.Model, (BO.WeightCategories)d.MaxWeight);
                droneList.Add(newDrone);
            }

            return droneList;
        }

        public IEnumerable<CustomerToList> CustomersList()
        {
            List<CustomerToList> customersList = new List<CustomerToList>();

            foreach (var c in dalObject.CustomerList())
            {
                //למלאות את כל ה כמות של חבילות עם קריטריונים
                int numOfParcelsDefined = 0, numOfParcelsAscribed = 0, numOfParcelsCollected = 0, numOfParcelsSupplied = 0;
                CustomerToList newCustomer = new CustomerToList(c.Id, c.Name, c.Phone, numOfParcelsDefined, numOfParcelsAscribed,
                    numOfParcelsCollected, numOfParcelsSupplied);
                customersList.Add(newCustomer);
            }

            return customersList;
        }

        public IEnumerable<ParcelToList> ParcelsList()
        {
            List<ParcelToList> parcelsList = new List<ParcelToList>();

            foreach (var p in dalObject.ParcelList())
            {
                //drone sttus?????????????????????????????????????????? in ParceltoList
                ParcelToList newParcel = new ParcelToList(p.Id, p.SenderId, p.TargilId, (BO.WeightCategories)p.Weight, (BO.Priorities)p.Priority, DroneStatuses.vacant);
                parcelsList.Add(newParcel);
            }

            return parcelsList;
        }

        public IEnumerable<StationToList> EmptyChangeSlotlList()
        {
            List<StationToList> stationWithEmptyChangeSlotl = new List<StationToList>();

            foreach (var s in dalObject.EmptyChangeSlotlList())
            {
                //לשנות את מספר העמדות טעינה שיכילו את המספר הנכון
                int numOfAllChargeSlot = s.ChargeSlot;
                StationToList newStation = new StationToList(s.Id, s.Name, s.ChargeSlot, s.ChargeSlot);
                stationWithEmptyChangeSlotl.Add(newStation);
            }

            return stationWithEmptyChangeSlotl;
        }

        public IEnumerable<ParcelToList> ParcesWithoutDronelList()
        {
            List<ParcelToList> parcesWithoutDrone = new List<ParcelToList>();

            foreach (var p in dalObject.ParcesWithoutDronelList())
            {
                //drone sttus?????????????????????????????????????????? in ParceltoList
                ParcelToList newParcel = new ParcelToList(p.Id, p.SenderId, p.TargilId, (BO.WeightCategories)p.Weight, (BO.Priorities)p.Priority, DroneStatuses.vacant);
                parcesWithoutDrone.Add(newParcel);
            }

            return parcesWithoutDrone;
        }
        public BO.Drone detailsDrone(IDAL.DO.Drone drone)
        {
            DroneToList droneToList = drones.Find(item => item.Id == drone.Id);
            return new Drone()
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (WeightCategories)drone.MaxWeight,
                BatteryStatuses = droneToList.BatteryStatuses,
                DroneStatuses = droneToList.DroneStatuses,
                CurrentLocation = droneToList.CurrentLocation,
                Parcel = droneToList.NumOfParcel != null ? throw("finish") : null;
            };
        }
        public BO.Customer 
    }
}