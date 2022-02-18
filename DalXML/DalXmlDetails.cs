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
    internal sealed partial class DalXml
    {
        static private string detailsPath = $@"{Directory.GetCurrentDirectory()}/../../../../xml/Details.xml";
        static public double vacent { get; set; } = XMLTools.GetInfo("vacent", detailsPath);
        static public double LightWeightCarrier { get; set; } = XMLTools.GetInfo("LightWeightCarrier", detailsPath);
        static public double MediumWeightCarrier { get; set; } = XMLTools.GetInfo("MediumWeightCarrier", detailsPath);
        static public double heavyWeightCarrier { get; set; } = XMLTools.GetInfo("heavyWeightCarrier", detailsPath);
        static public double SkimmerLoadingRate { get; set; } = XMLTools.GetInfo("SkimmerLoadingRate", detailsPath);
        static public double RunNumForParcel { get; set; } = XMLTools.GetInfo("RunNumForParcel", detailsPath);

    }
}