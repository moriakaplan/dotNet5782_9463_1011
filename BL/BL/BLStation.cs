using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLApi;
using System.Runtime.CompilerServices;

namespace BL
{
    internal partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
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
                lock (dl)
                {
                    dl.AddStation(dstation);//add the new station to the list in the data level
                }
            }
            catch (DO.StationException ex)
            {
                throw new ExistIdException(ex.Message, "-station");
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(int id, string name, int cargeSlots)
        {
            lock (dl)
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationId)
        {
            lock (dl)
            {
                DO.Station dstation;
                try { dstation = dl.GetStation(stationId); }
                catch (DO.StationException ex) { throw new NotExistIDException(ex.Message, " - station"); }
                Station bstation = new Station
                {
                    Id = dstation.Id,
                    Location = new Location { Latti = dstation.Lattitude, Longi = dstation.Longitude },
                    Name = dstation.Name
                };

                IEnumerable<DroneToList> drones = GetDronesList();
                bstation.DronesInCharge = from item in drones//find the drones that in charge in the station
                                          where item.Status == DroneStatus.Maintenance && item.CurrentLocation == bstation.Location
                                          select new DroneInCharge { Id = item.Id, Battery = item.Battery };

                int count = bstation.DronesInCharge.Count();
                bstation.AvailableChargeSlots = dstation.ChargeSlots - count;
                return bstation;
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetStationsList()
        {
            lock (dl)
            {
                return from dstation in dl.GetStationsList()
                       select displayStationToList(dstation.Id);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetListOfStationsWithAvailableCargeSlots()
        {
            lock (dl)
            {
                return from dstation in dl.GetStationsList(x => x.ChargeSlots > 0)
                       select displayStationToList(dstation.Id);
            }

        }

        /// <summary>
        /// display parcel to list
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        private StationToList displayStationToList(int stationId)
        {
            lock (dl)
            {
                DO.Station dstation = dl.GetStation(stationId);
                StationToList bstation = new StationToList
                {
                    Id = dstation.Id,
                    Name = dstation.Name,
                };
                int count = dl.GetDroneChargesList()//find the drones that in charge in the station
                            .Where(drCharge => drCharge.StationId == stationId)
                            .Count();
                bstation.AvailableChargeSlots = dstation.ChargeSlots - count;
                bstation.NotAvailableChargeSlots = count;
                return bstation;

            }

        }
    }
}
