using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Station class
/// </summary>
/// <param name="Id">Station id</param>
/// <param name="Name">Station name</param>
/// <param name="Longitude">Station Longitude</param>
/// <param name="Lattitude">Station Lattitude</param>
/// <param name="ChargeSlot">Station ChargeSlot</param>


namespace DO
{
    /// <summary>
    /// Id: Id of the station
    /// Name: the station name 
    /// Longitude: the longitude customer location
    /// Lattitude: the Lattitude customer location
    /// ChargeSlot: num of charge slot in station
    /// IsDelete: true if the customer is delete
    /// </summary>
    public struct Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public int ChargeSlot { get; set; }
        public bool IsDelete { get; set; }

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

