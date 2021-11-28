using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDal.IDal
    {
        /// <summary>
        /// coter that play the Initialize in DataSoutce
        /// </summary>
        public DalObject()
        {
            DataSource.Config.Initialize();
        }

        /// <summary>
        /// the function contected beetwen parcel and drone
        /// </summary>
        /// <param name="percelChoose">the choosen percel</param>
        /// <param name="droneChoose">the drone percel</param>
        public void ParcelToDrone(int percelChoose, int droneChoose)
        {
            WeightCategories itemDroneWeight = 0;
            bool check = false;
            int i = 0;
            foreach (var item in DataSource.DronesArr)
            {
                if (item.Id == droneChoose)
                {
                    check = true;
                    DataSource.DronesArr[i] = new Drone(DataSource.DronesArr[i].Id, DataSource.DronesArr[i].Model,
                    item.MaxWeight);
                    break;
                }
            }
            if (check == false)
                throw new ArgumentException("Error!! Ther is no drone with this id");

            int itemParcelDrone = 0;
            DateTime? itemParcelSchedulet;
            check = false;
            foreach (var item in DataSource.ParcelArr)
            {
                if (item.Id == percelChoose)
                {
                    check = true;
                    DataSource.ParcelArr[i] = new Parcel()
                    {
                        Id = DataSource.ParcelArr[i].Id,
                        SenderId = DataSource.ParcelArr[i].SenderId,
                        Getter = DataSource.ParcelArr[i].Getter,
                        Weight = DataSource.ParcelArr[i].Weight,
                        Priority = DataSource.ParcelArr[i].Priority,
                        ReQuested = DataSource.ParcelArr[i].ReQuested,
                        Droneld = item.Droneld,
                        Schedulet = DataSource.ParcelArr[i].Schedulet,
                        PickedUp = DataSource.ParcelArr[i].PickedUp,
                        Delivered = DataSource.ParcelArr[i].Delivered
                    };
                    itemParcelDrone = item.Droneld;
                    itemParcelSchedulet = item.Schedulet;
                    if (item.Weight > itemDroneWeight)
                        throw new ArgumentException("Error!! The drone cannot carry this weight");

                    itemParcelDrone = item.Id;
                    itemParcelSchedulet = DateTime.Now;
                    break;
                }
            }
            if (check == false)
                throw new ArgumentException("Error!! no percel with this id");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="percelChoose"></param>
        public void Destination(int percelChoose)
        {
            int i = DataSource.ParcelArr.FindIndex(p => p.Id == percelChoose);
            if (i < 0)
            {
                throw new ArgumentException("Error!! no drone with this id");

            }
            DataSource.ParcelArr[i] = new Parcel()
            {
                Id = DataSource.ParcelArr[i].Id,
                SenderId = DataSource.ParcelArr[i].SenderId,
                Getter = DataSource.ParcelArr[i].Getter,
                Weight = DataSource.ParcelArr[i].Weight,
                Priority = DataSource.ParcelArr[i].Priority,
                ReQuested = DataSource.ParcelArr[i].ReQuested,
                Droneld = DataSource.ParcelArr[i].Droneld,
                Schedulet = DataSource.ParcelArr[i].Schedulet,
                PickedUp = DataSource.ParcelArr[i].PickedUp,
                Delivered = DateTime.Now
            };
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