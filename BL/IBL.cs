using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public interface Ibl
    {
        //הוספה
        void AddStation(int id, string name, Location loc, int chargeSlots);
        void AddDrone(int id, string model, WeightCategories weight, int stationId);
        void AddCustomer(int id, string name, string phone, Location loc);
        void AddParcelToDelivery(int senderId, int targetId, WeightCategories weight, Priorities pri);//*מה הוא מקבל
         //עדכון
        void UpdateDroneModel(int id, string model);
        void UpdateStation(int id, string name, int cargeSlots);//לשאול אנשים
        void UpdateCustomer(int id, string name, string phone);//לשאול אנשים
        void SendDroneToCharge(int droneId);
        void ReleaseDroneFromeCharge(int droneId, DateTime timeInCharge);
        void AssignParcelToDrone(int droneId);//איפה הוא צריך להיות
        void PickParcelByDrone(int parcelId);//איפה הוא צריך להיות
        void DeliverParcelByDrone(int droneId);//איפה הוא צריך להיות
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
