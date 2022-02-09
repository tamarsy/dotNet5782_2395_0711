using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
            string res = "Id: " + Id + "\n"
                 + "SenderId: " + SenderId + "\n"
                 + "GetterId: " + GetterId + "\n"
                 + "Weight: " + Weight + "\n"
                 + "Priority: " + Priority;
            if (DroneDelivery != default)
                res += "\n" + "Drone Delivery :" + DroneDelivery.ToString();
            if (!AssignmentTime.Equals(default))
            {
                res += "\n" + "Assignment Time :" + AssignmentTime.ToString();
                if (!PickUpTime.Equals(default))
                {
                    res += "\n" + "Pick Up Time :" + PickUpTime.ToString();
                    if (!DeliveryTime.Equals(default))
                    {
                        res += "\n" + "Delivery Time :" + DeliveryTime.ToString();
                        if (!SupplyTime.Equals(default))
                            res += "\n" + "Supply Time :" + SupplyTime.ToString();
                    }
                }
            }
            return res;
        }
    }
}
