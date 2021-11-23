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
        public object DAL { get; private set; }

        public void AddParcel(Parcel IBL)
        {
            try
            {
                dal.AddParcel(Parcel_partial_BL.CustomerSender.Id, Parcel_partial_BL.CustomerReceives.Id, (IDAL.DO.WeightCategories)parcelBl.Weight, (IDAL.DO.Priorities)parcelBl.Priority);
            }
        }

        public IEnumerable<ParcelToList> GetParcelsNotAssignedToDrone()
        {
            return dal.GetParcelsNotAssignedToDrone().Select(parcel => mapParcelToList(parcel)); ;
        }

        public IEnumerable<ParcelToList> GetParcels()
        {
            return dal.GetParcels().Select(parcel => mapParcelToList(parcel));
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

    }


}
