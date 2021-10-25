using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public static class DalObject
    {
        public static void AddStationToTheList(Station st)
        {
            DataSource.stations.Add(st);
        }
        public static void AddDroneToTheList(Drone dr)
        {
            DataSource.drones.Add(dr);
        }
        public static void AddCustomerToTheList(Customer cu)
        {
            DataSource.customers.Add(cu);
        }
        public static int AddParcelToTheList(Parcel pa)
        {
            //לא ברור מה עושים עם המספר הרץ
            DataSource.parcels.Add(pa);
            DataSource.Config.parcelCode++;
            return DataSource.Config.parcelCode;
        }
        public static void AssignParcelToDrone(int parcelId, int droneId)//שיוך חבילה לרחפן
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
        public static void PickParcelByDrone(int parcelId)
        {
            Parcel parcel = DisplayParcel(parcelId);
            DataSource.parcels.Remove(parcel);
            parcel.PickedUp = DateTime.Now;
            AddParcelToTheList(parcel);

            Drone drone = DisplayDrone(parcel.Droneld); // פה לא בטוח צריך
            DataSource.drones.Remove(drone);
            drone.Status = DroneStatuses.Sending;
            AddDroneToTheList(drone);
        }
        public static void DeliverParcelToCustomer(int parcelId)
        {
            Parcel parcel = DisplayParcel(parcelId);
            DataSource.parcels.Remove(parcel);
            parcel.Delivered = DateTime.Now;
            AddParcelToTheList(parcel);

            Drone drone = DisplayDrone(parcel.Droneld);
            DataSource.drones.Remove(drone);
            drone.Status = DroneStatuses.Vacant;
            AddDroneToTheList(drone);
        }
        public static void SendDroneToCharge(int droneId, int stationId)
        {
            DroneCharge droneCharge = new DroneCharge { DroneId = droneId, StationId = stationId };
            DataSource.droneCharges.Add(droneCharge);

            Drone drone = DisplayDrone(droneId);
            DataSource.drones.Remove(drone);
            drone.Status = DroneStatuses.Maintenance;
            AddDroneToTheList(drone);

            Station station = DisplayStation(stationId);
            DataSource.stations.Remove(station);
            station.ChargeSlots++;
            AddStationToTheList(station);
        }
        public static void ReleaseDroneFromeCharge(int droneId)
        {
            DroneCharge dCharge = DataSource.droneCharges.Find(x => x.DroneId == droneId);
            //DroneCharge dCharge = new DroneCharge();
            //foreach (DroneCharge item in DataSource.droneCharges)
            //{
            //    if (item.DroneId == droneId)
            //        dCharge = item;
            //}
            DataSource.droneCharges.Remove(dCharge);

            Drone drone = DisplayDrone(droneId);
            DataSource.drones.Remove(drone);
            drone.Status = DroneStatuses.Vacant;
            AddDroneToTheList(drone);

            Station station = DisplayStation(dCharge.StationId);
            DataSource.stations.Remove(station);
            station.ChargeSlots--;
            AddStationToTheList(station);
        }
        public static Station DisplayStation(int stationId)
        {
            Station temp = new Station();
            foreach (Station item in DataSource.stations)
            {
                if (item.Id == stationId)
                    temp = item;
            }
            return temp;
        }
        public static Drone DisplayDrone(int droneId)
        {
            Drone temp = new Drone();
            foreach (Drone item in DataSource.drones)
            {
                if (item.Id == droneId)
                    temp = item;
            }
            return temp;
        }
        public static Customer DisplayCustomer(int customerId)
        {
            Customer temp = new Customer();
            foreach (Customer item in DataSource.customers)
            {
                if (item.Id == customerId)
                    temp = item;
            }
            return temp;
        }
        public static Parcel DisplayParcel(int parcelId)
        {
            Parcel temp = new Parcel();
            foreach (Parcel item in DataSource.parcels)
            {
                if (item.Id == parcelId)
                    temp = item;
            }
            return temp;
        }
        public static List<Station> DisplayListOfStations()
        {
            return DataSource.stations;
        }
        public static List<Drone> DisplayListOfDrones()
        {
            return DataSource.drones;
        }
        public static List<Customer> DisplayListOfCustomers()
        {
            return DataSource.customers;
        }
        public static List<Parcel> DisplayListOfParcels()
        {
            return DataSource.parcels;
        }
        public static List<Parcel> DisplayListOfUnassignedParcels()
        {
            List<Parcel> unassignedParcels = new List<Parcel>();
            foreach (Parcel item in DataSource.parcels)
            {
                if (item.Scheduled == DateTime.MinValue)
                    unassignedParcels.Add(item);
            }
            return unassignedParcels;
        }
        public static List<Station> DisplayListOfStationsWithAvailableCargingSlots()
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

//using IDAL.DO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DalObject
//{
//    public class DalObject
//    {
//        public void AddStationToTheList(Station st)
//        {
//            DataSource.stations.Add(st);
//        }
//        public void AddDroneToTheList(Drone dr)
//        {
//            DataSource.drones.Add(dr);
//        }
//        public void AddCustomerToTheList(Customer cu)
//        {
//            DataSource.customers.Add(cu);
//        }
//        public int AddParcelToTheList(Parcel pa)
//        {
//            //לא ברור מה עושים עם המספר הרץ
//            DataSource.parcels.Add(pa);
//            DataSource.Config.parcelCode++;
//            return DataSource.Config.parcelCode;
//        }
//        public void AssignParcelToDrone(int parcelId, int droneId)//שיוך חבילה לרחפן
//        {
//            //int index = 0;
//            //Parcel temp=new Parcel();
//            //foreach (Parcel item in DataSource.parcels)
//            //{
//            //    //index++;
//            //    if (item.Id == parcelId)
//            //    {
//            //        DataSource.parcels.Add(new Parcel
//            //        {
//            //            Id = item.Id,
//            //            Delivered = item.Delivered,
//            //            Droneld = droneId,
//            //            PickedUp = item.PickedUp,
//            //            Priority = item.Priority,
//            //            Requested = item.Requested,
//            //            Scheduled = item.Scheduled,
//            //            Senderld = item.Senderld,
//            //            TargetId = item.TargetId,
//            //            Weight = item.Weight
//            //        });
//            //        temp = item;
//            //    }
//            //}
//            //DataSource.parcels.Remove(temp);

//            //Parcel parcel = DataSource.parcels.Find(x=>x.Id==parcelId); //option a
//            Parcel parcel = DisplayParcel(parcelId); //option b
//            DataSource.parcels.Remove(parcel);
//            DataSource.Config.parcelCode--;
//            parcel.Droneld = droneId;
//            parcel.Scheduled = DateTime.Now;
//            AddParcelToTheList(parcel);
//        }
//        public void PickParcelByDrone(int parcelId)
//        {
//            Parcel parcel = DisplayParcel(parcelId); 
//            DataSource.parcels.Remove(parcel);
//            parcel.PickedUp = DateTime.Now;
//            AddParcelToTheList(parcel);

//            Drone drone = DisplayDrone(parcel.Droneld); // פה לא בטוח צריך
//            DataSource.drones.Remove(drone);
//            drone.Status = DroneStatuses.Sending;
//            AddDroneToTheList(drone);
//        }
//        public void DeliverParcelToCustomer(int parcelId)
//        {
//            Parcel parcel = DisplayParcel(parcelId);
//            DataSource.parcels.Remove(parcel);
//            parcel.Delivered = DateTime.Now;
//            AddParcelToTheList(parcel);

//            Drone drone = DisplayDrone(parcel.Droneld); 
//            DataSource.drones.Remove(drone);
//            drone.Status = DroneStatuses.Vacant;
//            AddDroneToTheList(drone);
//        }
//        public void SendDroneToCharge(int droneId, int stationId)
//        {
//            DroneCharge droneCharge = new DroneCharge{ DroneId = droneId, StationId = stationId };
//            DataSource.droneCharges.Add(droneCharge);

//            Drone drone = DisplayDrone(droneId);
//            DataSource.drones.Remove(drone);
//            drone.Status = DroneStatuses.Maintenance;
//            AddDroneToTheList(drone);

//            Station station = DisplayStation(stationId);
//            DataSource.stations.Remove(station);
//            station.ChargeSlots++;
//            AddStationToTheList(station);
//        }
//        public void ReleaseDroneFromeCharge(int droneId)
//        {
//            DroneCharge dCharge = DataSource.droneCharges.Find(x => x.DroneId == droneId);
//            //DroneCharge dCharge = new DroneCharge();
//            //foreach (DroneCharge item in DataSource.droneCharges)
//            //{
//            //    if (item.DroneId == droneId)
//            //        dCharge = item;
//            //}
//            DataSource.droneCharges.Remove(dCharge);

//            Drone drone = DisplayDrone(droneId);
//            DataSource.drones.Remove(drone);
//            drone.Status = DroneStatuses.Vacant;
//            AddDroneToTheList(drone);

//            Station station = DisplayStation(dCharge.StationId);
//            DataSource.stations.Remove(station);
//            station.ChargeSlots--;
//            AddStationToTheList(station);
//        }
//        public Station DisplayStation(int stationId)
//        {
//            Station temp= new Station();
//            foreach (Station item in DataSource.stations)
//            {
//                if (item.Id == stationId)
//                    temp = item;
//            }
//            return temp;
//        }
//        public Drone DisplayDrone(int droneId)
//        {
//            Drone temp = new Drone();
//            foreach (Drone item in DataSource.drones)
//            {
//                if (item.Id == droneId)
//                    temp = item;
//            }
//            return temp;
//        }
//        public Customer DisplayCustomer(int customerId)
//        {
//            Customer temp = new Customer();
//            foreach (Customer item in DataSource.customers)
//            {
//                if (item.Id == customerId)
//                    temp = item;
//            }
//            return temp;
//        }
//        public Parcel DisplayParcel(int parcelId)
//        {
//            Parcel temp = new Parcel();
//            foreach (Parcel item in DataSource.parcels)
//            {
//                if (item.Id == parcelId)
//                    temp = item;
//            }
//            return temp;
//        }
//        public List<Station> DisplayListOfStations()
//        {
//            return DataSource.stations;
//        }
//        public List<Drone> DisplayListOfDrones()
//        {
//            return DataSource.drones;
//        }
//        public List<Customer> DisplayListOfCustomers()
//        {
//            return DataSource.customers;
//        }
//        public List<Parcel> DisplayListOfParcels()
//        {
//            return DataSource.parcels;
//        }
//        public List<Parcel> DisplayListOfUnassignedParcels()
//        {
//            List<Parcel> unassignedParcels = new List<Parcel>();
//            foreach (Parcel item in DataSource.parcels)
//            {
//                if (item.Scheduled == DateTime.MinValue)
//                    unassignedParcels.Add(item);
//            }
//            return unassignedParcels;
//        }
//        public List<Station> DisplayListOfStationsWithAvailableCargingSlots()
//        {
//            List<Station> StationsWithAvailableCargingSlots = new List<Station>();
//            foreach (Station item in DataSource.stations)
//            {
//                if (item.ChargeSlots > 0)
//                    StationsWithAvailableCargingSlots.Add(item);
//            }
//            return StationsWithAvailableCargingSlots;
//        }
//    }
//}
