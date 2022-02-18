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
        public void AddParcel(Parcel newParcel)         
        {
            List<Parcel> config = XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            config.Add(newParcel);
            XMLTools.SaveListToXmlSerializer(config, parcelsPath);

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

        public IEnumerable<Parcel> ParcelList(Predicate<Parcel> selectList = null) =>
         selectList == null ?
             XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath) :
             XMLTools.LoadListFromXmlSerializer<Parcel>(parcelsPath).Where(p => selectList(p));

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


    }
}