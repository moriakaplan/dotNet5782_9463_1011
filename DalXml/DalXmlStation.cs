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
        /// add new station
        /// </summary>
        /// <param name="station"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station station)
        {
            List<Station> stations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            if (stations.Exists(item => item.Id == station.Id)) throw new StationException($"id: {station.Id} already exist");   
            stations.Add(station);
            XmlTools.SaveListToXmlSerializer<Station>(stations, stationsPath);

        }
        /// <summary>
        /// returns the station with the requested id
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationId)
        {
            List<Station> stations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            Station? result = stations.Find(x => x.Id == stationId);
            if (result == null) throw new StationException($"id: {stationId} does not exist");
            return (Station)result;
        }

        /// <summary>
        /// return all the stations
        /// </summary>
        /// <param name="pre"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStationsList(Predicate<Station> pre)
        {
            List<Station> stations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            List<Station> result = new List<Station>(stations);
            if (pre == null) return result;
            return result.FindAll(pre);
        }
        /// <summary>
        /// delete the station
        /// </summary>
        /// <param name="stationId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int stationId)
        {
            List<Station> stations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            try
            {
                stations.Remove(GetStation(stationId));
            }
            catch (ArgumentNullException)
            {
                throw new StationException($"id: {stationId} does not exist");
            }
            XmlTools.SaveListToXmlSerializer<Station>(stations, stationsPath);

        }
    }
}
