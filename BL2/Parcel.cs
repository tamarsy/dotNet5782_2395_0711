using System;
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
            public Parcel(int id, DeliveryCustomer senderId, DeliveryCustomer targilId, WeightCategories weight, Priorities priority, DroneDelivery droneDelivery, DateTime deliveryTime, DateTime assignmentTime, DateTime pickUpTime, DateTime supplyTime)
            {
                Id = id;
                SenderId = senderId;
                TargilId = targilId;
                Weight = weight;
                Priority = priority;
                DroneDelivery = droneDelivery;
                DeliveryTime = deliveryTime;
                AssignmentTime = assignmentTime;
                PickUpTime = pickUpTime;
                SupplyTime = supplyTime;

            }
            public int Id { get; set; }
            public DeliveryCustomer SenderId { get; set; }
            public DeliveryCustomer TargilId { get; set; }
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
                     + "SenderId: " + SenderId + "\n"
                     + "TargilId: " + TargilId + "\n"
                     + "Weight: " + Weight + "\n"
                     + "Priority: " + Priority + "\n"
                     + "DroneDelivery" + DroneDelivery + "\n"
                     + "DeliveryTime" + DeliveryTime + "\n"
                     + "AssignmentTime" + AssignmentTime + "\n"
                     + "PickUpTime" + PickUpTime + "\n"
                     + "SupplyTime" + SupplyTime;
            }
        }
    }
}
