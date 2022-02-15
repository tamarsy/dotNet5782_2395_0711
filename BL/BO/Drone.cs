using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    /// <summary>
    /// Drone:
    /// Id : the drone id
    /// Model : the drone model
    /// MaxWeight : the drone max weight to carry
    /// BatteryStatuses : the drone battery dtatuses
    /// DroneStatuses : the drone statuses (vacant, maintanance, sending)
    /// CurrentLocation : the drone current location
    /// Parcel : the pacel in drone details:
    /// Parcel - Id : parcel id
    /// Parcel - Weight : the parcel weight 
    /// Parcel - Priority : the parcel priority
    /// Parcel - StatusParcel : the parcel status
    /// Parcel - Collecting : the customer sender location
    /// Parcel - DeliveryDestination : the customer getter location
    /// Parcel - Distance : the distance for the delivery
    /// Parcel - SenderId : the Sender id
    /// Parcel - GetterId : the Getter id  
    /// </summary>
    public class Drone : Ilocatable
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public double BatteryStatuses { get; set; }
        public DroneStatuses DroneStatuses { get; set; }
        public Location CurrentLocation { get; set; }
        public ParcelDelivery Parcel { get; set; }
        public override string ToString()
        {
            string s = "Id: " + Id + "\n"
                + "Model: " + Model + "\n"
                + "MaxWeight: " + MaxWeight + "\n"
                + "BatteryStatuses: " + BatteryStatuses + "\n"
                + "DroneStatuses: " + DroneStatuses + "\n"
                + "CurrentLocation- " + "\n" + CurrentLocation.ToString() + "\n";
            if (Parcel != default)
                s += "Parcel- " + "\n" + Parcel.ToString();
            return s;
        }
    }
}
