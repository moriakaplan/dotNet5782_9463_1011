using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace IBL
{
    public partial class BL: Ibl
    {

        private List<DroneToList> lstdrn;
        private IDal dl;
        private static Random random = new Random();
        /// <summary>
       /// constractor
       /// </summary>
        public BL()
        {
            dl = new DalObject.DalObject();
            lstdrn = (List<DroneToList>)dl.DisplayListOfDrones();
            initializeDrone();
        }
        /// <summary>
        /// initialize the list of drones
        /// </summary>
        private void initializeDrone()
        {
            foreach (IDAL.DO.Drone drone in dl.DisplayListOfDrones())
            {
                lstdrn.Add(new DroneToList { Id = drone.Id, MaxWeight = (WeightCategories)drone.MaxWeight, Model = drone.Model });
            }
            foreach (DroneToList drone in lstdrn)
            {
                foreach (IDAL.DO.Parcel parcel in dl.DisplayListOfParcels())
                {
                    IDAL.DO.Customer tempCustomer = dl.DisplayCustomer(parcel.Senderld);//####
                    Location locOfCus = new Location { Longi = tempCustomer.Longitude, Latti = tempCustomer.Lattitude };
                    if ((parcel.Droneld == drone.Id) && (parcel.Delivered == DateTime.MinValue) && (parcel.Scheduled == DateTime.MinValue))//If there is a parcel that has not yet been delivered but the drone is associated
                    {
                        drone.Status = DroneStatus.Delivery;
                        if ((parcel.Scheduled != DateTime.MinValue) && (parcel.PickedUp == DateTime.MinValue))//If the parcel was associated but not picked up
                        {
                            drone.CurrentLocation = closestStation(new Location { Latti = tempCustomer.Lattitude, Longi = tempCustomer.Longitude });
                        }
                        if ((parcel.PickedUp != DateTime.MinValue) && (parcel.Delivered == DateTime.MinValue))//If the parcel was picked up but not delivered
                        {
                            drone.CurrentLocation = new Location() { Longi = tempCustomer.Longitude, Latti = tempCustomer.Lattitude };//The location of the drone is in the location of the sender
                        }
                        double batteryNeeded = minBattery(drone.Id, drone.CurrentLocation, locOfCus) + minBattery(drone.Id, locOfCus, closestStation(locOfCus));
                        drone.Battery = random.Next((int)batteryNeeded + 1, 100);
                    }
                    if (DroneNotInDelivery(drone))//If the drone does not ship
                    {
                        drone.Status = (DroneStatus)random.Next(1, 2);//Maintenance or availability
                    }
                    if (drone.Status == DroneStatus.Maintenance)
                    {
                        //המיקום מוגרל בין תחנות קיימות***********************************************
                        drone.Battery = random.Next(0, 19)+ rand.NextDouble();//random battery mode between 0 and 20
                    }
                    if (drone.Status == DroneStatus.Available)//הרחפן פנוי
                    {
                        //מיקום מוגרל בין לקוחות שיש חבילות שסופקו להם***********************************
                        drone.Battery = random.Next((int)minBattery(drone.Id, drone.CurrentLocation, closestStation(drone.CurrentLocation)) + 1,99)+ rand.NextDouble();//random  between a minimal charge that allows it to reach the nearest station and a full charge
                    }

                }
            }
        }
        /// <summary>
        /// Checks if the drone does not ship
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        private bool DroneNotInDelivery(DroneToList drone)
        {
            foreach (IDAL.DO.Parcel parcel in dl.DisplayListOfParcels())
            {
                if ((parcel.Droneld == drone.Id) && (parcel.PickedUp != DateTime.MinValue) && (parcel.Delivered == DateTime.MinValue))//If the drone in shipment (already collected the parcel) 
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Finds the closest station to the location
        /// </summary>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private Location closestStation(Location loc)
        {
            List<Station> stations = new List<Station>();
            foreach(IDAL.DO.Station dstation in dl.DisplayListOfStations())//Finds the location of all stations
            {
                stations.Add(new Station { Location = new Location { Latti = dstation.Lattitude, Longi = dstation.Lattitude } });
            }
            Location minLocation = stations[0].Location;
            double minDistance =  dl.Distance(loc.Latti, loc.Longi, stations[0].Location.Latti, stations[0].Location.Longi);
            foreach (Station station in stations)
            {
                if (dl.Distance(loc.Latti, loc.Longi, station.Location.Latti, station.Location.Longi)<minDistance)
                {
                    minDistance = dl.Distance(loc.Latti, loc.Longi, station.Location.Latti, station.Location.Longi);
                    minLocation=station.Location;
                }
            }
            return minLocation;
        }
        /// <summary>
        /// Finds the closest station with available charge slots 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private Station closestStationWithAvailableChargeSlosts(Location location)
        {
            List<Station> stations = new List<Station>();
            foreach (IDAL.DO.Station dstation in dl.DisplayListOfStationsWithAvailableCargeSlots())//Finds the location of all stations
            {
                stations.Add(new Station { Location = new Location { Latti = dstation.Lattitude, Longi = dstation.Lattitude }});
            }
           Station minLocation = stations[0];
            double minDistance = dl.Distance(location.Latti, location.Longi, stations[0].Location.Latti, stations[0].Location.Longi);
            foreach (Station station in stations)
            {
                if (dl.Distance(location.Latti, location.Longi, station.Location.Latti, station.Location.Longi) < minDistance)
                {
                    minDistance = dl.Distance(location.Latti, location.Longi, station.Location.Latti, station.Location.Longi);
                    minLocation = station;
                }
            }
            return minLocation;
        }
        /// <summary>
        /// Finds the minimum battery needed to get from the location to the nearest station
        /// </summary>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private double minBattery(int droneId,Location from, Location to)
        {
            Drone drone;
            try { drone = DisplayDrone(droneId); }
            catch () { throw Exeption; }
            double batteryForKil = 0;
            double[] data = dl.AskBattery(dl.DisplayDrone(droneId));
            if (drone.Status == DroneStatus.Available) batteryForKil = data[0];
            WeightCategories weight = drone.ParcelInT.Weight;
            if (weight == WeightCategories.Easy) batteryForKil = data[1];
            if (weight == WeightCategories.Medium) batteryForKil = data[2];
            if (weight == WeightCategories.Heavy) batteryForKil = data[3];
            double kils = dl.Distance(from.Longi, from.Latti, to.Longi, to.Latti);
            return batteryForKil * kils;
        }
    }
}
