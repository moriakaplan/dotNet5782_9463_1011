using IDAL.DO;
using System;
using System.Collections.Generic;

namespace DalObject
{
    public class DataSource
    {
        internal static List<Drone> drones = new List<Drone>();
        internal static List<DroneCharge> droneCharges = new List<DroneCharge>();
        internal static List<Parcel> parcels = new List<Parcel>();
        internal static List<Customer> customers = new List<Customer>();
        internal static List<Station> stations = new List<Station>();

        internal static Random random = new Random();
        internal static class Config
        {
            internal static int parcelCode = random.Next(10000000, 50000000);
        }
        public static void Initialize()//Initializes 2 stations, 5 drones, 10 customers and 10 parcels.
        {
            stations.Add(new Station
            {
                Id = random.Next(1000, 10000),
                Name = "the israelian station",
                Longitude = 34.8,
                Lattitude = 32,
                ChargeSlots = 163
            });
            stations.Add(new Station
            {
                Id = random.Next(1000, 10000),
                Name = "the biggest station",
                Longitude = -45,
                Lattitude = 78.3,
                ChargeSlots = 5408
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),
                Battery = random.NextDouble() * 100,
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "superFalcon",
                Status = DroneStatuses.Assigned
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000), 
                Battery = random.NextDouble() * 100,
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "superFalcon",
                Status = DroneStatuses.Assigned
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),
                Battery = random.NextDouble() * 100,
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "superFalcon2",
                Status = DroneStatuses.Sending
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),
                Battery = random.NextDouble() * 100,
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "nimbus2000",
                Status = DroneStatuses.Sending
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),
                Battery = random.NextDouble() * 100,
                MaxWeight = (WeightCategories)random.Next(0, 3),
                Model = "AnaAref",
                Status = DroneStatuses.Maintenance
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
                Id = ++Config.parcelCode,
                Senderld = customers[1].Id,
                TargetId = customers[2].Id,
                Droneld = 0,
                Weight = (WeightCategories)random.Next(0, 3),
                Priority = (Priorities)random.Next(0, 3),
                Requested = new DateTime(2021, 01, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[3].Id,
                TargetId = customers[4].Id,
                Droneld = 0,
                Weight = (WeightCategories)random.Next(0, 3),
                Priority = (Priorities)random.Next(0, 3),
                Requested = new DateTime(2021, 01, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[5].Id,
                TargetId = customers[6].Id,
                Droneld = 0,
                Weight = (WeightCategories)random.Next(0, 3),
                Priority = (Priorities)random.Next(0, 3),
                Requested = new DateTime(2021, 10, 19),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[7].Id,
                TargetId = customers[8].Id,
                Droneld = 0,
                Weight = (WeightCategories)random.Next(0, 3),
                Priority = (Priorities)random.Next(0, 3),
                Requested = new DateTime(2021, 10, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[1].Id,
                TargetId = customers[7].Id,
                Droneld = drones[0].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[0].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                Requested = new DateTime(2021, 01, 01),
                Scheduled = new DateTime(2021, 01, 02),
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[9].Id,
                TargetId = customers[8].Id,
                Droneld = drones[1].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[1].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                Requested = new DateTime(2021, 10, 18),
                Scheduled = new DateTime(2021, 10, 19),
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[1].Id,
                TargetId = customers[2].Id,
                Droneld = drones[2].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[2].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                Requested = new DateTime(2021, 01, 05),
                Scheduled = new DateTime(2021, 01, 08),
                PickedUp = new DateTime(2021, 01, 12),
                Delivered = DateTime.MinValue,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[3].Id,
                TargetId = customers[4].Id,
                Droneld = drones[3].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[3].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                Requested = new DateTime(2021, 08, 20),
                Scheduled = new DateTime(2021, 08, 21),
                PickedUp = new DateTime(2021, 08, 24),
                Delivered = DateTime.MinValue,
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[6].Id,
                TargetId = customers[4].Id,
                Droneld = drones[2].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[2].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                Requested = new DateTime(2021, 01, 01),
                Scheduled = new DateTime(2021, 01, 03),
                PickedUp = new DateTime(2021, 01, 05),
                Delivered = new DateTime(2021, 01, 06)
            });
            parcels.Add(new Parcel
            {
                Id = ++Config.parcelCode,
                Senderld = customers[2].Id,
                TargetId = customers[5].Id,
                Droneld = drones[3].Id,
                Weight = (WeightCategories)random.Next(0, (int)drones[3].MaxWeight),
                Priority = (Priorities)random.Next(0, 3),
                Requested = new DateTime(2021, 02, 15),
                Scheduled = new DateTime(2021, 02, 17),
                PickedUp = new DateTime(2021, 02, 23),
                Delivered = new DateTime(2021, 03, 01),
            });
        }
    }
}