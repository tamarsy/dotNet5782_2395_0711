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
        private double vacent;
        private double lightWeightCarrier;
        private double mediumWeightCarrier;
        private double heavyWeightCarrier;
        private double skimmerLoadingRate;
        private List<DroneToList> drones;
        private static Random rand = new Random();

        public BL()
        {
            dalObject = new DalObject.DalObject();
            double[] PowerConsumption = dalObject.PowerConsumptionRequest();
            vacent = PowerConsumption[0];
            lightWeightCarrier = PowerConsumption[1];
            mediumWeightCarrier = PowerConsumption[2];
            heavyWeightCarrier = PowerConsumption[3];
            skimmerLoadingRate = PowerConsumption[4];
            drones = new List<DroneToList>();
            initializeDronesList();
        }




        private void initializeDronesList()
        {
            foreach (var drone in dalObject.DroneList())
            {
                DroneToList newDrone = new DroneToList()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (WeightCategories)drone.MaxWeight
                };
                foreach (var parcel in dalObject.ParcelList())
                {
                    if (parcel.Droneld == drone.Id && parcel.Delivered != null)
                    {
                        newDrone.NumOfParcel = parcel.Id;
                        break;
                    }
                }
                if (newDrone.NumOfParcel != null)
                {
                    newDrone.DroneStatuses = DroneStatuses.sending;
                    newDrone.CurrentLocation = FindLocation(newDrone);
                    //to fill BatteryStatuses
                    newDrone.BatteryStatuses = FindBatteryStatusesForInitializeDronesList(newDrone);
                }
                else
                {
                    newDrone.DroneStatuses = (DroneStatuses)rand.Next(-1, 1);
                    newDrone.CurrentLocation = FindLocation(newDrone);
                    //to fill BatteryStatuses
                    newDrone.BatteryStatuses = FindBatteryStatusesForInitializeDronesList(newDrone);
                }
                drones.Add(newDrone);
            }
        }



        public void ParcelToDrone(int id)
        {
            int i = drones.FindIndex(d => d.Id == id);
            if (drones[i] != DroneStatuses.vacant)
                throw new ObjectNotAvailableForActionException($"drone with id: {id} is no vacant");
            Parcel choosenParcel;
            try
            {
                choosenParcel = FindParcelToCollecting(drones[i]);
            }
            catch (Exception)
            {
                throw;
            }
            drones[i].DroneStatuses = DroneStatuses.sending;
            choosenParcel.DroneDelivery = new DroneDelivery()
            {
                BatteryStatuses = drones[i].BatteryStatuses,
                CurentLocation = drones[i].CurrentLocation,
                Id = drones[i].Id
            };
            choosenParcel.AssignmentTime = DateTime.Now;
        }



        private Parcel FindParcelToCollecting(DroneToList drone)
        {
            List<ParcelToList> availableParcelList = ParcelsList().Select(p => p.Weight < drone.MaxWeight).ToList;                                                                                                                                                     );
            return ;
        }




        public void Destination(int id)
        {

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
