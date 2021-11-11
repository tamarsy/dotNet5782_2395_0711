using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
       class DroneToList
        {
            public DroneToList(int id, string model, WeightCategories maxWeight)
            {
                Id = id;
                Model = model;
                MaxWeight = maxWeight;
                //double BatteryStatuses = batteryStatuses;
                //DroneStatuses DroneStatuses = droneStatuses;
                //Siting CurrentSiting = currentSiting;
                //int? NumOfParcel = numOfParcel;
            }
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double BatteryStatuses { get; set; }
            public DroneStatuses DroneStatuses { get; set; }
            public Siting CurrentSiting { get; set; }
            public int? NumOfParcel { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Model: " + Model + "\n"
                    + "MaxWeight: " + MaxWeight + "\n"
                    + "BatteryStatuses: " + BatteryStatuses + "\n"
                    + "DroneStatuses: " + DroneStatuses + "\n"
                    + "CurrentSiting: " + CurrentSiting + "\n"
                    + "NumOfParcel: " + NumOfParcel;
            }
        }
    }
}
