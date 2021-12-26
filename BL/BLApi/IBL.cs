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
        void AddParcelToDelivery(int senderId, int targetId, WeightCategories weight, Priorities pri);
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

        #region display
        /// <summary>
        /// Returns the customer with the requested ID
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Customer DisplayCustomer(int customerId);
        /// <summary>
        ///  Returns the drone with the requested ID
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        Drone DisplayDrone(int droneId);
        /// <summary>
        /// Returns the parcel with the requested ID
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        Parcel DisplayParcel(int parcelId);
        /// <summary>
        /// return the station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        Station DisplayStation(int stationId);
        /// <summary>
        /// returns the list of the stations
        /// </summary>
        /// <returns></returns>
        IEnumerable<StationToList> DisplayListOfStations();
        /// <summary>
        ///  returns the list of the drones
        /// </summary>
        /// <returns></returns>
        IEnumerable<DroneToList> DisplayListOfDrones(Func<DroneToList, bool> pre=null);
        /// <summary>
        /// returns the list of the customers
        /// </summary>
        /// <returns></returns>
        IEnumerable<CustomerToList> DisplayListOfCustomers();
        /// <summary>
        /// Display List Of Parcels
        /// </summary>
        /// <returns></returns>
        IEnumerable<ParcelToList> DisplayListOfParcels();
        /// <summary>
        /// Display List Of Unassigned Parcels
        /// </summary>
        /// <returns></returns>
        IEnumerable<ParcelToList> DisplayListOfUnassignedParcels();
        /// <summary>
        /// returns the stations with available cargeing slots
        /// </summary>
        /// <returns></returns>
        IEnumerable<StationToList> DisplayListOfStationsWithAvailableCargeSlots();
        #endregion display

    }
}
