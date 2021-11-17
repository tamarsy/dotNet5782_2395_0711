using System;
using System.Collections.Generic;
using System.Text;

namespace IBL.BO
{
    class CustomerDelivery
    {
        public CustomerDelivery(int id, WeightCategories weight, Priorities priority,  ParcelStatuses status,  int sender, int target)
        {
            Id = id;
            Weight = weight;
            Priority = priority;
            Status = status;
            Sender = sender;
            Target = target;

        }
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses Status { get; set; }
        public int Sender { get; set; }
        public int Target { get; set; }
    }
}
