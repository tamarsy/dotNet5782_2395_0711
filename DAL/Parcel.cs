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
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargilId { get; set; }
            public WeightCategories Weight { get; set; }
            public priorities priority { get; set; }
            public DateTime ReQuested { get; set; }
            public int droneld { get; set; }
            public DateTime Schedulet { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

            public override string ToString()
            {
                return "Parcel";
            }
        }
    }
}
