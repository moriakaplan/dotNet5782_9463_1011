using System.Collections.Generic;

namespace IDAL.DO
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
        IEnumerable<Customer> DisplayListOfCustomers();
        IEnumerable<Drone> DisplayListOfDrones();
        IEnumerable<Parcel> DisplayListOfParcels();
        IEnumerable<Station> DisplayListOfStations();
        IEnumerable<Station> DisplayListOfStationsWithAvailableCargeSlots();
        IEnumerable<Parcel> DisplayListOfUnassignedParcels();
        Parcel DisplayParcel(int parcelId);
        Station DisplayStation(int stationId);
        double Distance(double lattitudeA, double longitudeA, double lattitudeB, double longitudeB);
        double DistanceForCustomer(double longitudeA, double lattitudeA, int id);
        double DistanceForStation(double longitudeA, double lattitudeA, int id);
        void PickParcelByDrone(int parcelId);
        void ReleaseDroneFromeCharge(int droneId);
        void SendDroneToCharge(int droneId, int stationId);
        double[] askBattery(Drone drone)
        {
            return new double[] { 
                DalObject.DataSource.Config.available, 
                DalObject.DataSource.Config.easy, 
                DalObject.DataSource.Config.medium, 
                DalObject.DataSource.Config.heavy, 
                DalObject.DataSource.Config.rate };
        }
    }
}