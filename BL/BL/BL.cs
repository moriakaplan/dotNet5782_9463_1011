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
            //singelton thread safe and lazy initializion(?)
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
        private double BatteryForAvailable;
        private double BatteryForEasy; //per kill
        private double BatteryForMedium; //per kill
        private double BatteryForHeavy; //per kill
        private double ChargeRatePerMinute;
        private static Random random = new Random();

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

            //lstdrn = (List<DroneToList>)dl.DisplayListOfDrones();
            lock(dl)
            {
                double[] batteryData = dl.GetBatteryData();
                BatteryForAvailable = batteryData[0];
                BatteryForEasy = batteryData[1];
                BatteryForMedium = batteryData[2];
                BatteryForHeavy = batteryData[3];
                ChargeRatePerMinute = batteryData[4];
            }
           
            try
            {
                initializeDrone();
            }
            catch (Exception) { Console.WriteLine("was a problem in the BL initialize"); }

        }
        /// <summary>
        /// initialize the list of drones
        /// </summary>
        private void initializeDrone()
        {
            lock(dl)
            {
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
                lock(dl)
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
                                drone.CurrentLocation = closestStation(locOfSender);
                            }
                            else //If the parcel was picked up but not delivered
                            {
                                drone.Status = DroneStatus.Delivery;
                                drone.CurrentLocation = locOfSender;//The location of the drone is in the location of the sender
                            }

                            double batteryForKil = 0;
                            DO.WeightCategories weight = parcel.Weight;
                            if (weight == DO.WeightCategories.Light) batteryForKil = BatteryForEasy;
                            if (weight == DO.WeightCategories.Medium) batteryForKil = BatteryForMedium;
                            if (weight == DO.WeightCategories.Heavy) batteryForKil = BatteryForHeavy;

                            //double batteryNeeded = BatteryForAvailable * distance(drone.CurrentLocation, locOfSender) + //=0 if the drone already took the parcel
                            //    batteryForKil * distance(locOfSender, closestStation(locOfSender));
                            double batteryNeeded = BatteryForAvailable * distance(drone.CurrentLocation, locOfSender) + //=0 if the drone already took the parcel
                                batteryForKil * distance(locOfSender, locOfTarget)+
                                BatteryForAvailable* distance(locOfTarget, closestStationWithChargeSlots(locOfTarget).Location);
                            //double batteryNeeded = 
                            //    minBattery(drone.Id, drone.CurrentLocation, locOfCus) +
                            //    minBattery(drone.Id, locOfCus, closestStation(locOfCus));
                            if (batteryNeeded >= 100) throw new DroneCantTakeParcelException("the drone has not enugh battery for take the parcel he suppose to take.");
                            drone.Battery = random.Next((int)batteryNeeded + 1, 100);
                            drone.ParcelId = parcel.Id;
                            break;
                        }
                    }
                }
               
                if (/*DroneNotInDelivery(drone)*/ drone.Status==DroneStatus.Zero)//If the drone does not ship
                {
                    //drone.Status = (DroneStatus)random.Next(0, 2);//Maintenance or availability
                    //try { lock (dl) { dl.ReleaseDroneFromeCharge(drone.Id); } }
                    //catch (DO.DroneChargeException) { } //לא היה בטעינה. עבור דאל אובגקט נגריל ועבור דאל אקסאמאל נעשה אוויילבל?
                    
                    try
                    {
                        lock (dl)
                        {
                            DO.DroneCharge dc = dl.GetDroneCharge(drone.Id); 
                            drone.Status = DroneStatus.Maintenance;
                            drone.Battery = random.Next(0, 19);//random battery mode between 0 and 20
                            DO.Station st = dl.GetStation(dc.StationId);
                            drone.CurrentLocation = new Location { Latti = st.Lattitude, Longi = st.Longitude };
                        }
                    }
                    catch (DO.DroneChargeException) 
                    { 
                        drone.Status = DroneStatus.Available;
                        IEnumerable<CustomerToList> customersWhoGotParcels = GetCustomersList().Where(x => x.numOfParclReceived > 0);
                        int index = random.Next(0, customersWhoGotParcels.Count());
                        CustomerToList customerForLocation = customersWhoGotParcels.ElementAt(index);
                        drone.CurrentLocation = GetCustomer(customerForLocation.Id).Location;
                        int battery = (int)minBattery(drone.Id, drone.CurrentLocation, closestStation(drone.CurrentLocation)) + 1;
                        if(battery>100) throw new DroneCantTakeParcelException("the drone has not enugh battery for go to the closest station.");
                        drone.Battery = random.Next(battery, 100); //random  between a minimal charge that allows it to reach the nearest station and a full charge
                    }
                    //if (drone.Status == DroneStatus.Maintenance)
                    //{
                    //    //the location is in random station
                    //    lock (dl) 
                    //    {
                    //        //in the dal
                    //        //IEnumerable<DO.Station> stations = dl.GetStationsList();
                    //        //int index = random.Next(0, stations.Count());
                    //        //DO.Station stationForLocation = stations.ElementAt(index);
                    //        //drone.CurrentLocation = new Location { Latti = stationForLocation.Lattitude, Longi = stationForLocation.Longitude };
                    //        //dl.SendDroneToCharge(drone.Id, stationForLocation.Id);
                            
                    //        drone.Battery = random.Next(0, 19) ;//random battery mode between 0 and 20
                    //        DO.Station st = dl.GetStation(dc.StationId);
                    //        drone.CurrentLocation = new Location { Latti = st.Lattitude, Longi = st.Longitude };
                    //    }
                    //}
                    //if (drone.Status == DroneStatus.Available)//the drone is available
                    //{
                    //    IEnumerable<CustomerToList> customersWhoGotParcels = GetCustomersList().Where (x=>x.numOfParclReceived > 0);
                    //    int index = random.Next(0, customersWhoGotParcels.Count());
                    //    CustomerToList customerForLocation = customersWhoGotParcels.ElementAt(index);
                    //    drone.CurrentLocation = GetCustomer(customerForLocation.Id).Location;
                    //    drone.Battery = random.Next((int)minBattery(drone.Id, drone.CurrentLocation, closestStation(drone.CurrentLocation)) + 1, 99) /*+ random.NextDouble()*/;//random  between a minimal charge that allows it to reach the nearest station and a full charge
                    //}
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
            lock(dl)
            {
                return dl.GetParcelsList()
                  .Count(x => (x.Droneld == drone.Id) && (x.DeliverTime == null)) == 0;
            }
               
        }
        /// <summary>
        /// Finds the closest station to the location
        /// </summary>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private Location closestStation(Location loc)
        {
            IEnumerable<StationToList> stations = GetStationsList();
            if (stations.Count() == 0) throw new Exception("there not stations");
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
            minDistance = stations.Min(x => distance(loc, GetStation(x.Id).Location));
            return minLocation;
        }
        /// <summary>
        /// Finds the closest station with available charge slots 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private Station closestStationWithChargeSlots(Location loc)
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
            if (drone.Status == DroneStatus.Available) batteryForKil = BatteryForAvailable;
            else
            {
                WeightCategories weight = drone.ParcelInT.Weight;
                if (weight == WeightCategories.Light) batteryForKil = BatteryForEasy;
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
            lock (dl)
            {
                return dl.Distance(a.Latti, a.Longi, b.Latti, b.Longi);
            }
        }

    }
}
