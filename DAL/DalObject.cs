using IDAL.DO;
using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Device.Location;

namespace DalObject
{
    public class DalObject : IDal
    {
        //public static DataSource DataSource { get; private set; }

        /// <summary>
        /// constructor.call the static function initialize.
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
            if (DataSource.stations.Exists(item => item.Id == station.Id)) throw new StationException($"id: {station.Id} already exist"); //it suppose to be this type of exception????**** 
            DataSource.stations.Add(station);
        }
        /// <summary>
        /// add the drone that he gets to the list of the drones.
        /// </summary>
        /// <param name="drone"></param>
        public void AddDroneToTheList(Drone drone)
        {
            if (DataSource.drones.Exists(item => item.Id == drone.Id)) throw new DroneException($"id: {drone.Id} already exist"); //it suppose to be this type of exception????**** 
            DataSource.drones.Add(drone);
        }
        /// <summary>
        /// add the customer that he gets to the list of the customers.
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomerToTheList(Customer customer)
        {
            if (DataSource.customers.Exists(item => item.Id == customer.Id)) throw new CustomerException($"id: {customer.Id} already exist"); //it suppose to be this type of exception????**** 
            DataSource.customers.Add(customer);
        }
        /// <summary>
        /// add the parcel that he gets to the list of the parcels.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public int AddParcelToTheList(Parcel parcel)
        {
            if (DataSource.parcels.Exists(item => item.Id == parcel.Id)) throw new ParcelException($"id: {parcel.Id} already exist"); //it suppose to be this type of exception????**** 
            parcel.Id = ++DataSource.Config.parcelCode;
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
            //Drone drone = DisplayDrone(droneId);
            //DataSource.drones.Remove(drone);
            ////drone.Status = DroneStatuses.Assigned;//update the status of the drone
            //AddDroneToTheList(drone);

            //Parcel parcel = DisplayParcel(parcelId);
            //DataSource.parcels.Remove(parcel);
            //DataSource.Config.parcelCode--;
            //parcel.Droneld = droneId;//assign the pecel to the drone
            //parcel.Scheduled = DateTime.Now;//update the time that the parcel was scheduled
            //AddParcelToTheList(parcel);
            if(!DataSource.drones.Exists(item => item.Id == droneId))
            { 
                throw new DroneException($"id: {droneId} does not exist"); 
            }
            for (int i=0;i<DataSource.parcels.Count;i++) //find the parcel and update its details
            {
                if(DataSource.parcels[i].Id==parcelId)
                {
                    Parcel p = DataSource.parcels[i];
                    p.Droneld = droneId;//assign the pecel to the drone
                    p.Scheduled = DateTime.Now;//update the time that the parcel was scheduled
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
                    p.PickedUp = DateTime.Now;//update the time that the drone pick up the parcel
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
            //Parcel parcel = DisplayParcel(parcelId);
            //DataSource.parcels.Remove(parcel);
            //parcel.Delivered = DateTime.Now;//update the delivering time
            //AddParcelToTheList(parcel);
            for (int i = 0; i < DataSource.parcels.Count; i++) //find the parcel and update its details
            {
                if (DataSource.parcels[i].Id == parcelId)
                {
                    Parcel p = DataSource.parcels[i];
                    p.Delivered = DateTime.Now;//update the time that the drone pick up the parcel
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
        /// send the drone for charging
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        public void SendDroneToCharge(int droneId, int stationId)
        {
            if (!DataSource.drones.Exists(item => item.Id == droneId))
            {
                throw new DroneException($"id: {droneId} does not exist");
            }
            DroneCharge droneCharge = new DroneCharge { DroneId = droneId, StationId = stationId }; //add drone charge to the list for charging the drone
            DataSource.droneCharges.Add(droneCharge);

            //Drone drone = DisplayDrone(droneId);
            //DataSource.drones.Remove(drone);
            ////drone.Status = DroneStatuses.Maintenance;
            //AddDroneToTheList(drone);

            //Station station = DisplayStation(stationId);
            //DataSource.stations.Remove(station);
            //station.ChargeSlots++;
            //AddStationToTheList(station);
            for (int i = 0; i < DataSource.stations.Count; i++) //find the station and update its details
            {
                if (DataSource.stations[i].Id == stationId)
                {
                    Station s = DataSource.stations[i];
                    s.ChargeSlots--; //++?
                    DataSource.stations[i] = s;
                    return;
                }
            }
            throw new StationException($"id: {stationId} does not exist");
        }
        /// <summary>
        /// release the drone frome charging 
        /// </summary>
        /// <param name="droneId"></param>
        public void ReleaseDroneFromeCharge(int droneId)
        {
            DroneCharge dCharge;
            try { dCharge = DataSource.droneCharges.Find(x => x.DroneId == droneId); }
            catch (ArgumentNullException)
            {
                throw new DroneChargeException($"drone chare with the drone ID {droneId} does not exist");
            }
            DataSource.droneCharges.Remove(dCharge);

            //Drone drone = DisplayDrone(droneId);
            //DataSource.drones.Remove(drone);
            ////drone.Status = DroneStatuses.Vacant;
            //AddDroneToTheList(drone);

            //Station station = DisplayStation(dCharge.StationId);
            //DataSource.stations.Remove(station);
            //station.ChargeSlots--;
            //AddStationToTheList(station);
            for (int i = 0; i < DataSource.stations.Count; i++) //find the station and update its details
            {
                if (DataSource.stations[i].Id == dCharge.StationId)
                {
                    Station s = DataSource.stations[i];
                    s.ChargeSlots++; //--?
                    DataSource.stations[i] = s;
                    return;
                }
            }
            throw new StationException($"id: {dCharge.StationId} does not exist");
        }
        /// <summary>
        /// display a station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public Station DisplayStation(int stationId)
        {
            //Station temp = new Station();
            //foreach (Station item in DataSource.stations)
            //{
            //    if (item.Id == stationId)
            //        temp = item;
            //}
            //return temp;
            try { return DataSource.stations.Find(item => item.Id == stationId); }
            catch (ArgumentNullException)
            {
                throw new StationException($"id: {stationId} does not exist");
            }
        }
        /// <summary>
        /// display a drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Drone DisplayDrone(int droneId)
        {
            //Drone temp = new Drone();
            //foreach (Drone item in DataSource.drones)
            //{
            //    if (item.Id == droneId)
            //        temp = item;
            //}
            //return temp;
            try { return DataSource.drones.Find(item => item.Id == droneId); }
            catch (ArgumentNullException)
            {
                throw new DroneException($"id: {droneId} does not exist");
            }
        }
        /// <summary>
        /// display a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer DisplayCustomer(int customerId)
        {
            //Customer temp = new Customer();
            //foreach (Customer item in DataSource.customers)
            //{
            //    if (item.Id == customerId)
            //        temp = item;
            //}
            //return temp;
            try { return DataSource.customers.Find(item => item.Id == customerId); }
            catch (ArgumentNullException)
            {
                throw new CustomerException($"id: {customerId} does not exist");
            }
        }
        /// <summary>
        /// display a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public Parcel DisplayParcel(int parcelId)
        {
            //Parcel temp = new Parcel();
            //foreach (Parcel item in DataSource.parcels)
            //{
            //    if (item.Id == parcelId)
            //        temp = item;
            //}
            //return temp;
            try { return DataSource.parcels.Find(item => item.Id == parcelId); }
            catch (ArgumentNullException)
            {
                throw new ParcelException($"id: {parcelId} does not exist");
            }
        }
        /// <summary>
        /// display the list of the stations.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> DisplayListOfStations()
        {
            List<Station> result = new List<Station>(DataSource.stations);
            return result;
        }
        /// <summary>
        /// display the list of the drones.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Drone> DisplayListOfDrones()
        {
            List<Drone> result = new List<Drone>(DataSource.drones);
            return result;
        }
        /// <summary>
        /// display the list of thecustomers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> DisplayListOfCustomers()
        {
            List<Customer> result = new List<Customer>(DataSource.customers);
            return result;
        }
        /// <summary>
        /// display the list of the customers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> DisplayListOfParcels()
        {
            List<Parcel> result = new List<Parcel>(DataSource.parcels);
            return result;
        }
        /// <summary>
        /// display the list of the unassign parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> DisplayListOfUnassignedParcels()
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
        public IEnumerable<Station> DisplayListOfStationsWithAvailableCargeSlots()
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
            Station st;
            try { st = DisplayStation(id); }
            catch(StationException sExc) { throw sExc; } //to chex why it is a problem*****
            return Distance(longitudeA, lattitudeA, st.Longitude, st.Lattitude);
        }
        public double DistanceForCustomer(double longitudeA, double lattitudeA, int id)
        {
            Customer cu;
            try { cu = DisplayCustomer(id); }
            catch(CustomerException cExc) { throw cExc; } //to chex why it is a problem*****
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
        public double[] askBattery(Drone drone)
        {
            return new double[] {
                DataSource.Config.available,
                DataSource.Config.easy,
                DataSource.Config.medium,
                DataSource.Config.heavy,
                DataSource.Config.rate };
        }
    }
}