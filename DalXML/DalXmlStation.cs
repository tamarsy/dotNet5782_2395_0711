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
        public static IDal Instance { get; } = new DalXML();

        public void AddCustomer(Customer newCustomer)
        {
            List<Customer> customers = XMLTools.LoadListFromXmlSerializer<Customer>(ConfigPath);
            customers.Add(newCustomer);
            XMLTools.SaveListToXmlSerializer(customers, customersPath);
        }

      
        public void AddDroneCharge(int droneId, int baseStationId)
        {
            List<DroneCharge> charges = XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            charges.Add(new DroneCharge() { DroneId = droneId, StationId = baseStationId, StartTime = DateTime.Now });
            XMLTools.SaveListToXmlSerializer(charges, droneChargesPath);
        }

        public void AddParcel(Parcel newParcel)
        {
            List<Parcel> config = XMLTools.LoadListFromXmlSerializer<Parcel>(ConfigPath);
            config.Add(newParcel);
            XMLTools.SaveListToXmlSerializer(config, parcelsPath);

        }

        public void AddStation(Station newStation)
        {
            List<Station> config = XMLTools.LoadListFromXmlSerializer<Station>(StationsPath);
            config.Add(newStation);
            XMLTools.SaveListToXmlSerializer(config, StationsPath);
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

        public void DeleteDroneCharge(int droneId)
        {
            List<DroneCharge> charges = XMLTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            DroneCharge charge = charges.FirstOrDefault(c => c.DroneId == droneId);
            charges.Remove(charge);

            XMLTools.SaveListToXmlSerializer(charges, droneChargesPath);
        }

        public void DeleteParcel(int id)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel parcel = parcels.FirstOrDefault(parcel => parcel.Id == id);
            parcels.Remove(parcel);
            parcel.IsDelete = true;
            parcels.Add(parcel);
            XMLTools.SaveListToXmlSerializer(parcels, parcelsPath);
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

        public void Destination(int percelChoose)
        {

            List<Parcel> parcels = XMLTools.LoadListFromXmlSerializer<Parcel>(customersPath);
            int index = parcels.FindIndex(p => p.Id == percelChoose && !p.IsDelete);
            if (index < 0)
                throw new ObjectNotExistException("Error!! Ther is no drone with this id");

            Parcel parcel = parcels[index];
            parcel.Delivered = DateTime.Now;
            parcels[index] = parcel;

            XMLTools.SaveListToXmlSerializer(parcels, parcelsPath);
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

        public IEnumerable<Parcel> ParcelList(Predicate<Parcel> selectList = null) =>
           selectList == null ?
               XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath) :
               XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath).Where(p => selectList(p));


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

        public void PickParcel(int percelChoose)
        {
            List<Parcel> parcels = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);

            int i = parcels.FindIndex(pa => pa.Id == percelChoose);
            if (i < 0)
                throw new ObjectNotExistException("Error!! Ther is no drone with this id");

            Parcel p = parcels[i];
            p.PickedUp = DateTime.Now;
            parcels[i] = p;

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

        /// <summary>
        /// Exception: ArgumentNullException, ObjectNotExistException
        /// Update the Customer details
        /// </summary>
        /// <param name="customer"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer customer)
        {
            if (customer.Equals(default(Customer)))
                throw new ArgumentNullException("Null argument");

            var customers = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
            int i = customers.FindIndex(s => s.Id == customer.Id);
            if (i < 0)
                throw new ObjectNotExistException("not found a customer with id = " + customer.Id);
            customers[i] = customer;

            XMLTools.SaveListToXmlSerializer(customers, customersPath);
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
