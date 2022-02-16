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
            int droneI = DataSource.DronesArr.FindIndex((d) => d.Id == droneChoose);
            if (droneI < 0)
                throw new ObjectNotExistException("Error!! Ther is no drone with this id");

            int parcelI = DataSource.ParcelArr.FindIndex((p => p.Id == percelChoose));
            if (parcelI < 0)
                throw new ObjectNotExistException("Error!! Ther is no percel with this id");

            if (DataSource.ParcelArr[parcelI].Weight > DataSource.DronesArr[droneI].MaxWeight || DataSource.ParcelArr[parcelI].Droneld != default)
                throw new ObjectNotAvailableForActionException("Error!! The drone cannot carry this weight");
            Parcel par = DataSource.ParcelArr[parcelI];
            par.Droneld = droneChoose;
            par.Schedulet = DateTime.Now;
            DataSource.ParcelArr[parcelI] = par;
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
            return new double[5]
            {
                DataSource.Config.vacent,
                DataSource.Config.LightWeightCarrier,
                DataSource.Config.MediumWeightCarrier,
                DataSource.Config.heavyWeightCarrier,
                DataSource.Config.SkimmerLoadingRate
            };
        }

        public DateTime StartChargeTime(int droneId)
        {
            int i = DataSource.listOfChargeSlot.FindIndex((l) => l.DroneId == droneId);
            if (i < 0)
                throw new ObjectNotExistException("No charge for drone: " + droneId);
            return DataSource.listOfChargeSlot[i].StartTime;
        }
    }
}
