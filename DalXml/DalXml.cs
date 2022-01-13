﻿using System;
using DalApi;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


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
        XElement configRoot;
        XElement usersRoot;
        string dronesPath = @"Data\DronesXml.xml";
        string droneChargesPath = @"Data\DroneChargesXml.xml";
        string stationsPath = @"Data\StationsXml.xml";
        string customersPath = @"Data\CustomersXml.xml";
        string parcelsPath = @"Data\ParcelsXml.xml";
        string configPath = @"Data\Config.xml";
        string usersPath = @"Data\Users.xml";

        //static string path = @"Data\";
        //string dronesPath = path + "DronesXml.xml";
        //string droneChargesPath = path + "DroneChargesXml.xml";
        //string stationsPath = path + "StationsXml.xml";
        //string customersPath = path + "CustomersXml.xml";
        //string parcelsPath = path + "ParcelsXml.xml";
        //string configPath = path + "Config.xml";
        #endregion

        #region create, load and convert
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DalXml()
        {
            //DataSource.Initialize();
            if (!File.Exists(dronesPath))
                //CreateFiles(dronesRoot, dronesPath, "drones");
                XmlTools.SaveListToXmlSerializer<Drone>(new List<Drone>(), dronesPath);
            if (!File.Exists(droneChargesPath))
                //CreateFiles(droneChargesRoot, droneChargesPath, "droneCharges");
                XmlTools.SaveListToXmlSerializer<DroneCharge>(new List<DroneCharge>(), droneChargesPath);
            if (!File.Exists(stationsPath))
                //CreateFiles(stationsRoot, stationsPath, "stations");
                XmlTools.SaveListToXmlSerializer<Station>(new List<Station>(), stationsPath);
            if (!File.Exists(customersPath))
                //CreateFiles(customersRoot, customersPath, "customers");
                XmlTools.SaveListToXmlSerializer<Customer>(new List<Customer>(), customersPath);
            if (!File.Exists(parcelsPath))
                //CreateFiles(parcelsRoot, parcelsPath, "parcels");
                XmlTools.SaveListToXmlSerializer<Parcel>(new List<Parcel>(), parcelsPath);
            if (!File.Exists(usersPath))
            {
                List<User> u = new List<User>();
                u.Add(new User { Id = null, UserName = "general manager", Password = "123456", IsManager = true });
                u.Add(new User { Id = null, UserName = "", Password = "", IsManager = true }); //****
                XmlTools.SaveListToXmlSerializer<User>(u, usersPath);
            }
            if (!File.Exists(configPath))
                CreateConfig();
            LoadData();
        }

        private void CreateFiles(XElement root, string path, string name)
        {
            root = new XElement(name);
            root.Save(path);
        }

        private void CreateConfig()
        {
            configRoot = new XElement("configData");
            configRoot.Add(new XElement("managmentPassword", getGoodPass()));
            configRoot.Add(new XElement("parcelCode", 10000000));
            configRoot.Add(new XElement("available", 0.01));
            configRoot.Add(new XElement("easy", 0.012));
            configRoot.Add(new XElement("medium", 0.013));
            configRoot.Add(new XElement("heavy", 0.014));
            configRoot.Add(new XElement("ratePerMinute", 10));
            configRoot.Save(configPath);
        }
        private string getGoodPass()
        {
            Random rand = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[8];
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[rand.Next(chars.Length)];
            }
            return new String(stringChars);
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
                    configRoot = XElement.Load(configPath);
                }
                catch
                {
                    throw new Exception("File upload problem");
                }
            }

        XElement ConvertCus(Customer cus)
        {
            XElement cusElement = new XElement("Customer");

            foreach (PropertyInfo item in typeof(Customer).GetProperties())
                cusElement.Add
                (
                new XElement(item.Name, item.GetValue(cus, null).ToString())
                );

            return cusElement;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public XElement ConvertSomething(object obj, string name)
        {
            XElement Element = new XElement(name);

            foreach (PropertyInfo item in obj.GetType().GetProperties())
                Element.Add
                (
                new XElement(item.Name, item.GetValue(obj, null).ToString())
                );

            return Element;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer ConvertCus(XElement element)
        {
            Customer cus = new Customer()
            {
                Id = int.Parse(element.Element("Id").Value),
                Name = element.Element("Name").Value,
                Phone = element.Element("Phone").Value,
                Longitude = double.Parse(element.Element("Longitude").Value),
                Lattitude = double.Parse(element.Element("Lattitude").Value)
            };
            return cus;
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

        #region users functions

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string getManagmentPassword()
        {
            return configRoot.Element("managmentPassword").Value;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string setNewManagmentPassword()
        {
            string pass = getGoodPass();
            configRoot.Element("managmentPassword").Remove();
            configRoot.Add(new XElement("managmentPassword", pass));
            return pass;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public User GetUser(string name)
        {
            List<User> users = XmlTools.LoadListFromXmlSerializer<User>(usersPath);
            User? result = users.Find(x => x.UserName == name);
            if (result == null)
                throw new UserException($"UserName {name} does not exist");
            return (User)result;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddUser(User user)
        {
            List<User> users = XmlTools.LoadListFromXmlSerializer<User>(usersPath);
            if (user.IsManager == false)
            {
                try { GetCustomer((int)user.Id); }
                catch (CustomerException) 
                { 
                    throw new UserException($"the customer {user.Id} is not exist"); 
                }
                if (users.Exists(item => item.Id == user.Id))
                    throw new UserException($"User for the customer {user.Id} already exist");
            }
            if (users.Exists(item => item.UserName == user.UserName)) 
                throw new UserException($"User with the usernam '{user.Id}' already exist");
            users.Add(user);
            XmlTools.SaveListToXmlSerializer<User>(users, usersPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetUsersList(Predicate<User> pre)
        {
            List<User> users = XmlTools.LoadListFromXmlSerializer<User>(usersPath);
            List<User> result = new List<User>(users);
            if (pre == null) return result;
            return result.FindAll(pre);
        }
        #endregion

        //from dalObject, maybe need changes or to be deleted or something
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetBatteryData()
        {
            //return configRoot.Elements().Select(x=>double.Parse(x.Value)).ToArray();

            return new double[] {
                double.Parse(configRoot.Element("available").Value),
                double.Parse(configRoot.Element("easy").Value),
                double.Parse(configRoot.Element("medium").Value),
                double.Parse(configRoot.Element("heavy").Value),
                double.Parse(configRoot.Element("ratePerMinute").Value)
            };
        }
    }
}
