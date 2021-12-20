using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;


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
            static public double vacent { get; set; } = 0.001;
            static public double LightWeightCarrier { get; set; } = 0.002;
            static public double MediumWeightCarrier { get; set; } = 0.003;
            static public double heavyWeightCarrier { get; set; } = 0.004;
            static public double SkimmerLoadingRate { get; set; } = 1;
            const int NUMOFDRONES = 5;
            public static int runNumForParcel = 0;
            public static void Initialize()
            {

                List<string> names = new List<string>();
                names.Add("Yoss"); names.Add("Dov"); names.Add("Shay"); names.Add("Gad"); names.Add("Ran");

                List<string> phones = new List<string>();
                phones.Add("9741945"); phones.Add("9089251"); phones.Add("9090508"); phones.Add("6722027"); phones.Add("8827664");

                Random random = new Random();
                for (int i = 0; i < NUMOFDRONES; ++i)
                {
                    DronesArr.Add(new Drone()
                    {
                        Id = i,
                        Model = "mavic_JDL" + i,
                        MaxWeight = (WeightCategories)(i % 3)
                    });
                }

                for (int i = 0; i < NUMOFDRONES; ++i)
                {
                    StationsArr.Add(new Station()
                    {
                        Id = i,
                        Name = phones[i % (names.Count())],
                        Lattitude = random.Next(0, 99) / 3.7,
                        Longitude = random.Next(0, 99) / 3.7,
                        ChargeSlot = random.Next(2, 10)
                    });
                }


                for (int i = 0; i < NUMOFDRONES * 2; ++i)
                {
                    CustomerArr.Add(new Customer()
                    {
                        Id = i,
                        Name = names[i % (names.Count())],
                        Phone = phones[i % (names.Count())],
                        Longitude = random.Next(0, 99) / 3.7,
                        Lattitude = random.Next(0, 99) / 3.7
                    });
                }
                for (int i = 0; i < NUMOFDRONES * 2; ++i)
                {
                    int? dronIdOrNull = null;
                    if (i % 2 == 0)
                        dronIdOrNull = random.Next(0, 5);
                    ParcelArr.Add(new Parcel()
                    {
                        Id = runNumForParcel++,
                        SenderId = random.Next(CustomerArr.Count - 1),
                        Getter = random.Next(CustomerArr.Count - 1),
                        Weight = 0,
                        Priority = (Priorities)(i % 3),
                        ReQuested = new DateTime(random.Next(1, 99)),
                        Droneld = dronIdOrNull,
                        Schedulet = dronIdOrNull != null? new DateTime(random.Next(1, 99)): default,
                        PickedUp = dronIdOrNull != null ? new DateTime(random.Next(1, 99)) : default,
                        Delivered = dronIdOrNull != null ? new DateTime(random.Next(1, 99)) : default
                    });
                }
            }
        }
    }
}
