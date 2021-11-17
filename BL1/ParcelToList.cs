using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class ParcelTOList
        {
            public ParcelTOList(int id, int senderId, int targilId, WeightCategories weight,
                Priorities priority,DroneStatuses parcelStatuses)
            {
                Id = id;
                SenderId = senderId;
                TargilId = targilId;
                Weight = weight;
                Priority = priority;
                ParcelStatuses = parcelStatuses;
            }
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargilId { get; set; }
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
                    + "TargilId: " + TargilId + "\n"
                     + "Weight: " + Weight + "\n"
                     + "Priority: " + Priority+"\n"
                     +"ParcelStatuses"+ParcelStatuses;
            }
        }
    }
}
