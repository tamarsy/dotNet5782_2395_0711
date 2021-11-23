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
    partial class BL
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


        public Parcel GetParcel(int requestedId)
        {
            IDAL.DO.Parcel tempParcel = dalObject.GetParcel(requestedId);
            return new Parcel();
        }


        public IEnumerable<ParcelToList> ParcelsList()
        {
            List<ParcelToList> parcelsList = new List<ParcelToList>();

            foreach (var p in dalObject.ParcelList())
            {
                //drone sttus?????????????????????????????????????????? in ParceltoList
                ParcelToList newParcel = new ParcelToList(p.Id, p.SenderId, p.Getter, (BO.WeightCategories)p.Weight, (BO.Priorities)p.Priority, DroneStatuses.vacant);
                parcelsList.Add(newParcel);
            }

            return parcelsList;
        }

        public IEnumerable<ParcelToList> ParcesWithoutDronelList()
        {
            List<ParcelToList> parcesWithoutDrone = new List<ParcelToList>();

            foreach (var p in dalObject.ParcesWithoutDronelList())
            {
                //drone sttus?????????????????????????????????????????? in ParceltoList
                ParcelToList newParcel = new ParcelToList(p.Id, p.SenderId, p.Getter, (BO.WeightCategories)p.Weight, (BO.Priorities)p.Priority, DroneStatuses.vacant);
                parcesWithoutDrone.Add(newParcel);
            }

            return parcesWithoutDrone;
        }


        public void PickParcel(int id)
        {

        }
    }
}
