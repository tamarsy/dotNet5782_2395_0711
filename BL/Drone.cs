using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Drone
        {
            public Drone(int id, string model, WeightCategories maxWeight, double batteryStatuses,
              DroneStatuses droneStatuses, Location currentLocation, ParcelDelivery parcel)
            {
                Id = id;
                Model = model;
                MaxWeight = maxWeight;
                BatteryStatuses = batteryStatuses;
                DroneStatuses = droneStatuses;
                CurrentLocation = currentLocation;
                Parcel = parcel;
            }
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double BatteryStatuses { get; set; }
            public DroneStatuses DroneStatuses { get; set; }
            public Location CurrentLocation { get; set; }
            public ParcelDelivery Parcel { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Model: " + Model + "\n"
                    + "MaxWeight: " + MaxWeight + "\n"
                    + "BatteryStatuses: " + BatteryStatuses + "\n"
                    + "DroneStatuses: " + DroneStatuses + "\n"
                    + "CurrentSiting: " + CurrentLocation + "\n"
                    + "ParcelDelivery: " + ParcelDelivery;
            }
        }
    }
}
