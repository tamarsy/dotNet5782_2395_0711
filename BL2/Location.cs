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
        /// Location:
        /// Latitude and Longitude
        /// </summary>
        public class Location : Ilocatable
        {
            public Location(double latitude = 0, double longitude = 0)
            {
                Latitude = latitude;
                Longitude = longitude;
                CurrentLocation = this;
            }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public Location CurrentLocation { get; set; }

            public override string ToString()
            {
                return "Latitude: " + Latitude + "\n"
                     + "Longitude: " + Longitude;
            }
        }
    }
}
