using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    public partial class DalXml
    {
        /// <summary>
        /// add drone
        /// </summary>
        /// <param name="drone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone drone)
        {
            List<Drone> drones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            if (drones.Exists(item => item.Id == drone.Id)) throw new DroneException($"id: {drone.Id} already exist"); //it suppose to be this type of exception????**** 
            drones.Add(drone);
            XmlTools.SaveListToXmlSerializer<Drone>(drones, dronesPath);
        }

        /// <summary>
        /// send drone to charge
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendDroneToCharge(int droneId, int stationId)
        {
            
            List<Drone> drones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            List<DroneCharge> dCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            List<Station> stations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            if (!drones.Exists(item => item.Id == droneId))
            {
                throw new DroneException($"id: {droneId} does not exist");
            }
            DroneCharge droneCharge = new DroneCharge { DroneId = droneId, StationId = stationId, StartedChargeTime = DateTime.Now }; //add drone charge to the list for charging the drone                                                                                                                          //DataSource.droneCharges.Add(droneCharge);
            dCharge.Add(droneCharge);
            for (int i = 0; i < stations.Count; i++) //find the station and update its details
            {
                if (stations[i].Id == stationId)
               {
                   if (stations[i].ChargeSlots < 1) throw new StationException("");
                    Station s = stations[i];
                    s.ChargeSlots--; 
                    stations[i] = s;
                    XmlTools.SaveListToXmlSerializer<Drone>(drones, dronesPath);
                    XmlTools.SaveListToXmlSerializer<DroneCharge>(dCharge, droneChargesPath);
                    XmlTools.SaveListToXmlSerializer<Station>(stations, stationsPath);
                    return;
                }
            }
            throw new StationException($"id: {stationId} does not exist");
        }

        /// <summary>
        /// release the drone from charge
        /// </summary>
        /// <param name="droneId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDroneFromeCharge(int droneId)
        {
            DroneCharge dCharge;
            List<DroneCharge> droneCharges = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            if (!droneCharges.Exists(x => x.DroneId == droneId))
                throw new DroneChargeException($"drone charge with the drone ID {droneId} does not exist");
            dCharge = droneCharges.Find(x => x.DroneId == droneId);
            droneCharges.Remove(dCharge);
            List<Station> stations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);

            for (int i = 0; i < stations.Count; i++) //find the station and update its details
            {
                if (stations[i].Id == dCharge.StationId)
                {
                    Station s = stations[i];
                    s.ChargeSlots++; //--?
                    stations[i] = s;
                    XmlTools.SaveListToXmlSerializer<DroneCharge>(droneCharges, droneChargesPath);
                    XmlTools.SaveListToXmlSerializer<Station>(stations, stationsPath);
                    return;
                }
            }
            throw new StationException($"id: {dCharge.StationId} does not exist");
        }

        /// <summary>
        /// returns the drone with the requested id
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneId)
        {
            List<Drone> drones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Drone? dr =drones.Find(item => item.Id == droneId);
            if (dr == null)
                throw new DroneException($"id: {droneId} does not exist");
            return (Drone)dr;
        }

        /// <summary>
        /// return drone charge with the requested id
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int droneId)
        {
            List<DroneCharge> droneCharges = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            if (!droneCharges.Exists(item => item.DroneId == droneId))
                throw new DroneChargeException($"DroneCharge for drone {droneId} does not exist");
            DroneCharge dc = droneCharges.Find(item => item.DroneId == droneId);
            return dc;
        }

        /// <summary>
        /// returns all drone charge
        /// </summary>
        /// <param name="pre"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDroneChargesList(Predicate<DroneCharge> pre)
        {
            List<DroneCharge> list = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            if (pre == null) return list;
            return list.FindAll(pre);
        }

        /// <summary>
        /// return all drone list
        /// </summary>
        /// <param name="pre"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDronesList(Predicate<Drone> pre)
        {
            List<Drone> drones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            List<Drone> result = new List<Drone>(drones);
            if (pre == null) return result;
            return result.FindAll(pre);
        }

        /// <summary>
        /// delete the drone
        /// </summary>
        /// <param name="droneId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int droneId)
        {
            List<Drone> drones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            List<DroneCharge> droneCharges = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            try { drones.Remove(GetDrone(droneId)); }
            catch (ArgumentNullException)
            {
                throw new DroneException($"id: {droneId} does not exist");
            }
            try { droneCharges.Remove(GetDroneCharge(droneId)); }
            catch { }
            XmlTools.SaveListToXmlSerializer<Drone>(drones, dronesPath);
            XmlTools.SaveListToXmlSerializer<DroneCharge>(droneCharges, droneChargesPath);
        }
    }
}