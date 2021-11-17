using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneDelivery
    {
        public DroneDelivery(int id, double batteryStatuses, Location curentLocation)
        {
            Id = id;
            BatteryStatuses = batteryStatuses;
            CurentLocation = curentLocation;
        }
        public int Id { get; set; }
        public Double BatteryStatuses { get; set; }
        public Location CurentLocation { get; set; }
          public override string ToString()
            {
                return "Id: " + Id + "\n"
                     + "BatteryStatuses: " + BatteryStatuses + "\n"
                     + "CurentLocation: " + CurentLocation + "\n"
            }
    }

}
