using DO;
using DalApi;
using System;
using System.Collections.Generic;

namespace Dal
{
    internal partial class DalObject
    {
        /// <summary>
        /// add the station that he gets to the list of the stations.
        /// </summary>
        /// <param name="station"></param>
        public void AddStationToTheList(Station station)
        {
            if (DataSource.stations.Exists(item => item.Id == station.Id)) throw new StationException($"id: {station.Id} already exist"); //it suppose to be this type of exception????**** 
            DataSource.stations.Add(station);
        }
        /// <summary>
        /// return a station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station DisplayStation(int stationId)
        {
            foreach (Station item in DataSource.stations)
            {
                if (item.Id == stationId)
                    return item;
            }
            throw new StationException($"id: {stationId} does not exist");
            //Station? st = DataSource.stations.Find(item => item.Id == stationId);
            //if (st == null) throw new StationException($"id: {stationId} does not exist");
            //return (Station)st;
        }
        /// <summary>
        /// display the list of the stations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> DisplayListOfStations(Predicate<Station> pre)
        {
            List<Station> result = new List<Station>(DataSource.stations);
            if (pre == null) return result;
            return result.FindAll(pre);
        }
        /// <summary>
        /// display a list of all the station with empty charge slots
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<Station> DisplayListOfStationsWithAvailableCargeSlots()
        //{
        //    List<Station> StationsWithAvailableCargingSlots = new List<Station>();
        //    foreach (Station item in DataSource.stations)
        //    {
        //        if (item.ChargeSlots > 0)
        //            StationsWithAvailableCargingSlots.Add(item);
        //    }
        //    return StationsWithAvailableCargingSlots;
        //}
        public void DeleteStation(int stationId)
        {
            try
            {
                DataSource.stations.Remove(DisplayStation(stationId));
            }
            catch (ArgumentNullException)
            {
                throw new StationException($"id: {stationId} does not exist");
            }
        }
    }
}