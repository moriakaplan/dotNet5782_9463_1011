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
        public void AddStation(Station station)
        {
            List<Station> stations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            if (stations.Exists(item => item.Id == station.Id)) throw new StationException($"id: {station.Id} already exist"); //it suppose to be this type of exception????**** 
            stations.Add(station);
            XmlTools.SaveListToXmlSerializer<Station>(stations, stationsPath);

        }
        public Station GetStation(int stationId)
        {
            List<Station> stations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            Station? result = stations.Find(x => x.Id == stationId);
            if (result == null) throw new StationException($"id: {stationId} does not exist");
            return (Station)result;
        }
        public IEnumerable<Station> GetStationsList(Predicate<Station> pre)
        {
            List<Station> stations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            List<Station> result = new List<Station>(stations);
            if (pre == null) return result;
            return result.FindAll(pre);
        }
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
