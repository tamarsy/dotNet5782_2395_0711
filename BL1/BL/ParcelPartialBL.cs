using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using BO;
using System.Runtime.CompilerServices;

namespace BL
{
    partial class BL
    {
        /// <summary>
        /// Exception: ObjectAlreadyExistException, ArgumentNullException
        /// adding new parcel
        /// </summary>
        /// <param name="sid">sid</param>
        /// <param name="tid">tid</param>
        /// <param name="weight">weight</param>
        /// <param name="priority">priority</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(int sid, int tid, WeightCategories weight, Priorities priority)
        {
            DO.Parcel newParcel = new DO.Parcel()
            {
                SenderId = sid,
                GetterId = tid,
                Weight = (DO.WeightCategories)weight,
                Priority = (DO.Priorities)priority,
                Requested = DateTime.Now
            };
            try
            {
                lock (dalObject) { dalObject.AddParcel(newParcel); }
            }
            catch (DO.ObjectAlreadyExistException e)
            {
                throw new ObjectAlreadyExistException(e.Message);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Exception: ObjectNotExistException
        /// get a parcel
        /// </summary>
        /// <param name="requestedId"></param>
        /// <returns>DlToBlParcel</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int requestedId)
        {
            DO.Parcel parcel;
            try
            {
                lock (dalObject) { parcel = dalObject.GetParcel(requestedId); }
            }
            catch (DO.ObjectNotExistException e)
            {
                throw new ObjectNotExistException(e.Message);
            }
            if (parcel.IsDelete) throw new ObjectNotExistException("Parcel deleted");
            return DlToBlParcel(parcel);
        }



        /// <summary>
        /// return list of the parcels
        /// </summary>
        /// <returns>IEnumerable ParcelToList</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> ParcelsList()
        {
            lock (dalObject) { return dalObject.ParcelList().Select(item => DlToBlParcelToList(item)); }
        }

        /// <summary>
        /// return list parcels without drone
        /// </summary>
        /// <returns>dalObject.ParcesWithoutDronelList</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> ParcesWithoutDronelList()
        {
            lock (dalObject)
            {
                return dalObject.ParcelList(p => p == default).Select(item => DlToBlParcelToList(item, ParcelStatuses.defined));
            }
        }



        /// <summary>
        /// Delete Parcel
        /// </summary>
        /// <param name="id">parcel id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int id) { lock (dalObject) {dalObject.DeleteParcel(id);} }
    }
}
