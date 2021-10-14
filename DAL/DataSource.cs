using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DalObject
{
    internal class DataSource
    {
        static internal Drone[] DronesArr = new Drone[10];
        static internal Station[] StationsArr = new Station[5];
        static internal Customer[] CustomerArr = new Customer[100];
        static internal Parcel[] ParcelArr = new Parcel[100];
        internal class Config
        {

            static int parcelIndex = 0;
            static int customerIndex = 0;
            static int droneIndex = 0;
            static int stationIndex = 0;
            static void Initialize()
            {

            }
        }
    }
}

