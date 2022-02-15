using System;
using BO;
using System.Threading;
using System.Linq;
using static System.Math;
using static BL.BL;

namespace BL
{
    internal class Simulator
    {
        enum Maintenance { Starting, Going, Charging }
        private const double VELOCITY = 1.0;
        private const int DELAY = 500;
        private const double TIME_STEP = DELAY / 1000.0;
        private const double STEP = VELOCITY / TIME_STEP;

        public Simulator(BL bl, int droneId, Action updateDrone, Func<bool> checkStop)
        {
            //DalApi.IDal dal = bl.dalObject;
            Drone drone = bl.GetDrone(droneId);
            int? baseStationId = null;
            Station station = null;
            double distance = 0.0;
            //int batteryUsage = 0;
            DO.Parcel? parcel = null;
            //bool pickedUp = false;
            Customer customer = null;
            Maintenance maintenance = Maintenance.Starting;
            void initDelivery(int id)
            {
                parcel = bl.dalObject.GetParcel(id);
                customer = bl.GetCustomer((int)(drone.Parcel.StatusParcel ? parcel?.GetterId : parcel?.SenderId));
            }
            do
            {
                switch (drone.DroneStatuses)
                {
                    case DroneStatuses.vacant:
                        if (!sleepDelayTime()) break;
                        //??????????????????????????????????????
                        lock (bl)
                        {
                            int? parcelId = bl.FindParcelToCollecting(drone.Id)?.Id;
                            if (!(parcelId == null) || !(drone.BatteryStatuses > 1.0))
                            {
                                if(parcelId == null)
                                {
                                    try
                                    {
                                        bl.ChargeOn(droneId);
                                        drone.DroneStatuses = DroneStatuses.maintanance;
                                        maintenance = Maintenance.Starting;
                                    }
                                    catch (ObjectNotExistException e) { throw e; }
                                    catch (ObjectNotAvailableForActionException) { }
                                }
                                else
                                {
                                    try
                                    {
                                        bl.ParcelToDrone(droneId);
                                        drone.Parcel = bl.ParcelToParcelDelivery(bl.dalObject.GetParcel((int)parcelId), drone);
                                        initDelivery((int)parcelId);
                                        //??????????????????????????????????????
                                        drone.DroneStatuses = DroneStatuses.sending;
                                    }
                                    catch (DO.ObjectAlreadyExistException ex) { throw new ObjectAlreadyExistException("Already Exist", ex); }
                                    break;
                                }
                            }
                        }
                        break;
                    case DroneStatuses.maintanance:
                        switch (maintenance)
                        {
                            case Maintenance.Starting:
                                lock (bl)
                                {
                                    try { station = bl.GetStation(baseStationId ?? bl.dalObject.GetStationIdOfDroneCharge(drone.Id)); }
                                    catch (DO.ObjectAlreadyExistException ex) { throw new ObjectAlreadyExistException("Internal error base station", ex); }
                                    distance = drone.Distance(station);
                                    maintenance = Maintenance.Going;
                                }
                                break;
                            case Maintenance.Going:
                                if (distance < 0.01)
                                    lock (bl)
                                    {
                                        drone.CurrentLocation = station.CurrentLocation;
                                        maintenance = Maintenance.Charging;
                                    }
                                else
                                {
                                    if (!sleepDelayTime()) break;
                                    lock (bl)
                                    {
                                        double CurrentStepD = distance < STEP ? distance : STEP;
                                        distance -= CurrentStepD;
                                        drone.BatteryStatuses = Max(0.0, drone.BatteryStatuses - bl.FindMinPowerForDistance(CurrentStepD));
                                    }
                                }
                                break;
                            case Maintenance.Charging:
                                if (drone.BatteryStatuses == 1.0)
                                    lock (bl)
                                    {
                                        drone.DroneStatuses = DroneStatuses.vacant;
                                        bl.ChargeOf(station.Id);
                                    }
                                else
                                {
                                    if (!sleepDelayTime()) break;
                                    lock (bl) drone.BatteryStatuses = Min(1.0, drone.BatteryStatuses + bl.skimmerLoadingRate * TIME_STEP);
                                }
                                break;
                            default:
                                throw new ObjectNotAvailableForActionException("wrong maintenance substate");
                        }
                        break;

                    case DroneStatuses.sending:
                        lock (bl)
                        {
                            //???????????????????????????????????????????????????????????????????????????????
                            try { if (true) initDelivery(drone.Parcel.Id); }
                            catch (DO.ObjectNotExistException ex) { throw new ObjectNotExistException("error getting parcel", ex); }
                            distance = drone.Distance(customer);
                        }

                        if (distance < 0.01 || drone.BatteryStatuses == 0.0)
                            lock (bl)
                            {
                                drone.CurrentLocation = customer.CurrentLocation;
                                if (drone.Parcel.StatusParcel)
                                {
                                    bl.Destination((int)parcel?.Id);
                                    drone.DroneStatuses = DroneStatuses.vacant;

                                }
                                else
                                {
                                    bl.PickParcel((int)parcel?.Id);
                                    customer = bl.GetCustomer((int)parcel?.SenderId);
                                    drone.Parcel.StatusParcel = true;
                                }
                            }
                        else
                        {
                            if (!sleepDelayTime()) break;
                            lock (bl)
                            {
                                double delta = distance < STEP ? distance : STEP;
                                double proportion = delta / distance;
                                drone.BatteryStatuses = Max(0.0, drone.BatteryStatuses - bl.FindMinPowerForDistance(delta, drone.Parcel != null ? drone.Parcel.Weight : default(WeightCategories?)));
                                double lat = drone.CurrentLocation.Latitude + (customer.CurrentLocation.Latitude - drone.CurrentLocation.Latitude) * proportion;
                                double lon = drone.CurrentLocation.Longitude + (customer.CurrentLocation.Longitude - drone.CurrentLocation.Longitude) * proportion;
                                drone.CurrentLocation = new Location() { Latitude = lat, Longitude = lon };
                            }
                        }
                        break;
                    default:
                        throw new ObjectNotAvailableForActionException("ERROR: not available");
                }
                updateDrone();
            } while (!checkStop());
        }

        private static bool sleepDelayTime()
        {
            try { Thread.Sleep(DELAY); } catch (ThreadInterruptedException) { return false; }
            return true;
        }
    }
}
