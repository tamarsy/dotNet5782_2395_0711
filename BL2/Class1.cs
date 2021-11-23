using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class Class1
    {

        public Customer ViewCustomer(int requestedId)
        {
            throw new NotImplementedException();
        }

        public Parcel ViewParcel(int requestedId)
        {
            IDAL.DO.Parcel tempParcel = dalObject.ViewParcel(requestedId);
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
                Parcel = droneToList.NumOfParcel != null ? throw ("finish") : null;
        };
    }
    public BO.Customer
}
}
