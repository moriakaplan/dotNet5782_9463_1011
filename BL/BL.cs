using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DalApi;

namespace BLApi
{
    public partial class BL : Ibl
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
            //lstdrn = (List<DroneToList>)dl.DisplayListOfDrones();
            lstdrn = new List<DroneToList>();
            double[] batteryData = dl.AskBattery();
            BatteryForAvailable = batteryData[0];
            BatteryForEasy = batteryData[1];
            BatteryForMedium = batteryData[2];
            BatteryForHeavy = batteryData[3];
            ChargeRatePerHour = batteryData[4];
            try
            {
                initializeDrone();
            }
            catch (Exception) { Console.WriteLine("was a problem in the initialize"); }
        }
        /// <summary>
        /// initialize the list of drones
        /// </summary>
        private void initializeDrone()
        {
            //add all the drones from the data layer to the list
            foreach (DO.Drone drone in dl.DisplayListOfDrones())
            {
                lstdrn.Add(new DroneToList { Id = drone.Id, MaxWeight = (WeightCategories)drone.MaxWeight, Model = drone.Model });
            }
            foreach (DroneToList drone in lstdrn)
            {
                foreach (DO.Parcel parcel in dl.DisplayListOfParcels())
                {
                    Location locOfCus = DisplayCustomer(parcel.Senderld).Location;
                    if ((parcel.Droneld == drone.Id) && (parcel.AssociateTime!=null) && (parcel.DeliverTime == null)) //If there is a parcel that has not yet been delivered but the drone is associated
                    {
                        if (parcel.PickUpTime == null) //If the parcel was associated but not picked up
                        {
                            drone.Status = DroneStatus.Associated;
                            drone.CurrentLocation = closestStation(locOfCus);
                        }
                        else //If the parcel was picked up but not delivered
                        {
                            drone.Status = DroneStatus.Delivery;
                            drone.CurrentLocation = locOfCus;//The location of the drone is in the location of the sender
                        }

                        double batteryForKil = 0;
                        DO.WeightCategories weight = parcel.Weight;
                        if (weight == DO.WeightCategories.Easy) batteryForKil = BatteryForEasy;
                        if (weight == DO.WeightCategories.Medium) batteryForKil = BatteryForMedium;
                        if (weight == DO.WeightCategories.Heavy) batteryForKil = BatteryForHeavy;

                        double batteryNeeded = BatteryForAvailable * distance(drone.CurrentLocation, locOfCus) + //=0 if the drone already took the parcel
                            batteryForKil * distance(locOfCus, closestStation(locOfCus));
                        //double batteryNeeded = 
                        //    minBattery(drone.Id, drone.CurrentLocation, locOfCus) +
                        //    minBattery(drone.Id, locOfCus, closestStation(locOfCus));
                        if (batteryNeeded > 100) throw new DroneCantTakeParcelException("the drone has not enugh battery for take the parcel he suppose to take.");
                        drone.Battery = random.Next((int)batteryNeeded /*+ 1*/, 100) + random.NextDouble();
                        drone.ParcelId = parcel.Id;
                        break;
                    }
                }
                if (DroneNotInDelivery(drone))//If the drone does not ship
                {
                    drone.Status = (DroneStatus)random.Next(0, 2);//Maintenance or availability
                    if (drone.Status == DroneStatus.Maintenance)
                    {
                        //the location is in random station
                        IEnumerable<DO.Station> stations = dl.DisplayListOfStations();
                        int index = random.Next(0, stations.Count());
                        DO.Station stationForLocation = stations.ElementAt(index);
                        drone.CurrentLocation = new Location { Latti = stationForLocation.Lattitude, Longi = stationForLocation.Longitude };
                        drone.Battery = random.Next(0, 19) + random.NextDouble();//random battery mode between 0 and 20
                        dl.SendDroneToCharge(drone.Id, stationForLocation.Id);
                    }
                    if (drone.Status == DroneStatus.Available)//the drone is available
                    {
                        //the location is in random customer location that received parcels
                        List<CustomerToList> customersWhoGotParcels = new List<CustomerToList>();
                        foreach (CustomerToList cus in DisplayListOfCustomers())
                        {
                            if (cus.numOfParclReceived > 0) { customersWhoGotParcels.Add(cus); }
                        }
                        int index = random.Next(0, customersWhoGotParcels.Count());
                        CustomerToList customerForLocation = customersWhoGotParcels[index];
                        drone.CurrentLocation = DisplayCustomer(customerForLocation.Id).Location;
                        drone.Battery = random.Next((int)minBattery(drone.Id, drone.CurrentLocation, closestStation(drone.CurrentLocation)) + 1, 99) + random.NextDouble();//random  between a minimal charge that allows it to reach the nearest station and a full charge
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
            foreach (DO.Parcel parcel in dl.DisplayListOfParcels())
            {
                if ((parcel.Droneld == drone.Id) && (parcel.DeliverTime == null)) //If the drone in shipment (already collected the parcel)) 
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
            if (stations.Count() == 0) throw new Exception("there not stations");
            Station station = DisplayStation(stations.First().Id); ;
            Location minLocation = station.Location;
            double minDistance = distance(loc, station.Location);
            foreach (StationToList dstation in stations)//find the station that is the closest to the location
            {
                station = DisplayStation(dstation.Id);
                if (distance(loc, station.Location) < minDistance)
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
                if (distance(loc, station.Location) < minDistance)
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
        private double minBattery(int droneId, Location from, Location to)
        {
            Drone drone = DisplayDrone(droneId);
            double batteryForKil = 0;
            if (drone.Status == DroneStatus.Available) batteryForKil = BatteryForAvailable;
            else
            {
                WeightCategories weight = drone.ParcelInT.Weight;
                if (weight == WeightCategories.Easy) batteryForKil = BatteryForEasy;
                if (weight == WeightCategories.Medium) batteryForKil = BatteryForMedium;
                if (weight == WeightCategories.Heavy) batteryForKil = BatteryForHeavy;
            }
            double kils = distance(from, to);
            return batteryForKil * kils;
        }
        /// <summary>
        /// Finds the distance between two locations
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private double distance(Location a, Location b)
        {
            return dl.Distance(a.Latti, a.Longi, b.Latti, b.Longi);
        }
    }
}
