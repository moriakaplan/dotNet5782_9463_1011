using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BLApi
{
    public interface IBL
    {
        #region adding
        /// <summary>
        /// add station
        /// </summary>
        /// <param name="station"></param>
        void AddStation(int id, string name, Location loc, int chargeSlots);
        /// <summary>
        /// adding drone
        /// </summary>
        /// <param name="drone"></param>
        void AddDrone(int id, string model, WeightCategories weight, int stationId);
        /// <summary>
        /// add customer to the kist of the customers
        /// </summary>
        /// <param name="customer"></param>
        void AddCustomer(int id, string name, string phone, Location loc);
        /// <summary>
        /// Add Parcel To Delivery
        /// </summary>
        /// <param name="parcel"></param>
        int AddParcelToDelivery(int senderId, int targetId, WeightCategories weight, Priorities pri);
        #endregion adding

        #region updating
        /// <summary>
        /// update the drone model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        void UpdateDroneModel(int id, string model);
        /// <summary>
        /// update the name or the number of charge slots' or both of them.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="cargeSlots"></param>
        void UpdateStation(int id, string name, int cargeSlots);
        /// <summary>
        /// Updates customer details(name, phone)
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        void UpdateCustomer(int id, string name, string phone);
        /// <summary>
        /// send the drone to charge
        /// </summary>
        /// <param name="droneId"></param>
        void SendDroneToCharge(int droneId);
        /// <summary>
        /// Release the Drone Frome Charge
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="timeInCharge"></param>
        void ReleaseDroneFromeCharge(int droneId);
        /// <summary>
        /// Assign Parcel To Drone
        /// </summary>
        /// <param name="droneId"></param>
        void AssignParcelToDrone(int droneId);
        /// <summary>
        /// Pick Parcel By Drone
        /// </summary>
        /// <param name="droneId"></param>
        void PickParcelByDrone(int parcelId);
        /// <summary>
        /// Deliver Parcel By Drone
        /// </summary>
        /// <param name="droneId"></param>
        void DeliverParcelByDrone(int droneId);
        #endregion updating

        #region gets
        /// <summary>
        /// Returns the customer with the requested ID
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Customer GetCustomer(int customerId);
        /// <summary>
        ///  Returns the drone with the requested ID
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        Drone GetDrone(int droneId);
        /// <summary>
        /// Returns the parcel with the requested ID
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        Parcel GetParcel(int parcelId);
        /// <summary>
        /// return the station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        Station GetStation(int stationId);
        /// <summary>
        /// returns the list of the stations
        /// </summary>
        /// <returns></returns>
        IEnumerable<StationToList> GetStationsList();
        /// <summary>
        ///  returns the list of the drones
        /// </summary>
        /// <returns></returns>
        IEnumerable<DroneToList> GetDronesList(Func<DroneToList, bool> pre = null);
        /// <summary>
        /// returns the list of the customers
        /// </summary>
        /// <returns></returns>
        IEnumerable<CustomerToList> GetCustomersList();
        /// <summary>
        /// Display List Of Parcels
        /// </summary>
        /// <returns></returns>
        IEnumerable<ParcelToList> GetParcelsList();
        /// <summary>
        /// Display List Of Unassigned Parcels
        /// </summary>
        /// <returns></returns>
        IEnumerable<ParcelToList> GetListOfUnassignedParcels();
        /// <summary>
        /// returns the stations with available cargeing slots
        /// </summary>
        /// <returns></returns>
        IEnumerable<StationToList> GetListOfStationsWithAvailableCargeSlots();
        #endregion display

        #region delete
        /// <summary>
        /// delete the requedted
        /// </summary>
        /// <param name="id"></param>
        void DeleteParcel(int id);
        #endregion

        #region users functions
        /// <summary>
        /// return the manager password
        /// </summary>
        /// <returns></returns>
        string GetManagmentPassword();
        /// <summary>
        /// change the manager password
        /// </summary>
        /// <returns></returns>
        string ChangeManagmentPassword();
        /// <summary>
        /// return the id of the user (by the name and the password)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        int GetUserId(string name, string password);
        /// <summary>
        /// checks if the user is a manager
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ExistManager(string name, string password);
        /// <summary>
        /// add new user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        void AddUser(int id, string name, string password);
        /// <summary>
        /// add new manager
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        void AddManager(string name, string password);
        /// <summary>
        /// get all the managers name
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetListOfManagersNames();
        /// <summary>
        /// get all the users name
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetListOfUsersNames();
        #endregion

        #region simolator
        /// <summary>
        /// turn on the simolator
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="UpdateDisplayDelegate"></param>
        /// <param name="checkStop"></param>
        void RunsTheSimulator(int droneId, Action UpdateDisplayDelegate, Func<bool> checkStop);
        #endregion
    }
}
