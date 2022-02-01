using DO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace Dal
{
    internal partial class DalObject
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone drone)
        {
            if (DataSource.drones.Exists(item => item.Id == drone.Id)) throw new DroneException($"id: {drone.Id} already exist");
            DataSource.drones.Add(drone);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendDroneToCharge(int droneId, int stationId)
        {
            if (!DataSource.drones.Exists(item => item.Id == droneId))
            {
                throw new DroneException($"id: {droneId} does not exist");
            }
            DroneCharge droneCharge = new DroneCharge { DroneId = droneId, StationId = stationId, StartedChargeTime=DateTime.Now }; //add drone charge to the list for charging the drone
            DataSource.droneCharges.Add(droneCharge);
            for (int i = 0; i < DataSource.stations.Count; i++) //find the station and update its details
            {
                if (DataSource.stations[i].Id == stationId)
                {
                    if (DataSource.stations[i].ChargeSlots < 1) throw new StationException("");
                    Station s = DataSource.stations[i];
                    s.ChargeSlots--; 
                    DataSource.stations[i] = s;
                    return;
                }
            }
            throw new StationException($"id: {stationId} does not exist");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDroneFromeCharge(int droneId)
        {
            DroneCharge dCharge;
            if(!DataSource.droneCharges.Exists(x => x.DroneId == droneId)) 
                throw new DroneChargeException($"drone charge with the drone ID {droneId} does not exist");
            dCharge = DataSource.droneCharges.Find(x => x.DroneId == droneId);
            DataSource.droneCharges.Remove(dCharge);
            for (int i = 0; i < DataSource.stations.Count; i++) //find the station and update its details
            {
                if (DataSource.stations[i].Id == dCharge.StationId)
                {
                    Station s = DataSource.stations[i];
                    s.ChargeSlots++; //--?
                    DataSource.stations[i] = s;
                    return;
                }
            }
            throw new StationException($"id: {dCharge.StationId} does not exist");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneId)
        {
            if (!DataSource.drones.Exists(item => item.Id == droneId)) 
                throw new DroneException($"id: {droneId} does not exist");
            return DataSource.drones.Find(item => item.Id == droneId);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int droneId)
        {
            if (!DataSource.droneCharges.Exists(item => item.DroneId == droneId))
                throw new DroneChargeException($"DroneCharge for drone {droneId} does not exist");
            return DataSource.droneCharges.Find(item => item.DroneId == droneId);
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDroneChargesList(Predicate<DroneCharge> pre)
        {
            List<DroneCharge> result = new List<DroneCharge>(DataSource.droneCharges);
            if (pre == null) return result;
            return result.FindAll(pre);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDronesList(Predicate<Drone> pre)
        {
            List<Drone> result = new List<Drone>(DataSource.drones);
            if (pre == null) return result;
            return result.FindAll(pre);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int droneId)
        {
            try { DataSource.drones.Remove(GetDrone(droneId)); }
            catch (ArgumentNullException)
            {
                throw new DroneException($"id: {droneId} does not exist");
            }
            try { DataSource.droneCharges.Remove(GetDroneCharge(droneId)); }
            catch { }
        }
    }
}