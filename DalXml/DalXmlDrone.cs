using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    public partial class DalXml
    {
        public void AddDroneToTheList(Drone drone)
        {
            //if (DataSource.drones.Exists(item => item.Id == drone.Id)) throw new DroneException($"id: {drone.Id} already exist"); //it suppose to be this type of exception????**** 
            //DataSource.drones.Add(drone);
            List<Drone> drones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            if (drones.Exists(item => item.Id == drone.Id)) throw new DroneException($"id: {drone.Id} already exist"); //it suppose to be this type of exception????**** 
            drones.Add(drone);
            XmlTools.SaveListToXmlSerializer<Drone>(drones, dronesPath);
        }
        public void SendDroneToCharge(int droneId, int stationId)
        {
            
            List<Drone> drones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            if (drones.Exists(item => item.Id == droneId)) throw new DroneException($"id: {droneId} already exist");
            DroneCharge droneCharge = new DroneCharge { DroneId = droneId, StationId = stationId, StartedChargeTime = DateTime.Now }; //add drone charge to the list for charging the drone                                                                                                                          //DataSource.droneCharges.Add(droneCharge);
            List<DroneCharge> dCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            dCharge.Add(droneCharge);
            List<Station> stations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            for (int i = 0; i < stations.Count; i++) //find the station and update its details
            {
                if (stations[i].Id == stationId)
               {
                   if (stations[i].ChargeSlots < 1) throw new StationException("");
                    Station s = stations[i];
                    s.ChargeSlots--; //++?
                    stations[i] = s;
                    return;
                }
            }
            throw new StationException($"id: {stationId} does not exist");
            XmlTools.SaveListToXmlSerializer<Drone>(drones, dronesPath);
            XmlTools.SaveListToXmlSerializer<DroneCharge>(dCharge, droneChargesPath);
            XmlTools.SaveListToXmlSerializer<Station>(stations, stationsPath);


        }
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
                    return;
                }
            }
            throw new StationException($"id: {dCharge.StationId} does not exist");
            XmlTools.SaveListToXmlSerializer<DroneCharge>(droneCharges, droneChargesPath);
            XmlTools.SaveListToXmlSerializer<Station>(stations, stationsPath);
        }
        public Drone DisplayDrone(int droneId)
        {
            List<Drone> drones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Drone? dr =drones.Find(item => item.Id == droneId);
            if (dr == null)
                throw new DroneException($"id: {droneId} does not exist");
            return (Drone)dr;
        }
        public IEnumerable<DroneCharge> DisplayListOfDroneCharge(Predicate<DroneCharge> pre)
        {
            List<DroneCharge> list = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargesPath);
            if (pre == null) return list;
            return list.FindAll(pre);
        }
        public IEnumerable<Drone> DisplayListOfDrones(Predicate<Drone> pre)
        {
            List<Drone> drones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            List<Drone> result = new List<Drone>(drones);
            if (pre == null) return result;
            return result.FindAll(pre);
        }
        public void DeleteDrone(int droneId)
        {
            List<Drone> drones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            try { drones.Remove(DisplayDrone(droneId)); }
            catch (ArgumentNullException)
            {
                throw new DroneException($"id: {droneId} does not exist");
            }
            XmlTools.SaveListToXmlSerializer<Drone>(drones, dronesPath);
        }
    }
}