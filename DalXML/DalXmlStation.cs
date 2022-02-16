using System;
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
        private readonly string StationsPath = "Stations.xml";
        //private readonly string dronesPath = "Drones.xml";

        //private readonly string customersPath = "Customers.xml";
        //private readonly string droneChargesPath = "DroneCharges.xml";
        public static IDal Instance { get; } = new Station;

        public void AddCustomer(Customer newCustomer)
        {
            throw new NotImplementedException();
        }

        public void AddDrone(Drone newDrone)
        {
            throw new NotImplementedException();
        }

        public void AddParcel(Parcel newpParcel)
        {
            throw new NotImplementedException();
        }

        public void AddStation(Station newStation)
        {
            throw new NotImplementedException();
        }

        public void ChargeOf(int droenId)
        {
            throw new NotImplementedException();
        }

        public void ChargeOn(int droenId, int stationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> CustomerList()
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteDrone(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteParcel(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteStation(int id)
        {
            throw new NotImplementedException();
        }

        public void Destination(int percelChoose)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Drone> DroneList()
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public Drone GetDrone(int id)
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int id)
        {
            throw new NotImplementedException();
        }

        public Station GetStation(int id)
        {
            XElement StationsXML = XMLTools.LoadListFromXmlElement(StationsPath);
            foreach (var StationElement in StationsXML.Elements())
            {
                List<Station> baseStations = new();
                foreach (var baseStationElement in StationsXML.Elements())
                {
                    baseStations.Add(
                        new Station()
                        {
                            Id = int.Parse(StationElement.Element("Id").Value),
                            ChargeSlot = int.Parse(StationElement.Element("ChargingPorts").Value),
                            Name = StationElement.Element("Name").Value,
                            Lattitude = double.Parse(StationElement.Element("Latitude").Value),
                            Longitude = double.Parse(StationElement.Element("Longitude").Value),
                        });
                }

                return baseStations;
            }
            throw new NotImplementedException();
        }

        public int GetStationIdOfDroneCharge(int droneId)
        {
            throw new NotImplementedException();
        }

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
            throw new NotImplementedException();
        }
    }

    public Station GetStation(int id)

    {
        XElement StationsXML = XMLTools.LoadListFromXmlElement(StationsPath);
        foreach (var StationElement in StationsXML.Elements())
        {
            List<Station> baseStations = new();
            foreach (var baseStationElement in StationsXML.Elements())
            {
                baseStations.Add(
                    new Station()
                    {
                        Id = int.Parse(StationElement.Element("Id").Value),
                        AvailableChargingPorts = int.Parse(StationElement.Element("ChargingPorts").Value),
                        Name = StationElement.Element("Name").Value,
                        Latitude = double.Parse(StationElement.Element("Latitude").Value),
                        Longitude = double.Parse(StationElement.Element("Longitude").Value),
                    });
            }

            return baseStations;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<Station> StationList(Predicate<bool> selectList = default)
        => StationsXML.Elements().Where((c) => selectList != null ? selectList(isEmptyChargeSlotInStation(c.Id, c.ChargeSlot)) : true);


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


    public void UpdateStation(Station station)
    {
        XElement Stations = XMLTools.LoadListFromXmlElement(stationsPath);
        XElement e = (from s in Stations.Elements()
                      where int.Parse(s.Element("Id").Value) == station.Id
                      select s).FirstOrDefault();

        e.Element("Name").Value = station.Id;

        //עדכון
    }



    /// <summary>
    /// Exception: ObjectNotExistException
    /// Delete Station
    /// </summary>
    /// <param name="station"></param>
    public void DeleteStation(int Id)
    {
        XElement Station= XMLTools.LoadListFromXmlElement(stationsPath);
        int i = Station.Elements().FindIndex(s => s.Id == Id);
        if (i < 0)
            throw new ObjectNotExistException("not found a station with id = " + Id);
        Station.Elements() = new Station()
        {
            Id = Id,
            Name = Station.Name,
            Lattitude = Station.Lattitude,
            Longitude = Station.Longitude,
            ChargeSlot = Station.ChargeSlot,
            IsDelete = true
        };
    }
}
