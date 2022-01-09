using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLApi;

namespace BL
{
    internal partial class BL
    {
        public void DeleteParcel(int id)
        {
            Parcel pa = GetParcel(id);
            if (pa.AssociateTime == null)
            {
                try { dl.DeleteParcel(id); }
                catch (DO.ParcelException) { throw new NotExistIDException(""); }
            }
            else throw new DeleteException($"parcel {id} can't be deleted");
        }
        public int AddParcelToDelivery(int senderId, int targetId, WeightCategories weight, Priorities pri)
        {
            DO.Parcel idalParcel = new DO.Parcel
            {
                Droneld = 0,
                Senderld = senderId,
                TargetId = targetId,
                Priority = (DO.Priorities)pri,
                Weight = (DO.WeightCategories)weight,
                CreateTime = DateTime.Now,
                AssociateTime = null,
                PickUpTime = null,
                DeliverTime = null
            };
            try
            {
                return dl.AddParcel(idalParcel);
                //add the new Parcel to the list in the data level
            }
            catch (DO.ParcelException ex)
            {
                throw new ExistIdException(ex.Message, "-parcel");
            }
        }     
        public void AssignParcelToDrone(int droneId)
        {
            Drone bdrone;
            bdrone = GetDrone(droneId);
            if (bdrone.Status != DroneStatus.Available)
            {
                throw new DroneCantTakeParcelException($"drone {droneId} is not available so it cant take a new parcel");
            }
            Parcel parcel = findClosestParcel(droneId);//אולי צריך שזה יהיה תנאי ביצירת רשימת החבילות?
            Location locOfSender = GetCustomer(parcel.Sender.Id).Location;
            Location locOfTarget = GetCustomer(parcel.Target.Id).Location;
            double batteryNeeded =
                minBattery(droneId, bdrone.CurrentLocation, locOfSender) +
                minBattery(droneId, locOfSender, locOfTarget) +
                minBattery(droneId, locOfTarget, closestStationWithChargeSlots(locOfTarget).Location);//לבדוק אם צריך את התחנה הכי קרובה עם עמדות טעינה
            if (batteryNeeded > bdrone.Battery)
            {
                throw new ThereNotGoodParcelToTakeException($"did not found a good parcel that the drone {droneId} can take");
            }
            try
            {
                dl.AssignParcelToDrone(parcel.Id, droneId); //update drone and scheduled time in parcel
            }
            catch (DO.DroneException ex)
            {
                throw new NotExistIDException(ex.Message, " - drone");
            }
            catch (DO.ParcelException ex)
            {
                throw new NotExistIDException(ex.Message, " - parcel");
            }

            foreach (DroneToList drone in lstdrn)
            {
                if (drone.Id == droneId)
                {
                    drone.Status = DroneStatus.Associated; //update the drone status
                    drone.ParcelId = parcel.Id;
                }
            }

        }     
        public void PickParcelByDrone(int droneId)
        {
            Parcel parcelToPick = GetParcel(GetDrone(droneId).ParcelInT.Id);
            if (!(parcelToPick.AssociateTime!= null && parcelToPick.Drone.Id==droneId && parcelToPick.PickUpTime == null))
            {
                throw new TransferException($"drone {droneId} can't pick up the parcel");
            }
            foreach (DroneToList drone in lstdrn)
            {
                if (drone.Id == droneId)
                {
                    double batteryNeeded = minBattery(droneId, drone.CurrentLocation, GetCustomer(parcelToPick.Sender.Id).Location);
                    if (batteryNeeded > drone.Battery) throw new DroneCantTakeParcelException("the battery of the drone is not enugh for pick the parcel");
                    drone.CurrentLocation = GetCustomer(parcelToPick.Sender.Id).Location;
                    drone.Status = DroneStatus.Delivery;
                    try { dl.PickParcelByDrone(parcelToPick.Id); } //update pick up time in the parcel
                    catch (DO.ParcelException ex) { throw new NotExistIDException(ex.Message, " - parcel"); }
                    return;
                }
            }

        }       
        public void DeliverParcelByDrone(int droneId)
        {
            Parcel parcelToDeliver = GetParcel(GetDrone(droneId).ParcelInT.Id);
            if ((parcelToDeliver.Drone.Id!=droneId || parcelToDeliver.PickUpTime == null || parcelToDeliver.DeliverTime != null))
            {
                throw new TransferException($"drone {droneId} can't deliver the parcel");
            }
            foreach (DroneToList drone in lstdrn)
            {
                if (drone.Id == droneId)
                {
                    double batteryNeeded = minBattery(droneId, drone.CurrentLocation, GetCustomer(parcelToDeliver.Target.Id).Location);
                    if(batteryNeeded>drone.Battery) throw new DroneCantTakeParcelException("the battery of the drone is not enugh for deliver the parcel");
                    drone.CurrentLocation = GetCustomer(parcelToDeliver.Target.Id).Location;
                    try
                    {
                        dl.DeliverParcelToCustomer(GetDrone(droneId).ParcelInT.Id);//update the deliver time in the data layer
                    }
                    catch(DO.ParcelException ex) { throw new NotExistIDException(ex.Message, " - parcel"); }
                    drone.Status = DroneStatus.Available;
                }
            }
        }     
        public Parcel GetParcel(int parcelId)
        {
            DO.Parcel parcelFromDal;
            Drone droneFromFunc=null;
            DroneInParcel drone = null;
            try
            {
                parcelFromDal = dl.GetParcel(parcelId);
            }
            catch (DO.ParcelException ex)
            {
                throw new NotExistIDException(ex.Message, "- parcel");
            }
            if(parcelFromDal.AssociateTime!= null) //if the parcel is associated
            {
                droneFromFunc = GetDrone(parcelFromDal.Droneld);
                drone = new DroneInParcel
                {
                    Id = droneFromFunc.Id,
                    Battery = droneFromFunc.Battery,
                    CurrentLocation = droneFromFunc.CurrentLocation
                };
            }
            CustomerInParcel sender = new CustomerInParcel
            {
                Id = parcelFromDal.Senderld,
                Name = GetCustomer(parcelFromDal.Senderld).Name
            };
            CustomerInParcel target = new CustomerInParcel
            {
                Id = parcelFromDal.TargetId,
                Name = GetCustomer(parcelFromDal.TargetId).Name
            };
            return new Parcel
            {
                Id = parcelFromDal.Id,
                DeliverTime = parcelFromDal.DeliverTime,
                Drone = drone,
                PickUpTime = parcelFromDal.PickUpTime,
                Priority = (Priorities)parcelFromDal.Priority, 
                CreateTime = parcelFromDal.CreateTime,
                AssociateTime = parcelFromDal.AssociateTime,
                Sender = sender,
                Target = target,
                Weight = (WeightCategories)parcelFromDal.Weight
            };
        }
        public IEnumerable<ParcelToList> GetParcelsList()
        {
            IEnumerable<DO.Parcel> listFromDal = dl.GetParcelsList();
            ParcelStatus st;
            foreach (DO.Parcel parcel in listFromDal)
            {
                if (parcel.DeliverTime != null) st = ParcelStatus.Delivered;
                else
                {
                    if (parcel.PickUpTime != null) st = ParcelStatus.PickedUp;
                    else
                    {
                        if (parcel.AssociateTime != null) st = ParcelStatus.Associated;
                        else st = ParcelStatus.Created;
                    }
                }
                ParcelToList answer = new ParcelToList
                {
                    Id = parcel.Id,
                    Priority = (Priorities)parcel.Priority,
                    SenderName = GetCustomer(parcel.Senderld).Name,
                    TargetName = GetCustomer(parcel.TargetId).Name,
                    Status = st,
                    Weight = (WeightCategories)parcel.Weight
                };
                yield return answer;
            }
        }
        public IEnumerable<ParcelToList> GetListOfUnassignedParcels()
        {
            IEnumerable<DO.Parcel> listFromDal = dl.GetParcelsList(x=> x.AssociateTime == null);
            foreach (DO.Parcel parcel in listFromDal)
            {
                ParcelToList answer = new ParcelToList
                {
                    Id = parcel.Id,
                    Priority = (Priorities)parcel.Priority,
                    SenderName = GetCustomer(parcel.Senderld).Name,
                    TargetName = GetCustomer(parcel.TargetId).Name,
                    Status = ParcelStatus.Created,
                    Weight = (WeightCategories)parcel.Weight
                };
                yield return answer;
            }
        }

        /// <summary>
        /// Returns all parcels with the highest priority
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Parcel> findHighesPrioritiy()
        {
            Priorities temp = Priorities.Regular;//gets the highest priority 
            foreach (ParcelToList parcelList in GetParcelsList())
            {
                if (parcelList.Priority > temp)
                {
                    temp = parcelList.Priority;
                }
            }
            Parcel parcel = new Parcel();
            foreach (ParcelToList parcelList in GetParcelsList())
            {
                if (parcelList.Priority == temp)
                {
                    yield return GetParcel(parcelList.Id);

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

            WeightCategories temp = WeightCategories.Easy;
            foreach (Parcel parcel in findHighesPrioritiy())
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
                    yield return GetParcel(parcel.Id);

                }
            }
        }
        /// <summary>
        /// find Closest Pacel
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        private Parcel findClosestParcel(int droneId)
        {
            Parcel result = findHighesWeight(GetDrone(droneId).MaxWeight).First();
            Location DroneLocation = new Location { Latti = GetDrone(droneId).CurrentLocation.Latti, Longi = GetDrone(droneId).CurrentLocation.Longi };
            double minDistance = distance(GetCustomer(result.Sender.Id).Location, DroneLocation);
            foreach (Parcel parcel in findHighesWeight(GetDrone(droneId).MaxWeight))
            {
                if (distance(GetCustomer(parcel.Sender.Id).Location, DroneLocation) < minDistance)
                {
                    minDistance = distance(GetCustomer(parcel.Sender.Id).Location, DroneLocation);
                    result = parcel;
                }
            }
            return result;
        }

    }
}
