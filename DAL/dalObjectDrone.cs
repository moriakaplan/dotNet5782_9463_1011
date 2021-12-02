using IDAL.DO;
using IDAL;
using System;
using System.Collections.Generic;
namespace DalObject
{
    public partial class DalObject
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
            try { dCharge = DataSource.droneCharges.Find(x => x.DroneId == droneId); }
            catch (ArgumentNullException)
            {
                throw new DroneChargeException($"drone chare with the drone ID {droneId} does not exist");
            }
            DataSource.droneCharges.Remove(dCharge);

            //Drone drone = DisplayDrone(droneId);
            //DataSource.drones.Remove(drone);
            ////drone.Status = DroneStatuses.Vacant;
            //AddDroneToTheList(drone);

            //Station station = DisplayStation(dCharge.StationId);
            //DataSource.stations.Remove(station);
            //station.ChargeSlots--;
            //AddStationToTheList(station);
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
            //Drone temp = new Drone();
            //foreach (Drone item in DataSource.drones)
            //{
            //    if (item.Id == droneId)
            //        temp = item;
            //}
            //return temp;
            try { return DataSource.drones.Find(item => item.Id == droneId); }
            catch (ArgumentNullException)
            {
                throw new DroneException($"id: {droneId} does not exist");
            }
        }
        
        public IEnumerable<DroneCharge> DisplayListOfDroneCharge()
        {
            List<DroneCharge> result = new List<DroneCharge>(DataSource.droneCharges);
            return result;
        }
        /// <summary>
        /// display the list of the drones.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> DisplayListOfDrones()
        {
            List<Drone> result = new List<Drone>(DataSource.drones);
            return result;
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