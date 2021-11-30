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
            if (name != null)//update the name
            {
                dstation.Name = name;
            }
            if (cargeSlots != -1)//update the charge slots
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
            int count = 0;
            List<IDAL.DO.DroneCharge> droneCharge = (List<IDAL.DO.DroneCharge>)dl.DisplayListOfDroneCharge();
            DroneInCharge temp = new DroneInCharge();
            bstation.DronesInCharge = new List<DroneInCharge>(null);
            foreach (IDAL.DO.DroneCharge ddrone in droneCharge)
            {
                if (ddrone.StationId == stationId)
                {
                    count++;
                    temp.Id = ddrone.DroneId;
                    temp.Battery = DisplayDrone(ddrone.DroneId).Battery;
                    bstation.DronesInCharge.Add(temp);
                }
            }
            bstation.AvailableChargeSlots = dstation.ChargeSlots - count;
            return bstation;
        }
        /// <summary>
        /// פונקצית עזר-תצוגת תחנה לרשימה
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        private StationToList DisplatStationToList(int stationId)
        {
            IDAL.DO.Station dstation = dl.DisplayStation(stationId);
            StationToList bstation = new StationToList
            {
                Id = dstation.Id,
                Name = dstation.Name,
            };
            List<IDAL.DO.DroneCharge> droneCharge = (List<IDAL.DO.DroneCharge>)dl.DisplayListOfDroneCharge();
            int count = 0;
            foreach (IDAL.DO.DroneCharge ddrone in droneCharge)
            {
                if (ddrone.StationId == stationId)
                {
                    count++;
                }
            }
            bstation.AvailableChargeSlots = dstation.ChargeSlots - count;
            bstation.NotAvailableChargeSlots = count;
            return bstation;

        }
        public IEnumerable<StationToList> DisplayListOfStations()
        {
            List < StationToList> result = new List<StationToList>(null);
            foreach (IDAL.DO.Station dstation in dl.DisplayListOfStations()) 
            {
                result.Add(DisplatStationToList(dstation.Id));
            }
            return result;
        }
        public IEnumerable<StationToList> DisplayListOfStationsWithAvailableCargeSlots()
        {
            List<StationToList> result = new List<StationToList>(null);
            StationToList tempStation;
            foreach (IDAL.DO.Station dstation in dl.DisplayListOfStations())
            {
                tempStation = DisplatStationToList(dstation.Id);
                if (tempStation.AvailableChargeSlots>0)
                {
                    result.Add(tempStation);
                }
            }
            return result;
        }
    }
}
