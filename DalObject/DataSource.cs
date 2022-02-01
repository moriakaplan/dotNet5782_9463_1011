using DO;
using System;
using System.Collections.Generic;

namespace Dal
{
    internal class DataSource
    {
        internal static List<Drone> drones = new List<Drone>();
        internal static List<DroneCharge> droneCharges = new List<DroneCharge>();
        internal static List<Parcel> parcels = new List<Parcel>();
        internal static List<Customer> customers = new List<Customer>();
        internal static List<Station> stations = new List<Station>();
        internal static List<User> users = new List<User>();

        internal static Random random = new Random();
        internal static class Config
        {
            internal static string managmentPassword = DalObject.getGoodPass();
            internal static int parcelCode = 10000000;
            public static double available=0.01; //battery per kilometer
            public static double easy=0.015;     //battery per kilometer
            public static double medium=0.02;    //battery per kilometer
            public static double heavy=0.025;    //battery per kilometer
            public static double ratePerMinute = 180;

            
        }
        
        public static void Initialize()//Initializes 2 stations, 5 drones, 10 customers and 10 parcels.
        {
            users.Add(new User
            {
                IsManager = true,
                UserName = "general manager",
                Password = "123456aA"
            });

            #region station
            stations.Add(new Station
            {
                Id = random.Next(1000, 10000),
                Name = "the israelian station",
                Longitude = 32,
                Lattitude = 32,
                ChargeSlots = 13
            });
            stations.Add(new Station
            {
                Id = random.Next(1000, 10000),
                Name = "the biggest station",
                Longitude = 32.5,
                Lattitude = 32,
                ChargeSlots = 112
            });
            #endregion
            #region drones
            drones.Add(new Drone //associated
            {
                Id = random.Next(100000, 1000000),
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "superFalcon",
            });
            drones.Add(new Drone //associated
            {
                Id = random.Next(100000, 1000000), 
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "superFalcon1",
            });
            drones.Add(new Drone//associated
            {
                Id = random.Next(100000, 1000000),
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "superFalcon2",
            });
            drones.Add(new Drone//associated
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
            droneCharges.Add(new DroneCharge 
            {
                DroneId = drones[4].Id, 
                StartedChargeTime = DateTime.Now,
                StationId = stations[random.Next(0, 2)].Id 
            });
            #endregion
            #region customers
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Yosef",
                Phone = "0501234567",
                Longitude = 32,
                Lattitude = 31.9
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Avi",
                Phone = "0503456789",
                Longitude = 32.6,
                Lattitude = 31.8
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Nahum",
                Phone = "0545678901",
                Longitude = 32.01,
                Lattitude = 32.8
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Moshe",
                Phone = "0523456789",
                Longitude = 32.7,
                Lattitude = 32.8
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Shlomo",
                Phone = "0521234567",
                Longitude = 32.7,
                Lattitude = 31.2
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Shira",
                Phone = "0502345678",
                Longitude = 32.9,
                Lattitude = 31.9
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Naama",
                Phone = "0531234567",
                Longitude = 32.2,
                Lattitude = 32.9
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Etya",
                Phone = "0501234569",
                Longitude = 32.8,
                Lattitude = 32.15
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Yosefa",
                Phone = "0501237567",
                Longitude = 32.78,
                Lattitude = 32.23
            });
            customers.Add(new Customer
            {
                Id = random.Next(100000000, 1000000000),
                Name = "Yosi",
                Phone = "0541234567",
                Longitude = 32.2,
                Lattitude = 31.7
            });
            #endregion customers
            #region parcels
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
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
                Id = ++Config.parcelCode,
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
                Id = ++Config.parcelCode,
                Senderld = customers[5].Id,
                TargetId = customers[6].Id,
                Droneld = 0,
                Weight = (WeightCategories)random.Next(0, 3),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime =  new DateTime(2021, 10, 19),
                AssociateTime = null,
                PickUpTime = null,
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[7].Id,
                TargetId = customers[8].Id,
                Droneld = 0,
                Weight = (WeightCategories)random.Next(0, 3),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime =  new DateTime(2021, 10, 01),
                AssociateTime = null,
                PickUpTime = null,
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[1].Id,
                TargetId = customers[7].Id,
                Droneld = drones[0].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[0].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime =  new DateTime(2021, 01, 01),
                AssociateTime= new DateTime(2021, 01, 02),
                PickUpTime = null,
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[9].Id,
                TargetId = customers[8].Id,
                Droneld = drones[1].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[1].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime =  new DateTime(2021, 10, 18),
                AssociateTime= new DateTime(2021, 10, 19),
                PickUpTime = null,
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[1].Id,
                TargetId = customers[2].Id,
                Droneld = drones[2].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[2].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime =  new DateTime(2021, 01, 05),
                AssociateTime = new DateTime(2021, 01, 08),
                PickUpTime = new DateTime(2021, 01, 12),
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[3].Id,
                TargetId = customers[4].Id,
                Droneld = drones[3].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[3].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime =  new DateTime(2021, 08, 20),
                AssociateTime = new DateTime(2021, 08, 21),
                PickUpTime = new DateTime(2021, 08, 24),
                DeliverTime = null,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[6].Id,
                TargetId = customers[4].Id,
                Droneld = drones[2].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[2].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime =  new DateTime(2021, 01, 01),
                AssociateTime = new DateTime(2021, 01, 03),
                PickUpTime = new DateTime(2021, 01, 05),
                DeliverTime = new DateTime(2021, 01, 06)
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[2].Id,
                TargetId = customers[5].Id,
                Droneld = drones[3].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[3].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                CreateTime =  new DateTime(2021, 02, 15),
                AssociateTime = new DateTime(2021, 02, 17),
                PickUpTime = new DateTime(2021, 02, 23),
                DeliverTime = new DateTime(2021, 03, 01),
            });
            #endregion parcels
        }
    }
}