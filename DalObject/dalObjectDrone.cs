﻿using DO;
using DalApi;
using System;
using System.Collections.Generic;
namespace DalObject
{
    internal partial class DalObject
    {
        /// <summary>
        /// add the drone that he gets to the list of the drones.
        /// </summary>
        /// <param name="drone"></param>
        public void AddDroneToTheList(Drone drone)
        {
            if (DataSource.drones.Exists(item => item.Id == drone.Id)) throw new DroneException($"id: {drone.Id} already exist"); //it suppose to be this type of exception????**** 
            DataSource.drones.Add(drone);
        }
        /// <summary>
        /// send the drone for charging
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        public void SendDroneToCharge(int droneId, int stationId)
        {
            if (!DataSource.drones.Exists(item => item.Id == droneId))
            {
                throw new DroneException($"id: {droneId} does not exist");
            }
            DroneCharge droneCharge = new DroneCharge { DroneId = droneId, StationId = stationId }; //add drone charge to the list for charging the drone
            DataSource.droneCharges.Add(droneCharge);
            for (int i = 0; i < DataSource.stations.Count; i++) //find the station and update its details
            {
                if (DataSource.stations[i].Id == stationId)
                {
                    if (DataSource.stations[i].ChargeSlots < 1) throw new StationException("");
                    Station s = DataSource.stations[i];
                    s.ChargeSlots--; //++?
                    DataSource.stations[i] = s;
                    return;
                }
            }
            throw new StationException($"id: {stationId} does not exist");
        }
        /// <summary>
        /// release the drone frome charging 
        /// </summary>
        /// <param name="droneId"></param>
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
        /// <summary>
        /// display a drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Drone DisplayDrone(int droneId)
        {
            foreach (Drone item in DataSource.drones)
            {
                if (item.Id == droneId)
                    return item;
            }
            throw new DroneException($"id: {droneId} does not exist");
            //Drone? dr = DataSource.drones.Find(item => item.Id == droneId);
            //if(dr==null) throw new DroneException($"id: {droneId} does not exist");
            //return (Drone)dr;
        }
        
        public IEnumerable<DroneCharge> DisplayListOfDroneCharge(Predicate<DroneCharge> pre)
        {
            List<DroneCharge> result = new List<DroneCharge>(DataSource.droneCharges);
            if (pre == null) return result;
            return result.FindAll(pre);
        }
        /// <summary>
        /// display the list of the drones.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> DisplayListOfDrones(Predicate<Drone> pre)
        {
            List<Drone> result = new List<Drone>(DataSource.drones);
            if (pre == null) return result;
            return result.FindAll(pre);
        }
        
        public void DeleteDrone(int droneId)
        {
            try { DataSource.drones.Remove(DisplayDrone(droneId)); }
            catch (ArgumentNullException)
            {
                throw new DroneException($"id: {droneId} does not exist");
            }
        }
    }
}