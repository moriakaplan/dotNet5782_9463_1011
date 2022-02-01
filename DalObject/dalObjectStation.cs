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
        public void AddStation(Station station)
        {
            if (DataSource.stations.Exists(item => item.Id == station.Id)) 
                throw new StationException($"id: {station.Id} already exist");  
            DataSource.stations.Add(station);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationId)
        {
            if (!DataSource.stations.Exists(x => x.Id == stationId)) 
                throw new StationException($"id: {stationId} does not exist");
            return DataSource.stations.Find(x => x.Id == stationId);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStationsList(Predicate<Station> pre)
        {
            List<Station> result = new List<Station>(DataSource.stations);
            if (pre == null) return result;
            return result.FindAll(pre);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int stationId)
        {
            try
            {
                DataSource.stations.Remove(GetStation(stationId));
            }
            catch (ArgumentNullException)
            {
                throw new StationException($"id: {stationId} does not exist");
            }
        }
    }
}