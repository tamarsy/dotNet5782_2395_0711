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

    }
}
