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
        public void AddParcelToDelivery(Parcel parcel)
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

        /// <summary>
        /// מחזיר את כל החבילות עם העדיפות הכי גבוהה
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Parcel> findHighesPrioritiy()
        {
            //List<ParcelInTransfer> result = new List<ParcelInTransfer>(null);
            Priorities temp = Priorities.Regular;
            foreach (ParcelToList parcelList in DisplayListOfParcels())
            {
                if (parcelList.Priority > temp)
                {
                    temp = parcelList.Priority;
                }
            }
            Parcel parcel = new Parcel();
            foreach (ParcelToList parcelList in DisplayListOfParcels())
            {
                if (parcelList.Priority == temp)
                {
                    yield return DisplayParcel(parcelList.Id);

                }
            }
        }
        private IEnumerable<Parcel> findHighesWeight(WeightCategories weight)
        {

            WeightCategories temp = WeightCategories.Easy;
            foreach (Parcel parcel in findHighesPrioritiy())//מוצא את המשקל הכי גדול שהרחפן יכולה לקחת שיש חבילות במשקל הזה
            {
                if ((parcel.Weight < weight) && (parcel.Weight > temp))
                {
                    temp = parcel.Weight;
                }
            }
            foreach (Parcel parcel in findHighesPrioritiy())
            {
                if (parcel.Weight == temp)
                {
                    yield return DisplayParcel(parcel.Id);

                }
            }
        }
        private Parcel findClosestPacel(int droneId)
        {
            List<Parcel> parcelLst = (List<Parcel>)findHighesWeight(DisplayDrone(droneId).MaxWeight);
            Parcel result = parcelLst[0];
            Location DroneLocation = new Location { Latti = DisplayDrone(droneId).CurrentLocation.Latti, Longi = DisplayDrone(droneId).CurrentLocation.Longi };
            double minDistance = dl.Distance(DisplayCustomer(parcelLst[0].Sender.Id).Location.Latti, DisplayCustomer(parcelLst[0].Sender.Id).Location.Longi, DroneLocation.Latti, DroneLocation.Longi);
            foreach (Parcel parcel in parcelLst)
            {
                if (dl.Distance(DisplayCustomer(parcel.Sender.Id).Location.Latti, DisplayCustomer(parcel.Sender.Id).Location.Longi, DroneLocation.Latti, DroneLocation.Longi) < minDistance)
                {
                    minDistance = dl.Distance(DisplayCustomer(parcel.Sender.Id).Location.Latti, DisplayCustomer(parcel.Sender.Id).Location.Longi, DroneLocation.Latti, DroneLocation.Longi);
                    result = parcel;
                }
            }
            return result;
        }

        public void AssignParcelToDrone(int droneId)//איפה הוא צריך להיות
        {
            Drone bdrone = DisplayDrone(droneId);
            if (bdrone.Status == DroneStatus.Available)
            {
                Parcel parcel = findClosestPacel(droneId);//מצאנו את הרחפן המתאים, צריך למצוא אם הסוללה מתאימה 
                if (DisplayDrone(droneId).Battery <= (minBattery(droneId, DisplayDrone(droneId).CurrentLocation, DisplayCustomer(DisplayParcel(parcel.Id).Sender.Id).Location) + minBattery(droneId, DisplayCustomer(DisplayParcel(parcel.Id).Sender.Id).Location, closestStation(DisplayCustomer(DisplayParcel(parcel.Id).Sender.Id).Location))))
                {
                    dl.AssignParcelToDrone(parcel.Id, droneId);
                    foreach (DroneToList drone in lstdrn)
                    {
                        if (drone.Id == droneId)
                        {
                            drone.Status = DroneStatus.Associated;
                        }
                    }
                }

            }
        }
        public void PickParcelByDrone(int droneId)//איפה הוא צריך להיות
        {
            if ((DisplayDrone(droneId).Status == DroneStatus.Associated) && (DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).PickedUp == DateTime.MinValue))
            {
                foreach (DroneToList drone in lstdrn)
                {
                    if (drone.Id == droneId)
                    {
                        drone.Battery = drone.Battery - minBattery(droneId, DisplayCustomer(DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).Sender.Id).Location, DisplayCustomer(DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).Target.Id).Location);
                        drone.CurrentLocation = DisplayCustomer(DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).Target.Id).Location;

                        dl.PickParcelByDrone(DisplayDrone(droneId).ParcelInT.Id);//מעדכן את הזמן של האיסוף בדאל
                    }
                }
            }
            else
            {//######
                //לזרוק חריגה!!!!!!!!
                //כתוב בהוראות
            }
        }
        public void DeliverParcelByDrone(int droneId)//איפה הוא צריך להיות
        {
            if((DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).PickedUp != DateTime.MinValue)&& (DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).Delivered == DateTime.MinValue))
            {
                foreach (DroneToList drone in lstdrn)
                {
                    if (drone.Id == droneId)
                    {
                        //drone.Battery = drone.Battery - minBattery(droneId, DisplayCustomer(DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).Sender.Id).Location, DisplayCustomer(DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).Target.Id).Location);
                        //drone.CurrentLocation = DisplayCustomer(DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).Target.Id).Location;
                        //לעדכן את הסוללה שוב?
                        //לעדכן את המיקום שוב?
                        drone.Status = DroneStatus.Available;
                        dl.DeliverParcelToCustomer(DisplayDrone(droneId).ParcelInT.Id);//מעדכן את הזמן של האיסוף בדאל
                    }
                }
                

            }
            else
            {
                //######
                //לזרוק חריגה!!!!!!!!
                //כתוב בהוראות
            }
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
