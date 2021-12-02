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
       /// קונסטרקטור
       /// </summary>
        public BL()
        {
            dl = new DalObject.DalObject();
            lstdrn = (List<DroneToList>)dl.DisplayListOfDrones();
            initializeDrone();
        }
        /// <summary>
        /// הגדרת רשימת הרחפנים
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
                    IDAL.DO.Customer tempCustomer = dl.DisplayCustomer(parcel.Senderld);//לבדוק אם הוא קיים
                    Location locOfCus = new Location { Longi = tempCustomer.Longitude, Latti = tempCustomer.Lattitude };
                    if ((parcel.Droneld == drone.Id) && (parcel.Delivered == DateTime.MinValue))//אם יש חבילה שעוד לא סופקה אבל אבל הרחפן שוייך
                    {
                        drone.Status = DroneStatus.Delivery;
                        if ((parcel.Scheduled != DateTime.MinValue) && (parcel.PickedUp == DateTime.MinValue))//אם החבילה שויכה אבל לא נאספה
                        {
                            drone.CurrentLocation=closestStation(new Location { Latti = tempCustomer.Lattitude, Longi = tempCustomer.Longitude });//המיקום צריך להיות בתחנה הקרובה לשולח
                        }
                        if ((parcel.PickedUp != DateTime.MinValue) && (parcel.Delivered == DateTime.MinValue))//אם החבילה נאספה אבל לא סופקה
                        {
                            drone.CurrentLocation = new Location() { Longi = tempCustomer.Longitude, Latti = tempCustomer.Lattitude };//מיקום הרחפן הוא במיקום השולח
                        }
                        double batteryNeeded = minBattery(drone.Id, drone.CurrentLocation, locOfCus)+ minBattery(drone.Id, locOfCus, closestStation(locOfCus));
                        drone.Battery = random.Next((int)batteryNeeded+1, 100);//יוגרל בין טעינה מינימאלית שתאפשר לרחפן לבצע את המשלוח ולהגיע לתחנה הקרובה לבין טעינה מלאה. צריך לתקן, עכשיו זה כמו בקריאה השנייה
                    }
                    if (DroneNotInDelivery(drone))//אם הרחפן לא מבצע משלוח
                    {
                        drone.Status = (DroneStatus)random.Next(1, 2);//מוגרל בין תחזוקה לפנוי
                    }
                    if (drone.Status == DroneStatus.Maintenance)//הרחפן בתחזוקה
                    {
                        //המיקום מוגרל בין תחנות קיימות***********************************************
                        drone.Battery = random.Next(0, 20);//מצב סוללה מוגרל בין 0 ל-20
                    }
                    if (drone.Status == DroneStatus.Available)//הרחפן פנוי
                    {
                        //מיקום מוגרל בין לקוחות שיש חבילות שסופקו להם***********************************
                        drone.Battery = random.Next((int)minBattery(drone.Id, drone.CurrentLocation, closestStation(drone.CurrentLocation)) + 1, 100);//מצב סוללה מוגרל בין טעינה מינימאלית שמאפשרת לו להגיע לתחנה הקרובה לבין טעינה מלאה
                    }

                }
            }
        }
        /// <summary>
        /// בודק אם הרחפן לא מבצע משלוח
        /// </summary>
        /// <param name="drone"></param>
        /// <returns></returns>
        private bool DroneNotInDelivery(DroneToList drone)
        {
            foreach (IDAL.DO.Parcel parcel in dl.DisplayListOfParcels())
            {
                if ((parcel.Droneld == drone.Id) && (parcel.PickedUp != DateTime.MinValue) && (parcel.Delivered == DateTime.MinValue))//אם הרחפן במשלוח (כבר אסף את החבילה) 
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// return the location of the station that the distance between it and the locations is the smallest
        /// </summary>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private Location closestStation(Location loc)
        {
            IEnumerable<StationToList> stations = DisplayListOfStations();
            Station station= DisplayStation(stations.First().Id); ;
            Location minLocation = station.Location;
            double minDistance =  distance(loc, station.Location);
            foreach (StationToList dstation in stations)//find the station that is the closest to the location
            {
                station = DisplayStation(dstation.Id);
                if (distance(loc, station.Location)<minDistance)//update to be the closest
                {
                    minDistance = distance(loc, station.Location);
                    minLocation=station.Location;
                }
            }
            return minLocation;
        }
        /// <summary>
        /// return the location of the station has available charge slots and the distance between it and the locations is the smallest
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
        /// מוצא את הבטריה המינימאלית שצריך בשביל להגיע מהמיקום לתחנה הקרובה
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
            double kils = distance(from, to);
            return batteryForKil * kils;
        }
        /// <summary>
        /// return the distance between 2 locations
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
