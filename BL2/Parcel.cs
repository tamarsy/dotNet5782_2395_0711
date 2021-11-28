﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Parcel
        {
            public int Id { get; set; }
            public DeliveryCustomer SenderId { get; set; }
            public DeliveryCustomer GetterId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DroneDelivery DroneDelivery { get; set; }
            public DateTime DeliveryTime { get; set; }
            public DateTime AssignmentTime { get; set; }
            public DateTime PickUpTime { get; set; }
            public DateTime SupplyTime { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                     + "Sender Id: " + SenderId.ToString() + "\n"
                     + "Getter Id: " + GetterId.ToString() + "\n"
                     + "Weight: " + Weight + "\n"
                     + "Priority: " + Priority + "\n"
                     + "DroneDelivery" + DroneDelivery.ToString() + "\n"
                     + "DeliveryTime" + DeliveryTime + "\n"
                     + "AssignmentTime" + AssignmentTime + "\n"
                     + "PickUpTime" + PickUpTime + "\n"
                     + "SupplyTime" + SupplyTime;
            }
        }
    }
}
