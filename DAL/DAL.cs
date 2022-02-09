using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Singelton;
using DO;

namespace DalObject
{
    public sealed partial class DAL : Singleton<DAL>, DalApi.IDal
    {
        static DAL() { }
        /// <summary>
        /// coter that play the Initialize in DataSoutce
        /// </summary>
        private DAL()
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
            //WeightCategories itemDroneWeight = 0;
            int droneI = DataSource.DronesArr.FindIndex((d) => d.Id == droneChoose);
            if (droneI < 0)
                throw new ObjectNotExistException("Error!! Ther is no drone with this id");


            int parcelI = DataSource.ParcelArr.FindIndex((p => p.Id == percelChoose));
            if (parcelI < 0)
                throw new ObjectNotExistException("Error!! Ther is no percel with this id");

            if (DataSource.ParcelArr[parcelI].Weight > DataSource.DronesArr[droneI].MaxWeight || DataSource.ParcelArr[parcelI].Droneld != default)
                throw new ObjectNotAvailableForActionException("Error!! The drone cannot carry this weight");

            DataSource.ParcelArr[parcelI] = new Parcel()
            {
                Id = DataSource.ParcelArr[parcelI].Id,
                SenderId = DataSource.ParcelArr[parcelI].SenderId,
                GetterId = DataSource.ParcelArr[parcelI].GetterId,
                Weight = DataSource.ParcelArr[parcelI].Weight,
                Priority = DataSource.ParcelArr[parcelI].Priority,
                Requested = DataSource.ParcelArr[parcelI].Requested,
                Droneld = droneChoose,
                Schedulet = DateTime.Now,
                PickedUp = DataSource.ParcelArr[droneI].PickedUp,
                Delivered = DataSource.ParcelArr[droneI].Delivered
            };
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
                    DataSource.ParcelArr[i] = new Parcel()
                    {
                        Id = DataSource.ParcelArr[i].Id,
                        SenderId = DataSource.ParcelArr[i].SenderId,
                        GetterId = DataSource.ParcelArr[i].GetterId,
                        Weight = DataSource.ParcelArr[i].Weight,
                        Priority = DataSource.ParcelArr[i].Priority,
                        Requested = DataSource.ParcelArr[i].Requested,
                        Droneld = DataSource.ParcelArr[i].Droneld,
                        Schedulet = DataSource.ParcelArr[i].Schedulet,
                        PickedUp = DataSource.ParcelArr[i].PickedUp,
                        Delivered = DateTime.Now
                    };
                    //?????????????????????????
                    for (int j = 0; j < DataSource.DronesArr.Count; ++j)
                    {
                        if (DataSource.DronesArr[j].Id == DataSource.ParcelArr[i].Droneld)
                            DataSource.DronesArr[j] = new Drone()
                            {
                                Id = DataSource.DronesArr[j].Id,
                                Model = DataSource.DronesArr[j].Model,
                                MaxWeight = DataSource.DronesArr[j].MaxWeight,
                                IsDelete = DataSource.DronesArr[j].IsDelete
                            };
                    }
                    break;
                }
            }
            if (check == false)
                throw new ArgumentException("Error!! Ther is no drone with this id");
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

        public DateTime StartChargeTime(int droneId)
        {
            return DataSource.listOfChargeSlot[DataSource.listOfChargeSlot.FindIndex((l)=>l.DroneId == droneId)].StartTime;
        }
    }
}
