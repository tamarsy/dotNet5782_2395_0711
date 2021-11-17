using System;
using System.Collections.Generic;
using System.Text;

namespace IBL.BO
{
    public class Delivery
    {
        public int id { get; set; }
        public Priorities Priority { get; set; }
        public WeightCategories Weight { get; set; }
        public DeliveryCustomer Sender { get; set; }
        public DeliveryCustomer Target { get; set; }
        public Location SenderLocation { get; set; }
        public Location TargetLocation { get; set; }
        public double Distance { get; set; }
    }
}
