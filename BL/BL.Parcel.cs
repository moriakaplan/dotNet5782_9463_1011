using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace IBL
{
    public partial class BL
    {
        IDal dalObject = new DalObject.DalObject();
        void AddParcelToDelivery(Parcel parcel)//*מה הוא מקבל
        {
            IDAL.DO.Parcel pa=new IDAL.DO.Parcel { Id=parcel.Id, Delivered=parcel.Delivered, Droneld=parcel.Drone.Id, }
            dalObject.AddParcelToTheList(pa);
        }
        void AssignParcelToDrone(int parcelId, int droneId)//איפה הוא צריך להיות
        {

        }
        void AssignParcelToDrone(int droneId)//איפה הוא צריך להיות
        {

        }
        void PickParcelByDrone(int parcelId)//איפה הוא צריך להיות
        {

        }
        void DeliverParcelByDrone(int droneId)//איפה הוא צריך להיות
        {

        }
        Parcel DisplayParcel(int parcelId)
        {
            return ;
        }
        IEnumerable<ParcelToList> DisplayListOfParcels()
        {
            return null;
        }
        IEnumerable<ParcelToList> DisplayListOfUnassignedParcels()
        {
            return null;
        }

    }
}
