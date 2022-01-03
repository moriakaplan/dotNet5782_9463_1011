using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    public partial class DalXml
    {
        public int AddParcelToTheList(Parcel parcel)
        {
            //if (DataSource.parcels.Exists(item => item.Id == parcel.Id)) throw new ParcelException($"id: {parcel.Id} already exist"); //it suppose to be this type of exception????**** 
            //parcel.Id = ++DataSource.Config.parcelCode;
            //DataSource.parcels.Add(parcel);
            //return DataSource.Config.parcelCode;
        }
        public Parcel DisplayParcel(int parcelId)
        {
            //Parcel? result = DataSource.parcels.Find(x => x.Id == parcelId);
            //if (result == null)
            //    throw new ParcelException($"id: {parcelId} does not exist");
            //return (Parcel)result;
        }
        public IEnumerable<Parcel> DisplayListOfParcels(Predicate<Parcel> pre)
        {
            //List<Parcel> result = new List<Parcel>(DataSource.parcels);
            //if (pre == null) return result;
            //return result.FindAll(pre);
        }
        public void DeleteParcel(int parcelId)
        {
            //try
            //{
            //    DataSource.drones.Remove(DisplayDrone(parcelId));
            //}
            //catch (ArgumentNullException)
            //{
            //    throw new ParcelException($"id: {parcelId} does not exist");
            //}
        }
        public void AssignParcelToDrone(int parcelId, int droneId)
        {
            //if (!DataSource.drones.Exists(item => item.Id == droneId))
            //{
            //    throw new DroneException($"id: {droneId} does not exist");
            //}
            //for (int i = 0; i < DataSource.parcels.Count; i++) //find the parcel and update its details
            //{
            //    if (DataSource.parcels[i].Id == parcelId)
            //    {
            //        Parcel p = DataSource.parcels[i];
            //        p.Droneld = droneId;//assign the parcel to the drone
            //        p.AssociateTime = DateTime.Now;//update the time that the parcel was scheduled
            //        DataSource.parcels[i] = p;
            //        return;
            //    }
            //}
            //throw new ParcelException($"id: {parcelId} does not exist");
        }
        public void PickParcelByDrone(int parcelId)
        {
            //for (int i = 0; i < DataSource.parcels.Count; i++) //find the parcel and update its details
            //{
            //    if (DataSource.parcels[i].Id == parcelId)
            //    {
            //        Parcel p = DataSource.parcels[i];
            //        p.PickUpTime = DateTime.Now;//update the time that the drone pick up the parcel
            //        DataSource.parcels[i] = p;
            //        return;
            //    }
            //}
            //throw new ParcelException($"id: {parcelId} does not exist");
        }
        public void DeliverParcelToCustomer(int parcelId)
        {
            //for (int i = 0; i < DataSource.parcels.Count; i++) //find the parcel and update its details
            //{
            //    if (DataSource.parcels[i].Id == parcelId)
            //    {
            //        Parcel p = DataSource.parcels[i];
            //        p.DeliverTime = DateTime.Now;//update the time that the drone pick up the parcel
            //        DataSource.parcels[i] = p;
            //        return;
            //    }
            //}
            //throw new ParcelException($"id: {parcelId} does not exist");
        }
    }
}