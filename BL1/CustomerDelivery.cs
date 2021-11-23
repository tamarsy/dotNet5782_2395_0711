using System;
using System.Collections.Generic;
using System.Text;

namespace IBL.BO
{
    /// <summary>
    /// CustomerDelivery:
    /// </summary>
    public class CustomerDelivery
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses Status { get; set; }
        public DeliveryCustomer Customer { get; set; }
        public override string ToString()
        {
            return "Id: " + Id + "\n"
                + "Weight: " + Weight + "\n"
                + "Priority: " + Priority + "\n"
                + "Status: " + Status + "\n"
                + "Customer: " + Customer;
        }
    }
}