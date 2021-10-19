using System;
using DalObject;

namespace ConsoleUI
{
    class Program
    {
        public enum OPTIONS
        {
            ADD = 1, UPDATE, VIEW, SHOWLIST, EXIT
        }

        public enum THESTRUCTS
        {
            STATION = 1, DRONE, CUSTOMER, PARCEL
        }
        public enum UPDATE
        {
            PARCELTODRONE, PICKPARCEL, CHARGEON, CHARGEOF
        }

        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to the drones sending\n" +
                "Choos your option: Add, Update, View, ShowList, Exit (1-5)");
            int choos = int.Parse(Console.readLine());
            do
            {
                switch (choos)
                {
                    case ADD:
                        {
                            Console.WriteLine("Choos your option: STATION, DRONE, CUSTOMER, PARCEL (1-4)");
                            choos = int.Parse(Console.readLine());
                            switch (choos)
                            {
                                case STATION:
                                    {
                                        AddStation();
                                        break;
                                    }
                                case DRONE:
                                    {
                                        AddDrone();
                                        break;
                                    }
                                case CUSTOMER:
                                    {
                                        AddCustomer();
                                        break;
                                    }
                                case PARCEL:
                                    {
                                        AddParcel();
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Error, input number only from 1 to 4");
                                    break;
                            }
                            break;
                        }
                    case UPDATE:
                        {
                            Console.WriteLine("Choos your option: PARCEL TO DRONE, PICK PARCEL, CHARGE ON, CHARGE OF (1-4)");
                            choos = int.Parse(Console.readLine());
                            switch (choos)
                            {
                                case PARCELTODRONE:
                                    {
                                        ParcelToDrone();
                                        break;
                                    }
                                case PICKPARCEL:
                                    {
                                        PickParcel();
                                        break;
                                    }
                                case CHARGEON:
                                    {
                                        ChargeOn();
                                        break;
                                    }
                                case CHARGEOF:
                                    {
                                        ChargeOf();
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Error, input number only from 1 to 4");
                                    break;
                            }
                        }
                    case VIEW:
                        {
                            Console.WriteLine("Choos your option: STATION, DRONE, CUSTOMER, PARCEL (1-4) ");
                            choos = int.Parse(Console.readLine());
                            switch (choos)
                            {
                                case STATION:
                                    {
                                        ViewStation();
                                        break;
                                    }
                                case DRONE:
                                    {
                                        ViewDrone();
                                        break;
                                    }
                                case CUSTOMER:
                                    {
                                        ViewCustomer();
                                        break;
                                    }
                                case PARCEL:
                                    {
                                        ViewParcel();
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Error, input number only from 1 to 4");
                                    break;
                            }
                        }
                    case SHOWLIST:
                        {
                            Console.WriteLine("Choos your option: STATION LIST, DRONE LIST, CUSTOMER LIST, PARCEL LIST (1-4) ");
                            choos = int.Parse(Console.readLine());
                            switch (choos)
                            {
                                case STATION:
                                    {
                                        StationList();
                                        break;
                                    }
                                case DRONE:
                                    {
                                        DroneList();
                                        break;
                                    }
                                case CUSTOMER:
                                    {
                                        CustomerList();
                                        break;
                                    }
                                case PARCEL:
                                    {
                                        ParcelList();
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Error, input number only from 1 to 4");
                                    break;
                            }
                        }
                    case EXIT:
                        {
                            break;
                        }
                    default:
                        Console.WriteLine("Error, input number only from 1 to 5");
                        break;
                }

            } while (choos != EXIT);
        }
    }
}
