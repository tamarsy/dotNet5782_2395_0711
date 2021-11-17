using System;
using System.Collections.Generic;
using System.Text;

namespace IBL.BO
{
    public class CustomerDelivery
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities priority { get; set; }
        public ParcelStatuses Status { get; set; }
        public int Sender { get; set; }
        public int Target { get; set; }
    }
}
