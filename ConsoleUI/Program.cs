using System;

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
                                        dalObject.AddStation();
                                        break;
                                    }
                                case (int)THESTRUCTS.DRONE:
                                    {
                                        dalObject.AddDrone();
                                        break;
                                    }
                                case (int)THESTRUCTS.CUSTOMER:
                                    {
                                        dalObject.AddCustomer();
                                        break;
                                    }
                                case (int)THESTRUCTS.PARCEL:
                                    {
                                        dalObject.AddParcel();
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
                                        dalObject.ParcelToDrone();
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
                            Console.WriteLine("Choos your option: STATION LIST, DRONE LIST, CUSTOMER LIST, PARCEL LIST (1-4) ");
                            choos = int.Parse(Console.ReadLine());
                            switch (choos)
                            {
                                case (int)THESTRUCTS.STATION:
                                    {
                                        dalObject.StationList();
                                        break;
                                    }
                                case (int)THESTRUCTS.DRONE:
                                    {
                                        dalObject.DroneList();
                                        break;
                                    }
                                case (int)THESTRUCTS.CUSTOMER:
                                    {
                                        dalObject.CustomerList();
                                        break;
                                    }
                                case (int)THESTRUCTS.PARCEL:
                                    {
                                        dalObject.ParcelList();
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
