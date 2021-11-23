using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        /// <summary>
        /// the class customer
        /// id = the customer id
        /// name = the customer name
        /// phone = the customer phone
        /// currentLoction = the customer Location(Latitude and Longitude)
        /// fromCustomer = a list with the dalivery from the customer
        /// ToCustomer = a list with the dalivery to the customer
        /// </summary>
        public class Customer:Ilocatable
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location CurrentLocation { get; set; }
            public List<CustomerDelivery> FromCustomer { get; set; }
            public List<CustomerDelivery> ToCustomer { get; set; }
            public override string ToString()
            {
                return $"Id: {Id}\n" +
                    $"Name: { Name}\n" +
                    $"Phone: { Phone}\n" +
                    $"CurrentSiting: { CurrentLocation}\n" +
                    $"FromCustomer: { FromCustomer}\n" +
                    $"ToCustomer: { ToCustomer}";
            }
        }
    }
}
