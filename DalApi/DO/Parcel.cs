using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public struct Parcel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int Getter { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DateTime? ReQuested { get; set; }
        public int? Droneld { get; set; }
        public DateTime? Schedulet { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + "\n"
                + "SenderId: " + SenderId + "\n"
                + "Getter: " + Getter + "\n"
                 + "Weight: " + Weight + "\n"
                 + "Priority: " + Priority + "\n"
                 + "ReQuested: " + ReQuested + "\n"
                + "Droneld: " + Droneld + "\n"
                 + "Schedulet: " + Schedulet + "\n"
                 + "PickedUp: " + PickedUp + "\n"
                 + "Delivered" + Delivered;
        }
    }
}

