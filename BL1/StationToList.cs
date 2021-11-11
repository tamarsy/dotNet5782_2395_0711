using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class StationToList
        {
            public Station(int id, int name, int numOfChargeSlots)
            {
                Id = id;
                Name = name;
                NumOfChargeSlots = numOfChargeSlots;
            }
            public int Id { get; set; }
            public int Name { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public int NumOfChargeSlots { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Name: " + Name + "\n"
                     + "NumOfChargeSlots: " + NumOfChargeSlots;
            }
        }
    }
}
