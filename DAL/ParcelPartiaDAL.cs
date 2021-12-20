using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {

        /// <summary>
        /// the function add a new parcel to the arry
        /// </summary>
        /// <param name="parcel">the new parcel to add</param>
        public void AddParcel(Parcel newpParcel)
        {
            newpParcel.Id = DataSource.Config.runNumForParcel++;
            DataSource.ParcelArr.Add(newpParcel);
        }

        /// <summary>
        /// the function pick the percel from the dron
        /// </summary>
        /// <param name="percelChoose">the choosen percel</param>
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
                        Getter = DataSource.ParcelArr[i].Getter,
                        Weight = DataSource.ParcelArr[i].Weight,
                        Priority = DataSource.ParcelArr[i].Priority,
                        ReQuested = DataSource.ParcelArr[i].ReQuested,
                        Droneld = DataSource.ParcelArr[i].Droneld,
                        Schedulet = DataSource.ParcelArr[i].Schedulet,
                        PickedUp = DateTime.Now,
                        Delivered = DataSource.ParcelArr[i].Delivered
                    };
                    break;
                }
            }
            if (check == false)
                throw new ArgumentException("Error!! Ther is no drone with this id");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">the parcel id</param>
        /// <returns></returns>
        public Parcel GetParcel(int id)
        {
            int i = DataSource.ParcelArr.FindIndex((p) => p.Id == id);
            if (i < 0)
                throw new ObjectNotExistException("not found a parcel with id = " + id);
            return DataSource.ParcelArr[i];
        }


        public IEnumerable<Parcel> ParcelList(Predicate<int?> selectList = default)
            => DataSource.ParcelArr.Where((c) => selectList != null ? selectList(c.Droneld) : true);
    }
}
