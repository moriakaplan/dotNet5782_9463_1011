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
        //IDal dalObject = new DalObject.DalObject();
        public void AddParcelToDelivery(Parcel parcel)//*מה הוא מקבל
        {
            dl.AddParcelToTheList(new IDAL.DO.Parcel
            {
                Id = parcel.Id,
                Delivered = parcel.Delivered,
                Droneld = parcel.Drone.Id,
                PickedUp = parcel.PickedUp,
                Priority = (IDAL.DO.Priorities)parcel.Priority,
                Requested = parcel.Requested,
                Scheduled = parcel.Scheduled,
                Senderld = parcel.Sender.Id,
                TargetId = parcel.Target.Id,
                Weight = (IDAL.DO.WeightCategories)parcel.Weight
            });
        }
        public void AssignParcelToDrone(int parcelId, int droneId)//איפה הוא צריך להיות
        {

        }
        public void AssignParcelToDrone(int droneId)//איפה הוא צריך להיות
        {

        }
        public void PickParcelByDrone(int parcelId)//איפה הוא צריך להיות
        {

        }
        public void DeliverParcelByDrone(int droneId)//איפה הוא צריך להיות
        {

        }
        public Parcel DisplayParcel(int parcelId)
        {
            IDAL.DO.Parcel parcelFromDal = dl.DisplayParcel(parcelId);
            Drone droneFromFunc = DisplayDrone(parcelFromDal.Droneld);
            DroneInParcel drone = new DroneInParcel
            {
                Id = droneFromFunc.Id,
                Battery = droneFromFunc.Battery,
                CurrentLocation = droneFromFunc.CurrentLocation
            };
            CustomerInParcel sender = new CustomerInParcel
            {
                Id = parcelFromDal.Senderld,
                Name = DisplayCustomer(parcelFromDal.Senderld).Name
            };
            CustomerInParcel target = new CustomerInParcel
            {
                Id = parcelFromDal.TargetId,
                Name = DisplayCustomer(parcelFromDal.TargetId).Name
            };
            return new Parcel
            {
                Id = parcelFromDal.Id,
                Delivered = parcelFromDal.Delivered,
                Drone = drone,
                PickedUp = parcelFromDal.PickedUp,
                Priority = (Priorities)parcelFromDal.Priority,
                Requested = parcelFromDal.Requested,
                Scheduled = parcelFromDal.Scheduled,
                Sender = sender,
                Target = target,
                Weight = (WeightCategories)parcelFromDal.Weight
            };
        }
        public IEnumerable<ParcelToList> DisplayListOfParcels()
        {
            List<ParcelToList> answer = new List<ParcelToList>();
            IEnumerable<IDAL.DO.Parcel> listFromDal = dl.DisplayListOfParcels();
            ParcelStatus st;
            foreach (IDAL.DO.Parcel parcel in listFromDal)
            {
                if (parcel.Delivered != DateTime.MinValue) st = ParcelStatus.Delivered;
                else
                {
                    if (parcel.PickedUp != DateTime.MinValue) st = ParcelStatus.PickedUp;
                    else
                    {
                        if (parcel.Scheduled != DateTime.MinValue) st = ParcelStatus.Associated;
                        else st = ParcelStatus.Created;
                    }
                }
                answer.Add(new ParcelToList
                {
                    Id = parcel.Id,
                    Priority = (Priorities)parcel.Priority,
                    SenderName = DisplayCustomer(parcel.Senderld).Name,
                    TargetName = DisplayCustomer(parcel.TargetId).Name,
                    Status = st,
                    Weight = (WeightCategories)parcel.Weight
                });
            }
            return answer;
        }
        public IEnumerable<ParcelToList> DisplayListOfUnassignedParcels()
        {
            List<ParcelToList> answer = new List<ParcelToList>();
            IEnumerable<IDAL.DO.Parcel> listFromDal = dl.DisplayListOfParcels();
            ParcelStatus st;
            foreach (IDAL.DO.Parcel parcel in listFromDal)
            {

                if (parcel.Scheduled == DateTime.MinValue) st = ParcelStatus.Created;
                else st = ParcelStatus.Associated;
                if (st == ParcelStatus.Created)
                {
                    answer.Add(new ParcelToList
                    {
                        Id = parcel.Id,
                        Priority = (Priorities)parcel.Priority,
                        SenderName = DisplayCustomer(parcel.Senderld).Name,
                        TargetName = DisplayCustomer(parcel.TargetId).Name,
                        Status = st,
                        Weight = (WeightCategories)parcel.Weight
                    });
                }
            }
            return answer;
        }

    }
}
