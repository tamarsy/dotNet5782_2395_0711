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
    public partial class BL : IBL
    {
        private IDal.IDal dalObject;
        private double vacent;
        private double lightWeightCarrier;
        private double mediumWeightCarrier;
        private double heavyWeightCarrier;
        private double skimmerLoadingRate;
        private List<DroneToList> drones;
        private static Random rand = new Random();

        public BL()
        {
            dalObject = new DalObject.DalObject();
            double[] PowerConsumption = dalObject.PowerConsumptionRequest();
            vacent = PowerConsumption[0];
            lightWeightCarrier = PowerConsumption[1];
            mediumWeightCarrier = PowerConsumption[2];
            heavyWeightCarrier = PowerConsumption[3];
            skimmerLoadingRate = PowerConsumption[4];
            drones = new List<DroneToList>();
            initializeDronesList();
        }




        private void initializeDronesList()
        {
            foreach (var drone in dalObject.DroneList())
            {
                DroneToList newDrone = new DroneToList()
                {
                    Id = drone.Id,
                    Model = drone.Model,
                    MaxWeight = (WeightCategories)drone.MaxWeight
                };
                foreach (var parcel in dalObject.ParcelList())
                {
                    if (parcel.Droneld == drone.Id && parcel.Delivered != null)
                    {
                        newDrone.NumOfParcel = parcel.Id;
                        break;
                    }
                }
                if (newDrone.NumOfParcel != null)
                {
                    newDrone.DroneStatuses = DroneStatuses.sending;
                    newDrone.CurrentLocation = FindLocation(newDrone);
                    //to fill BatteryStatuses
                    newDrone.BatteryStatuses = FindBatteryStatusesForInitializeDronesList(newDrone);
                }
                else
                {
                    newDrone.DroneStatuses = (DroneStatuses)rand.Next(-1, 1);
                    newDrone.CurrentLocation = FindLocation(newDrone);
                    //to fill BatteryStatuses
                    newDrone.BatteryStatuses = FindBatteryStatusesForInitializeDronesList(newDrone);
                }
                drones.Add(newDrone);
            }
        }



        public void ParcelToDrone(int id)
        {
            int i = drones.FindIndex(d => d.Id == id);
            if (drones[i] != DroneStatuses.vacant)
                throw new ObjectNotAvailableForActionException($"drone with id: {id} is no vacant");
            Parcel choosenParcel;
            try
            {
                choosenParcel = FindParcelToCollecting(drones[i]);
            }
            catch (Exception)
            {
                throw;
            }
            drones[i].DroneStatuses = DroneStatuses.sending;
            choosenParcel.DroneDelivery = new DroneDelivery()
            {
                BatteryStatuses = drones[i].BatteryStatuses,
                CurentLocation = drones[i].CurrentLocation,
                Id = drones[i].Id
            };
            choosenParcel.AssignmentTime = DateTime.Now;
        }



        private Parcel FindParcelToCollecting(DroneToList drone)
        {
            List<ParcelToList> availableParcelList = ParcelsList().Select(p => p.Weight < drone.MaxWeight).ToList;                                                                                                                                                     );
            return ;
        }




        public void Destination(int id)
        {

        }
    }
}
