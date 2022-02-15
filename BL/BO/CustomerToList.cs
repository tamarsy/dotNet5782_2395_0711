using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    /// <summary>
    /// CustomerToList:
    /// Id : the customer id
    /// Name : the customer name
    /// Phone : the customer phone
    /// NumOfParcelsDefined : num Of Parcels Defined
    /// NumOfParcelsAscribed : num Of Parcels Ascribed
    /// NumOfParcelsCollected : num Of Parcels Collected
    /// NumOfParcelsSupplied : num Of Parcels Supplied
    /// </summary>
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

