using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLApi;
using DalApi;
using System.Runtime.CompilerServices;

namespace BL
{
    internal partial class BL : IBL
    {
        private static BL instance;
        private static object syncRoot = new object();

        /// <summary>
        /// The public Instance property to use
        /// </summary>
        internal static BL Instance
        {
            //singelton thread safe and lazy initializion
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new BL();
                }
                return instance;
            }
        }


        private List<DroneToList> lstdrn;
        internal readonly IDal dl;
        internal double batteryForAvailable;
        internal double batteryForLight; //per kilometer
        internal double batteryForMedium; //per kilometer
        internal double batteryForHeavy; //per kilometer
        internal double chargeRatePerMinute;
        internal static Random random = new Random();

        /// <summary>
        /// Turns on the simulator
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="UpdateDisplayDelegate"></param>
        /// <param name="checkStop"></param>
        public void RunsTheSimulator(int droneId, Action UpdateDisplayDelegate, Func<bool> checkStop)
        {
            //call the constructor of the class simulator
            new Simulator(this, droneId, UpdateDisplayDelegate, checkStop);
        }

        /// <summary>
        /// constractor
        /// </summary>
        public BL()
        {
            dl = DalFactory.GetDal();
            lock (dl)
            {
                double[] batteryData = dl.GetBatteryData();
                batteryForAvailable = batteryData[0];
                batteryForLight = batteryData[1];
                batteryForMedium = batteryData[2];
                batteryForHeavy = batteryData[3];
                chargeRatePerMinute = batteryData[4];
            }
            try
            {
                initializeDrone();
            }
            catch (DroneCantTakeParcelException) { Console.WriteLine("was a problem in the BL initialize"); }
        }

        /// <summary>
        /// initialize the list of drones
        /// </summary>
        private void initializeDrone()
        {
            lock (dl)
            {
                //Initializes the id, max weight and the model of each drone
                lstdrn = dl.GetDronesList()
               .Select(drone => new DroneToList
               {
                   Id = drone.Id,
                   MaxWeight = (WeightCategories)drone.MaxWeight,
                   Model = drone.Model
               })
               .ToList();
            }
            foreach (DroneToList drone in lstdrn)
            {
                lock (dl)
                {
                    foreach (DO.Parcel parcel in dl.GetParcelsList())
                    {
                        Location locOfSender = GetCustomer(parcel.Senderld).Location;
                        Location locOfTarget = GetCustomer(parcel.TargetId).Location;
                        if ((parcel.Droneld == drone.Id) && (parcel.AssociateTime != null) && (parcel.DeliverTime == null)) //If there is a parcel that has not yet been delivered but the drone is associated
                        {
                            if (parcel.PickUpTime == null) //If the parcel was associated but not picked up
                            {
                                drone.Status = DroneStatus.Associated;
                                drone.CurrentLocation = closestStationLocation(locOfSender) ?? new Location { Latti = 31, Longi = 31 };//the drone location is the closest station to the sender (if there is no station- it is 31,31)
                            }
                            else //If the parcel was picked up but not delivered
                            {
                                drone.Status = DroneStatus.Delivery;
                                drone.CurrentLocation = locOfSender;//The location of the drone is in the location of the sender
                            }

                            //Initializes the battery of the drone
                            double batteryForKil = 0;
                            DO.WeightCategories weight = parcel.Weight;
                            if (weight == DO.WeightCategories.Light) batteryForKil = batteryForLight;
                            if (weight == DO.WeightCategories.Medium) batteryForKil = batteryForMedium;
                            if (weight == DO.WeightCategories.Heavy) batteryForKil = batteryForHeavy;
                            double batteryNeeded = batteryForAvailable * distance(drone.CurrentLocation, locOfSender) + //=0 if the drone already took the parcel (the battey frome where the drone is to the sender)
                                batteryForKil * distance(locOfSender, locOfTarget) +//the battery from the sender to the target
                                batteryForAvailable * distance(locOfTarget, closestStationWithChargeSlots(locOfTarget).Location);//the battery from the target to the closest station (with available charge slots)
                            if (batteryNeeded > 100) throw new DroneCantTakeParcelException("the drone has not enugh battery for take the parcel he suppose to take.");
                            drone.Battery = random.Next((int)Math.Ceiling(batteryNeeded), 99) + random.NextDouble();//the battery is random number between the needed battery and 100
                            drone.ParcelId = parcel.Id;
                            break;
                        }
                    }
                }

                if (drone.Status == DroneStatus.Maintenance)//If the drone does not ship
                {
                    try
                    {
                        lock (dl)
                        {
                            DO.DroneCharge dc = dl.GetDroneCharge(drone.Id);
                            drone.Status = DroneStatus.Maintenance;
                            drone.Battery = random.Next(0, 19) + random.NextDouble();//random battery mode between 0 and 20
                            DO.Station st = dl.GetStation(dc.StationId);
                            drone.CurrentLocation = new Location { Latti = st.Lattitude, Longi = st.Longitude };
                        }
                    }
                    catch (DO.DroneChargeException)
                    {
                        drone.Status = DroneStatus.Available;
                        IEnumerable<CustomerToList> customersWhoGotParcels = GetCustomersList().Where(x => x.numOfParclReceived > 0);//get all the customers that get parcels
                        int count = customersWhoGotParcels.Count();
                        if (count > 0)
                        {
                            int index = random.Next(0, count);
                            CustomerToList customerForLocation = customersWhoGotParcels.ElementAt(index);//get random customer (from the customers that got parcels)
                            drone.CurrentLocation = GetCustomer(customerForLocation.Id).Location;
                        }
                        else
                        {
                            IEnumerable<DO.Station> stations = dl.GetStationsList();
                            if (stations.Count() > 0)
                            {
                                int index = random.Next(0, stations.Count());
                                DO.Station stationForLocation = stations.ElementAt(index);//get a random station
                                drone.CurrentLocation = new Location { Latti = stationForLocation.Lattitude, Longi = stationForLocation.Longitude };
                            }
                            else drone.CurrentLocation = new Location { Latti = 30, Longi = 30 };
                        }
                        Location closestStationLoc = closestStationLocation(drone.CurrentLocation) ?? new Location { Latti = 31, Longi = 31 };
                        double battery = minBattery(drone.Id, drone.CurrentLocation, closestStationLoc) + 1;
                        if (battery > 100) throw new DroneCantTakeParcelException("the drone has not enugh battery for go to the closest station.");
                        drone.Battery = random.Next((int)Math.Ceiling(battery), 99) + random.NextDouble(); //random  between a minimal charge that allows it to reach the nearest station and a full charge
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the drone does not ship
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        private bool droneNotInDelivery(DroneToList drone)
        {
            lock (dl)
            {
                return dl.GetParcelsList()
                  .Count(x => (x.Droneld == drone.Id) && (x.DeliverTime == null)) == 0;
            }

        }

        /// <summary>
        /// Finds the closest station to the location
        /// return null if there is no stations
        /// </summary>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        internal Location closestStationLocation(Location loc)
        {
            IEnumerable<StationToList> stations = GetStationsList();
            if (stations.Count() == 0) return null;
            Station station = GetStation(stations.First().Id); ;
            Location minLocation = station.Location;
            double minDistance = distance(loc, station.Location);
            foreach (StationToList dstation in stations)//find the station that is the closest to the location
            {
                station = GetStation(dstation.Id);
                double dis = distance(loc, station.Location);
                if (dis < minDistance)
                {
                    minDistance = dis;
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
        internal Station closestStationWithChargeSlots(Location loc)
        {
            IEnumerable<StationToList> stations = GetListOfStationsWithAvailableCargeSlots();
            Station station = GetStation(stations.First().Id); ;
            Station minStation = station;
            double minDistance = distance(loc, station.Location);
            foreach (StationToList dstation in stations)//find the station that is the closest to the location
            {
                station = GetStation(dstation.Id);
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
            Drone drone = GetDrone(droneId);
            double batteryForKil = 0;
            if (drone.Status == DroneStatus.Available) batteryForKil = batteryForAvailable;
            else
            {
                WeightCategories weight = drone.ParcelInT.Weight;
                if (weight == WeightCategories.Light) batteryForKil = batteryForLight;
                if (weight == WeightCategories.Medium) batteryForKil = batteryForMedium;
                if (weight == WeightCategories.Heavy) batteryForKil = batteryForHeavy;
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
        internal double distance(Location a, Location b)
        {
            lock (dl)
            {
                return dl.Distance(a.Latti, a.Longi, b.Latti, b.Longi);
            }
        }

    }
}
