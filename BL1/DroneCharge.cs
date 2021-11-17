using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
    {
       class DroneCharge
    {
        public DroneCharge(int id, double batteryStatuses)
        {
            Id = id;
            BatteryStatuses = batteryStatuses;
        }
        public int Id { get; set; }
        public double BatteryStatuses { get; set; }
           public override string ToString()
            {
                return "Id: " + Id + "\n"
                     + "BatteryStatuses: " + BatteryStatuses + "\n"
            }
    }
    }
}
