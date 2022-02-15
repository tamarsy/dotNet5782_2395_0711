using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    /// <summary>
    /// Id: Id of the parcel
    /// SenderId: The sender customer id
    /// GetterId: The getter customer id
    /// Weight: the parcel Weight
    /// Priority: the parcel Priority
    /// Droneld: the Drone id if the parcel is schedulet
    /// Requested: the parcel Requested time if exists
    /// Schedulet: the parcel Schedulet time if exists
    /// PickedUp: the parcel PickedUp time if exists
    /// Delivered: the parcel Delivered time if exists
    /// IsDelete: true if the customer is delete
    /// </summary>
    public struct Parcel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int GetterId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DateTime Requested { get; set; }
        public int? Droneld { get; set; }
        public DateTime? Schedulet { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public bool IsDelete { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + "\n"
                + "SenderId: " + SenderId + "\n"
                + "Getter: " + GetterId + "\n"
                 + "Weight: " + Weight + "\n"
                 + "Priority: " + Priority + "\n"
                 + "ReQuested: " + Requested + "\n"
                + "Droneld: " + Droneld + "\n"
                 + "Schedulet: " + Schedulet + "\n"
                 + "PickedUp: " + PickedUp + "\n"
                 + "Delivered" + Delivered;
        }
    }
}

