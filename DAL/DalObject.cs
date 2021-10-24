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
            DataSource.Config.parcelCode++;
        }
        public void AssignParcelToDrone(int parcelId, int droneId)//שיוך חבילה לרחפן
        {
            //int index = 0;
            //Parcel temp=new Parcel();
            //foreach (Parcel item in DataSource.parcels)
            //{
            //    //index++;
            //    if (item.Id == parcelId)
            //    {
            //        DataSource.parcels.Add(new Parcel
            //        {
            //            Id = item.Id,
            //            Delivered = item.Delivered,
            //            Droneld = droneId,
            //            PickedUp = item.PickedUp,
            //            Priority = item.Priority,
            //            Requested = item.Requested,
            //            Scheduled = item.Scheduled,
            //            Senderld = item.Senderld,
            //            TargetId = item.TargetId,
            //            Weight = item.Weight
            //        });
            //        temp = item;
            //    }
            //}
            //DataSource.parcels.Remove(temp);

            //Parcel parcel = DataSource.parcels.Find(x=>x.Id==parcelId); //option a
            Parcel parcel = DisplayParcel(parcelId); //option b
            DataSource.parcels.Remove(parcel);
            DataSource.Config.parcelCode--;
            parcel.Droneld = droneId;
            parcel.Scheduled = DateTime.Now;
            AddParcelToTheList(parcel);
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
            Station temp= new Station();
            foreach (Station item in DataSource.stations)
            {
                if (item.Id == stationId)
                    temp = item;
            }
            return temp;
        }
        public Drone DisplayDrone(int droneId)
        {
            Drone temp = new Drone();
            foreach (Drone item in DataSource.drones)
            {
                if (item.Id == droneId)
                    temp = item;
            }
            return temp;
        }
        public Customer DisplayCustomer(int customerId)
        {
            Customer temp = new Customer();
            foreach (Customer item in DataSource.customers)
            {
                if (item.Id == customerId)
                    temp = item;
            }
            return temp;
        }
        public Parcel DisplayParcel(int parcelId)
        {
            Parcel temp = new Parcel();
            foreach (Parcel item in DataSource.parcels)
            {
                if (item.Id == parcelId)
                    temp = item;
            }
            return temp;
        }
        public List<Station> DisplayListOfStations()
        {
            return DataSource.stations;
        }
        public List<Drone> DisplayListOfDrones()
        {
            return DataSource.drones;
        }
        public List<Customer> DisplayListOfCustomers()
        {
            return DataSource.customers;
        }
        public List<Parcel> DisplayListOfParcels()
        {
            return DataSource.parcels;
        }
        public List<Parcel> DisplayListOfUnassignedParcels()
        {
            List<Parcel> unassignedParcels = new List<Parcel>();
            foreach (Parcel item in DataSource.parcels)
            {
                if (item.Scheduled == DateTime.MinValue)
                    unassignedParcels.Add(item);
            }
            return unassignedParcels;
        }
        public List<Station> DisplayListOfStationsWithAvailableCargingSlots()
        {
            List<Station> StationsWithAvailableCargingSlots = new List<Station>();
            foreach (Station item in DataSource.stations)
            {
                if (item.ChargeSlots > 0)
                    StationsWithAvailableCargingSlots.Add(item);
            }
            return StationsWithAvailableCargingSlots;
        }



    }

}
