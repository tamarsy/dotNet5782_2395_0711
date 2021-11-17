using System;
using System.Collections.Generic;
using System.Text;

namespace IBL.BO
{
    public class CustomerDelivery
    {
        public int Id { get; set; }
        public  Weightcategories Weight { get; set; }
        public Priorities priority { get; set; }
        public parcelStatus Status { get; set; }
        public int Sender { get; set; }
        public int Target { get; set; }
    }
}
