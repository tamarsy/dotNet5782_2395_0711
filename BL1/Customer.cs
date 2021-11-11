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
            public Customer(int id, string name, string phone, Siting currentSiting, List<delivery> fromCustomer,
                List<delivery> toCustomer)
            {
                Id = id;
                Name = name;
                Phone = phone;
                Siting CurrentSiting = currentSiting;
                List<delivery> FromCustomer = fromCustomer;
                List<delivery> ToCustomer = toCustomer;
            }
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Siting CurrentSiting { get; set; }
            public List<delivery> FromCustomer { get; set; }
            public List<delivery> ToCustomer { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Name: " + Name + "\n"
                    + "Phone: " + Phone + "\n"
                    + "CurrentSiting: " + CurrentSiting + "\n"
                     + "FromCustomer: " + FromCustomer + "\n"
                     + "ToCustomer: " + ToCustomer;
            }
        }


    }
}
