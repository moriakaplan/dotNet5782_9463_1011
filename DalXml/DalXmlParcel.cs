using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DO;
using System.Runtime.CompilerServices;


namespace Dal
{
    public partial class DalXml
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddParcel(Parcel parcel)
        {
            List<Parcel> parcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            if (parcels.Exists(item => item.Id == parcel.Id)) throw new ParcelException($"id: {parcel.Id} already exist"); //it suppose to be this type of exception????**** 
            int newCode = int.Parse(configRoot.Element("parcelCode").Value) + 1;
            configRoot.Element("parcelCode").Value = newCode.ToString();
            configRoot.Save(configPath);
            parcel.Id = newCode;
            parcels.Add(parcel);
            XmlTools.SaveListToXmlSerializer<Parcel>(parcels, parcelsPath);
            return newCode;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int parcelId)
        {
            List<Parcel> parcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel? result = parcels.Find(x => x.Id == parcelId);
            if (result == null)
                throw new ParcelException($"id: {parcelId} does not exist");
            return (Parcel)result;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcelsList(Predicate<Parcel> pre)
        {
            List<Parcel> parcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            List<Parcel> result = new List<Parcel>(parcels);
            if (pre == null) return result;
            return result.FindAll(pre);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int parcelId)
        {
            List<Parcel> parcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            try
            {
                parcels.Remove(GetParcel(parcelId));
            }
            catch (ArgumentNullException)
            {
                throw new ParcelException($"id: {parcelId} does not exist");
            }
            XmlTools.SaveListToXmlSerializer<Parcel>(parcels, parcelsPath);

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AssignParcelToDrone(int parcelId, int droneId)
        {
            List<Drone> drones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            if (!drones.Exists(item => item.Id == droneId))
            {
                throw new DroneException($"id: {droneId} does not exist");
            }
            List<Parcel> parcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            for (int i = 0; i < parcels.Count; i++) //find the parcel and update its details
            {
                if (parcels[i].Id == parcelId)
                {
                    Parcel p = parcels[i];
                    p.Droneld = droneId;//assign the parcel to the drone
                    p.AssociateTime = DateTime.Now;//update the time that the parcel was scheduled
                    parcels[i] = p;
                    XmlTools.SaveListToXmlSerializer<Drone>(drones, dronesPath);
                    XmlTools.SaveListToXmlSerializer<Parcel>(parcels, parcelsPath); 
                    return;
                }
            }
            throw new ParcelException($"id: {parcelId} does not exist");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickParcelByDrone(int parcelId)
        {
            List<Parcel> parcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            for (int i = 0; i < parcels.Count; i++) //find the parcel and update its details
            {
                if (parcels[i].Id == parcelId)
                {
                    Parcel p = parcels[i];
                    p.PickUpTime = DateTime.Now;//update the time that the drone pick up the parcel
                    parcels[i] = p;
                    XmlTools.SaveListToXmlSerializer<Parcel>(parcels, parcelsPath);
                    return;
                }
            }
            throw new ParcelException($"id: {parcelId} does not exist");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliverParcelToCustomer(int parcelId)
        {
            List<Parcel> parcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            for (int i = 0; i < parcels.Count; i++) //find the parcel and update its details
            {
                if (parcels[i].Id == parcelId)
                {
                    Parcel p = parcels[i];
                    p.DeliverTime = DateTime.Now;//update the time that the drone pick up the parcel
                    parcels[i] = p;
                    XmlTools.SaveListToXmlSerializer<Parcel>(parcels, parcelsPath);
                    return;
                }
            }
            throw new ParcelException($"id: {parcelId} does not exist");
        }
    }
}