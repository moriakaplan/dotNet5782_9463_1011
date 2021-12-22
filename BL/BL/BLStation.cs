using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLApi;

namespace BL
{
    public partial class BL
    {
       
        public void AddStation(int id, string name, Location loc, int chargeSlots)
        {
            //creates a new station in the data layer
            DO.Station dstation = new DO.Station
            {
                Id = id,
                Name = name,
                ChargeSlots = chargeSlots,
                Lattitude = loc.Latti,
                Longitude = loc.Longi
            };
            try
            {
                dl.AddStationToTheList(dstation);//add the new station to the list in the data level
            }
            catch(DO.StationException ex)
            {
                throw new ExistIdException(ex.Message, "-station");
            }
        } 
       
        public void UpdateStation(int id, string name, int cargeSlots)
        {
            DO.Station dstation = dl.DisplayStation(id);
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
       
        public Station DisplayStation(int stationId)
        {
            DO.Station dstation;
            try { dstation = dl.DisplayStation(stationId); }
            catch(DO.StationException ex) { throw new NotExistIDException(ex.Message, " - station"); }
            Station bstation = new Station
            {
                Id = dstation.Id,
                Location = new Location { Latti = dstation.Lattitude, Longi = dstation.Longitude },
                Name = dstation.Name
            };
            int count = 0;
            //IEnumerable<DO.DroneCharge> droneCharge = dl.DisplayListOfDroneCharge();
            DroneInCharge temp = new DroneInCharge();
            bstation.DronesInCharge = new List<DroneInCharge>();
            foreach (DO.DroneCharge dCharge in dl.DisplayListOfDroneCharge())
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
        /// display parcel to list
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        private StationToList DisplayStationToList(int stationId)
        {
            DO.Station dstation = dl.DisplayStation(stationId);
            StationToList bstation = new StationToList
            {
                Id = dstation.Id,
                Name = dstation.Name,
            };
            List<DO.DroneCharge> droneCharge = (List<DO.DroneCharge>)dl.DisplayListOfDroneCharge();
            int count = 0;
            foreach (DO.DroneCharge ddrone in droneCharge)
            {
                if (ddrone.StationId == stationId)
                {
                    count++;
                }
            }
            bstation.AvailableChargeSlots = dstation.ChargeSlots /*- count*/;
            bstation.NotAvailableChargeSlots = count;
            return bstation;

        }
       
        public IEnumerable<StationToList> DisplayListOfStations()
        {
            foreach (DO.Station dstation in dl.DisplayListOfStations()) 
            {
                yield return DisplayStationToList(dstation.Id);
            }
        }
        public IEnumerable<StationToList> DisplayListOfStationsWithAvailableCargeSlots()
        {
            foreach (DO.Station dstation in dl.DisplayListOfStations(x=>x.ChargeSlots>0))
            {
                yield return DisplayStationToList(dstation.Id);
            }
        }
    }
}
