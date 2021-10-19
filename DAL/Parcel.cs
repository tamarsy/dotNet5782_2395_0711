using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Parcel
        {
            public Parcel(int id, int senderId, int targilId, WeightCategories weight, Priorities priority,
                DateTime reQuested, int droneld, DateTime schedulet, DateTime pickedUp, DateTime delivered)
            {
                Id = id;
                SenderId = senderId;
                TargilId = targilId;
                Weight = weight;
                Priority = priority;
                ReQuested = reQuested;
                Droneld = droneld;
                Schedulet = schedulet;
                PickedUp = pickedUp;
                Delivered = delivered;
            }
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargilId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime ReQuested { get; set; }
            public int Droneld { get; set; }
            public DateTime Schedulet { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

            public override string ToString()
            {
                return "id: " + Id;
            }
        }
    }
}
