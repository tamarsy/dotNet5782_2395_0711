using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class ParcelToList
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int GetterId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses ParcelStatuses { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Id: " + Id + "\n"
                + "SenderId: " + SenderId + "\n"
                + "GetterId: " + GetterId + "\n"
                 + "Weight: " + Weight + "\n"
                 + "Priority: " + Priority + "\n"
                 + "Parcel Statuses: " + ParcelStatuses;
        }
    }
}
