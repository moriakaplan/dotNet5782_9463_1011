﻿using System;
using DalApi;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using DO;

namespace Dal
{
    public partial class DalXml: IDal
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

        #region create, load and convert
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

        XElement ConvertDrone(Drone drone)
        {
            XElement DroneElement = new XElement("drone");

            foreach (PropertyInfo item in drone.GetType().GetProperties())
                    DroneElement.Add
                    (
                    new XElement(item.Name, item.GetValue(drone, null).ToString())
                    );
            
            return DroneElement;
        }

        XElement ConvertSomething(object obj, string name)
        {
            XElement Element = new XElement(name);

            foreach (PropertyInfo item in obj.GetType().GetProperties())
                Element.Add
                (
                new XElement(item.Name, item.GetValue(obj, null).ToString())
                );

            return Element;
        }

        Drone ConvertDrone(XElement element)
        {
            Drone drone = new Drone();

            foreach (PropertyInfo item in typeof(Drone).GetProperties())
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(item.PropertyType);
                object convertValue = typeConverter.ConvertFromString(element.Element(item.Name).Value);

                if (item.CanWrite)
                    item.SetValue(drone, convertValue);
            }

            return drone;
        }
        object ConvertSomething(XElement element, Type type)
        {
            object obj = new object();

            foreach (PropertyInfo item in type.GetProperties())
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(item.PropertyType);
                object convertValue = typeConverter.ConvertFromString(element.Element(item.Name).Value);

                if (item.CanWrite)
                    item.SetValue(obj, convertValue);
            }

            return obj;
        }
        #endregion

        //from dalObject, maybe need changes or to be deleted or something

        /// <summary>
        /// bonus 2
        /// calculates the distance between a point and a station
        /// </summary>
        /// <param name="longitudeA"></param>
        /// <param name="lattitudeA"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public double DistanceForStation(double longitudeA, double lattitudeA, int id)
        {
            Station st = DisplayStation(id);
            return Distance(longitudeA, lattitudeA, st.Longitude, st.Lattitude);
        }
        /// <summary>
        /// bonus 2
        /// calculates the distance between a point and a Customer
        /// </summary>
        /// <param name="longitudeA"></param>
        /// <param name="lattitudeA"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public double DistanceForCustomer(double longitudeA, double lattitudeA, int id)
        {
            Customer cu = DisplayCustomer(id);
            return Distance(longitudeA, lattitudeA, cu.Longitude, cu.Lattitude);
        }
        public double Distance(double lattitudeA, double longitudeA, double lattitudeB, double longitudeB)
        {
            var radiansOverDegrees = (Math.PI / 180.0);

            var latitudeRadiansA = lattitudeA * radiansOverDegrees;
            var longitudeRadiansA = longitudeA * radiansOverDegrees;
            var latitudeRadiansB = lattitudeB * radiansOverDegrees;
            var longitudeRadiansB = longitudeB * radiansOverDegrees;
            // Haversine formula
            double dlon = longitudeB - longitudeA;
            double dlat = lattitudeB - lattitudeA;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(lattitudeA) * Math.Cos(lattitudeB) *
                       Math.Pow(Math.Sin(dlon / 2), 2);
            double c = 2 * Math.Asin(Math.Sqrt(a));
            //Radius of earth in kilometers.
            double r = 6371;
            // calculate the result
            return (c * r);
        }
        public double[] GetBatteryData()
        {
            //return new double[] {
            //    DataSource.Config.available,
            //    DataSource.Config.easy,
            //    DataSource.Config.medium,
            //    DataSource.Config.heavy,
            //    DataSource.Config.ratePerHour };
        }
    }
}