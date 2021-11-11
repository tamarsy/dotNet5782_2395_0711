using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BO
    {
        public class Location
        {
            public Location(int latitude, int longitude)
            {
                Latitude = latitude;
                Longitude = longitude;
            }
            public int Latitude {get;set;}
            public int Longitude { get;set; }
        }
    }
}
