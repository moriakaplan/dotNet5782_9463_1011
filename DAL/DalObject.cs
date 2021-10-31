using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Device.Location;

namespace DalObject
{
    public class DalObject
    {
        /// <summary>
        /// constractor.call the static function initialize.
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }
        /// <summary>
        /// add the station that he gets to the list of the stations.
        /// </summary>
        /// <param name="station"></param>
        public void AddStationToTheList(Station station)
        {
            DataSource.stations.Add(station);
        }
        /// <summary>
        /// add the drone that he gets to the list of the drones.
        /// </summary>
        /// <param name="drone"></param>
        public void AddDroneToTheList(Drone drone)
        {
            DataSource.drones.Add(drone);
        }
        /// <summary>
        /// add the customer that he gets to the list of the customers.
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomerToTheList(Customer customer)
        {
            DataSource.customers.Add(customer);
        }
        /// <summary>
        /// add the parcel that he gets to the list of the parcels.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public int AddParcelToTheList(Parcel parcel)
        {
            parcel.Id = DataSource.Config.parcelCode++;
            DataSource.parcels.Add(parcel);
            return DataSource.Config.parcelCode;
        }
        /// <summary>
        /// assign the parcel to the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void AssignParcelToDrone(int parcelId, int droneId)
        {
            Drone drone = DisplayDrone(droneId);
            DataSource.drones.Remove(drone);
            drone.Status = DroneStatuses.Assigned;//update the status of the drone
            AddDroneToTheList(drone);

            Parcel parcel = DisplayParcel(parcelId);
            DataSource.parcels.Remove(parcel);
            DataSource.Config.parcelCode--;
            parcel.Droneld = droneId;//assign the pecel to the drone
            parcel.Scheduled = DateTime.Now;//update the time that the parcel was scheduled
            AddParcelToTheList(parcel);
        }
        /// <summary>
        /// pick the parcel by the drone
        /// </summary>
        /// <param name="parcelId"></param>
        public void PickParcelByDrone(int parcelId)
        {
            Parcel parcel = DisplayParcel(parcelId);
            DataSource.parcels.Remove(parcel);
            parcel.PickedUp = DateTime.Now;//update the time that the drone pick up the parcel
            AddParcelToTheList(parcel);

            Drone drone = DisplayDrone(parcel.Droneld);
            DataSource.drones.Remove(drone);
            drone.Status = DroneStatuses.Sending;//update the status of the drone(sending)
            AddDroneToTheList(drone);
        }
        /// <summary>
        /// deliver the parcel to the customer
        /// </summary>
        /// <param name="parcelId"></param>
        public void DeliverParcelToCustomer(int parcelId)
        {
            Parcel parcel = DisplayParcel(parcelId);
            DataSource.parcels.Remove(parcel);
            parcel.Delivered = DateTime.Now;//update the delivering time
            AddParcelToTheList(parcel);

            Drone drone = DisplayDrone(parcel.Droneld);
            DataSource.drones.Remove(drone);
            drone.Status = DroneStatuses.Vacant;//after the drone gives the parcel, he is vacant 
            AddDroneToTheList(drone);
        }
       /// <summary>
       /// send the drone for charging
       /// </summary>
       /// <param name="droneId"></param>
       /// <param name="stationId"></param>
        public void SendDroneToCharge(int droneId, int stationId)
        {
            DroneCharge droneCharge = new DroneCharge { DroneId = droneId, StationId = stationId };
            DataSource.droneCharges.Add(droneCharge);

            Drone drone = DisplayDrone(droneId);
            DataSource.drones.Remove(drone);
            drone.Status = DroneStatuses.Maintenance;
            AddDroneToTheList(drone);

            Station station = DisplayStation(stationId);
            DataSource.stations.Remove(station);
            station.ChargeSlots++;
            AddStationToTheList(station);
        }
        /// <summary>
        /// release the drone frome charging 
        /// </summary>
        /// <param name="droneId"></param>
        public void ReleaseDroneFromeCharge(int droneId)
        {
            DroneCharge dCharge = DataSource.droneCharges.Find(x => x.DroneId == droneId);
            //DroneCharge dCharge = new DroneCharge();
            //foreach (DroneCharge item in DataSource.droneCharges)
            //{
            //    if (item.DroneId == droneId)
            //        dCharge = item;
            //}
            DataSource.droneCharges.Remove(dCharge);

            Drone drone = DisplayDrone(droneId);
            DataSource.drones.Remove(drone);
            drone.Status = DroneStatuses.Vacant;
            AddDroneToTheList(drone);

            Station station = DisplayStation(dCharge.StationId);
            DataSource.stations.Remove(station);
            station.ChargeSlots--;
            AddStationToTheList(station);
        }
       /// <summary>
       /// display a station
       /// </summary>
       /// <param name="stationId"></param>
       /// <returns></returns>
        public Station DisplayStation(int stationId)
        {
            Station temp = new Station();
            foreach (Station item in DataSource.stations)
            {
                if (item.Id == stationId)
                    temp = item;
            }
            return temp;
        }
        /// <summary>
        /// display a drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Drone DisplayDrone(int droneId)
        {
            Drone temp = new Drone();
            foreach (Drone item in DataSource.drones)
            {
                if (item.Id == droneId)
                    temp = item;
            }
            return temp;
        }
        /// <summary>
        /// display a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer DisplayCustomer(int customerId)
        {
            Customer temp = new Customer();
            foreach (Customer item in DataSource.customers)
            {
                if (item.Id == customerId)
                    temp = item;
            }
            return temp;
        }
        /// <summary>
        /// display a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public Parcel DisplayParcel(int parcelId)
        {
            Parcel temp = new Parcel();
            foreach (Parcel item in DataSource.parcels)
            {
                if (item.Id == parcelId)
                    temp = item;
            }
            return temp;
        }
       /// <summary>
       /// display the list of the stations.
       /// </summary>
       /// <returns></returns>
        public List<Station> DisplayListOfStations()
        {
            return DataSource.stations;
        }
        /// <summary>
        /// display the list of the drones.
        /// </summary>
        /// <returns></returns>
        public List<Drone> DisplayListOfDrones()
        {
            return DataSource.drones;
        }
        /// <summary>
        /// display the list of thecustomers
        /// </summary>
        /// <returns></returns>
        public List<Customer> DisplayListOfCustomers()
        {
            return DataSource.customers;
        }
        /// <summary>
       /// display the list of the customers
       /// </summary>
       /// <returns></returns>
        public List<Parcel> DisplayListOfParcels()
        {
            return DataSource.parcels;
        }
        /// <summary>
        /// display the list of the unassign parcels
        /// </summary>
        /// <returns></returns>
        public List<Parcel> DisplayListOfUnassignedParcels()
        {
            List<Parcel> unassignedParcels = new List<Parcel>();
            foreach (Parcel item in DataSource.parcels)
            {
                if (item.Scheduled == DateTime.MinValue)
                    unassignedParcels.Add(item);
            }
            return unassignedParcels;
        }
        /// <summary>
        /// display a list of all the station with empty charge slots
        /// </summary>
        /// <returns></returns>
        public List<Station> DisplayListOfStationsWithAvailableCargeSlots()
        {
            List<Station> StationsWithAvailableCargingSlots = new List<Station>();
            foreach (Station item in DataSource.stations)
            {
                if (item.ChargeSlots > 0)
                    StationsWithAvailableCargingSlots.Add(item);
            }
            return StationsWithAvailableCargingSlots;
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
            double longitudeB = 0, lattitudeB = 0;
            Station st = DisplayStation(id);
            longitudeB = st.Longitude;
            lattitudeB = st.Lattitude;
            return Distance(longitudeA, lattitudeA, longitudeB, lattitudeB);
        }
        /// <summary>
        /// bonus2
        /// Calculates the distance between a point and a customer
        /// </summary>
        /// <param name="longitudeA"></param>
        /// <param name="lattitudeA"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public double DistanceForCustomer(double longitudeA, double lattitudeA, int id)
        {
            double longitudeB = 0, lattitudeB = 0;
            Customer cu = DisplayCustomer(id);
            longitudeB = cu.Longitude;
            lattitudeB = cu.Lattitude;
            return Distance(longitudeA, lattitudeA, longitudeB, lattitudeB);
        }
        /// <summary>
        /// bonus 2
        /// Calculates the distance between two places.
        /// </summary>
        /// <param name="latitudeA"></param>
        /// <param name="longitudeA"></param>
        /// <param name="latitudeB"></param>
        /// <param name="longitudeB"></param>
        /// <returns></returns>
        public double Distance(double latitudeA, double longitudeA, double latitudeB, double longitudeB)
        {
            var radiansOverDegrees = (Math.PI / 180.0);

            var latitudeRadiansA = latitudeA * radiansOverDegrees;
            var longitudeRadiansA = longitudeA * radiansOverDegrees;
            var latitudeRadiansB = latitudeB * radiansOverDegrees;
            var longitudeRadiansB = longitudeB * radiansOverDegrees;

            var dLongitude = longitudeRadiansB - longitudeRadiansA;
            var dLatitude = latitudeRadiansB - latitudeRadiansA;

            var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) + Math.Cos(latitudeRadiansB) * Math.Cos(latitudeRadiansA) * Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            // Using 3956 as the number of miles around the earth
            var result2 = 3956.0 * 2.0 * Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return result2;
        }
    }
}
     









