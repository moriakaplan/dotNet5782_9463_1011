using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject
    {
        public void AddStationToTheList(Station st)
        {
            DataSource.stations.Add(st);
        }
        public void AddDroneToTheList(Drone dr)
        {
            DataSource.drones.Add(dr);
        }
        public void AddCustomerToTheList(Customer cu)
        {
            DataSource.customers.Add(cu);
        }
        public void AddParcelToTheList(Parcel pa)
        {
            DataSource.parcels.Add(pa);
        }
        public void AssignParcelToDrone(int parcelId, int droneId)//שיוך חבילה לרחפן
        {
            //int index = 0;
            Parcel temp=new Parcel();
            foreach (Parcel item in DataSource.parcels)
            {
                //index++;
                if (item.Id == parcelId)
                {
                    DataSource.parcels.Add(new Parcel
                    {
                        Id = item.Id,
                        Delivered = item.Delivered,
                        Droneld = droneId,
                        PickedUp = item.PickedUp,
                        Priority = item.Priority,
                        Requested = item.Requested,
                        Scheduled = item.Scheduled,
                        Senderld = item.Senderld,
                        TargetId = item.TargetId,
                        Weight = item.Weight
                    });
                    temp = item;
                }
            }
            DataSource.parcels.Remove(temp);
        }
        public void PickParcelByDrone(int parcelId)
        {

        }
        public void DeliverParcelToCustomer(int parcelId)
        {

        }
        public void SendDroneToCharge(int DroneId, int stationId)
        {

        }
        public void ReleaseDroneFromeCharge(int droneId)
        {

        }
        public Station DisplayStation(int stationId)
        {

        }
        public Drone DisplayDrone(int droneIdId)
        {

        }
        public Customer DisplayCustomer(int customerId)
        {

        }
        public Parcel DisplayParcel(int parcelId)
        {

        }
        public List<Station> DisplayListOfStations()
        {

        }
        public List<Drone> DisplayListOfDrones()
        {

        }
        public List<Customer> DisplayListOfCustomers()
        {

        }
        public List<Parcel> DisplayListOfParcels()
        {

        }
        public List<Parcel> DisplayListOfUnassignedParcels()
        {

        }
        public List<Station> DisplayListOfStationsWithAvailableCargingSlots()
        {

        }



    }

}
