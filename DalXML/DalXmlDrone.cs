using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using DalApi;
using DO;
using Dal;
using System.Runtime.CompilerServices;
using System.IO;

namespace DAL
{
    internal sealed partial class DalXML : IDal
    {
        public void AddDrone(Drone newDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            drones.Add(newDrone);
            XMLTools.SaveListToXmlSerializer(drones, dronesPath);
        }


        /// <summary>
        /// Exception: ObjectNotExistException, ArgumentNullException
        /// Update Drone details
        /// </summary>
        /// <param name="drone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone drone)
        {
            if (drone.Equals(default(Drone)))
                throw new ArgumentNullException("Null argument");

            var drones = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            int i = drones.FindIndex(s => s.Id == drone.Id && !s.IsDelete);
            if (i < 0)
                throw new ObjectNotExistException("no drone whith id: " + drone.Id + "in charge slot");
            drones[i] = drone;

            XMLTools.SaveListToXmlSerializer(drones, dronesPath);
        }

        public Drone GetDrone(int id)
        {
            try
            {
                Drone drone = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath).FirstOrDefault(item => item.Id == id);
                if (drone.Equals(default(Drone)))
                    throw new KeyNotFoundException("There isnt suitable parcel in the data!");
                return drone;
            }
            catch
            {
                throw new KeyNotFoundException();
            }
        }

        public void DeleteDroneCharge(int droneId)
        {
            List<DroneCharge> charges = XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            DroneCharge charge = charges.FirstOrDefault(c => c.DroneId == droneId);
            charges.Remove(charge);

            XMLTools.SaveListToXmlSerializer(charges, droneChargesPath);
        }

        public IEnumerable<Drone> DroneList()
        {
            return XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
        }


        public void ChargeOn(int droenId, int stationId)
        {
            Drone drone = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath).FirstOrDefault(item => item.Id == droenId);
            if (!XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath).Exists(d => d.Id == droenId))
                throw new ObjectNotExistException("no drone whith id: " + droenId);
            Station s = GetStation(stationId);
            if (!isEmptyChargeSlotInStation(stationId, s.ChargeSlot))
                throw new ObjectNotAvailableForActionException($"no empty charge slot in station with id: {stationId}");
            if (XMLTools.LoadListFromXmlSerializer<Station>(StationsPath).Exists(d => d.Id == droenId))
                throw new ObjectNotAvailableForActionException("Exist in charge drone whith id: " + droenId);
            StationDroneIn(stationId);
            AddDroneCharge(droenId, stationId);
        }

        public void ChargeOf(int droenId)
        {
            Drone drone = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath).FirstOrDefault(item => item.Id == droenId);
            if (drone.Id < 0)
                throw new ObjectNotExistException("no drone whith id: " + droenId + "in charge slot");
            //remove from the list Of Charge Slot in DataSource
            StationDroneOut(droenId);
            DeleteDroneCharge(droenId);
        }
    }
}
