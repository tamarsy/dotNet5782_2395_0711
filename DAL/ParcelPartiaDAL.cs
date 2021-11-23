﻿using System;
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
            if (DataSource.CustomerArr.Exists(drone => drone.Id == newpParcel.Id))
            {
                throw new ObjectAlreadyExistException("Can't add, There is already a parcel with this ID");
            }
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
            foreach (var item in DataSource.ParcelArr)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            throw new ArgumentException("not found a parcel with id = " + id);
        }


        public IEnumerable<Parcel> ParcelList()
        {
            List<Parcel> ParcelList = new List<Parcel>();
            foreach (var item in DataSource.ParcelArr)
            {
                ParcelList.Add(item);
            }
            return ParcelList;
        }

        public IEnumerable<Parcel> ParcesWithoutDronelList()
        {
            List<Parcel> ParcesWithoutDronelList = new List<Parcel>();
            foreach (var item in DataSource.ParcelArr)
            {
                if (item.Droneld != -1)
                {
                    ParcesWithoutDronelList.Add(item);
                }
            }
            return ParcesWithoutDronelList;
        }
    }
}
