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
        internal static class Config
        {
            internal static int parcelCode = 0;
            internal static Random random = new Random();
        }
        static void Initialize()
        {
            drones.Add(new Drone { 
                Id = 3456,// for parcel use: Config.parcelCode++, 
                Battery = 98.99,
                MaxWeight = WeightCategories.Easy, 
                Model = "superFalcon",
                Status = DroneStatuses.Vacant 
            });
            drones.Add(new Drone { 
                Id = 6778, 
                Battery = 98.99,
                MaxWeight = WeightCategories.Medium, 
                Model = "AnaAref",
                Status = DroneStatuses.Sending 
            });
        }

    }
}
