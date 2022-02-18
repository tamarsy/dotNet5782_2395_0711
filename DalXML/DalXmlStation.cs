using Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL
{
    internal sealed partial class DalXML : IDal
    {

        public void AddStation(Station newStation)
        {
            List<Station> config = XMLTools.LoadListFromXmlSerializer<Station>(StationsPath);
            config.Add(newStation);
            XMLTools.SaveListToXmlSerializer(config, StationsPath);
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
                        ChargeSlot = int.Parse(StationElement.Element("ChargeSlot").Value),
                        Name = StationElement.Element("Name").Value,
                        Lattitude = double.Parse(StationElement.Element("Lattitude").Value),
                        Longitude = double.Parse(StationElement.Element("Longitude").Value),
                    };
                }
            }
            throw new DO.ObjectNotExistException();
        }
    }
}
