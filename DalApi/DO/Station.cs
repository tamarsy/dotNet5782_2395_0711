using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public int ChargeSlot { get; set; }
        public override string ToString()
        {
            return "Id: " + Id + "\n"
                + "Name: " + Name + "\n"
                 + "Longitude: " + Longitude + "\n"
                 + "Lattitude: " + Lattitude + "\n"
                 + "ChargeSlot: " + ChargeSlot;
        }
    }
}

