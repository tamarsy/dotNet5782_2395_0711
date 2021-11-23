using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class CustomerToList
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public int NumOfParcelsDefined { get; set; }
            public int NumOfParcelsAscribed { get; set; }
            public int NumOfParcelsCollected { get; set; }
            public int NumOfParcelsSupplied { get; set; }

            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Name: " + Name + "\n"
                    + "Phone: " + Phone;
            }
        }
    }
}
