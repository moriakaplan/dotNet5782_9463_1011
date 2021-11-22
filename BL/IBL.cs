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
       public void AddStation(Station station);
       public void AddDrone(Drone drone);
       public void AddCustomer(Customer customer);
       public void AddParcelToDelivery(Parcel parcel);//*מה הוא מקבל
        //עדכון
       public void UpdateDroneModel(int id, string model);
       public void UpdateStation(int id, string name, int cargeSlots);//לשאול אנשים
       public void UpdateCustomer(int id, params string[] args /*name and phone*/);//לשאול אנשים
       public void SendDroneToCharge(int droneId);
       public void ReleaseDroneFromeCharge(int droneId, DateTime timeInCharge);
       public void AssignParcelToDrone(int parcelId, int droneId);//איפה הוא צריך להיות
       public void AssignParcelToDrone(int droneId);//איפה הוא צריך להיות
       public void PickParcelByDrone(int parcelId);//איפה הוא צריך להיות
       public void DeliverParcelByDrone(int droneId);//איפה הוא צריך להיות
        //תצוגה
       public Customer DisplayCustomer(int customerId);
       public Drone DisplayDrone(int droneId);
       public Parcel DisplayParcel(int parcelId);
       public Station DisplayStation(int stationId);
       public IEnumerable<StationToList> DisplayListOfStations();
       public IEnumerable<DroneToList> DisplayListOfDrones();
       public IEnumerable<CustomerToList> DisplayListOfCustomers();
       public IEnumerable<ParcelToList> DisplayListOfParcels();
       public IEnumerable<ParcelToList> DisplayListOfUnassignedParcels();
       public IEnumerable<StationToList> DisplayListOfStationsWithAvailableCargeSlots();
    }
}
