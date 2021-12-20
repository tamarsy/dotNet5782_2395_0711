using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using BO;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// function to adding new parcel
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="tid"></param>
        /// <param name="weight"></param>
        /// <param name="priority"></param>
        public void AddParcel(int sid, int tid, WeightCategories weight, Priorities priority)
        {
            DO.Parcel newParcel = new DO.Parcel()
            {
                SenderId = sid,
                Getter = tid,
                Weight = (DO.WeightCategories)weight,
                Priority = (DO.Priorities)priority,
                ReQuested = DateTime.Now
            };
            try
            {
                dalObject.AddParcel(newParcel);
            }
            catch (DO.ObjectAlreadyExistException e)
            {
                throw new ObjectAlreadyExistException(e.Message);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// the function get a parcel
        /// </summary>
        /// <param name="requestedId"></param>
        /// <returns>DlToBlParcel</returns>
        public Parcel GetParcel(int requestedId)
        {
            DO.Parcel parcel;
            try
            {
                parcel = dalObject.GetParcel(requestedId);
            }
            catch (DO.ObjectNotExistException e)
            {
                throw new ObjectNotExistException(e.Message);
            }
            catch (Exception)
            {
                throw new Exception();
            }
            return DlToBlParcel(parcel);
        }

        /// <summary>
        /// the function change the "Idal" details to "bl" details
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns>Parecel</returns>
        private Parcel DlToBlParcel(DO.Parcel parcel)
        {
            Drone drone = (parcel.Droneld == default) ? default:GetDrone((int)parcel.Droneld);
            return new Parcel()
            {
                Id = parcel.Id,
                AssignmentTime = (DateTime)parcel.ReQuested,
                DeliveryTime = drone == default? default : (DateTime)parcel.Delivered,
                DroneDelivery = drone == default ? default : new DroneDelivery() { Id = drone.Id, BatteryStatuses = drone.BatteryStatuses, CurrentLocation = drone.CurrentLocation },
                PickUpTime = (DateTime)parcel.PickedUp,
                Priority = (Priorities)parcel.Priority,
                SenderId = new DeliveryCustomer() { Id = parcel.SenderId, Name = GetCustomer(parcel.SenderId).Name },
                GetterId = new DeliveryCustomer() { Id = parcel.Getter, Name = GetCustomer(parcel.Getter).Name },
                SupplyTime = (DateTime)parcel.Schedulet,
                Weight = (WeightCategories)parcel.Weight
            };
        }

        /// <summary>
        /// return list of the parcels
        /// </summary>
        /// <returns>IEnumerable ParcelToList</returns>
        public IEnumerable<ParcelToList> ParcelsList()
        {
            return dalObject.ParcelList().Select(item => DlToBlParcelToList(item));
        }

        /// <summary>
        /// return list parcels without drone
        /// </summary>
        /// <returns>dalObject.ParcesWithoutDronelList</returns>
        public IEnumerable<ParcelToList> ParcesWithoutDronelList()
        {
            return dalObject.ParcelList((int? Droneld) => Droneld == default).Select(item => DlToBlParcelToList(item, ParcelStatuses.defined));
        }

        /// <summary>
        /// change bl parcel to list 
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="parcelStatuses"></param>
        /// <returns>ParcelToList</returns>
        private ParcelToList DlToBlParcelToList(DO.Parcel parcel, ParcelStatuses parcelStatuses = default)
        {
            return new ParcelToList()
            {
                Id = parcel.Id,
                SenderId = parcel.SenderId,
                GetterId = parcel.Getter,
                Weight = (WeightCategories)parcel.Weight,
                Priority = (Priorities)parcel.Priority,
                ParcelStatuses = parcelStatuses != default ? ParcelStatuses.defined : FindParcelStatuses(parcel)
            };
        }

        /// <summary>
        /// find parcel status
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns>ParcelStatuses</returns>
        private ParcelStatuses FindParcelStatuses(DO.Parcel parcel)
        {
            if (parcel.Delivered != default)
                return ParcelStatuses.supplied;
            if (parcel.PickedUp != default)
                return ParcelStatuses.collected;
            if (parcel.Schedulet != default)
                return ParcelStatuses.ascribed;
            return ParcelStatuses.defined;
        }
    }
}
