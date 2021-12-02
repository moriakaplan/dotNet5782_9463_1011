using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL
    {
        public void AddParcelToDelivery(Parcel parcel)
        {
            IDAL.DO.Parcel idalParcel = new IDAL.DO.Parcel
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
            };
            try
            {
                dl.AddParcelToTheList(idalParcel);
                //add the new Parcel to the list in the data level
            }
            catch (IDAL.DO.ParcelException ex)
            {
                throw new ExistIdException(ex.Message, "-parcel");
            }
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
        private Parcel findClosestParcel(int droneId)
        {
            //List<Parcel> parcelLst = (List<Parcel>)findHighesWeight(DisplayDrone(droneId).MaxWeight);
            Parcel result = findHighesWeight(DisplayDrone(droneId).MaxWeight).First();
            Location DroneLocation = new Location { Latti = DisplayDrone(droneId).CurrentLocation.Latti, Longi = DisplayDrone(droneId).CurrentLocation.Longi };
            double minDistance = dl.Distance(DisplayCustomer(result.Sender.Id).Location.Latti, DisplayCustomer(result.Sender.Id).Location.Longi, DroneLocation.Latti, DroneLocation.Longi);
            foreach (Parcel parcel in findHighesWeight(DisplayDrone(droneId).MaxWeight))
            {
                if (dl.Distance(DisplayCustomer(parcel.Sender.Id).Location.Latti, DisplayCustomer(parcel.Sender.Id).Location.Longi, DroneLocation.Latti, DroneLocation.Longi) < minDistance)
                {
                    minDistance = dl.Distance(DisplayCustomer(parcel.Sender.Id).Location.Latti, DisplayCustomer(parcel.Sender.Id).Location.Longi, DroneLocation.Latti, DroneLocation.Longi);
                    result = parcel;
                }
            }
            return result;
        }

        public void AssignParcelToDrone(int droneId)
        {
            Drone bdrone;
            bdrone = DisplayDrone(droneId);
            if (bdrone.Status == DroneStatus.Available)
            {
                throw new DroneCantTakeParcelExeption($"drone {droneId} is not available so it cant take a new parcel");
            }
            Parcel parcel = findClosestParcel(droneId);//####מצאנו את הרחפן המתאים, צריך למצוא אם הסוללה מתאימה 
            Location locOfCus = DisplayCustomer(parcel.Sender.Id).Location;
            if (bdrone.Battery <= (minBattery(droneId, bdrone.CurrentLocation, locOfCus) + minBattery(droneId, locOfCus, closestStation(locOfCus))))
            {
                try { dl.AssignParcelToDrone(parcel.Id, droneId); }
                catch (IDAL.DO.DroneException ex)
                {
                    throw new NotExistIDExeption(ex.Message, " - drone");
                }
                catch (IDAL.DO.ParcelException ex)
                {
                    throw new NotExistIDExeption(ex.Message, " - parcel");
                }
                foreach (DroneToList drone in lstdrn)
                {
                    if (drone.Id == droneId)
                    {
                        drone.Status = DroneStatus.Associated;
                    }
                }
            }
        }
        public void PickParcelByDrone(int droneId)
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
        public void DeliverParcelByDrone(int droneId)
        {
            if ((DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).PickedUp != DateTime.MinValue) && (DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).Delivered == DateTime.MinValue))
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
            IDAL.DO.Parcel parcelFromDal;
            Drone droneFromFunc;
            try
            {
                parcelFromDal = dl.DisplayParcel(parcelId);
                droneFromFunc = DisplayDrone(parcelFromDal.Droneld);
            }
            catch (IDAL.DO.ParcelException ex)
            {
                throw new NotExistIDExeption(ex.Message, "- parcel");
            }
            catch (IDAL.DO.DroneException ex)
            {
                throw new NotExistIDExeption(ex.Message, "- drone");
            }

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
            //List<ParcelToList> answer = new List<ParcelToList>();
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
                ParcelToList answer = new ParcelToList
                {
                    Id = parcel.Id,
                    Priority = (Priorities)parcel.Priority,
                    SenderName = DisplayCustomer(parcel.Senderld).Name,
                    TargetName = DisplayCustomer(parcel.TargetId).Name,
                    Status = st,
                    Weight = (WeightCategories)parcel.Weight
                };
                yield return answer;
            }
            //return answer;
        }
        public IEnumerable<ParcelToList> DisplayListOfUnassignedParcels()
        {
            //List<ParcelToList> answer = new List<ParcelToList>();
            IEnumerable<IDAL.DO.Parcel> listFromDal = dl.DisplayListOfParcels();
            ParcelStatus st;
            foreach (IDAL.DO.Parcel parcel in listFromDal)
            {

                if (parcel.Scheduled == DateTime.MinValue) st = ParcelStatus.Created;
                else st = ParcelStatus.Associated;
                if (st == ParcelStatus.Created)
                {
                    ParcelToList answer = new ParcelToList
                    {
                        Id = parcel.Id,
                        Priority = (Priorities)parcel.Priority,
                        SenderName = DisplayCustomer(parcel.Senderld).Name,
                        TargetName = DisplayCustomer(parcel.TargetId).Name,
                        Status = st,
                        Weight = (WeightCategories)parcel.Weight
                    };
                    yield return answer;
                }
            }
            //return answer;
        }

    }
}
