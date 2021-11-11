﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Drone
        {
            public Drone(int id, string model, WeightCategories maxWeight, double batteryStatuses,
              DroneStatuses droneStatuses, Location currentLocation, NumOfParcel numOfParcel)
            {
                Id = id;
                Model = model;
                MaxWeight = maxWeight;
                double BatteryStatuses = batteryStatuses;
                DroneStatuses DroneStatuses = droneStatuses;
                Location CurrentLocation = currentLocation;
                int? NumOfParcel = numOfParcel;
            }
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double BatteryStatuses { get; set; }
            public DroneStatuses DroneStatuses { get; set; }
            public Location CurrentLocation { get; set; }
            public int? NumOfParcel { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Model: " + Model + "\n"
                    + "MaxWeight: " + MaxWeight + "\n"
                    + "BatteryStatuses: " + BatteryStatuses + "\n"
                    + "DroneStatuses: " + DroneStatuses + "\n"
                    + "CurrentSiting: " + CurrentLocation + "\n"
                    + "NumOfParcel: " + NumOfParcel;
            }
        }
    }
}
