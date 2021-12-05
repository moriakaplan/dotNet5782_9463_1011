using System;
using System.Collections.Generic;
using IDAL.DO;

namespace IDAL
{
    public interface IDal
    {
        void AddCustomerToTheList(Customer customer);
        void AddDroneToTheList(Drone drone);
        int AddParcelToTheList(Parcel parcel);
        void AddStationToTheList(Station station);
        void AssignParcelToDrone(int parcelId, int droneId);
        void DeliverParcelToCustomer(int parcelId);
        Customer DisplayCustomer(int customerId);
        Drone DisplayDrone(int droneId);
        Parcel DisplayParcel(int parcelId);
        Station DisplayStation(int stationId);
        IEnumerable<Customer> DisplayListOfCustomers(Predicate<Customer> pre = null);
        IEnumerable<Drone> DisplayListOfDrones(Predicate<Drone> pre = null);
        IEnumerable<Parcel> DisplayListOfParcels(Predicate<Parcel> pre = null);
        IEnumerable<Station> DisplayListOfStations(Predicate<Station> pre = null);
        IEnumerable<DroneCharge> DisplayListOfDroneCharge(Predicate<DroneCharge> pre = null);
        //IEnumerable<Station> DisplayListOfStationsWithAvailableCargeSlots();
        //IEnumerable<Parcel> DisplayListOfUnassignedParcels();
        double Distance(double lattitudeA, double longitudeA, double lattitudeB, double longitudeB);
        double DistanceForCustomer(double longitudeA, double lattitudeA, int id);
        double DistanceForStation(double longitudeA, double lattitudeA, int id);
        void PickParcelByDrone(int parcelId);
        void ReleaseDroneFromeCharge(int droneId);
        void SendDroneToCharge(int droneId, int stationId);
        double[] AskBattery();
        void DeleteDrone(int droneId);
        void DeleteCustomer(int customerId);
        void DeleteStation(int stationId);
        void DeleteParcel(int parcelId);
    }
}