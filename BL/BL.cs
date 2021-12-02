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
        private double BatteryForAvailable;
        private double BatteryForEasy; //per kill
        private double BatteryForMedium; //per kill
        private double BatteryForHeavy; //per kill
        private double ChargeRatePerHour;
        private static Random random = new Random();
        /// <summary>
       /// constractor
       /// </summary>
        public BL()
        {
            dl = new DalObject.DalObject();
            lstdrn = (List<DroneToList>)dl.DisplayListOfDrones();
            initializeDrone();
            double[] batteryData = dl.AskBattery();
            BatteryForAvailable = batteryData[0];
            BatteryForEasy = batteryData[1];
            BatteryForMedium = batteryData[2];
            BatteryForHeavy = batteryData[3];
            ChargeRatePerHour = batteryData[4];
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
                    Location locOfCus = DisplayCustomer(parcel.Senderld).Location;
                    if ((parcel.Droneld == drone.Id) && (parcel.DeliverTime == DateTime.MinValue)) //If there is a parcel that has not yet been delivered but the drone is associated
                    {
                        drone.Status = DroneStatus.Delivery;
                        if ((parcel.AssociateTime != DateTime.MinValue) && (parcel.PickUpTime == DateTime.MinValue)) //If the parcel was associated but not picked up
                        {
                            drone.CurrentLocation = closestStation(locOfCus);
                        }
                        if ((parcel.PickUpTime != DateTime.MinValue) && (parcel.DeliverTime == DateTime.MinValue)) //If the parcel was picked up but not delivered
                        {
                            drone.CurrentLocation = locOfCus;//The location of the drone is in the location of the sender
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
                        IEnumerable<IDAL.DO.Station> stations = dl.DisplayListOfStations();
                        int index = random.Next(0, stations.Count());
                        IDAL.DO.Station stationForLocation= stations.ElementAt(index);
                        drone.CurrentLocation = new Location { Latti = stationForLocation.Lattitude, Longi = stationForLocation.Longitude };
                        drone.Battery = random.Next(0, 19)+ random.NextDouble();//random battery mode between 0 and 20
                    }
                    if (drone.Status == DroneStatus.Available)//הרחפן פנוי
                    {
                        //מיקום מוגרל בין לקוחות שיש חבילות שסופקו להם***********************************
                        
                        drone.Battery = random.Next((int)minBattery(drone.Id, drone.CurrentLocation, closestStation(drone.CurrentLocation)) + 1,99)+ random.NextDouble();//random  between a minimal charge that allows it to reach the nearest station and a full charge
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
                if ((parcel.Droneld == drone.Id) && (parcel.PickUpTime != DateTime.MinValue) && (parcel.DeliverTime == DateTime.MinValue)) //If the drone in shipment (already collected the parcel)) 
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
            IEnumerable<StationToList> stations = DisplayListOfStations();
            Station station = DisplayStation(stations.First().Id); ;
            Location minLocation = station.Location;
            double minDistance = distance(loc, station.Location);
            foreach (StationToList dstation in stations)//find the station that is the closest to the location
            {
                station = DisplayStation(dstation.Id);
                if (distance(loc, station.Location) < minDistance)//update to be the closest
                {
                    minDistance = distance(loc, station.Location);
                    minLocation = station.Location;
                }
            }
            return minLocation;
        }
        /// <summary>
        /// Finds the closest station with available charge slots 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private Station closestStationWithChargeSlots(Location loc)
        {
            IEnumerable<StationToList> stations = DisplayListOfStationsWithAvailableCargeSlots();
            Station station = DisplayStation(stations.First().Id); ;
            Station minStation = station;
            double minDistance = distance(loc, station.Location);
            foreach (StationToList dstation in stations)//find the station that is the closest to the location
            {
                station = DisplayStation(dstation.Id);
                if (distance(loc, station.Location) < minDistance)//update to be the closest
                {
                    minDistance = distance(loc, station.Location);
                    minStation = station;
                }
            }
            return minStation;
        }
        /// <summary>
        /// Finds the minimum battery needed to get from the location to the nearest station
        /// </summary>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private double minBattery(int droneId,Location from, Location to)
        {
            Drone drone = DisplayDrone(droneId); 
            double batteryForKil = 0;
            if (drone.Status == DroneStatus.Available) batteryForKil = BatteryForAvailable;
            WeightCategories weight = drone.ParcelInT.Weight;
            if (weight == WeightCategories.Easy) batteryForKil = BatteryForEasy;
            if (weight == WeightCategories.Medium) batteryForKil = BatteryForMedium;
            if (weight == WeightCategories.Heavy) batteryForKil = BatteryForHeavy;
            double kils = distance(from, to);
            return batteryForKil * kils;
        }
        private double distance(Location a, Location b)
        {
            return dl.Distance(a.Latti, a.Longi, b.Latti, b.Longi);
        }
    }
}
