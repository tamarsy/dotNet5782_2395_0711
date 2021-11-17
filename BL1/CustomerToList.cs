using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class CustomerToList
        {
            public CustomerToList(int id, string name, string phone)
            {
                Id = id;
                Name = name;
                Phone = phone;
                
            }
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Name: " + Name + "\n"
                    + "Phone: " + Phone;
            }
        }
    }
}
