using System;
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
    public partial class DalXml : IDal
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
        string dronesPath = @"Data\DronesXml.xml";
        string droneChargesPath = @"Data\DroneChargesXml.xml";
        string stationsPath = @"Data\StationsXml.xml";
        string customersPath = @"Data\CustomersXml.xml";
        string parcelsPath = @"Data\ParcelsXml.xml";
        string configPath = @"Data\Config.xml";
        string usersPath = @"Data\Users.xml";

        
        #endregion

        #region create, load and convert
        /// <summary>
        /// constractor
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DalXml()
        {
            
            if (!File.Exists(configPath)||!File.Exists(dronesPath)||!File.Exists(droneChargesPath)||!File.Exists(stationsPath)||!File.Exists(customersPath)||!File.Exists(parcelsPath)||!File.Exists(usersPath))
                CreateFiles();
            LoadData();
        }

        
        /// <summary>
        /// create the config xml
        /// </summary>
        private void createConfig()
        {
            configRoot = new XElement("configData");
            configRoot.Add(new XElement("managmentPassword", getGoodPass()));
            configRoot.Add(new XElement("parcelCode", 10000000));
            configRoot.Add(new XElement("available", 0.01));
            configRoot.Add(new XElement("light", 0.015));
            configRoot.Add(new XElement("medium", 0.02));
            configRoot.Add(new XElement("heavy", 0.025));
            configRoot.Add(new XElement("ratePerMinute", 120));
            configRoot.Save(configPath);
        }
        /// <summary>
        /// get strong password
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// load the xml
        /// </summary>
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
                throw new Exception("File upload problem");//***
            }
        }

        /// <summary>
        /// Convert Customer to  XElement
        /// </summary>
        /// <param name="cus"></param>
        /// <returns></returns>
        internal XElement ConvertCus(Customer cus)
        {
            XElement cusElement = new XElement("Customer");

            foreach (PropertyInfo item in typeof(Customer).GetProperties())
                cusElement.Add
                (
                new XElement(item.Name, item.GetValue(cus, null).ToString())
                );

            return cusElement;
        }

        /// <summary>
        /// Convert object to XElement
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Convert XElement to  customer
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer ConvertCus(XElement element)
        {
            return new Customer()
            {
                Id = int.Parse(element.Element("Id").Value),
                Name = element.Element("Name").Value,
                Phone = element.Element("Phone").Value,
                Longitude = double.Parse(element.Element("Longitude").Value),
                Lattitude = double.Parse(element.Element("Lattitude").Value)
            };
        }

        /// <summary>
        ///  Convert XElement to  object
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <summary>
        /// returns the manager password
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetManagmentPassword()
        {
            return configRoot.Element("managmentPassword").Value;
        }

        /// <summary>
        /// set new mananger password
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string SetNewManagmentPassword()
        {
            string pass = getGoodPass();
            configRoot.Element("managmentPassword").Remove();
            configRoot.Add(new XElement("managmentPassword", pass));
            return pass;
        }

        /// <summary>
        /// returns the user with the requested name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public User GetUser(string name)
        {
            List<User> users = XmlTools.LoadListFromXmlSerializer<User>(usersPath);
            User? result = users.Find(x => x.UserName == name);
            if (result == null)
                throw new UserException($"UserName {name} does not exist");
            return (User)result;
        }

        /// <summary>
        /// add user
        /// </summary>
        /// <param name="user"></param>
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

        /// <summary>
        /// get all the users
        /// </summary>
        /// <param name="pre"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetUsersList(Predicate<User> pre)
        {
            List<User> users = XmlTools.LoadListFromXmlSerializer<User>(usersPath);
            List<User> result = new List<User>(users);
            if (pre == null) return result;
            return result.FindAll(pre);
        }
        #endregion

        /// <summary>
        /// distance between 2 locations
        /// </summary>
        /// <param name="lattitudeA"></param>
        /// <param name="longitudeA"></param>
        /// <param name="lattitudeB"></param>
        /// <param name="longitudeB"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double Distance(double lattitudeA, double longitudeA, double lattitudeB, double longitudeB)
        {
            
            double lat1 = lattitudeA * (Math.PI / 180.0);
            double long1 = longitudeA * (Math.PI / 180.0);
            double lat2 = lattitudeB * (Math.PI / 180.0);
            double long2 = longitudeB * (Math.PI / 180.0) - long1;
            double distance = Math.Pow(Math.Sin((lat2 - lat1) / 2.0), 2.0) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(long2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(distance), Math.Sqrt(1.0 - distance))) / 1000;
        }

        /// <summary>
        /// return an array with the data of the config file- battery for available drone, drone with light/medium/heavy parcel and charging rate per minute.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetBatteryData()
        {
            //return configRoot.Elements().Select(x=>double.Parse(x.Value)).ToArray();
            return new double[] {
                double.Parse(configRoot.Element("available").Value),
                double.Parse(configRoot.Element("light").Value),
                double.Parse(configRoot.Element("medium").Value),
                double.Parse(configRoot.Element("heavy").Value),
                double.Parse(configRoot.Element("ratePerMinute").Value)
            };
        }

        /// <summary>
        /// create the xml files of the drones, customers, station, drone charges and users (prussume the config file is updated).
        /// </summary>
        private void CreateFiles()
        {
            List<User> u = new List<User>();
            u.Add(new User { Id = null, UserName = "general manager", Password = "123456", IsManager = true });
            XmlTools.SaveListToXmlSerializer<User>(u, usersPath);

            List<Drone> drones = new List<Drone>();
            List<DroneCharge> droneCharges = new List<DroneCharge>();
            List<Parcel> parcels = new List<Parcel>();
            List<Customer> customers = new List<Customer>();
            List<Station> stations = new List<Station>();
            Random random = new Random();
            configRoot = XElement.Load(configPath);
            int parcelCode = int.Parse(configRoot.Element("parcelCode").Value);
            stations.Add(new Station
            {
                Id = random.Next(1000, 10000),
                Name = "the israelian station",
                Longitude = 34.8,
                Lattitude = 32,
                ChargeSlots = 13
            });
            stations.Add(new Station
            {
                Id = random.Next(1000, 10000),
                Name = "the biggest station",
                Longitude = 35,
                Lattitude = 32,
                ChargeSlots = 112
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "superFalcon",
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "superFalcon",
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "superFalcon2",
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "nimbus2000",
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "AnaAref",
            });
            
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Yosef",
                Phone = "0501234567",
                Longitude = 35.2,
                Lattitude = 31.1
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Avi",
                Phone = "0503456789",
                Longitude = 34.8,
                Lattitude = 31.8
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Nahum",
                Phone = "0545678901",
                Longitude = 35.2,
                Lattitude = 31.7
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Moshe",
                Phone = "0523456789",
                Longitude = 34.7,
                Lattitude = 32
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Shlomo",
                Phone = "0521234567",
                Longitude = 34.6,
                Lattitude = 31.2
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Shira",
                Phone = "0502345678",
                Longitude = 35,
                Lattitude = 29.5
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Naama",
                Phone = "0531234567",
                Longitude = 35.2,
                Lattitude = 32.9
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Etya",
                Phone = "0501234569",
                Longitude = 34.8,
                Lattitude = 32.15
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Yosefa",
                Phone = "0501237567",
                Longitude = 34.78,
                Lattitude = 32.23
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Yosi",
                Phone = "0541234567",
                Longitude = 35.2,
                Lattitude = 31.7
            });

            parcels.Add(new Parcel
            {
                Id = ++parcelCode,
                Senderld = customers[1].Id,
                TargetId = customers[2].Id,
                Droneld = 0,
                Weight = (WeightCategories)random.Next(0, 3),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime = new DateTime(2021, 01, 01),
                AssociateTime = null,
                PickUpTime = null,
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++parcelCode,
                Senderld = customers[3].Id,
                TargetId = customers[4].Id,
                Droneld = 0,
                Weight = (WeightCategories)random.Next(0, 3),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime = new DateTime(2021, 01, 01),
                AssociateTime = null,
                PickUpTime = null,
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++parcelCode,
                Senderld = customers[5].Id,
                TargetId = customers[6].Id,
                Droneld = 0,
                Weight = (WeightCategories)random.Next(0, 3),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime = new DateTime(2021, 10, 19),
                AssociateTime = null,
                PickUpTime = null,
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++parcelCode,
                Senderld = customers[7].Id,
                TargetId = customers[8].Id,
                Droneld = 0,
                Weight = (WeightCategories)random.Next(0, 3),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime = new DateTime(2021, 10, 01),
                AssociateTime = null,
                PickUpTime = null,
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++parcelCode,
                Senderld = customers[1].Id,
                TargetId = customers[7].Id,
                Droneld = drones[0].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[0].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime = new DateTime(2021, 01, 01),
                AssociateTime = new DateTime(2021, 01, 02),
                PickUpTime = null,
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++parcelCode,
                Senderld = customers[9].Id,
                TargetId = customers[8].Id,
                Droneld = drones[1].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[1].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime = new DateTime(2021, 10, 18),
                AssociateTime = new DateTime(2021, 10, 19),
                PickUpTime = null,
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++parcelCode,
                Senderld = customers[1].Id,
                TargetId = customers[2].Id,
                Droneld = drones[2].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[2].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime = new DateTime(2021, 01, 05),
                AssociateTime = new DateTime(2021, 01, 08),
                PickUpTime = new DateTime(2021, 01, 12),
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++parcelCode,
                Senderld = customers[3].Id,
                TargetId = customers[4].Id,
                Droneld = drones[3].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[3].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime = new DateTime(2021, 08, 20),
                AssociateTime = new DateTime(2021, 08, 21),
                PickUpTime = new DateTime(2021, 08, 24),
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++parcelCode,
                Senderld = customers[6].Id,
                TargetId = customers[4].Id,
                Droneld = drones[2].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[2].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime = new DateTime(2021, 01, 01),
                AssociateTime = new DateTime(2021, 01, 03),
                PickUpTime = new DateTime(2021, 01, 05),
                DeliverTime = new DateTime(2021, 01, 06)
            });
            parcels.Add(new Parcel
            {
                Id = ++parcelCode,
                Senderld = customers[2].Id,
                TargetId = customers[5].Id,
                Droneld = drones[3].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[3].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime = new DateTime(2021, 02, 15),
                AssociateTime = new DateTime(2021, 02, 17),
                PickUpTime = new DateTime(2021, 02, 23),
                DeliverTime = new DateTime(2021, 03, 01),
            });
            configRoot.Element("parcelCode").Value = parcelCode.ToString();
            configRoot.Save(configPath);
            XmlTools.SaveListToXmlSerializer<Drone>(drones, dronesPath);
            XmlTools.SaveListToXmlSerializer<DroneCharge>(droneCharges, droneChargesPath);
            XmlTools.SaveListToXmlSerializer<Station>(stations, stationsPath);
            XmlTools.SaveListToXmlSerializer<Customer>(customers, customersPath);
            XmlTools.SaveListToXmlSerializer<Parcel>(parcels, parcelsPath);
        }
    }
}
