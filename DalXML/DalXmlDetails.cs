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

namespace DalXML
{
     partial class DalXml
    {
        private readonly string DetailsPath = $@"{Directory.GetCurrentDirectory()}/../../../../xml/Details.xml";
        static public double vacent { get; set; } = XMLTools.LoadListFromXmlSerializer<double>(detailsPath).Add(vacent = 4);
        static public double LightWeightCarrier { get; set; } = XMLTools.LoadListFromXmlSerializer<double>(DetailsPath).Add(LightWeightCarrier = 6);
        static public double MediumWeightCarrier { get; set; } = XMLTools.LoadListFromXmlSerializer<double>(DetailsPath).Add(MediumWeightCarrier = 6);
        static public double heavyWeightCarrier { get; set; } = XMLTools.LoadListFromXmlSerializer<double>(DetailsPath).Add(heavyWeightCarrier = 7);
        static public double SkimmerLoadingRate { get; set; } = XMLTools.LoadListFromXmlSerializer<double>(DetailsPath).Add(SkimmerLoadingRate = 3);
        public static int runNumForParcel = XMLTools.LoadListFromXmlSerializer<double>(DetailsPath).Add(runNumForParcel =03);
    }
}