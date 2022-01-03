using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.Linq;


namespace Dal
{
    class XmlTools
    {
        static string dir = @"..\dotNet5782_9463_1011\";
        static XmlTools()
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        public static void SaveListToXmlSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(dir + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex) //****
            {
                throw new Exception(ex.Message+$"\nfail to save {filePath}");
            }
        }
        public static List<T> LoadListFromXmlSerializer<T>(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    //XElement file = XElement.Load(filePath);
                    //return file.Elements().Select(x => (T)DalXml.ConvertSomething(x, typeof(T))).ToList();
                }
                if (File.Exists(dir + filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(dir + filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex) //****
            {
                throw new Exception(ex.Message+$"\nfail to load {filePath}");
            }
        }
    }
}
