using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;

namespace DalObject
{
    public partial class DAL
    {

        /// <summary>
        /// add a new parcel to the arry
        /// </summary>
        /// <param name="parcel">the new parcel to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel newpParcel)
        {
            newpParcel.Id = DataSource.Config.runNumForParcel++;
            DataSource.ParcelArr.Add(newpParcel);
        }

        /// <summary>
        /// return parcel by id
        /// </summary>
        /// <param name="id">the parcel id</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int id)
        {
            int i = DataSource.ParcelArr.FindIndex((p) => p.Id == id);
            if (i < 0)
                throw new ObjectNotExistException("not found a parcel with id = " + id);
            return DataSource.ParcelArr[i];
        }

        /// <summary>
        /// Exception: ObjectNotExistException
        /// Delete Parcel
        /// </summary>
        /// <param name="parcel id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int Id)
        {
            int i = DataSource.ParcelArr.FindIndex(p => p.Id == Id && !p.IsDelete);
            if (i < 0)
                throw new ObjectNotExistException("not found a parcel with id = " + Id);
            Parcel parcel = DataSource.ParcelArr[i];
            parcel.IsDelete = true;
            DataSource.ParcelArr[i] = parcel;
        }

        /// <summary>
        /// return Parcel List
        /// </summary>
        /// <param name="selectList">select parcel</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> ParcelList(Predicate<int?> selectList = default)
            => DataSource.ParcelArr.Where((c) => !c.IsDelete && (selectList == null || selectList(c.Droneld)));


        /// <summary>
        /// pick the percel from the dron
        /// </summary>
        /// <param name="percelChoose">the choosen percel</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickParcel(int percelChoose)
        {
            int i = DataSource.ParcelArr.FindIndex(pa => pa.Id == percelChoose);
            if (i < 0)
                throw new ObjectNotExistException("Error!! Ther is no drone with this id");
            Parcel p = DataSource.ParcelArr[i];
            p.PickedUp = DateTime.Now;
            DataSource.ParcelArr[i] = p;
        }


        /// <summary>
        /// Destination
        /// </summary>
        /// <param name="percelChoose">parcel id</param>
        public void Destination(int percelChoose)
        {
            int i = DataSource.ParcelArr.FindIndex(p => p.Id == percelChoose && !p.IsDelete);
            if (i < 0)
                throw new ObjectNotExistException("Error!! Ther is no drone with this id");
            Parcel parcel = DataSource.ParcelArr[i];
            parcel.Delivered = DateTime.Now;
            DataSource.ParcelArr[i] = parcel;
        }
    }
}
