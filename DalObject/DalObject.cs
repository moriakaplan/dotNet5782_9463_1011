using DO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Device.Location;

namespace Dal
{
    internal partial class DalObject : IDal
    {
        private static DalObject instance;
        private static object syncRoot = new object();
        //private static readonly DalObject instance= new DalObject();
        //public static DalObject Instance { get => instance; }
        /// <summary>
        /// constructor.call the static function initialize.
        /// </summary>
        private DalObject()
        {
            DataSource.Initialize();
        }
        /// <summary>
        /// The public Instance property to use
        /// </summary>
        internal static DalObject Instance
        {
            //singelton thread safe and lazy initializion(?)
            get
            {
                if (instance == null)
                {
                    lock(syncRoot)
                    {

                        if (instance == null)
                            instance = new DalObject();
                    }

                }

                return instance;
            }
        }



        /// <summary>
        /// assign the parcel to the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void AssignParcelToDrone(int parcelId, int droneId)
        {
            if (!DataSource.drones.Exists(item => item.Id == droneId))
            {
                throw new DroneException($"id: {droneId} does not exist");
            }
            for (int i = 0; i < DataSource.parcels.Count; i++) //find the parcel and update its details
            {
                if (DataSource.parcels[i].Id == parcelId)
                {
                    Parcel p = DataSource.parcels[i];
                    p.Droneld = droneId;//assign the parcel to the drone
                    p.AssociateTime = DateTime.Now;//update the time that the parcel was scheduled
                    DataSource.parcels[i] = p;
                    return;
                }
            }
            throw new ParcelException($"id: {parcelId} does not exist");
        }

        public void PickParcelByDrone(int parcelId)
        {
            //Parcel parcel = DisplayParcel(parcelId);
            //DataSource.parcels.Remove(parcel);
            //parcel.PickedUp = DateTime.Now;//update the time that the drone pick up the parcel
            //AddParcelToTheList(parcel);
            for (int i = 0; i < DataSource.parcels.Count; i++) //find the parcel and update its details
            {
                if (DataSource.parcels[i].Id == parcelId)
                {
                    Parcel p = DataSource.parcels[i];
                    p.PickUpTime = DateTime.Now;//update the time that the drone pick up the parcel
                    DataSource.parcels[i] = p;
                    return;
                }
            }
            throw new ParcelException($"id: {parcelId} does not exist");

            //Drone drone = DisplayDrone(parcel.Droneld);
            //DataSource.drones.Remove(drone);
            ////drone.Status = DroneStatuses.Sending;//update the status of the drone(sending)
            //AddDroneToTheList(drone);
        }
        /// <summary>
        /// deliver the parcel to the customer
        /// </summary>
        /// <param name="parcelId"></param>
        public void DeliverParcelToCustomer(int parcelId)
        {
            for (int i = 0; i < DataSource.parcels.Count; i++) //find the parcel and update its details
            {
                if (DataSource.parcels[i].Id == parcelId)
                {
                    Parcel p = DataSource.parcels[i];
                    p.DeliverTime = DateTime.Now;//update the time that the drone pick up the parcel
                    DataSource.parcels[i] = p;
                    return;
                }
            }
            throw new ParcelException($"id: {parcelId} does not exist");

            //Drone drone = DisplayDrone(parcel.Droneld);
            //DataSource.drones.Remove(drone);
            ////drone.Status = DroneStatuses.Vacant;//after the drone gives the parcel, he is vacant 
            //AddDroneToTheList(drone);
        }

        /// <summary>
        /// bonus 2
        /// calculates the distance between a point and a station
        /// </summary>
        /// <param name="longitudeA"></param>
        /// <param name="lattitudeA"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public double DistanceForStation(double longitudeA, double lattitudeA, int id)
        {
            Station st = DisplayStation(id);
            return Distance(longitudeA, lattitudeA, st.Longitude, st.Lattitude);
        }
        /// <summary>
        /// bonus 2
        /// calculates the distance between a point and a Customer
        /// </summary>
        /// <param name="longitudeA"></param>
        /// <param name="lattitudeA"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public double DistanceForCustomer(double longitudeA, double lattitudeA, int id)
        {
            Customer cu = DisplayCustomer(id);
            return Distance(longitudeA, lattitudeA, cu.Longitude, cu.Lattitude);
        }
        /// <summary>
        /// bonus 2
        /// Calculates the distance between two places.
        /// (https://www.geeksforgeeks.org/program-distance-two-points-earth/) לקחנו משם את הנוסחא לפונקציה
        /// </summary>
        /// <param name="latitudeA"></param>
        /// <param name="longitudeA"></param>
        /// <param name="latitudeB"></param>
        /// <param name="longitudeB"></param>
        /// <returns></returns>
        public double Distance(double lattitudeA, double longitudeA, double lattitudeB, double longitudeB)
        {
            var radiansOverDegrees = (Math.PI / 180.0);

            var latitudeRadiansA = lattitudeA * radiansOverDegrees;
            var longitudeRadiansA = longitudeA * radiansOverDegrees;
            var latitudeRadiansB = lattitudeB * radiansOverDegrees;
            var longitudeRadiansB = longitudeB * radiansOverDegrees;
            // Haversine formula
            double dlon = longitudeB - longitudeA;
            double dlat = lattitudeB - lattitudeA;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(lattitudeA) * Math.Cos(lattitudeB) *
                       Math.Pow(Math.Sin(dlon / 2), 2);
            double c = 2 * Math.Asin(Math.Sqrt(a));
            //Radius of earth in kilometers.
            double r = 6371;
            // calculate the result
            return (c * r);
        }

        public double[] AskBattery()
        {
            return new double[] {
                DataSource.Config.available,
                DataSource.Config.easy,
                DataSource.Config.medium,
                DataSource.Config.heavy,
                DataSource.Config.ratePerHour };
        }
    }
}