using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
         class Customer
        {
            public Customer(int id, string name, string phone, Location currentLocation, List<CustomerDelivery> fromCustomer,
                List<CustomerDelivery> toCustomer)
            {
                Id = id;
                Name = name;
                Phone = phone;
                Location CurrentLocation = currentLocation;
                List<CustomerDelivery> FromCustomer = fromCustomer;
                List<CustomerDelivery> ToCustomer = toCustomer;
            }
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location CurrentLocation { get; set; }
            public List<CustomerDelivery> FromCustomer { get; set; }
            public List<CustomerDelivery> ToCustomer { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Name: " + Name + "\n"
                    + "Phone: " + Phone + "\n"
                    + "CurrentSiting: " + CurrentLocation + "\n"
                    + "FromCustomer: " + FromCustomer + "\n"
                    + "ToCustomer: " + ToCustomer;
            }
        }


    }
}
