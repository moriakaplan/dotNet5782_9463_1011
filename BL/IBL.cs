using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        //הוספה
        void AddStation(Station station);
        void AddStation(Drone station);
        void AddCustomer(Customer station);
        void AddParcelToDelivery(Parcel parcel);//*מה הוא מקבל
        //עדכון
        void UpdateDroneModel(int id, string model);
        void UpdateStation(int id, string name, int cargeSlots);//לשאול אנשים
        void UpdateCustomer(int id, string[] args /*name and phone*/);//לשאול אנשים
        void SendDroneToCharge(int droneId);
        void ReleaseDroneFromeCharge(int droneId, DateTime timeInCharge);
        void AssignParcelToDrone(int parcelId, int droneId);
        void AssignParcelToDrone(int droneId);
        void PickParcelByDrone(int parcelId);
        void DeliverParcelByDrone(int droneId);
        //תצוגה
        Customer DisplayCustomer(int customerId);
        Drone DisplayDrone(int droneId);
        Parcel DisplayParcel(int parcelId);
        Station DisplayStation(int stationId);
        IEnumerable<StationToList> DisplayListOfStations();
        IEnumerable<DroneToList> DisplayListOfDrones();
        IEnumerable<CustomerToList> DisplayListOfCustomers();
        IEnumerable<ParcelToList> DisplayListOfParcels();
        IEnumerable<ParcelToList> DisplayListOfUnassignedParcels();
        IEnumerable<StationToList> DisplayListOfStationsWithAvailableCargeSlots();
    }
}
