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
            static Random random;
            static public double vacent { get; set; } = 1;
            static public double LightWeightCarrier { get; set; } = 2;
            static public double MediumWeightCarrier { get; set; } = 3;
            static public double heavyWeightCarrier { get; set; } = 4;
            static public double SkimmerLoadingRate { get; set; } = 4;
            const int NUMOFDRONES = 5;
            public static int runNumForParcel = 0;
            public static void Initialize()
            {

                List<string> names = new List<string>();
                names.Add("Yoss"); names.Add("Dov"); names.Add("Shay"); names.Add("Gad"); names.Add("Ran");

                List<string> phones = new List<string>();
                phones.Add("9741945875"); phones.Add("9543089251"); phones.Add("9096540508"); phones.Add("6726542027"); phones.Add("8823457664");

                random = new Random();
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
                        Name = phones[i % names.Count()],
                        Lattitude = getRandomCoordinate(random.NextDouble() * 5),
                        Longitude = getRandomCoordinate(random.NextDouble() * 5),
                        ChargeSlot = random.Next(2, 10)
                    });
                }


                for (int i = 0; i < NUMOFDRONES * 2; ++i)
                {
                    CustomerArr.Add(new Customer()
                    {
                        Id = random.Next(11111111, 99999999),
                        Name = names[i % names.Count()],
                        Phone = phones[i % names.Count()],
                        Longitude = getRandomCoordinate(random.NextDouble() * 5),
                        Lattitude = getRandomCoordinate(random.NextDouble() * 5)
                    });
                }
                for (int i = 0; i < NUMOFDRONES * 2; ++i)
                {
                    int? dronIdOrNull = (random.Next() % 2 == 0)? null: dronIdOrNull = random.Next(0, 5);

                    int index = random.Next(CustomerArr.Count - 1);
                    Parcel p = new Parcel()
                    {
                        Id = runNumForParcel++,
                        SenderId = CustomerArr[index].Id,
                        GetterId = (from c in CustomerArr where c.Id != CustomerArr[index].Id select c).FirstOrDefault().Id,
                        Weight = 0,
                        Priority = (Priorities)(i % 3),
                        Droneld = dronIdOrNull,
                        Requested = (DateTime)getRandomDateTime(DateTime.Now, 200, false),
                        Schedulet = default(DateTime?),
                        PickedUp = default(DateTime?),
                        Delivered = default(DateTime?)
                    };
                    if (dronIdOrNull != null)
                    {
                        p.Schedulet = getRandomDateTime(p.Requested, 20);
                        if(random.Next() % 2 == 0)
                        {
                            p.PickedUp = getRandomDateTime(p.Schedulet, 20);
                            if(random.Next() % 2 == 0)
                                p.Delivered = getRandomDateTime(p.PickedUp.Value, 15);
                        }
                    }
                    
                    ParcelArr.Add(p);
                }
            }
            static DateTime? getRandomDateTime(DateTime? dt, int minute, bool after = true)
            {
                int afterOrBefore = (after) ? 1 : -1;
                return dt?.AddMinutes(afterOrBefore * random.Next(minute));
            }

            static double getRandomCoordinate(double coord) => coord + random.NextDouble() / 10;
        }
    }
}
