using System;
using System.Collections.Generic;
using System.Text;

namespace BL
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
            PARCELTODRONE, PICKPARCEL, DESTINATION, CHARGEON, CHARGEOF
        }

        static private bool chackID(int ID)
        {
            if (ID < 111111111 || ID > 999999999)
            {
                return false;
            }
            return true;
        }
        static void Main(string[] args)
        {
            Console.WriteLine(434.GetType());
            IDal.IDal DalObject = new DalObject.DalObject();
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
                                        try { DalObject.AddStation(station); }
                                        catch (Exception e) { Console.WriteLine(e); }
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
                                        try { DalObject.AddDrone(drone); }
                                        catch (Exception e) { Console.WriteLine(e); }
                                        break;
                                    }
                                case (int)THESTRUCTS.CUSTOMER:
                                    {
                                        Console.WriteLine("enter customer details:");
                                        Customer customer = new Customer();
                                        customer.Id = int.Parse(Console.ReadLine());
                                        while (!chackID(customer.Id))
                                        {
                                            Console.WriteLine("weung id");
                                            customer.Id = int.Parse(Console.ReadLine());
                                        }
                                        customer.Lattitude = double.Parse(Console.ReadLine());
                                        customer.Longitude = double.Parse(Console.ReadLine());
                                        customer.Name = Console.ReadLine();
                                        customer.Phone = Console.ReadLine();
                                        try { DalObject.AddCustomer(customer); }
                                        catch (Exception e) { Console.WriteLine(e); }
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
                                        try { DalObject.AddParcel(parcel); }
                                        catch (Exception e) { Console.WriteLine(e); }
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
                                case (int)UPDATE.PARCELTODRONE:
                                    {
                                        Console.WriteLine("Choose a parcel (id)");
                                        int percelChoose = int.Parse(Console.ReadLine());
                                        Console.WriteLine("Choose a drone (id)");
                                        int droneChoose = int.Parse(Console.ReadLine());
                                        DalObject.ParcelToDrone(percelChoose, droneChoose);
                                        try { DalObject.ParcelToDrone(percelChoose, droneChoose); }
                                        catch (Exception e) { Console.WriteLine(e); }
                                        break;
                                    }
                                case (int)UPDATE.PICKPARCEL:
                                    {

                                        Console.WriteLine("Choose a parcel (id)");
                                        int percelChoose = int.Parse(Console.ReadLine());
                                        DalObject.PickParcel(percelChoose);
                                        try { DalObject.PickParcel(percelChoose); }
                                        catch (Exception e) { Console.WriteLine(e); }
                                        break;
                                    }
                                case (int)UPDATE.DESTINATION:
                                    {
                                        Console.WriteLine("Choose a parcel (id)");
                                        int percelChoose = int.Parse(Console.ReadLine());
                                        DalObject.Destination(percelChoose);
                                        try { DalObject.Destination(percelChoose); }
                                        catch (Exception e) { Console.WriteLine(e); }
                                        break;
                                    }
                                case (int)UPDATE.CHARGEON:
                                    {
                                        Console.WriteLine("enter id of the drone to charge on");
                                        int droenId = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            DalObject.ChargeOn(droenId);
                                        }
                                        catch (Exception e) { Console.WriteLine(e); }
                                        break;
                                    }
                                case (int)UPDATE.CHARGEOF:
                                    {
                                        Console.WriteLine("enter id of the drone to charge on");
                                        int droenId = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            DalObject.ChargeOf(droenId);
                                        }
                                        catch (Exception e) { Console.WriteLine(e); }
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
                                        Console.WriteLine("enter station id to view");
                                        int i = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Station station = DalObject.ViewStation(i);
                                            Console.WriteLine(station.ToString());
                                        }
                                        catch (Exception e) { Console.WriteLine(e); }
                                        break;
                                    }
                                case (int)THESTRUCTS.DRONE:
                                    {
                                        Console.WriteLine("enter drone id to view");
                                        int i = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Drone drone = DalObject.ViewDrone(i);
                                            Console.WriteLine(drone.ToString());
                                        }
                                        catch (Exception e) { Console.WriteLine(e); }
                                        break;
                                    }
                                case (int)THESTRUCTS.CUSTOMER:
                                    {
                                        Console.WriteLine("enter customer id to view");
                                        int i = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Customer customer = DalObject.ViewCustomer(i);
                                            Console.WriteLine(customer.ToString());
                                        }
                                        catch (Exception e) { Console.WriteLine(e); }
                                        break;
                                    }
                                case (int)THESTRUCTS.PARCEL:
                                    {
                                        Console.WriteLine("enter parcel id to view");
                                        int i = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            Parcel parcel = DalObject.ViewParcel(i);
                                            Console.WriteLine(parcel.ToString());
                                        }
                                        catch (Exception e) { Console.WriteLine(e); }
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
                                        foreach (var item in DalObject.StationList())
                                        {
                                            Console.WriteLine(item.ToString());
                                        };
                                        break;
                                    }
                                case (int)LIST.DRONE:
                                    {
                                        foreach (var item in DalObject.DroneList())
                                        {
                                            Console.WriteLine(item.ToString());
                                        };
                                        break;
                                    }
                                case (int)LIST.CUSTOMER:
                                    {
                                        foreach (var item in DalObject.CustomerList())
                                        {
                                            Console.WriteLine(item.ToString());
                                        };
                                        break;
                                    }
                                case (int)LIST.PARCEL:
                                    {
                                        foreach (var item in DalObject.ParcelList())
                                        {
                                            Console.WriteLine(item.ToString());
                                        };
                                        break;
                                    }
                                case (int)LIST.PARCELSWITHOUTDRONE:
                                    {
                                        foreach (var item in DalObject.ParcesWithoutDronelList())
                                        {
                                            Console.WriteLine(item.ToString());
                                        };
                                        break;
                                    }
                                case (int)LIST.EMPTYCHARGESLOT:
                                    {
                                        foreach (var item in DalObject.EmptyChangeSlotlList())
                                        {
                                            Console.WriteLine(item.ToString());
                                        };
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Error, input number only from 1 to 6");
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
