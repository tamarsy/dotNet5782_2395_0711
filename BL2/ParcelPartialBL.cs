using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using DalObject;
using IBL.BO;

namespace IBL
{
    public partial class BL : IblParcel 
    {
        public void AddParcel(int sid, int tid, WeightCategories weigth, Priorities priority)
        {
            IDAL.DO.Parcel newParcel = new IDAL.DO.Parcel(0, sid, tid, (IDAL.DO.WeightCategories)weigth, (IDAL.DO.Priorities)priority);
            try
            {
                dalObject.AddParcel(newParcel);
            }
            catch (DalObject.ObjectAlreadyExistException e)
            {
                throw new ObjectAlreadyExistException(e.Message);
        }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public IEnumerable<ParcelToList> GetParcelsNotAssignedToDrone()

        public Parcel GetParcel(int requestedId)
        {
            return dal.GetParcelsNotAssignedToDrone().Select(parcel => mapParcelToList(parcel)); ;
            IDAL.DO.Parcel tempParcel = dalObject.GetParcel(requestedId);
            return new Parcel();
        }

        {
        }

            return parcelsList;
        }

        private Parcel mapParcel(IDAL.DO.Parcel parcel)
        {
            var tmpDrone = drones.FirstOrDefault(drone => drone.Id == parcel.DorneId);
            return new Parcel()
            {
                Id = parcel.Id,
                SenderId = MapCustomerInParcel(dal.GetCustomer(parcel.TargetId)),
                TargilId = MapCustomerInParcel(dal.GetCustomer(parcel.SenderId)),
                Weight = (BO.WeightCategories)parcel.Weigth,
                Priority = (BO.Priorities)parcel.Priority,
                AssignmentTime = parcel.Sceduled,
                PickUpTime = parcel.PickedUp,
                SupplyTime = parcel.Requested,
                DeliveryTime = parcel.Delivered,
                Drone = tmpDrone != default ? mapDroneWithParcel(tmpDrone) : null
            };
        }

            return parcesWithoutDrone;
    }


        public void PickParcel(int id)
        {

}
    }
}
