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
        static internal List<DroneCharge> listOfChargeSlot;
        static internal Drone[] DronesArr = new Drone[10];
        static internal Station[] StationsArr = new Station[5];
        static internal Customer[] CustomerArr = new Customer[100];
        static internal Parcel[] ParcelArr = new Parcel[1000];
        internal class Config
        {
            const int NUMOFDRONES = 5;
            public static int parcelIndex = 0;
            public static int customerIndex = 0;
            public static int droneIndex = 0;
            public static int stationIndex = 0;
            public static int runNumForParcel = 0;
            public static void Initialize()
            {
                listOfChargeSlot = new List<DroneCharge>();
                Random random = new Random();
                for (int i = 0; i < NUMOFDRONES; ++i)
                {
                    DronesArr[i] = new Drone(i, "mavic_JDL" + i, (WeightCategories)(i % 3), (DroneStatuses)(i % 3), 99.9);
                    ++droneIndex;
                }

                for (int i = 0; i < NUMOFDRONES / 2; ++i)
                {
                    StationsArr[i] = new Station(i, random.Next(1111, 9999), random.Next(0, 99)/3.7,
                        random.Next(0, 99) / 3.7, random.Next(2, 10));
                    ++stationIndex;
                }

                List<string> names = new List<string>();
                names.Add("Yoss"); names.Add("Dov"); names.Add("Shay"); names.Add("Gad"); names.Add("Ran");

                List<string> phones = new List<string>();
                names.Add("9741945"); names.Add("9089251"); names.Add("9090508"); names.Add("6722027"); names.Add("8827664");

                for (int i = 0; i < NUMOFDRONES * 2; ++i)
                {
                    CustomerArr[i] = new Customer(i, random.Next(0, 99) / 3.7, random.Next(0, 99) / 3.7, names[1], phones[4]);
                    ++customerIndex;
                }

                for (int i = 0; i < NUMOFDRONES * 2; ++i)
                {
                    ParcelArr[i] = new Parcel(i,random.Next(111111111, 999999999), random.Next(1, 99), (WeightCategories)(i % 3)
                        ,(Priorities)(i % 3), new DateTime(random.Next(1, 99)), random.Next(0, 5), new DateTime(random.Next(1, 99)),
                        new DateTime(random.Next(1, 99)), new DateTime(random.Next(1, 99)));
                    ++parcelIndex;
                }
            }
        }
    }
}
