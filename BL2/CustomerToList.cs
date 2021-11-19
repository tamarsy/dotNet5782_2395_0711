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
            public CustomerToList(int id, string name, string phone, int numOfParcelsDefined, int numOfParcelsAscribed, int numOfParcelsCollected, int numOfParcelsSupplied)
            {
                Id = id;
                Name = name;
                Phone = phone;
                NumOfParcelsDefined = numOfParcelsDefined;
                NumOfParcelsAscribed = numOfParcelsAscribed;
                NumOfParcelsCollected = numOfParcelsCollected;
                NumOfParcelsSupplied = numOfParcelsSupplied;
            }

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
                    + "Phone: " + Phone + "\n"
                    + "NumOfParcelsDefined" + NumOfParcelsDefined + "\n"
                    + "NumOfParcelsAscribed" + NumOfParcelsAscribed + "\n"
                    + "NumOfParcelsCollected" + NumOfParcelsCollected + "\n"
                    + "NumOfParcelsSupplied" + NumOfParcelsSupplied;
            }
        }
    }
}
