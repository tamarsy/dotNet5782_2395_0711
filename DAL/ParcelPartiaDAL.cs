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
        /// pick the percel from the dron
        /// </summary>
        /// <param name="percelChoose">the choosen percel</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickParcel(int percelChoose)
        {
            bool check = false;
            int i = 0;
            for (; i < DataSource.ParcelArr.Count; i++)
            {
                if (DataSource.ParcelArr[i].Id == percelChoose)
                {
                    check = true;
                    //itemParcelPickedUp = DataSource.ParcelArr[i].PickedUp;

                    DataSource.ParcelArr[i] = new Parcel()
                    {
                        Id = DataSource.ParcelArr[i].Id,
                        SenderId = DataSource.ParcelArr[i].SenderId,
                        GetterId = DataSource.ParcelArr[i].GetterId,
                        Weight = DataSource.ParcelArr[i].Weight,
                        Priority = DataSource.ParcelArr[i].Priority,
                        Requested = DataSource.ParcelArr[i].Requested,
                        Droneld = DataSource.ParcelArr[i].Droneld,
                        Schedulet = DataSource.ParcelArr[i].Schedulet,
                        PickedUp = DateTime.Now,
                        Delivered = DataSource.ParcelArr[i].Delivered
                    };
                    break;
                }
            }
            if (check == false)
                throw new ObjectNotExistException("Error!! Ther is no drone with this id");
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
    }
}
