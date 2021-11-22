using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
using DalObject;
using IBL.BO;

namespace BL
{
    public partial class BL : Ibl
    {
        public void AddStation(Station station)
        {
            //creates a new station in the data level
            IDAL.DO.Station dstation = new IDAL.DO.Station
            {
                Id = station.Id,
                Name = station.Name,
                ChargeSlots = station.AvailableChargeSlots,
                Lattitude = station.Location.Latti,
                Longitude = station.Location.Longi
            };
            dl.AddStationToTheList(dstation);//add the new station to the list in the data level
        }
        public void UpdateStation(int id, string name, int cargeSlots)//כתוב שהוא צריך לקבל פה אחד או יותר,איך עושים את זה?
        {

        }
        public Station DisplayStation(int stationId)
        {
            return null;
        }
        public IEnumerable<StationToList> DisplayListOfStations()
        {
            return null;
        }
        public IEnumerable<StationToList> DisplayListOfStationsWithAvailableCargeSlots()
        {
            return null;
        }
    }
}
