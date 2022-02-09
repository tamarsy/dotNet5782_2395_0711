using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    /// <summary>
    /// Id: Id of the customer
    /// Name: The customer model
    /// Phone: The customer phone
    /// Longitude: the longitude customer location
    /// Lattitude: the Lattitude customer location
    /// IsDelete: true if the customer is delete
    /// </summary>
    public struct Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public bool IsDelete { get; set; }
        public override string ToString()
        {
            return "Id: " + Id + "\n"
                + "Name: " + Name + "\n"
                + "Phone: " + Phone + "\n"
                 + "Longitude: " + Longitude + "\n"
                 + "Lattitude: " + Lattitude;
        }
    }
}

