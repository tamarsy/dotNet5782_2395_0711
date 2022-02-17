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

    }
}
