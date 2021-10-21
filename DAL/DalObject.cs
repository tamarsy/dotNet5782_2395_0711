using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        /// <summary>
        /// 
        /// </summary>
        public DalObject()
        {
            DataSource.Config.Initialize();
        }
        /// <summary>
        /// the function add a new station to the arry
        /// </summary>
        /// <param name="station">the new station to add</param>
        public void AddStation(Station station)
        {
            if (DataSource.Config.stationIndex > DataSource.StationsArr.Length - 1)
            {
                throw new ArgumentException("station's place are full! you can't add a new station!");
            }
            DataSource.StationsArr[DataSource.Config.stationIndex] = station;
        }
        /// <summary>
        /// the function add a new drone to the arry
        /// </summary>
        /// <param name="drone">the new drone to add</param>
        public void AddDrone(Drone drone)
        {
            if (DataSource.Config.stationIndex > DataSource.StationsArr.Length - 1)
            {
                throw new ArgumentException("drone's place are full! you can't add a new drone!");
            }
            DataSource.DronesArr[DataSource.Config.droneIndex] = drone;
        }
        /// <summary>
        /// the function add a new customer to the arry
        /// </summary>
        /// <param name="customer">the new customer to add</param>
        public void AddCustomer(Customer customer)
        {
            if (DataSource.Config.stationIndex > DataSource.StationsArr.Length - 1)
            {
                throw new ArgumentException("customer's place are full! you can't add a new customer!");
            }
            DataSource.CustomerArr[DataSource.Config.customerIndex] = customer;
        }
        /// <summary>
        /// the function add a new parcel to the arry
        /// </summary>
        /// <param name="parcel">the new parcel to add</param>
        public void AddParcel(Parcel parcel)
        {
            if (DataSource.Config.stationIndex > DataSource.StationsArr.Length - 1)
            {
                throw new ArgumentException("parcel's place are full! you can't add a new parcel!");
            }
            DataSource.ParcelArr[DataSource.Config.parcelIndex] = parcel;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="percelChoose"></param>
        /// <param name="droneChoose"></param>
        public void ParcelToDrone(int percelChoose, int droneChoose)
        {
            int i = 0;
            int index;
            for (; i < DataSource.Config.droneIndex ; i++)
            {
               if( DataSource.DronesArr[i].Id == droneChoose)
                {  
                    break;
                }
            }
            if (i == DataSource.Config.droneIndex)
                throw new ArgumentException("Error!! Ther is no drone with this id");
            int j = 0;
            for (; j < DataSource.Config.parcelIndex; j++)
            {
                if (DataSource.ParcelArr[j].Id == percelChoose)
                {
                    if(DataSource.ParcelArr[j].Weight>DataSource.DronesArr[i].MaxWeight)
                        throw new ArgumentException("Error!! Ther is no drone with this id");

                    if (DataSource.DronesArr[i].Status!=DroneStatuses.vacant)
                        throw new ArgumentException("We can't send this drone");

                    DataSource.ParcelArr[j].Droneld = DataSource.DronesArr[i].Id;
                    break;
                }
            }
            if (i == DataSource.Config.droneIndex)
                throw new ArgumentException("Error!! Ther is no drone with this id");
        }
        public void PickParcel()
        {

        }
        public void ChargeOn()
        {

        }
        public void ChargeOf()
        {

        }
        public void ViewStation()
        {

        }
        public void ViewDrone()
        {

        }
        public void ViewCustomer()
        {

        }
        public void ViewParcel()
        {

        }
        public void StationList()
        {

        }
        public void DroneList()
        {

        }
        public void CustomerList()
        {

        }
        public void ParcelList()
        {

        }
        public void ParcesWithoutDronelList()
        {

        }
        public void EmptyChangeSlotlList()
        {

        }
    }
}