using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Dal
{
    class XMLTools
    {
        private static readonly string dirPath = @"xml\";
        static XMLTools()
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
        }
        
        #region SaveLoadWithXElement
        public static void SaveListToXmlElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(filePath);
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }

        public static XElement LoadListFromXmlElement(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    return XElement.Load(filePath);
                }
                else
                {
                    XElement rootElem = new XElement(filePath);
                    rootElem.Save(filePath);
                    return rootElem;
                }
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion                

        #region SaveLoadWithXMLSerializer
        public static void SaveListToXmlSerializer<T>(IEnumerable<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());

                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileException(filePath, $"fail to create xml file: {filePath}", ex);
            }
        }
        public static List<T> LoadListFromXmlSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {
                throw new DO.XMLFileException(filePath, $"fail to load xml file: {filePath}", ex);
            }
        }
        #endregion

        public static double GetInfo(string prop, string path)
        {
            XDocument document = XDocument.Load(path);
            XElement element = document.Root.Element(prop);
            return double.Parse(element.Value);
        }
    }
}
