using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// DroneDelivery:
    /// Id : the drone id
    /// BatteryStatuses : the drone Battery Statuses
    /// CurentLocation : the drone current location
    /// </summary>
    public class DroneDelivery
    {
        public int Id { get; set; }
        public Double BatteryStatuses { get; set; }
        public Location CurrentLocation { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + "\n"
                + "BatteryStatuses: " + BatteryStatuses + "\n"
                + "CurrentLocation: " + CurrentLocation.ToString();
        }
    }
}
