﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public struct DroneCharge
    {
        public int DroneId { get; set; }
        public int StationId { get; set; }
        public DateTime StartTime { get; set; }
        public override string ToString()
        {
            return "Drone Id: " + DroneId + "\n" + "Station Id: " + StationId;
        }
    }
}

