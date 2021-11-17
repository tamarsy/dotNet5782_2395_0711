﻿using System;
using IBL.BO;


namespace ConsoleUI_BL
{
    using System;
    using System.Collections;
    using IBL.BO;


    namespace ConsoleUI
    {
        class Program
        {
            enum Menu { Add, Update, Display, DisplayList, Exit }
            enum Add { Station, Drone, Customer, Parcel }
            enum Update { DroneName, StationDetails, CustomerDedails, SendDroneForCharg, RealsDroneFromChargh, AssingParcelToDrone, CollectParcelByDrone, SupplyParcelToDestination }
            enum DisplayList { Sations, Drones, Customers, Parcels, ParcelNotAssignToDrone, AvailableChargingSations }
            enum Display { Station, Drone, Customer, Parcel }

            static void Main(string[] args)
            {
                IBL.IBL bal = new IBL.BL();
                Menu option;
                do
                {
                    DisplayMenus(typeof(Menu));
                    Enum.TryParse(Console.ReadLine(), out option);
                    switch (option)

                    {
                        case Menu.Add:
                            {
                                DisplayMenus(typeof(Add));
                                try
                                {
                                    switchAdd(ref bal);
                                }
                                catch
                                {
                                    Console.WriteLine("error");
                                }

                                break;
                            }

                        case Menu.Update:
                            {

                                DisplayMenus(typeof(Update));
                                try
                                {
                                    switchUpdate(ref bal);
                                }
                                catch (ArgumentNullException)
                                {
                                    Console.WriteLine("incorrect id");
                                }
                                catch (ArgumentException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                catch
                                {
                                    Console.WriteLine("ERROR");
                                }

                                break;
                            }
                        case Menu.Display:
                            {
                                DisplayMenus(typeof(Display));
                                try
                                {
                                    switchDisplay(ref bal);
                                }
                                catch (ArgumentNullException)
                                {
                                    Console.WriteLine("incorrect id");
                                }
                                catch
                                {
                                    Console.WriteLine("ERROR");
                                }

                                break;
                            }
                        case Menu.DisplayList:
                            {
                                DisplayMenus(typeof(DisplayList));
                                try
                                {
                                    switchDisplayList(ref bal);

                                }
                                catch
                                {
                                    Console.WriteLine("ERROR");
                                }


                                break;
                            }

                        case Menu.Exit:
                            break;
                        default:
                            break;

                    }
                } while (option != Menu.Exit);


            }
            /// <summary>
            /// </summary>
            /// <param name="en"> type of enum</param>
            static public void DisplayMenus(Type en)
            {
                int idx = 0;
                foreach (var item in Enum.GetValues(en))
                {
                    string tmp = item.ToString();
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        if (char.IsUpper(tmp[i]) && i != 0)
                            Console.Write(" {0}", tmp[i].ToString().ToLower());
                        else
                            Console.Write(tmp[i]);
                    }
                    Console.WriteLine(" press " + idx++);
                }
            }
            /// <summary>
            /// </summary>
            /// <param name="dalObject"></param>
            public static void switchAdd(ref IBL.IBL bl)
            {
                Add option;
                Enum.TryParse(Console.ReadLine(), out option);
                int id;
                switch (option)
                {
                    case Add.Station:
                        {
                            Console.WriteLine("enter values to station properties:id, name,latitude,longitude,chargeSlots");
                            double latitude, longitude;
                            int chargeslots;
                            if (int.TryParse(Console.ReadLine(), out id) && double.TryParse(Console.ReadLine(), out latitude) && double.TryParse(Console.ReadLine(), out longitude) && int.TryParse(Console.ReadLine(), out chargeslots))
                            {
                                string name = Console.ReadLine();
                                Location location = new Location();
                                location.Longitude = longitude;
                                location.Latitude = latitude;
                                bl.AddStation(id, name, location, chargeslots);
                            }
                            else
                                Console.WriteLine("The conversion failed and therefore the addition was not made");
                            break;
                        }
                    case Add.Drone:
                        {
                            Console.WriteLine("enter values to drone properties:id,max wheight,station id,model");
                            WeightCategories maxWeight;
                            int stationId;
                            if (int.TryParse(Console.ReadLine(), out id) && Enum.TryParse(Console.ReadLine(), out maxWeight) && int.TryParse(Console.ReadLine(), out stationId))
                            {

                                bl.AddDrone(id, Console.ReadLine(), maxWeight, stationId);
                            }
                            else
                                Console.WriteLine("The conversion failed and therefore the addition was not made");

                            break;
                        }
                    case Add.Customer:
                        {
                            double latitude, longitude;
                            Console.WriteLine("enter values to station properties:id,latitude,longitude, name,phone");
                            if (int.TryParse(Console.ReadLine(), out id) && double.TryParse(Console.ReadLine(), out latitude) && double.TryParse(Console.ReadLine(), out longitude))
                            {
                                Location location = new Location();
                                location.Longitude = longitude;
                                location.Latitude = latitude;
                                bl.AddCustomer(id, Console.ReadLine(), Console.ReadLine(), location);
                            }
                            else
                                Console.WriteLine("The conversion failed and therefore the addition was not made");
                            break;
                        }
                    case Add.Parcel:
                        {
                            Console.WriteLine("enter values to station properties: sender id,target id,weigth,priority");
                            int senderId, targetId;
                            WeightCategories weigth;
                            Priorities priority;
                            if (int.TryParse(Console.ReadLine(), out senderId) && int.TryParse(Console.ReadLine(), out targetId) && Enum.TryParse(Console.ReadLine(), out weigth) && Enum.TryParse(Console.ReadLine(), out priority))
                            {
                                bl.AddParcel(senderId, targetId, weigth, priority);
                            }
                            else
                                Console.WriteLine("The conversion failed and therefore the addition was not made");

                            break;
                        }

                    default:
                        break;

                }
            }
            /// <summary>
            /// </summary>
            /// <param name="dalObject"></param>
            public static void switchUpdate(ref IBL.IBL bl)
            {
                Update option;
                Enum.TryParse(Console.ReadLine(), out option);
                int id;
                switch (option)
                {
                    case Update.DroneName:
                        {
                            Console.WriteLine("enter an id of drone");
                            if (int.TryParse(Console.ReadLine(), out id))
                            {
                                Console.WriteLine("enter the new model name");
                                bl.UpdateDrone(id, Console.ReadLine());
                            }
                            else
                                Console.WriteLine("The conversion failed and therefore the updating was not made");
                            break;
                        }
                    case Update.StationDetails:
                        {
                            int chargeSlots;
                            Console.WriteLine("enter an id of station");
                            if (int.TryParse(Console.ReadLine(), out id))
                            {
                                Console.WriteLine("if you want only update one details press enter instead enter an input");
                                Console.WriteLine("the new  number of chrge slots ");
                                if (!int.TryParse(Console.ReadLine(), out chargeSlots))
                                    chargeSlots = -1;
                                Console.WriteLine("enter the new name");
                                string name = Console.ReadLine();
                                bl.UpdateStation(id, name, chargeSlots);
                            }
                            else
                                Console.WriteLine("The conversion failed and therefore the updating was not made");
                            break;
                        }
                    case Update.CustomerDedails:
                        {
                            Console.WriteLine("enter an id of customer");
                            if (int.TryParse(Console.ReadLine(), out id))
                            {
                                Console.WriteLine("if you want only update one details press enter instead enter an input");
                                Console.WriteLine("enter the new name");
                                string name = Console.ReadLine();
                                Console.WriteLine("enter the new number phone");
                                string phone = Console.ReadLine();
                                bl.UpdateCusomer(id, name, phone);
                            }
                            else
                                Console.WriteLine("The conversion failed and therefore the updating was not made");
                            break;
                        }
                    case Update.SendDroneForCharg:
                        {
                            Console.WriteLine("enter an id of drone");
                            if (int.TryParse(Console.ReadLine(), out id))
                                bl.ChargeOn(id);
                            break;

                        }
                    case Update.RealsDroneFromChargh:
                        {
                            float timeOfCharge;
                            Console.WriteLine("enter an id of drone and time of charge");
                            if (int.TryParse(Console.ReadLine(), out id) && float.TryParse(Console.ReadLine(), out timeOfCharge))
                                bl.ChargeOf(id, timeOfCharge);
                            break;
                        }
                    case Update.AssingParcelToDrone:
                        {
                            Console.WriteLine("enter an id of drone");
                            if (int.TryParse(Console.ReadLine(), out id))
                                bl.ParcelToDrone(id);
                            break;
                        }
                    case Update.CollectParcelByDrone:
                        {
                            Console.WriteLine("enter an id of drone");
                            if (int.TryParse(Console.ReadLine(), out id))
                                bl.PickParcel(id);
                            break;
                        }
                    case Update.SupplyParcelToDestination:
                        {
                            Console.WriteLine("enter an id of parcel");
                            if (int.TryParse(Console.ReadLine(), out id))
                                bl.Destination(id);
                            break;
                        }
                    default:
                        break;

                }
            }
            /// <summary>
            /// Receives input from the user what type of organ to print as well as ID number and calls to the appropriate printing method
            /// </summary>
            /// <param name="dalObject"></param>
            public static void switchDisplay(ref IBL.IBL bl)
            {
                Display option;
                Enum.TryParse(Console.ReadLine(), out option);
                int id;
                switch (option)
                {
                    case Display.Station:
                        {
                            Console.WriteLine("enter an id of station");
                            if (int.TryParse(Console.ReadLine(), out id))
                                Console.WriteLine(bl.GetStation(id));
                            break;
                        }
                    case Display.Drone:
                        {
                            Console.WriteLine("enter an id of drone");
                            if (int.TryParse(Console.ReadLine(), out id))
                                Console.WriteLine(bl.GetDrone(id));
                            break;
                        }
                    case Display.Customer:
                        {
                            Console.WriteLine("enter an id of customer");
                            if (int.TryParse(Console.ReadLine(), out id))
                                Console.WriteLine(bl.GetCustomer(id));
                            break;
                        }
                    case Display.Parcel:
                        {
                            Console.WriteLine("enter an id of parcel");
                            if (int.TryParse(Console.ReadLine(), out id))
                                Console.WriteLine(bl.GetParcel(id));
                            break;
                        }
                    default:
                        break;

                }
            }
            /// <summary>
            /// </summary>
            /// <param name="dalObject"></param>
            public static void switchDisplayList(ref IBL.IBL bl)
            {
                DisplayList option;
                Enum.TryParse(Console.ReadLine(), out option);
                switch (option)
                {
                    case DisplayList.Sations:
                        printList(bl.GetStations());
                        break;
                    case DisplayList.Drones:
                        printList(bl.GetDrones());
                        break;
                    case DisplayList.Customers:
                        printList(bl.GetCustomers());
                        break;
                    case DisplayList.Parcels:
                        printList(bl.GetParcels());
                        break;
                    case DisplayList.AvailableChargingSations:
                        printList(bl.EmptyChangeSlotlList());
                        break;
                    case DisplayList.ParcelNotAssignToDrone:
                        printList(bl.ParcesWithoutDronelList());
                        break;
                    default:
                        break;
                }
            }
            /// <summary>
            /// </summary>
            /// <param name="list">collection for printing</param>
            public static void printList(IEnumerable list)
            {
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }


}
