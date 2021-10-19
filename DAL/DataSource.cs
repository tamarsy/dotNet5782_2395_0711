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
            static void Initialize()
            {
                Random random = new Random();
                for (int i = 0; i < NUMOFDRONES; ++i)
                {
                    DronesArr[i] = new Drone(i, "mavic_JDL" + i, (WeightCategories)(i % 3), (DroneStatuses)(i % 3), 99.9);
                }

                for (int i = 0; i < NUMOFDRONES / 2; ++i)
                {
                    StationsArr[i] = new Station(i, random.Next(1111, 9999), random.Next(0, 99)/3.7,
                        random.Next(0, 99) / 3.7, random.Next(2, 10));
                }

                List<string> names = new List<string>();
                names.Add("Yoss"); names.Add("Dov"); names.Add("Shay"); names.Add("Gad"); names.Add("Ran");

                List<string> phones = new List<string>();
                names.Add("9741945"); names.Add("9089251"); names.Add("9090508"); names.Add("6722027"); names.Add("8827664");

                for (int i = 0; i < NUMOFDRONES * 2; ++i)
                {        
                    CustomerArr[i] = new Customer(i, random.Next(0, 99) / 3.7, random.Next(0, 99) / 3.7, names[i], phones[i]);
                }
            }
        }
    }
}
