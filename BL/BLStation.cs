using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IBL.BO;
using IDAL;

namespace IBL
{
    public partial class BL
    {
        /// <summary>
        /// add station
        /// </summary>
        /// <param name="station"></param>
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
        /// <summary>
        /// update the name or the number of charge slots' or both of them.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="cargeSlots"></param>
        public void UpdateStation(int id, string name, int cargeSlots)
        {
            IDAL.DO.Station dstation = dl.DisplayStation(id);
            dl.DeleteStation(id);
            if(name!=null)//update the name
            {
                dstation.Name = name;
            }
            if(cargeSlots!=-1)//update the charge slots
            {
                dstation.ChargeSlots = cargeSlots;
            }
            dl.AddStationToTheList(dstation);
        }
        /// <summary>
        /// return the station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station DisplayStation(int stationId)
        {
            IDAL.DO.Station dstation = dl.DisplayStation(stationId);
            Station bstation = new Station
            {
                Id = dstation.Id,
                Location = new Location { Latti = dstation.Lattitude, Longi = dstation.Longitude },
                Name = dstation.Name
            };
            dl.droneCharges.
            bstation.DronesInCharge=
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
