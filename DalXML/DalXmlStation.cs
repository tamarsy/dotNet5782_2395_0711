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
        private readonly string parcelsPath = @$"{Directory.GetCurrentDirectory()}/../../../../xml/Parcels.xml";
        private readonly string StationsPath = $@"{Directory.GetCurrentDirectory()}/../../../../xml/Stations.xml";
        private readonly string dronesPath = $@"{Directory.GetCurrentDirectory()}/../../../../xml/Drones.xml";
        private readonly string ConfigPath = $@"{Directory.GetCurrentDirectory()}/../../../../xml/XmlConfig.xml";
        private readonly string customersPath = $@"{Directory.GetCurrentDirectory()}/../../../../xml/Customers.xml";
        private readonly string droneChargesPath = $@"{Directory.GetCurrentDirectory()}/../../../../xml/DroneCharges.xml";
        private readonly string detailsPath = $@"{Directory.GetCurrentDirectory()}/../../../../xml/Details.xml";
        public static IDal Instance { get; } = new DalXML();
        
       

      
        public void AddDroneCharge(int droneId, int baseStationId)
        {
            List<DroneCharge> charges = XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            charges.Add(new DroneCharge() { DroneId = droneId, StationId = baseStationId, StartTime = DateTime.Now });
            XMLTools.SaveListToXmlSerializer(charges, droneChargesPath);
        }

      
        public void AddStation(Station newStation)
        {
            List<Station> config = XMLTools.LoadListFromXmlSerializer<Station>(StationsPath);
            config.Add(newStation);
            XMLTools.SaveListToXmlSerializer(config, StationsPath);
        }

       

        public IEnumerable<Customer> CustomerList()
        {
            return XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
        }

     
        public void DeleteDrone(int id)
        {
            List<Drone> drones = XMLTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Drone drone = drones.FirstOrDefault(drone => drone.Id == id);
            drones.Remove(drone);
            drone.IsDelete = true;
            drones.Add(drone);
            XMLTools.SaveListToXmlSerializer(drones, dronesPath);
        }

 
        public void DeleteStation(int id)
        {
            XElement stationList = XMLTools.LoadListFromXmlElement(StationsPath);
            var station = stationList.Elements()
                                     .FirstOrDefault(xElement => int.Parse(xElement.Element(nameof(Station.Id)).Value) == id
                                                                 && !bool.Parse(xElement.Element(nameof(Station.IsDelete)).Value));

            if (station == null)
            {
                throw new DO.ObjectNotExistException("Station does not exist");
            }

            station.SetElementValue(nameof(Station.IsDelete), true);
            XMLTools.SaveListToXmlElement(stationList, StationsPath);
        }
      

        public Station GetStation(int id)
        {
            XElement StationsXML = XMLTools.LoadListFromXmlElement(StationsPath);
            foreach (var StationElement in StationsXML.Elements())
            {
                if (int.Parse(StationElement.Element("Id").Value) == id)
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
            throw new DO.ObjectNotExistException();
        }

        public int GetStationIdOfDroneCharge(int droneId)
        {

            return XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath).First(c => c.DroneId == droneId).StationId;
        }

   
        /// <summary>
        /// return Parcel List
        /// </summary>
        /// <param name="selectList">select parcel</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> ParcelList(Predicate<int?> selectList = default)
            => XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath).Where((c) => !c.IsDelete && (selectList == null || selectList(c.Droneld)));

        public void ParcelToDrone(int percelChoose, int droneChoose)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel parcel = GetParcel(percelChoose);
            parcels.Remove(parcel);
            parcel.Id = droneChoose;
            parcel.Schedulet = DateTime.Now;
            parcels.Add(parcel);
            XMLTools.SaveListToXmlSerializer(parcels, parcelsPath);
        }

       

        public double[] PowerConsumptionRequest()
        {
            XElement config = XDocument.Load(ConfigPath).Root;
            return config.Elements().Select(item => double.Parse(item.Value)).ToArray();
        }

        public DateTime StartChargeTime(int droneId)
        {
            List<DroneCharge> droneCharges = XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            int i = droneCharges.FindIndex((l) => l.DroneId == droneId);
            if (i < 0)
                throw new ObjectNotExistException("No charge for drone: " + droneId);
            return droneCharges[i].StartTime;
        }

        public void StationDroneIn(int baseStationId)
        {
            List<Station> stations = XMLTools.LoadListFromXmlSerializer<Station>(StationsPath);

            int i = stations.FindIndex(s => s.Id == baseStationId);
            if (i < 0)
                throw new ObjectNotExistException("Station not exist");
            Station station = stations[i];
            --station.ChargeSlot;
            stations[i] = station;

            XMLTools.SaveListToXmlSerializer(stations, StationsPath);
        }

        public void StationDroneOut(int baseStationId)
        {
            List<Station> stations = XMLTools.LoadListFromXmlSerializer<Station>(StationsPath);
            int i = stations.FindIndex(s => s.Id == baseStationId);
            if (i == -1)
                throw new ObjectNotExistException("station not exist");
            Station station = stations[i];
            ++station.ChargeSlot;
            stations[i] = station;

            XMLTools.SaveListToXmlSerializer(stations, StationsPath);

        }

        /// <summary>
        /// return StationList
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> StationList(Predicate<bool> selectList = default)
            => XMLTools.LoadListFromXmlSerializer<Station>(StationsPath).Where((c) => !c.IsDelete && (selectList == null || selectList(isEmptyChargeSlotInStation(c.Id, c.ChargeSlot))));

        public IEnumerable<Station> StationListStationList(Predicate<bool> selectList = null) =>
           selectList == null ?
               XMLTools.LoadListFromXmlSerializer<Station>(StationsPath) :
               XMLTools.LoadListFromXmlSerializer<Station>(StationsPath).Where(p => selectList(true));

       



        public void UpdateStation(Station id)
        {
            GetStation(id.Id);

            List<Station> stations = XMLTools.LoadListFromXmlSerializer<Station>(StationsPath);
            Station station = stations.First(e => e.Id == id.Id && !id.IsDelete);

            stations.Remove(station);
            stations.Add(id);

            XMLTools.SaveListToXmlSerializer(stations, StationsPath);
        }

        /// <summary>
        /// return if is empty charge slot in a station
        /// </summary>
        /// <param name="station">the choosen station</param>
        /// <returns></returns>
        private bool isEmptyChargeSlotInStation(int stationId, int stationChargeSlot)
        {
            int counter = 0;
            foreach (var ChargeSlot in XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath))
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

    }
}
