using System;
using System.Collections.Generic;
using DO;

namespace DalApi
{
    public interface IDal
    {
        #region adding
        /// <summary>
        /// add the customer that he gets to the list of the customers.
        /// </summary>
        /// <param name="customer"></param>
        void AddCustomerToTheList(Customer customer);
        /// <summary>
        /// add the drone that he gets to the list of the drones.
        /// </summary>
        /// <param name="drone"></param>
        void AddDroneToTheList(Drone drone);
        /// <summary>
        /// add the parcel that he gets to the list of the parcels.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        int AddParcelToTheList(Parcel parcel);
        /// <summary>
        /// add the station that he gets to the list of the stations.
        /// </summary>
        /// <param name="station"></param>
        void AddStationToTheList(Station station);
        #endregion adding

        #region updating
        /// <summary>
        /// assign the parcel to the drone.
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        void AssignParcelToDrone(int parcelId, int droneId);
        /// <summary>
        /// deliver the parcel to the customer
        /// </summary>
        /// <param name="parcelId"></param>
        void DeliverParcelToCustomer(int parcelId);
        /// <summary>
        /// pick the parcel by the drone
        /// </summary>
        /// <param name="parcelId"></param>
        void PickParcelByDrone(int parcelId);
        /// <summary>
        /// release the drone frome charging 
        /// </summary>
        /// <param name="droneId"></param>
        void ReleaseDroneFromeCharge(int droneId);
        /// <summary>
        /// send the drone for charging
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        void SendDroneToCharge(int droneId, int stationId);
        #endregion updating

        #region display
        /// <summary>
        /// display a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Customer DisplayCustomer(int customerId);
        /// <summary>
        /// display a drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        Drone DisplayDrone(int droneId);
        /// <summary>
        /// display a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        Parcel DisplayParcel(int parcelId);
        /// <summary>
        /// return a station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        Station DisplayStation(int stationId);
        /// <summary>
        /// display the list of thecustomers
        /// </summary>
        /// <returns></returns>
        IEnumerable<Customer> DisplayListOfCustomers(Predicate<Customer> pre = null);
        /// <summary>
        /// display the list of the drones.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Drone> DisplayListOfDrones(Predicate<Drone> pre = null);
        /// <summary>
        /// display the list of the customers
        /// </summary>
        /// <returns></returns>
        IEnumerable<Parcel> DisplayListOfParcels(Predicate<Parcel> pre = null);
        /// <summary>
        /// display the list of the stations.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Station> DisplayListOfStations(Predicate<Station> pre = null);
        /// <summary>
        /// display the list of drone charge
        /// </summary>
        /// <param name="pre"></param>
        /// <returns></returns>
        IEnumerable<DroneCharge> DisplayListOfDroneCharge(Predicate<DroneCharge> pre = null);
        #endregion display

        #region delete
        /// <summary>
        /// remove the drone from the list
        /// </summary>
        /// <param name="droneId"></param>
        void DeleteDrone(int droneId);
        /// <summary>
        /// remove the customer from the list (delete)
        /// </summary>
        /// <param name="customerId"></param>
        void DeleteCustomer(int customerId);
        /// <summary>
        /// remove the station from the list
        /// </summary>
        /// <param name="stationId"></param>
        void DeleteStation(int stationId);
        /// <summary>
        /// delete the parcel from the list
        /// </summary>
        /// <param name="parcelId"></param>
        void DeleteParcel(int parcelId);
        #endregion delete

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
        double Distance(double lattitudeA, double longitudeA, double lattitudeB, double longitudeB); //maybe need to be deleted from the interface
        
        
        double[] GetBatteryData();

    }
}