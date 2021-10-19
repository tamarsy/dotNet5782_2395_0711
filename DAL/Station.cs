using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            /// <summary>
            /// Station class
            /// </summary>
            /// <param name="Id">Station id</param>
            /// <param name="Name">Station name</param>
            /// <param name="Longitude">Station Longitude</param>
            /// <param name="Lattitude">Station Lattitude</param>
            /// <param name="ChargeSlot">Station ChargeSlot</param>
            public Station(int id, int name, double longitude, double lattitude, int chargeSlot)
            {
                Id = id;
                Name = name;
                Longitude = longitude;
                Lattitude = lattitude;
                ChargeSlot = chargeSlot;
            }
            public int Id { get; set; }
            public int Name { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public int ChargeSlot { get; set; }
            public override string ToString()
            {
                return "Id: " + Id;
            }
        }
    }
}
