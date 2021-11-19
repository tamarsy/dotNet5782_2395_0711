using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelDelivery
    {
        public ParcelDelivery(int id, bool statusParcel, Priorities priority, WeightCategories weight, DeliveryCustomer senderId,
            DeliveryCustomer getterId, Location collecting, Location deliveryDestination, double distance)
        {
            Id = id;
            StatusParcel = statusParcel;
            Priority = priority;
            Weight = weight;
            SenderId = senderId;
            GetterId = getterId;
            Collecting = collecting;
            DeliveryDestination = deliveryDestination;+
            Distance = distance;
        }
        public int Id { get; set; }
        public bool StatusParcel { get; set; }
        public Priorities Priority { get; set; }
        public WeightCategories Weight { get; set; }
        public DeliveryCustomer SenderId { get; set; }
        public DeliveryCustomer GetterId { get; set; }
        public Location Collecting { get; set; }
        public Location DeliveryDestination { get; set; }
        public double Distance { get; set; }
    }
}
