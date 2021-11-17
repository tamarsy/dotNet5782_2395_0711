using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public IEnomerable<CustomerDelievery> ReceivedParcels { get; set; }
        public IEnomerable<CustomerDelievery> ShippedParcels { get; set; }
        public Location Location { get; set; }
        public override string toString()
        {
            return $"customer #{id}: {Name}, tel-{Phone}, location-{Location}";
        }
    }
}
