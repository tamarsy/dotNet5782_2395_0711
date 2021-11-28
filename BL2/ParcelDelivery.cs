using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelDelivery
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public bool StatusParcel { get; set; }
        public Location Collecting { get; set; }
        public Location DeliveryDestination { get; set; }
        public double Distance { get; set; }
        public DeliveryCustomer SenderId { get; set; }
        public DeliveryCustomer GetterId { get; set; }
        public override string ToString()
        {
            return "Id: " + Id + "\n"
                 + "Sender Id: " + SenderId.ToString() + "\n"
                 + "Getter Id: " + GetterId.ToString() + "\n"
                 + "Weight: " + Weight + "\n"
                 + "Priority: " + Priority + "\n"
                 + "StatusParcel: " + StatusParcel + "\n"
                 + "Delivery Destination: " + DeliveryDestination.ToString() + "\n"
                 + "Distance: " + Distance + "\n";
        }
    }
}
