using System;
using IDAL.DO;
namespace ConsoleUI
{
    public class Program
    {
        public enum OPTIONS
        {
            ADD = 1, UPDATE, VIEW, SHOWLIST, EXIT
        }
        public enum THESTRUCTS
        {
            STATION = 1, DRONE, CUSTOMER, PARCEL
        }
        public enum LIST
        {
            STATION = 1, DRONE, CUSTOMER, PARCEL, PARCELSWITHOUTDRONE, EMPTYCHARGESLOT
        }
        public enum UPDATE
        {
            PARCELTODRONE, PICKPARCEL, CHARGEON, CHARGEOF
        }
        static void Main(string[] args)
        {
            DalObject.DalObject dalObject = new DalObject.DalObject();
            Console.WriteLine("Welcome to the drones sending\n" +
                "Choos your option: Add, Update, View, ShowList, Exit (1-5)");
            int choos = int.Parse(Console.ReadLine());
            do
            {
                switch (choos)
                {
                    case (int)OPTIONS.ADD:
                        {
                            Console.WriteLine("Choos your option: STATION, DRONE, CUSTOMER, PARCEL (1-4)");
                            choos = int.Parse(Console.ReadLine());
                            switch (choos)
                            {
                                case (int)THESTRUCTS.STATION:
                                    {
                                        Console.WriteLine("enter station details:");
                                        Station station = new Station();
                                        station.ChargeSlot = int.Parse(Console.ReadLine());
                                        station.Id = int.Parse(Console.ReadLine());
                                        station.Lattitude = double.Parse(Console.ReadLine());
                                        station.Longitude = double.Parse(Console.ReadLine());
                                        station.Name = int.Parse(Console.ReadLine());
                                        dalObject.AddStation(station);
                                        break;
                                    }
                                case (int)THESTRUCTS.DRONE:
                                    {
                                        Console.WriteLine("enter drone details:");
                                        Drone drone = new Drone();
                                        drone.Battery = double.Parse(Console.ReadLine());
                                        drone.Id = int.Parse(Console.ReadLine());
                                        drone.MaxWeight = (WeightCategories)int.Parse(Console.ReadLine());
                                        drone.Model = Console.ReadLine();
                                        drone.Status = (DroneStatuses)int.Parse(Console.ReadLine());
                                        dalObject.AddDrone(drone);
                                        break;
                                    }
                                case (int)THESTRUCTS.CUSTOMER:
                                    {
                                        Console.WriteLine("enter customer details:");
                                        Customer customer = new Customer();
                                        customer.Id = int.Parse(Console.ReadLine());
                                        customer.Lattitude = double.Parse(Console.ReadLine());
                                        customer.Longitude = double.Parse(Console.ReadLine());
                                        customer.Name = Console.ReadLine();
                                        customer.Phone = Console.ReadLine();
                                        dalObject.AddCustomer(customer);
                                        break;
                                    }
                                case (int)THESTRUCTS.PARCEL:
                                    {
                                        Console.WriteLine("enter parcel details:");
                                        Parcel parcel = new Parcel();
                                        parcel.Droneld = int.Parse(Console.ReadLine());
                                        parcel.Delivered = new DateTime(int.Parse(Console.ReadLine()));
                                        parcel.Id = int.Parse(Console.ReadLine());
                                        parcel.PickedUp = new DateTime(int.Parse(Console.ReadLine()));
                                        parcel.Priority = (Priorities)int.Parse(Console.ReadLine());
                                        parcel.ReQuested = new DateTime(int.Parse(Console.ReadLine()));
                                        parcel.Schedulet = new DateTime(int.Parse(Console.ReadLine()));
                                        parcel.SenderId = int.Parse(Console.ReadLine());
                                        parcel.TargilId = int.Parse(Console.ReadLine());
                                        parcel.Weight = (WeightCategories)int.Parse(Console.ReadLine());
                                        dalObject.AddParcel(parcel);
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Error, input number only from 1 to 4");
                                    break;
                            }
                            break;
                        }
                    case (int)OPTIONS.UPDATE:
                        {
                            Console.WriteLine("Choos your option: PARCEL TO DRONE, PICK PARCEL, CHARGE ON, CHARGE OF (1-4)");
                            choos = int.Parse(Console.ReadLine());
                            switch (choos)
                            {
                                case (int) UPDATE.PARCELTODRONE:
                                    {                              
                                        Console.WriteLine("Choose a parcel (id)");
                                        int percelChoose = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Choose a drone (id)");
                                        int droneChoose = int.Parse(Console.ReadLine());
                                        dalObject.ParcelToDrone(percelChoose, droneChoose);
                                        break;
                                    }
                                case (int)UPDATE.PICKPARCEL:
                                    {
                                        dalObject.PickParcel();
                                        break;
                                    }
                                case (int)UPDATE.CHARGEON:
                                    {
                                        dalObject.ChargeOn();
                                        break;
                                    }
                                case (int)UPDATE.CHARGEOF:
                                    {
                                        dalObject.ChargeOf();
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Error, input number only from 1 to 4");
                                    break;
                            }
                            break;
                        }
                    case (int)OPTIONS.VIEW:
                        {
                            Console.WriteLine("Choos your option: STATION, DRONE, CUSTOMER, PARCEL (1-4) ");
                            choos = int.Parse(Console.ReadLine());
                            switch (choos)
                            {
                                case (int)THESTRUCTS.STATION:
                                    {
                                        dalObject.ViewStation();
                                        break;
                                    }
                                case (int)THESTRUCTS.DRONE:
                                    {
                                        dalObject.ViewDrone();
                                        break;
                                    }
                                case (int)THESTRUCTS.CUSTOMER:
                                    {
                                        dalObject.ViewCustomer();
                                        break;
                                    }
                                case (int)THESTRUCTS.PARCEL:
                                    {
                                        dalObject.ViewParcel();
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Error, input number only from 1 to 4");
                                    break;
                            }
                            break;
                        }
                    case (int)OPTIONS.SHOWLIST:
                        {
                            Console.WriteLine("Choos your option: STATION LIST, DRONE LIST, CUSTOMER LIST, PARCEL LIST, Parcels without drone, stations with empty ChargeSlot (1-6)");
                            choos = int.Parse(Console.ReadLine());
                            switch (choos)
                            {
                                case (int)LIST.STATION:
                                    {
                                        dalObject.StationList();
                                        break;
                                    }
                                case (int)LIST.DRONE:
                                    {
                                        dalObject.DroneList();
                                        break;
                                    }
                                case (int)LIST.CUSTOMER:
                                    {
                                        dalObject.CustomerList();
                                        break;
                                    }
                                case (int)LIST.PARCEL:
                                    {
                                        dalObject.ParcelList();
                                        break;
                                    }
                                case (int)LIST.PARCELSWITHOUTDRONE:
                                    {
                                        dalObject.ParcesWithoutDronelList();
                                        break;
                                    }
                                case (int)LIST.EMPTYCHARGESLOT:
                                    {
                                        dalObject.EmptyChangeSlotlList();
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Error, input number only from 1 to 4");
                                    break;
                            }
                            break;
                        }
                    case (int)OPTIONS.EXIT:
                        {
                            break;
                        }
                    default:
                        Console.WriteLine("Error, input number only from 1 to 5");
                        break;
                }

            } while (choos != (int)OPTIONS.EXIT);
        }
    }
}
