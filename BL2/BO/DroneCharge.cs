using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// DroneCharge:
    /// Id: the drone id
    /// BatteryStatuses: the drone battery statuses
    /// </summary>
    public class DroneCharge
    {
        public int Id { get; set; }
        public double BatteryStatuses { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + "\n"
                + "BatteryStatuses:" + BatteryStatuses;
        }
    }
}
