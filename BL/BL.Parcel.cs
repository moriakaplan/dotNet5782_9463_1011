using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public partial class BL:IBL.Ibl
    {
        void AddParcelToDelivery(Parcel parcel)//*מה הוא מקבל
        {

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
            return null;
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
