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
        public DalObject()
        {
            DataSource.Config.Initialize();
        }
        public void AddStation(Station station)
        {
            DataSource.StationsArr[DataSource.Config.stationIndex] = station;
        }
        public void AddDrone(Drone drone)
        {

        }
        public void AddCustomer(Customer customer)
        {

        }
        public void AddParcel(Parcel parcel)
        {

        }
        public void ParcelToDrone(int percelChoose, int droneChoose)
        {
            int i = 0;
            for (; i < DataSource.Config.droneIndex ; i++)
            {
               if( DataSource.DronesArr[i].Id == droneChoose)
                {
                    break;
                }
            }
            if (i == DataSource.Config.droneIndex)
                throw 'ERROR'
            i = 0;
          
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
