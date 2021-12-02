using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IBL.BO;

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
            //creates a new station in the data layer
            IDAL.DO.Station dstation = new IDAL.DO.Station
            {
                Id = station.Id,
                Name = station.Name,
                ChargeSlots = station.AvailableChargeSlots,
                Lattitude = station.Location.Latti,
                Longitude = station.Location.Longi
            };
            try
            {
                dl.AddStationToTheList(dstation);//add the new station to the list in the data level
            }
            catch(IDAL.DO.StationException ex)
            {
                throw new ExistIdException(ex.Message, "-station");
            }
        } 
        /// <summary>
        /// update the name or the number of charge slots' or both of them.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="cargeSlots"></param>
        public void UpdateStation(int id, string name, int cargeSlots)
        {
            IDAL.DO.Station dstation;
            try
            {
                dstation = dl.DisplayStation(id);
                dl.DeleteStation(id);
            }
            catch (IDAL.DO.StationException ex)
            {
                throw new NotExistIDExeption(ex.Message, "- station");
            }
            if (name != null)//update the name
            {
                dstation.Name = name;
            }
            if (cargeSlots != -1)//update the charge slots
            {
                dstation.ChargeSlots = cargeSlots;
            }
            try { dl.AddStationToTheList(dstation); }
            catch (IDAL.DO.StationException ex)
            {
                throw new ExistIdException(ex.Message, "- station");
            }
        }
        /// <summary>
        /// return the station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station DisplayStation(int stationId)
        {
            IDAL.DO.Station dstation;
            try { dstation = dl.DisplayStation(stationId); }
            catch (IDAL.DO.StationException ex)
            {
                throw new NotExistIDExeption(ex.Message, "- station");
            }
            Station bstation = new Station
            {
                Id = dstation.Id,
                Location = new Location { Latti = dstation.Lattitude, Longi = dstation.Longitude },
                Name = dstation.Name
            };
            int count = 0;
            IEnumerable<IDAL.DO.DroneCharge> droneCharge = dl.DisplayListOfDroneCharge();
            DroneInCharge temp = new DroneInCharge();
            bstation.DronesInCharge = new List<DroneInCharge>(null);
            foreach (IDAL.DO.DroneCharge dCharge in droneCharge)
            {
                if (dCharge.StationId == stationId)
                {
                    count++;
                    temp.Id = dCharge.DroneId;
                    temp.Battery = DisplayDrone(dCharge.DroneId).Battery; 
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
        private StationToList DisplayStationToList(int stationId)
        {
            IDAL.DO.Station dstation;
            try { dstation = dl.DisplayStation(stationId); }
            catch (IDAL.DO.StationException ex)
            {
                throw new NotExistIDExeption(ex.Message, "- station");
            }
            StationToList bstation = new StationToList
            {
                Id = dstation.Id,
                Name = dstation.Name,
            };
            IEnumerable<IDAL.DO.DroneCharge> droneCharge = dl.DisplayListOfDroneCharge();
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
            //List < StationToList> result = new List<StationToList>(null);
            foreach (IDAL.DO.Station dstation in dl.DisplayListOfStations()) 
            {
                yield return DisplayStationToList(dstation.Id);
            }
            //return result;
        }
        public IEnumerable<StationToList> DisplayListOfStationsWithAvailableCargeSlots()
        {
            //List<StationToList> result = new List<StationToList>(null);
            StationToList tempStation;
            foreach (IDAL.DO.Station dstation in dl.DisplayListOfStations())
            {
                tempStation = DisplayStationToList(dstation.Id);
                if (tempStation.AvailableChargeSlots>0)
                {
                    yield return tempStation ;
                }
            }
            //return result;
        }
    }
}
