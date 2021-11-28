using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        /// <summary>
        /// DroneToList:
        /// Id : the dorone id 
        /// Model : the dorone model
        /// MaxWeight : the dorone max weight (easy, medium, heavy)
        /// BatteryStatuses : the dorone Battery Statuses
        /// DroneStatuses : the dorone Statuses (vacant, maintanance, sending)
        /// CurrentLocation : the dorone current location
        /// NumOfParcel : the parcel id
        /// </summary>
        public class DroneToList:Ilocatable
        {
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
                    + "CurrentLocation: " + CurrentLocation + "\n"
                    + "deliveryId: " + NumOfParcel + "\n";
            }
        }
    }
}