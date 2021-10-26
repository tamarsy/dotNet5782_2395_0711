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
        static internal List<DroneCharge> listOfChargeSlot = new List<DroneCharge>();
        static internal List<Drone> DronesArr = new List<Drone>();
        static internal List<Station> StationsArr = new List<Station>();
        static internal List<Customer> CustomerArr = new List<Customer>();
        static internal List<Parcel> ParcelArr = new List<Parcel>();
        internal class Config
        {
            const int NUMOFDRONES = 5;
            public static int runNumForParcel = 0;
            public static void Initialize()
            {
                Random random = new Random();
                for (int i = 0; i < NUMOFDRONES; ++i)
                {
                    DronesArr.Add(new Drone(i, "mavic_JDL" + i, (WeightCategories)(i % 3), (DroneStatuses)(i % 3), 99.9));
                }

                for (int i = 0; i < NUMOFDRONES / 2; ++i)
                {
                    StationsArr.Add(new Station(i, random.Next(1111, 9999), random.Next(0, 99) / 3.7,
                        random.Next(0, 99) / 3.7, random.Next(2, 10)));
                }

                List<string> names = new List<string>();
                names.Add("Yoss"); names.Add("Dov"); names.Add("Shay"); names.Add("Gad"); names.Add("Ran");

                List<string> phones = new List<string>();
                phones.Add("9741945"); phones.Add("9089251"); phones.Add("9090508"); phones.Add("6722027"); phones.Add("8827664");

                for (int i = 0; i < NUMOFDRONES * 2; ++i)
                {
                    CustomerArr.Add(new Customer(i, random.Next(0, 99) / 3.7, random.Next(0, 99) / 3.7, names[i%(names.Count())], phones[i % (names.Count())]));
                }

                for (int i = 0; i < NUMOFDRONES * 2; ++i)
                {
                    ParcelArr.Add(new Parcel(i, random.Next(111111111, 999999999), random.Next(1, 99), (WeightCategories)(i % 3)
                        , (Priorities)(i % 3), new DateTime(random.Next(1, 99)), random.Next(0, 5), new DateTime(random.Next(1, 99)),
                        new DateTime(random.Next(1, 99)), new DateTime(random.Next(1, 99))));
                }
            }
        }
    }
}
