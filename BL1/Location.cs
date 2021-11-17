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
            public Location(double latitude, double longitude)
            {
                Latitude = latitude;
                Longitude = longitude;
            }
            public Location()
            {

            }
            public double Latitude {get;set;}
            public double Longitude { get;set; }
        }
    }
}
