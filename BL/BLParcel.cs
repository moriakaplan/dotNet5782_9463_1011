﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL
    {
        /// <summary>
        /// Add Parcel To Delivery
        /// </summary>
        /// <param name="parcel"></param>
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
        /// Returns all parcels with the highest priority
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Parcel> findHighestPrioritiy()
        {
            IEnumerable<ParcelToList> parcels = DisplayListOfParcels();
            Priorities temp = Priorities.Regular;
            foreach (ParcelToList parcelList in parcels) //find the highest priority
            {
                if (parcelList.Priority > temp)
                {
                    temp = parcelList.Priority;
                }
            }
            Parcel parcel = new Parcel();
            foreach (ParcelToList parcelList in parcels) //return all the parcels with the highest priority
            {
                if (parcelList.Priority == temp)
                {
                    yield return DisplayParcel(parcelList.Id);
                }
            }
        }
        /// <summary>
        /// Returns all parcels with the highest weight the drone can take
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        private IEnumerable<Parcel> findHighesWeight(WeightCategories weight)
        {
            IEnumerable<Parcel> parcels = findHighestPrioritiy();
            WeightCategories temp = WeightCategories.Easy;
            foreach (Parcel parcel in parcels)//find the highest weight that the drone can take
            {
                if ((parcel.Weight < weight) && (parcel.Weight > temp))
                {
                    temp = parcel.Weight;
                }
            }
            foreach (Parcel parcel in parcels)//return all the parcels with this weight
            {
                if (parcel.Weight == temp)
                {
                    yield return DisplayParcel(parcel.Id);
                }
            }
        }
        /// <summary>
        /// find Closest Pacel
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        private Parcel findClosestPacel(int droneId)
        {
            Parcel result = findHighesWeight(DisplayDrone(droneId).MaxWeight).First();
            Location DroneLocation = new Location { Latti = DisplayDrone(droneId).CurrentLocation.Latti, Longi = DisplayDrone(droneId).CurrentLocation.Longi };
            double minDistance = distance(DisplayCustomer(result.Sender.Id).Location, DroneLocation);
            foreach (Parcel parcel in findHighesWeight(DisplayDrone(droneId).MaxWeight))
            {
                if (distance(DisplayCustomer(parcel.Sender.Id).Location, DroneLocation) < minDistance)
                {
                    minDistance = distance(DisplayCustomer(parcel.Sender.Id).Location, DroneLocation);
                    result = parcel;
                }
            }
            return result;
        }
        /// <summary>
        /// Assign Parcel To Drone
        /// </summary>
        /// <param name="droneId"></param>
        public void AssignParcelToDrone(int droneId)
        {
            Drone bdrone;
            bdrone = DisplayDrone(droneId);
            if (bdrone.Status == DroneStatus.Available)
            {
                throw new DroneCantTakeParcelExeption($"drone {droneId} is not available so it cant take a new parcel");
            }
            Parcel parcel = findClosestParcel(droneId);//####מצאנו את החבילה המתאימה, צריך למצוא אם הסוללה מספיקה 
            Location locOfSender = DisplayCustomer(parcel.Sender.Id).Location;
            Location locOfTarget = DisplayCustomer(parcel.Target.Id).Location;
            double batteryNeeded =
                minBattery(droneId, bdrone.CurrentLocation, locOfSender) +
                minBattery(droneId, locOfSender, locOfTarget) +
                minBattery(droneId, locOfTarget, closestStationWithChargeSlots(locOfTarget).Location);//לבדוק אם צריך את התחנה הכי קרובה עם עמדות טעינה
            if (batteryNeeded > bdrone.Battery)
            {
                throw new ThereNotGoodParcelToTake($"didn't found a good parcel that the drone {droneId} can take");
            }
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
        /// <summary>
        /// Pick Parcel By Drone
        /// </summary>
        /// <param name="droneId"></param>
        public void PickParcelByDrone(int droneId)
        {
            if ((DisplayDrone(droneId).Status == DroneStatus.Associated) && (DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).PickedUp == DateTime.MinValue))//####
            {
                foreach (DroneToList drone in lstdrn)
                {
                    if (drone.Id == droneId)
                    {
                        drone.Battery = drone.Battery - minBattery(droneId, DisplayCustomer(DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).Sender.Id).Location, DisplayCustomer(DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).Target.Id).Location);
                        drone.CurrentLocation = DisplayCustomer(DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).Target.Id).Location;

                        dl.PickParcelByDrone(DisplayDrone(droneId).ParcelInT.Id);
                    }
                }
            }
            else
            {//######
                //לזרוק חריגה!!!!!!!!
                //כתוב בהוראות
            }
        }
        /// <summary>
        /// Deliver Parcel By Drone
        /// </summary>
        /// <param name="droneId"></param>
        public void DeliverParcelByDrone(int droneId)
        {
            if((DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).PickedUp != DateTime.MinValue)&& (DisplayParcel(DisplayDrone(droneId).ParcelInT.Id).Delivered == DateTime.MinValue))//#####
            {
                foreach (DroneToList drone in lstdrn)
                {
                    if (drone.Id == droneId)
                    {
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
        /// <summary>
        /// Returns the parcel with the requested ID
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Display List Of Parcels
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Display List Of Unassigned Parcels
        /// </summary>
        /// <returns></returns>
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
