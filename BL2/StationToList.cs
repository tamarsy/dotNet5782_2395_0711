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
            private int numOfChargeSlot1;
            private int numOfChargeSlot2;

            public StationToList()
            {
            }

            public StationToList(int id, string name, int numOfChargeSlot1, int numOfChargeSlot2)
            {
                Id = id;
                Name = name;
                this.numOfChargeSlot1 = numOfChargeSlot1;
                this.numOfChargeSlot2 = numOfChargeSlot2;
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public int NumOfEmptyChargeSlots { get; set; }
            public int NumOfCatchChargeSlots { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Name: " + Name + "\n"
                     + "NumOfEmptyChargeSlots: " + NumOfEmptyChargeSlots + "\n"
                     + "NumOfCatchChargeSlots: " + NumOfCatchChargeSlots + "\n";
            }
        }
    }
}
