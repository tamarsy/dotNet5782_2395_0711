using System;
using System.Collections.Generic;
using IBL;
using IBL.BO;


namespace ConsoleUI_BL
{
    class Program
    {
        private static IBL.IBL bl;
        static void Main(string[] args)
        {
            bl = new IBL.BL();
            ShowMenu();
        }

        enum MenuOptions { Exit, Add, Update, Show_One, Show_List }
        enum EntityOptions { Exit, BaseStation, Drone, Customer, Parcel }
        enum UpdateOptions { Exit, Assignment, Pickedup, Delivery, Recharge, Release }
        enum ListOptions { Exit, BaseStations, Drones, Customers, Parcels, UnAssignmentParcels, AvailableChargingStations }
        private static void ShowMenu()
        {
            MenuOptions menuOption;
            do
            {
                Console.WriteLine("Welcome!");
                Console.WriteLine("option:\n 1-Add,\n 2-Update,\n 3-Show item,\n 4-Show list,\n 0-Exit");
                menuOption = (MenuOptions)int.Parse(Console.ReadLine());
                switch (menuOption)
                {
                    case MenuOptions.Add:
                        MenuAddOptions();
                        break;
                    case MenuOptions.Update:
                        MenuUpdateOptions();
                        break;
                    case MenuOptions.Show_One:
                        MenuShowOneOptions();
                        break;
                    case MenuOptions.Show_List:
                        MenuShowListOptions();
                        break;
                    case MenuOptions.Exit:
                        break;
                }
            } while (menuOption != MenuOptions.Exit);
        }

        private static void MenuShowListOptions()
        {
            Console.WriteLine(
                "List option:\n 1-Base Stations,\n 2-Drones,\n 3-Customers,\n 4-Parcels,\n 5-UnAssignment Parcels,\n 6-Available Charging Stations,\n 0-Exit");
            ListOptions listOptions = (ListOptions)int.Parse(Console.ReadLine());
            switch (listOptions)
            {
                case ListOptions.BaseStations:
                    //foreach (var item in bl.GetBaseStations())
                    //{
                    //    Console.WriteLine(item);
                    //}
                    ShowList(bl.GetBaseStations());
                    break;
                case ListOptions.Drones:
                    foreach (var item in bl.GetDrones())
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ListOptions.Customers:
                    foreach (var item in bl.GetCustomers())
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ListOptions.Parcels:
                    foreach (var item in bl.GetParcels())
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ListOptions.UnAssignmentParcels:
                    foreach (var item in bl.UnAssignmentParcels())
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ListOptions.AvailableChargingStations:
                    foreach (var item in bl.AvailableChargingStations())
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ListOptions.Exit:
                    break;
            }
        }

        private static void MenuShowOneOptions()
        {
            EntityOptions entityOption;
            Console.WriteLine("View item option:\n 1-Base Station,\n 2-Drone,\n 3-Customer,\n 4-Parcel\n, 0-Exit");
            entityOption = (EntityOptions)int.Parse(Console.ReadLine());
            Console.WriteLine($"Enter a requested {entityOption} id");
            int requestedId;
            int.TryParse(Console.ReadLine(), out requestedId);
            switch (entityOption)
            {
                case EntityOptions.BaseStation:
                    BaseStation baseStation = bl.GetBaseStation(requestedId);
                    Console.WriteLine(baseStation);
                    ShowList(baseStation.DronesInCharging);
                    break;
                case EntityOptions.Drone:
                    Console.WriteLine(bl.GetDrone(requestedId));
                    break;
                case EntityOptions.Customer:
                    Customer customer = bl.GetCustomer(requestedId);
                    Console.WriteLine(customer);
                    break;
                case EntityOptions.Parcel:
                    Console.WriteLine(bl.GetParcel(requestedId));
                    break;
                case EntityOptions.Exit:
                    break;
            }
        }

        private static void MenuUpdateOptions()
        {
            Console.WriteLine("Update option:\n 1-Assignment,\n 2-Pickedup,\n 3-Delivery,\n 4-Recharge,\n 5-Release,\n 0-Exit");
            UpdateOptions updateOptions;
            updateOptions = (UpdateOptions)int.Parse(Console.ReadLine());
            int parcelId;
            int droneId;
            switch (updateOptions)
            {
                case UpdateOptions.Assignment:
                    Console.WriteLine("Enter IDs for parcel and drone:");
                    parcelId = int.Parse(Console.ReadLine());
                    droneId = int.Parse(Console.ReadLine());
                    bl.AssignmentParcelToDrone(parcelId, droneId);
                    break;
                case UpdateOptions.Pickedup:
                    Console.WriteLine("Enter parcel Id:");
                    parcelId = int.Parse(Console.ReadLine());
                    bl.PickedupParcel(parcelId);
                    break;
                case UpdateOptions.Delivery:

                    break;
                case UpdateOptions.Recharge:
                    Console.WriteLine("Enter IDs for drone and base station:");
                    droneId = int.Parse(Console.ReadLine());
                    var baseStationId = int.Parse(Console.ReadLine());
                    bl.SendDroneToRecharge(droneId, baseStationId);
                    break;
                case UpdateOptions.Release:
                    Console.WriteLine("Enter ID for drone:");
                    droneId = int.Parse(Console.ReadLine());
                    bl.ReleaseDroneFromRecharge(droneId);
                    break;
                case UpdateOptions.Exit:
                    break;
            }
        }

        private static void MenuAddOptions()
        {
            EntityOptions entityOption;
            Console.WriteLine("Adding option:\n 1-Base Station,\n 2-Drone,\n 3-Customer,\n 4-Parcel");
            entityOption = (EntityOptions)int.Parse(Console.ReadLine());
            switch (entityOption)
            {
                case EntityOptions.BaseStation:
                    Console.WriteLine("Enter Name and Number of charging positions:");
                    string stationName = Console.ReadLine();
                    int positions;
                    int.TryParse(Console.ReadLine(), out positions);
                    bl.AddBaseStation(stationName, positions);
                    break;
                case EntityOptions.Drone:
                    bl.AddDrone();
                    break;
                case EntityOptions.Exit:
                    break;
            }
            return;
        }

        private static void ShowList<T>(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }
    }
}
