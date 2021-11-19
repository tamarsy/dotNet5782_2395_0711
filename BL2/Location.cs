using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Location
        {
            public Location(double latitude = 0, double longitude = 0)
            {
                Latitude = latitude;
                Longitude = longitude;
                CurrentLocation.Latitude = latitude;
                CurrentLocation.Longitude = longitude;
            }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            Location CurrentLocation { get; set; }

            public override string ToString()
            {
                return "Latitude: " + Latitude + "\n"
                     + "Longitude: " + Longitude;
            }
        }
    }
}
