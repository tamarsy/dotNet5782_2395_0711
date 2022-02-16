﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using DalApi;
using DO;
using Dal;


namespace DAL
{
    internal sealed class DalXML : IDal
    {
        private readonly string parcelsPath = "Parcels.xml";
        private readonly string StationsPath = "Stations.xml";
        private readonly string dronesPath = "Drones.xml";
        private readonly string ConfigPath = @"XmlConfig.xml";
        private readonly string customersPath = "Customers.xml";
        private readonly string droneChargesPath = "DroneCharges.xml";
        public static IDal Instance { get; } = new Station;

        public void AddCustomer(Customer newCustomer)
        {
            List<Customer> config = XMLTools.LoadListFromXmlSerializer(ConfigPath);
            config.Add(newCustomer);
            XMLTools.SaveListToXmlSerializer(config, customersPath);
            throw new NotImplementedException();
        }

        public void AddDrone(Drone newDrone)
        {
            List<Drone> config = XMLTools.LoadListFromXmlSerializer(ConfigPath);
            config.Add(newDrone);
            XMLTools.SaveListToXmlSerializer(config, dronesPath);
            throw new NotImplementedException();
        }

        public void AddParcel(Parcel newParcel)
        {
            List<Parcel> config = XMLTools.LoadListFromXmlSerializer(ConfigPath);         
            config.Add(newParcel);
            XMLTools.SaveListToXmlSerializer(config, parcelsPath);

        }

        public void AddStation(Station newStation)
        {
            List<Station> config = XMLTools.LoadListFromXmlSerializer(StationsPath);
            config.Add(newStation);
            XMLTools.SaveListToXmlSerializer(config, StationsPath);
        }

        public void ChargeOf(int droenId)
        {
            int i = DataSource.DronesArr.FindIndex((d) => d.Id == droenId);
            if (i < 0)
                throw new ObjectNotExistException("no drone whith id: " + droenId + "in charge slot");
            //remove from the list Of Charge Slot in DataSource
            int f = DataSource.listOfChargeSlot.RemoveAll((dch) => dch.DroneId == droenId);
            if (f < 0)
                throw new ObjectNotAvailableForActionException($"no charge for drone: {droenId}");
            throw new NotImplementedException();
        }

        public void ChargeOn(int droenId, int stationId)
        {
            Station s;
            if (!DataSource.DronesArr.Exists(d => d.Id == droenId))
                throw new ObjectNotExistException("no drone whith id: " + droenId);
            s = GetStation(stationId);
            if (!isEmptyChargeSlotInStation(stationId, s.ChargeSlot))
                throw new ObjectNotAvailableForActionException($"no empty charge slot in station with id: {stationId}");
            DataSource.listOfChargeSlot.Add(
                new DroneCharge()
                {
                    DroneId = droenId,
                    StationId = stationId,
                    StartTime = DateTime.Now
                });
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> CustomerList(Predicate<bool> selectList = null) =>
           selectList == null ?
               XMLTools.LoadListFromXmlSerializer<Customer>(customersPath) :
               XMLTools.LoadListFromXmlSerializer<Customer>(customersPath).Where(p => selectList(true));

        public IEnumerable<Customer> CustomerList()
        {
            return XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
        }

        public void DeleteCustomer(int id)
        {
            List<Customer> customers = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
            Customer customer = customers.FirstOrDefault(customer => customer.Id == id);
            customers.Remove(customer);
            customer.IsDelete = true;
            customers.Add(customer);
            XMLTools.SaveListToXmlSerializer(customers, customersPath);
            throw new NotImplementedException();
        }

        public void DeleteDrone(int id)
        {
            List<Drone> drones = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Drone drone = drones.FirstOrDefault(drone => drone.Id == id);
            drones.Remove(drone);
            drone.IsDelete = true;
            drones.Add(drone);
            XMLTools.SaveListToXmlSerializer(drones, dronesPath);
            throw new NotImplementedException();
        }

        public void DeleteParcel(int id)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel parcel = parcels.FirstOrDefault(parcel => parcel.Id == id);
            parcels.Remove(parcel);
            parcel.IsDelete = true;
            parcels.Add(parcel);
            XMLTools.SaveListToXmlSerializer(parcels, parcelsPath);
            throw new NotImplementedException();
        }

        public void DeleteStation(int id)
        {
            XElement Station = XMLTools.LoadListFromXmlElement(StationsPath);
            int i = Station.Elements().FirstOrDefault( Station.Elements("Id").Value == id);
            if (i < 0)
                throw new ObjectNotExistException("not found a station with id = " + id);
            Station.Element() = new Station()
            {
                Id = int.Parse(Station.Element("Id").Value),
                ChargeSlot = int.Parse(Station.Element("ChargingPorts").Value),
                Name = Station.Element("Name").Value,
                Lattitude = double.Parse(Station.Element("Latitude").Value),
                Longitude = double.Parse(Station.Element("Longitude").Value),
                IsDelete = true
            };
            throw new NotImplementedException();
        }

        public void Destination(int percelChoose)
        {
            XMLTools.LoadListFromXmlSerializer<Parcel>(customersPath);

            int i = DataSource.ParcelArr.FindIndex(p => p.Id == percelChoose && !p.IsDelete);
            if (i < 0)
                throw new ObjectNotExistException("Error!! Ther is no drone with this id");
            Parcel parcel = DataSource.ParcelArr[i];
            parcel.Delivered = DateTime.Now;
            DataSource.ParcelArr[i] = parcel;
            throw new NotImplementedException();
        }

        public IEnumerable<Drone> DroneList()
        {
            return XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
        }

        public Customer GetCustomer(int id)
        {
            try
            {
                Customer customer = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath).FirstOrDefault(item => item.Id == id);
                if (customer.Equals(default(Customer)))
                    throw new KeyNotFoundException("There isnt suitable parcel in the data!");
                return customer;
            }
            catch
            {
                throw new KeyNotFoundException();
            }
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

        public Parcel GetParcel(int id)
        {
            try
            {
                Parcel parcel = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath).FirstOrDefault(item => item.Id == id);
                if (parcel.Equals(default(Parcel)))
                    throw new KeyNotFoundException("There isnt suitable parcel in the data!");
                return parcel;
            }
            catch
            {
                throw new KeyNotFoundException();
            }
        }

        public Station GetStation(int id)
        {
            XElement StationsXML = XMLTools.LoadListFromXmlElement(StationsPath);
            foreach (var StationElement in StationsXML.Elements())
            {     
                    if (int.Parse(StationElement.Element("Id").Value )== id)
                    {
                    return new Station()
                        {
                            Id = int.Parse(StationElement.Element("Id").Value),
                            ChargeSlot = int.Parse(StationElement.Element("ChargingPorts").Value),
                            Name = StationElement.Element("Name").Value,
                            Lattitude = double.Parse(StationElement.Element("Latitude").Value),
                            Longitude = double.Parse(StationElement.Element("Longitude").Value),
                        };
                    }
            }
            throw new NotImplementedException();
        }

        public int GetStationIdOfDroneCharge(int droneId)
        {

            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> ParcelList(Predicate<Parcel> selectList = null) =>
           selectList == null ?
               XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath) :
               XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath).Where(p => selectList(p));

        public IEnumerable<Parcel> ParcelList(Predicate<int?> selectList = null)
        {
            throw new NotImplementedException();
        }

        public void ParcelToDrone(int percelChoose, int droneChoose)
        {
            throw new NotImplementedException();
        }

        public void PickParcel(int percelChoose)
        {
            throw new NotImplementedException();
        }

        public double[] PowerConsumptionRequest()
        {
            throw new NotImplementedException();
        }

        public DateTime StartChargeTime(int droneId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> StationList(Predicate<bool> selectList = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> StationListStationList(Predicate<bool> selectList = null) =>
           selectList == null ?
               XMLTools.LoadListFromXmlSerializer<Station>(StationsPath) :
               XMLTools.LoadListFromXmlSerializer<Station>(StationsPath).Where(p => selectList(true));

        public void UpdateCustomer(Customer id)
        {

            throw new NotImplementedException();
        }

        public void UpdateDrone(Drone id)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(Station id)
        {
            XElement Stations = XMLTools.LoadListFromXmlElement(StationsPath);
            XElement e = (from s in Stations.Elements()
                          where int.Parse(s.Element("Id").Value) == station.Id
                          select s).FirstOrDefault();

            e.Element("Name").Value = station.Id;

            //עדכון
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<Station>

    /// <summary>
    /// return if is empty charge slot in a station
    /// </summary>
    /// <param name="station">the choosen station</param>
    /// <returns></returns>
    private bool isEmptyChargeSlotInStation(int stationId, int stationChargeSlot)
    {
        XElement ChargeSlotXML = XMLTools.LoadListFromXmlElement(stationsPath);
        int counter = 0;
        foreach (var ChargeSlot in ChargeSlotXML.Elements())
        {
            if (ChargeSlot.StationId == stationId)
            {
                ++counter;
            }
        }
        if (counter < stationChargeSlot)
        {
            return true;
        }
        return false;
    }
                throw new NotImplementedException();
}


