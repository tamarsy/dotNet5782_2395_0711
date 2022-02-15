using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Id: parcel id
    /// Weight: parcel Weight
    /// Priority: parcel Priority
    /// StatusParcel: parcel status
    /// Collecting Location = Location of Sender
    /// Destination Location = Location of Getter
    /// Distance: the current step distance
    /// Sender- Sender id and name
    /// Getter- Getter id and name
    /// </summary>
    public class ParcelDelivery
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public bool StatusParcel { get; set; }
        public Location Collecting { get; set; }
        public Location DeliveryDestination { get; set; }
        public double Distance { get; set; }
        public DeliveryCustomer Sender { get; set; }
        public DeliveryCustomer Getter { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + "\n"
                + "Weight: " + Weight + "\n"
                + "Priority: " + Priority + "\n"
                + "StatusParcel: " + StatusParcel + "\n"
                + "Collecting Location" + "\n" + Collecting.ToString() + "\n"
                + "Destination Location" + "\n" + DeliveryDestination.ToString() + "\n"
                + "Distance:" + Distance + "\n"
                + "Sender-" + "\n" + Sender.ToString() + "\n"
                + "Getter-" + "\n" + Getter.ToString();
        }
    }
}
