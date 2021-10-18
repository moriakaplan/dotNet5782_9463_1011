using IDAL.DO;
using System;
using System.Collections.Generic;

namespace DalObject
{
    class DataSource
    {
        internal List<Drone> drones = new List<Drone>();
        internal List<DroneCharge> droneCharges = new List<DroneCharge>();
        internal List<Parcel> parcels = new List<Parcel>();
        internal List<Customer> customers = new List<Customer>();
        internal List<Station> stations = new List<Station>();
    }
}
