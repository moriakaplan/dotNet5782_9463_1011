using DO;
using DalApi;
using System;
using System.Collections.Generic;

namespace Dal
{
    internal partial class DalObject
    {       
        public void AddStationToTheList(Station station)
        {
            if (DataSource.stations.Exists(item => item.Id == station.Id)) throw new StationException($"id: {station.Id} already exist"); //it suppose to be this type of exception????**** 
            DataSource.stations.Add(station);
        }       
        public Station DisplayStation(int stationId)
        {
            foreach (Station item in DataSource.stations)
            {
                if (item.Id == stationId)
                    return item;
            }
            throw new StationException($"id: {stationId} does not exist");
        }
        public IEnumerable<Station> DisplayListOfStations(Predicate<Station> pre)
        {
            List<Station> result = new List<Station>(DataSource.stations);
            if (pre == null) return result;
            return result.FindAll(pre);
        }
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