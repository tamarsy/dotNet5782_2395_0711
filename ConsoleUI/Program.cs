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
            PARCELTODRONE, PICKPARCEL, DESTINATION, CHARGEON, CHARGEOF
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
                                        try { dalObject.AddStation(station); }
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
                                        try { dalObject.AddDrone(drone); }
                                        catch (Exception e) { Console.WriteLine(e); }
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
                                        try { dalObject.AddCustomer(customer); }
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
                                        try { dalObject.AddParcel(parcel); }
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
                                       
                                        Console.WriteLine("Choose a parcel (id)");
                                        int percelChoose = int.Parse(Console.ReadLine());
                                        dalObject.PickParcel(percelChoose);
                                        break;
                                    }
                                case (int)UPDATE.DESTINATION:
                                    {
                                        Console.WriteLine("Choose a parcel (id)");
                                        int percelChoose = int.Parse(Console.ReadLine());
                                        dalObject.Destination(percelChoose);
                                        break;
                                    }
                                case (int)UPDATE.CHARGEON:
                                    {
                                        Console.WriteLine("enter id of the drone to charge on");
                                        int droenId = int.Parse(Console.ReadLine());
                                        try
                                        {
                                            dalObject.ChargeOn(droenId);
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
                                            dalObject.ChargeOf(droenId);
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
                                        try {
                                            Station station = dalObject.ViewStation(i);
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
                                            Drone drone = dalObject.ViewDrone(i);
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
                                            Customer customer = dalObject.ViewCustomer(i);
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
                                            Parcel parcel = dalObject.ViewParcel(i);
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
                                        Console.WriteLine(dalObject.StationList());
                                        break;
                                    }
                                case (int)LIST.DRONE:
                                    {
                                        Console.WriteLine(dalObject.DroneList());
                                        break;
                                    }
                                case (int)LIST.CUSTOMER:
                                    {
                                        Console.WriteLine(dalObject.CustomerList());
                                        break;
                                    }
                                case (int)LIST.PARCEL:
                                    {
                                        Console.WriteLine(dalObject.ParcelList());
                                        break;
                                    }
                                case (int)LIST.PARCELSWITHOUTDRONE:
                                    {
                                        Console.WriteLine(dalObject.ParcesWithoutDronelList());
                                        break;
                                    }
                                case (int)LIST.EMPTYCHARGESLOT:
                                    {
                                        Console.WriteLine(dalObject.EmptyChangeSlotlList());
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
