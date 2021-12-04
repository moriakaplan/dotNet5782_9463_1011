using IDAL.DO;
using IDAL;
using System;
using System.Collections.Generic;
namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// add the parcel that he gets to the list of the parcels.
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public int AddParcelToTheList(Parcel parcel)
        {
            if (DataSource.parcels.Exists(item => item.Id == parcel.Id)) throw new ParcelException($"id: {parcel.Id} already exist"); //it suppose to be this type of exception????**** 
            parcel.Id = ++DataSource.Config.parcelCode;
            DataSource.parcels.Add(parcel);
            return DataSource.Config.parcelCode;
        }
        /// <summary>
        /// display a parcel
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public Parcel DisplayParcel(int parcelId)
        {
            foreach (Parcel item in DataSource.parcels)
            {
                if (item.Id == parcelId)
                    return item;
            }
            throw new ParcelException($"id: {parcelId} does not exist");

            //Parcel? pa = DataSource.parcels.Find(item => item.Id == parcelId);
            //if(pa==null) throw new ParcelException($"id: {parcelId} does not exist");
            //return (Parcel)pa;
        }
        /// <summary>
        /// display the list of the customers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> DisplayListOfParcels()
        {
            List<Parcel> result = new List<Parcel>(DataSource.parcels);
            return result;
        }
        /// <summary>
        /// display the list of the unassign parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> DisplayListOfUnassignedParcels()
        {
            List<Parcel> unassignedParcels = new List<Parcel>();
            foreach (Parcel item in DataSource.parcels)
            {
                if (item.AssociateTime == DateTime.MinValue)
                    unassignedParcels.Add(item);
            }
            return unassignedParcels;
        }
        public void DeleteParcel(int parcelId)
        {
            try
            {
                DataSource.drones.Remove(DisplayDrone(parcelId));
            }
            catch (ArgumentNullException)
            {
                throw new ParcelException($"id: {parcelId} does not exist");
            }
        }
    }
}