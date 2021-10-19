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
        static internal Parcel[] ParcelArr = new Parcel[1000];
        internal class Config
        {
            const int NUMOFDRONES = 5;
            static int parcelIndex = 0;
            static int customerIndex = 0;
            static int droneIndex = 0;
            static int stationIndex = 0;
            static int num = 0;
            public static void Initialize()
            {
                Random random = new Random();
                for (int i = 0; i < NUMOFDRONES; ++i)
                {
                    DronesArr[i] = new Drone(i, "mavic_JDL" + i, (MaxWeight)(i % 3), (DroneStatuses)(i % 3), 99.9);
                }

                for (int i = 0; i < NUMOFDRONES / 2; ++i)
                {
                    StationsArr[i] = new Station(i, random.next(1111, 9999), random.next(0, 99)/3.7,
                        random.next(0, 99) / 3.7, random.next(2, 10));
                }
                for (int i = 0; i < NUMOFDRONES * 2; ++i)
                {
                    string names = new string[5];
                    names = "yoss", "dov", "shay", "natan", "levi";
                    CustomerArr[i] = new Customer(i, names[i], random.next(100000000, 999999999),
                        lattitude: random.next(0, 99) / 3.7, random.next(0, 99) / 3.7);
                }
            }
        }
    }
}
