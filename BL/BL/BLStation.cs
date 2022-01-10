using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLApi;

namespace BL
{
    internal partial class BL
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
                lock(dl)
                {
                    dl.AddStation(dstation);//add the new station to the list in the data level
                }
            }
            catch(DO.StationException ex)
            {
                throw new ExistIdException(ex.Message, "-station");
            }
        }  
        public void UpdateStation(int id, string name, int cargeSlots)
        {
            lock(dl)
            {
                DO.Station dstation = dl.GetStation(id);
                dl.DeleteStation(id);
                if (name != null)//update the name
                {
                    dstation.Name = name;
                }
                if (cargeSlots != -1)//update the charge slots
                {
                    dstation.ChargeSlots = cargeSlots;
                }
                dl.AddStation(dstation);
            }     
        }    
        public Station GetStation(int stationId)
        {
            DO.Station dstation;
            lock(dl)
            {
                try { dstation = dl.GetStation(stationId); }
                catch (DO.StationException ex) { throw new NotExistIDException(ex.Message, " - station"); }
                Station bstation = new Station
                {
                    Id = dstation.Id,
                    Location = new Location { Latti = dstation.Lattitude, Longi = dstation.Longitude },
                    Name = dstation.Name
                };
                IEnumerable<DO.DroneCharge> droneCharges = dl.GetDroneChargesList();
                bstation.DronesInCharge = from item in droneCharges
                                          select new DroneInCharge { Id = item.DroneId, Battery = GetDrone(item.DroneId).Battery };
                int count = droneCharges.Count(x => x.StationId == stationId);
                bstation.AvailableChargeSlots = dstation.ChargeSlots - count;
                return bstation;
            }
        }
        public IEnumerable<StationToList> GetStationsList()
        {
            lock(dl)
            {
                return from dstation in dl.GetStationsList()
                       select DisplayStationToList(dstation.Id);
            }      
        }
        public IEnumerable<StationToList> GetListOfStationsWithAvailableCargeSlots()
        {
            lock(dl)
            {
                return from dstation in dl.GetStationsList(x => x.ChargeSlots > 0)
                       select DisplayStationToList(dstation.Id);
            }   
        }

        /// <summary>
        /// display parcel to list
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        private StationToList DisplayStationToList(int stationId)
        {
            DO.Station dstation = dl.GetStation(stationId);
            StationToList bstation = new StationToList
            {
                Id = dstation.Id,
                Name = dstation.Name,
            };
            //List<DO.DroneCharge> droneCharge = (List<DO.DroneCharge>)dl.DisplayListOfDroneCharge();
            //int count = 0;
            //foreach (DO.DroneCharge ddrone in droneCharge)
            //{
            //    if (ddrone.StationId == stationId)
            //    {
            //        count++;
            //    }
            //}
            int count = dl.GetDroneChargesList()
                        .Where(drCharge=>drCharge.StationId == stationId)
                        .Count();
            bstation.AvailableChargeSlots = dstation.ChargeSlots - count;
            bstation.NotAvailableChargeSlots = count;
            return bstation;

        }
       
       
    }
}
