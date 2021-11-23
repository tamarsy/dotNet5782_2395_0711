using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class StationToList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int NumOfEmptyChargeSlots { get; set; }
            public int NumOfCatchChargeSlots { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Name: " + Name + "\n"
                     + "NumOfEmptyChargeSlots: " + NumOfEmptyChargeSlots + "\n"
                     + "NumOfCatchChargeSlots: " + NumOfCatchChargeSlots;
            }
        }
    }
}
