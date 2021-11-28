using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// DeliveryCustomer:
    /// Id : the customer id
    /// Name : the customer name
    /// </summary>
    public class DeliveryCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} Name: {Name}";
        }
    }
}
