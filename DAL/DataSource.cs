using IDAL.DO;
using System;
using System.Collections.Generic;

namespace DalObject
{
    class DataSource
    {
        internal static List<Drone> drones = new List<Drone>();
        internal static List<DroneCharge> droneCharges = new List<DroneCharge>();
        internal static List<Parcel> parcels = new List<Parcel>();
        internal static List<Customer> customers = new List<Customer>();
        internal static List<Station> stations = new List<Station>();
        internal static Random random = new Random();
        internal static class Config
        {
            internal static int parcelCode = 0;
        }
        static void Initialize()
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
                Id = random.Next(100000, 1000000),// for parcel use: Config.parcelCode++, 
                Battery = random.Next(100),
                MaxWeight = WeightCategories.Easy,
                Model = "superFalcon",
                Status = DroneStatuses.Vacant
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),
                Battery = random.Next(100),
                MaxWeight = WeightCategories.Medium,
                Model = "AnaAref",
                Status = DroneStatuses.Maintenance
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),// for parcel use: Config.parcelCode++, 
                Battery = random.Next(100),
                MaxWeight = WeightCategories.Easy,
                Model = "superFalcon",
                Status = DroneStatuses.Vacant
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),// for parcel use: Config.parcelCode++, 
                Battery = random.Next(100),
                MaxWeight = WeightCategories.Heavy,
                Model = "superFalcon2",
                Status = DroneStatuses.Sending
            });
            drones.Add(new Drone
            {
                Id = random.Next(100000, 1000000),// for parcel use: Config.parcelCode++, 
                Battery = random.Next(100),
                MaxWeight = WeightCategories.Easy,
                Model = "nimbus2000",
                Status = DroneStatuses.Vacant
            });
            customers.Add(new Customer
            {
                Id = random.Next(1000000000),
                Name = "Yosef",
                Phone = "0501234567",
                Longitude = random.Next(-180, 180)/*35.2*/,
                Lattitude = random.Next(-180, 180)/*31.1*/
            });
            customers.Add(new Customer
            {
                Id = random.Next(1000000000),
                Name = "Avi",
                Phone = "0503456789",
                Longitude = random.Next(-180, 180),
                Lattitude = random.Next(-180, 180)
            });
            customers.Add(new Customer
            {
                Id = random.Next(1000000000),
                Name = "Nahum",
                Phone = "0545678901",
                Longitude = random.Next(-180, 180),
                Lattitude = random.Next(-180, 180)
            });
            customers.Add(new Customer
            {
                Id = random.Next(1000000000),
                Name = "Moshe",
                Phone = "0523456789",
                Longitude = random.Next(-180, 180),
                Lattitude = random.Next(-180, 180)
            });
            customers.Add(new Customer
            {
                Id = random.Next(1000000000),
                Name = "Shlomo",
                Phone = "0521234567",
                Longitude = random.Next(-180, 180),
                Lattitude = random.Next(-180, 180)
            });
            customers.Add(new Customer
            {
                Id = random.Next(1000000000),
                Name = "Shira",
                Phone = "0502345678",
                Longitude = random.Next(-180, 180),
                Lattitude = random.Next(-180, 180)
            });
            customers.Add(new Customer
            {
                Id = random.Next(1000000000),
                Name = "Naama",
                Phone = "0531234567",
                Longitude = random.Next(-180, 180),
                Lattitude = random.Next(-180, 180)
            });
            customers.Add(new Customer
            {
                Id = random.Next(1000000000),
                Name = "Etya",
                Phone = "0501234569",
                Longitude = random.Next(-180, 180),
                Lattitude = random.Next(-180, 180)
            });
            customers.Add(new Customer
            {
                Id = random.Next(1000000000),
                Name = "Yosefa",
                Phone = "0501237567",
                Longitude = random.Next(-180, 180),
                Lattitude = random.Next(-180, 180)
            });
            customers.Add(new Customer
            {
                Id = random.Next(1000000000),
                Name = "Yosi",
                Phone = "0541234567",
                Longitude = random.Next(-180, 180),
                Lattitude = random.Next(-180, 180)
            });
            parcels.Add(new Parcel
            {
                Id = random.Next(10000, 100000),
                Senderld = random.Next(1000000000),
                TargetId = random.Next(1000000000),
                Droneld = random.Next(100000, 1000000),
                Weight = WeightCategories.Easy,
                Priority = Priorities.Emergency,
                Requested = new DateTime(2021, 01, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            });
            parcels.Add(new Parcel
            {
                Id = random.Next(10000, 100000),
                Senderld = random.Next(1000000000),
                TargetId = random.Next(1000000000),
                Droneld = random.Next(100000, 1000000),
                Weight = WeightCategories.Easy,
                Priority = Priorities.Emergency,
                Requested = new DateTime(2021, 01, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            }); parcels.Add(new Parcel
            {
                Id = random.Next(10000, 100000),
                Senderld = random.Next(1000000000),
                TargetId = random.Next(1000000000),
                Droneld = random.Next(100000, 1000000),
                Weight = WeightCategories.Easy,
                Priority = Priorities.Emergency,
                Requested = new DateTime(2021, 01, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            }); parcels.Add(new Parcel
            {
                Id = random.Next(10000, 100000),
                Senderld = random.Next(1000000000),
                TargetId = random.Next(1000000000),
                Droneld = random.Next(100000, 1000000),
                Weight = WeightCategories.Easy,
                Priority = Priorities.Emergency,
                Requested = new DateTime(2021, 01, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            }); parcels.Add(new Parcel
            {
                Id = random.Next(10000, 100000),
                Senderld = random.Next(1000000000),
                TargetId = random.Next(1000000000),
                Droneld = random.Next(100000, 1000000),
                Weight = WeightCategories.Easy,
                Priority = Priorities.Emergency,
                Requested = new DateTime(2021, 01, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            }); parcels.Add(new Parcel
            {
                Id = random.Next(10000, 100000),
                Senderld = random.Next(1000000000),
                TargetId = random.Next(1000000000),
                Droneld = random.Next(100000, 1000000),
                Weight = WeightCategories.Easy,
                Priority = Priorities.Emergency,
                Requested = new DateTime(2021, 01, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            }); parcels.Add(new Parcel
            {
                Id = random.Next(10000, 100000),
                Senderld = random.Next(1000000000),
                TargetId = random.Next(1000000000),
                Droneld = random.Next(100000, 1000000),
                Weight = WeightCategories.Easy,
                Priority = Priorities.Emergency,
                Requested = new DateTime(2021, 01, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            }); parcels.Add(new Parcel
            {
                Id = random.Next(10000, 100000),
                Senderld = random.Next(1000000000),
                TargetId = random.Next(1000000000),
                Droneld = random.Next(100000, 1000000),
                Weight = WeightCategories.Easy,
                Priority = Priorities.Emergency,
                Requested = new DateTime(2021, 01, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            }); parcels.Add(new Parcel
            {
                Id = random.Next(10000, 100000),
                Senderld = random.Next(1000000000),
                TargetId = random.Next(1000000000),
                Droneld = random.Next(100000, 1000000),
                Weight = WeightCategories.Easy,
                Priority = Priorities.Emergency,
                Requested = new DateTime(2021, 01, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            }); parcels.Add(new Parcel
            {
                Id = random.Next(10000, 100000),
                Senderld = random.Next(1000000000),
                TargetId = random.Next(1000000000),
                Droneld = random.Next(100000, 1000000),
                Weight = WeightCategories.Easy,
                Priority = Priorities.Emergency,
                Requested = new DateTime(2021, 01, 01),
                Scheduled = DateTime.MinValue,
                PickedUp = DateTime.MinValue,
                Delivered = DateTime.MinValue,
            });
        }
    }
}