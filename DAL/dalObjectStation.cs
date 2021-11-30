using IDAL.DO;
using IDAL;
using System;
using System.Collections.Generic;
namespace DalObject
{
    public partial class DalObject
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
        /// display a station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station DisplayStation(int stationId)
        {
            //Station temp = new Station();
            //foreach (Station item in DataSource.stations)
            //{
            //    if (item.Id == stationId)
            //        temp = item;
            //}
            //return temp;
            try { return DataSource.stations.Find(item => item.Id == stationId); }
            catch (ArgumentNullException)
            {
                throw new StationException($"id: {stationId} does not exist");
            }
        }
        /// <summary>
        /// display the list of the stations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> DisplayListOfStations()
        {
            List<Station> result = new List<Station>(DataSource.stations);
            return result;
        }
        /// <summary>
        /// display a list of all the station with empty charge slots
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> DisplayListOfStationsWithAvailableCargeSlots()
        {
            List<Station> StationsWithAvailableCargingSlots = new List<Station>();
            foreach (Station item in DataSource.stations)
            {
                if (item.ChargeSlots > 0)
                    StationsWithAvailableCargingSlots.Add(item);
            }
            return StationsWithAvailableCargingSlots;
        }

    }
}