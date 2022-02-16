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
            Station station = null;
            Maintenance maintenance = Maintenance.Starting;
            do
            {
                Drone drone = bl.GetDrone(droneId);
                switch (drone.DroneStatuses)
                {
                    case DroneStatuses.vacant:
                        if (!sleepDelayTime()) break;
                        lock (bl)
                        {
                            try
                            {
                                bl.ParcelToDrone(droneId);
                            }
                            catch (ObjectNotAvailableForActionException)
                            {
                                try
                                {
                                    station = bl.FindCloseStationWithChargeSlot(drone);
                                    if (station == null)
                                    {
                                        station = bl.FindClosetStationLocation(drone);
                                        if (bl.FindMinPowerForDistance(drone.Distance(station)) < 0)
                                            throw new ObjectNotAvailableForActionException("Failed go to charge");
                                    }

                                    bl.StartChargeing(droneId, station.Id);
                                    maintenance = Maintenance.Starting;
                                }
                                catch (ObjectNotExistException e) { throw e; }
                                catch (ObjectNotAvailableForActionException) { };
                            }
                        }
                        break;
                    case DroneStatuses.maintanance:
                        switch (maintenance)
                        {
                            case Maintenance.Starting:
                                lock (bl)
                                {
                                    try { station = bl.GetStation(bl.dalObject.GetStationIdOfDroneCharge(drone.Id)); }
                                    catch (DO.ObjectNotExistException ex) { throw new ObjectNotExistException("Error base station", ex); }
                                    maintenance = Maintenance.Going;
                                }
                                break;
                            case Maintenance.Going:
                                double distanceToStation = drone.Distance(station);
                                if (distanceToStation < 0.01)
                                    lock (bl)
                                    {
                                        bl.DronStepTo(droneId, distanceToStation, distanceToStation, station);
                                        maintenance = Maintenance.Charging;
                                    }
                                else
                                {
                                    if (!sleepDelayTime()) break;
                                    lock (bl)
                                    {
                                        double CurrentStep = Min(distanceToStation, STEP);
                                        bl.DronStepTo(droneId, CurrentStep, distanceToStation, station);
                                    }
                                }
                                break;
                            case Maintenance.Charging:
                                if (drone.BatteryStatuses == 1.0)
                                    lock (bl) { bl.ChargeOf(droneId); }
                                else
                                {
                                    if (!sleepDelayTime()) break;
                                    lock (bl) bl.ChargeStep(droneId, TIME_STEP);
                                }
                                break;
                            default:
                                throw new ObjectNotAvailableForActionException("wrong maintenance substate");
                        }
                        break;

                    case DroneStatuses.sending:
                        double distance = drone.Parcel.Distance;
                        if (distance < 0.01 || drone.BatteryStatuses == 0.0)
                            lock (bl)
                            {
                                if (drone.Parcel.StatusParcel)
                                    bl.DestinationEnd(drone.Id);
                                else
                                    bl.dalObject.PickParcel(drone.Parcel.Id);
                            }
                        else
                        {
                            if (!sleepDelayTime()) break;
                            lock (bl)
                            {
                                double currentDistance = Min(distance, STEP);
                                bl.DronStepTo(droneId, currentDistance, distance, drone.Parcel.StatusParcel ? drone.Parcel.DeliveryDestination : drone.Parcel.Collecting);
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
