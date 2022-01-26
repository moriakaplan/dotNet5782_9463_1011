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
        void AddCustomer(Customer customer);
        /// <summary>
        /// add the drone that he gets to the list of the drones.
        /// </summary>
        /// <param name="drone"></param>
        void AddDrone(Drone drone);
        /// <summary>
        /// add the parcel that he gets to the list of the parcels.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        int AddParcel(Parcel parcel);
        /// <summary>
        /// add the station that he gets to the list of the stations.
        /// </summary>
        /// <param name="station"></param>
        void AddStation(Station station);
        /// <summary>
        /// add the user that he gets to the list of the users.
        /// </summary>
        /// <param name="user"></param>
        void AddUser(User user);
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
        /// display a user of the system
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        User GetUser(string name);
        /// <summary>
        /// display a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Customer GetCustomer(int customerId);
        /// <summary>
        /// display a drone
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        Drone GetDrone(int droneId);
        /// <summary>
        /// display a drone charge
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        DroneCharge GetDroneCharge(int droneId);
        /// <summary>
        /// display a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        Parcel GetParcel(int parcelId);
        /// <summary>
        /// return a station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        Station GetStation(int stationId);
        /// <summary>
        /// display the list of thecustomers
        /// </summary>
        /// <returns></returns>
        IEnumerable<Customer> GetCustomersList(Predicate<Customer> pre = null);
        /// <summary>
        /// display the list of the drones.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Drone> GetDronesList(Predicate<Drone> pre = null);
        /// <summary>
        /// display the list of the customers
        /// </summary>
        /// <returns></returns>
        IEnumerable<Parcel> GetParcelsList(Predicate<Parcel> pre = null);
        /// <summary>
        /// display the list of the stations.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Station> GetStationsList(Predicate<Station> pre = null);
        /// <summary>
        /// display the list of drone charge
        /// </summary>
        /// <param name="pre"></param>
        /// <returns></returns>
        IEnumerable<DroneCharge> GetDroneChargesList(Predicate<DroneCharge> pre = null);
        /// <summary>
        /// display the list of users (regulars and managers)
        /// </summary>
        /// <param name="pre"></param>
        /// <returns></returns>
        IEnumerable<User> GetUsersList(Predicate<User> pre = null);
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
        /// (https://www.geeksforgeeks.org/program-distance-two-points-earth/) 
        /// </summary>
        /// <param name="latitudeA"></param>
        /// <param name="longitudeA"></param>
        /// <param name="latitudeB"></param>
        /// <param name="longitudeB"></param>
        /// <returns></returns>
        double Distance(double lattitudeA, double longitudeA, double lattitudeB, double longitudeB);
        /// <summary>
        /// return the battery data
        /// </summary>
        /// <returns></returns>
        double[] GetBatteryData();

        #region password
        /// <summary>
        /// return the manager password
        /// </summary>
        /// <returns></returns>
        string GetManagmentPassword();
        /// <summary>
        /// change the manager password
        /// </summary>
        /// <returns></returns>
        string SetNewManagmentPassword();
        #endregion
    }
}