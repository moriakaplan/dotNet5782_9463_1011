using System;
using DalApi;
using System.Xml.Linq;
using System.IO;

namespace Dal
{
    public class DalXml: IDal
    {
        #region singleton
        private static DalXml instance;
        private static object syncRoot = new object();
        /// <summary>
        /// The public Instance property to use
        /// </summary>
        internal static DalXml Instance
        {
            //singelton thread safe and lazy initializion(?)
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DalXml();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region xml files definition
        XElement dronesRoot;
        XElement droneChargesRoot;
        XElement stationsRoot;
        XElement customersRoot;
        XElement parcelsRoot;
        string dronesPath = @"DalXml\Data\DronesXml.xml";
        string droneChargesPath = @"DalXml\Data\DroneChargesXml.xml";
        string stationsPath = @"DalXml\Data\StationsXml.xml";
        string customersPath = @"DalXml\Data\CustomersXml.xml";
        string parcelsPath = @"DalXml\Data\ParcelsXml.xml";
        #endregion

        public DalXml()
        {
            //DataSource.Initialize();
            if (!File.Exists(dronesPath))
                CreateFiles(dronesRoot, dronesPath, "drones");
            if (!File.Exists(droneChargesPath))
                CreateFiles(droneChargesRoot, droneChargesPath, "droneCharges");
            if (!File.Exists(stationsPath))
                CreateFiles(stationsRoot, stationsPath, "stations");
            if (!File.Exists(customersPath))
                CreateFiles(customersRoot, customersPath, "customers");
            if (!File.Exists(parcelsPath))
                CreateFiles(parcelsRoot, parcelsPath, "parcels");
            LoadData();
        }

        private void CreateFiles(XElement root, string path, string name)
        {
            root = new XElement(name);
            root.Save(path);
        }

        private void LoadData()
        {
            try
            {
                dronesRoot = XElement.Load(dronesPath);
                droneChargesRoot = XElement.Load(droneChargesPath);
                stationsRoot = XElement.Load(stationsPath);
                customersRoot = XElement.Load(customersPath);
                parcelsRoot = XElement.Load(parcelsPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }
    }
}
