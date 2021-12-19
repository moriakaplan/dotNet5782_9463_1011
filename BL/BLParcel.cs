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
        /// <summary>
        /// Add Parcel To Delivery
        /// </summary>
        /// <param name="parcel"></param>
        public void AddParcelToDelivery(int senderId, int targetId, WeightCategories weight, Priorities pri)
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
                dl.AddParcelToTheList(idalParcel);
                //add the new Parcel to the list in the data level
            }
            catch (DO.ParcelException ex)
            {
                throw new ExistIdException(ex.Message, "-parcel");
            }
        }
        /// <summary>
        /// Returns all parcels with the highest priority
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Parcel> findHighesPrioritiy()
        {
            Priorities temp = Priorities.Regular;//gets the highest priority 
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
                    yield return DisplayParcel(parcel.Id);

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
            if (bdrone.Status != DroneStatus.Available)
            {
                throw new DroneCantTakeParcelException($"drone {droneId} is not available so it cant take a new parcel");
            }
            Parcel parcel = findClosestParcel(droneId);//אולי צריך שזה יהיה תנאי ביצירת רשימת החבילות?
            Location locOfSender = DisplayCustomer(parcel.Sender.Id).Location;
            Location locOfTarget = DisplayCustomer(parcel.Target.Id).Location;
            double batteryNeeded =
                minBattery(droneId, bdrone.CurrentLocation, locOfSender) +
                minBattery(droneId, locOfSender, locOfTarget) +
                minBattery(droneId, locOfTarget, closestStationWithChargeSlots(locOfTarget).Location);//לבדוק אם צריך את התחנה הכי קרובה עם עמדות טעינה
            if (batteryNeeded > bdrone.Battery)
            {
                throw new ThereNotGoodParcelToTake($"did not found a good parcel that the drone {droneId} can take");
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
        /// <summary>
        /// Pick Parcel By Drone
        /// </summary>
        /// <param name="droneId"></param>
        public void PickParcelByDrone(int droneId)
        {
            Parcel parcelToPick = DisplayParcel(DisplayDrone(droneId).ParcelInT.Id);
            if (!(parcelToPick.AssociateTime!= null && parcelToPick.Drone.Id==droneId && parcelToPick.PickUpTime == null))
            {
                throw new TransferException($"drone {droneId} can't pick up the parcel");
            }
            foreach (DroneToList drone in lstdrn)
            {
                if (drone.Id == droneId)
                {
                    double batteryNeeded = minBattery(droneId, drone.CurrentLocation, DisplayCustomer(parcelToPick.Sender.Id).Location);
                    if (batteryNeeded > drone.Battery) throw new DroneCantTakeParcelException("the battery of the drone is not enugh for pick the parcel");
                    drone.CurrentLocation = DisplayCustomer(parcelToPick.Sender.Id).Location;
                    drone.Status = DroneStatus.Delivery;
                    try { dl.PickParcelByDrone(parcelToPick.Id); } //update pick up time in the parcel
                    catch (DO.ParcelException ex) { throw new NotExistIDException(ex.Message, " - parcel"); }
                    return;
                }
            }

        }
        /// <summary>
        /// Deliver Parcel By Drone
        /// </summary>
        /// <param name="droneId"></param>
        public void DeliverParcelByDrone(int droneId)
        {
            Parcel parcelToDeliver = DisplayParcel(DisplayDrone(droneId).ParcelInT.Id);
            if ((parcelToDeliver.Drone.Id!=droneId || parcelToDeliver.PickUpTime == null || parcelToDeliver.DeliverTime != null))
            {
                throw new TransferException($"drone {droneId} can't deliver the parcel");
            }
            foreach (DroneToList drone in lstdrn)
            {
                if (drone.Id == droneId)
                {
                    double batteryNeeded = minBattery(droneId, drone.CurrentLocation, DisplayCustomer(parcelToDeliver.Target.Id).Location);
                    if(batteryNeeded>drone.Battery) throw new DroneCantTakeParcelException("the battery of the drone is not enugh for deliver the parcel");
                    drone.CurrentLocation = DisplayCustomer(parcelToDeliver.Target.Id).Location;
                    try
                    {
                        dl.DeliverParcelToCustomer(DisplayDrone(droneId).ParcelInT.Id);//update the deliver time in the data layer
                    }
                    catch(DO.ParcelException ex) { throw new NotExistIDException(ex.Message, " - parcel"); }
                    drone.Status = DroneStatus.Available;
                }
            }
        }
        /// <summary>
        /// Returns the parcel with the requested ID
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        public Parcel DisplayParcel(int parcelId)
        {
            DO.Parcel parcelFromDal;
            Drone droneFromFunc=null;
            DroneInParcel drone = null;
            try
            {
                parcelFromDal = dl.DisplayParcel(parcelId);
            }
            catch (DO.ParcelException ex)
            {
                throw new NotExistIDException(ex.Message, "- parcel");
            }
            if(parcelFromDal.AssociateTime!= null) //if the parcel is associated
            {
                droneFromFunc = DisplayDrone(parcelFromDal.Droneld);
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
        /// <summary>
        /// Display List Of Parcels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ParcelToList> DisplayListOfParcels()
        {
            //List<ParcelToList> answer = new List<ParcelToList>();
            IEnumerable<DO.Parcel> listFromDal = dl.DisplayListOfParcels();
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
            IEnumerable<DO.Parcel> listFromDal = dl.DisplayListOfParcels(x=> x.AssociateTime == null);
            foreach (DO.Parcel parcel in listFromDal)
            {
                ParcelToList answer = new ParcelToList
                {
                    Id = parcel.Id,
                    Priority = (Priorities)parcel.Priority,
                    SenderName = DisplayCustomer(parcel.Senderld).Name,
                    TargetName = DisplayCustomer(parcel.TargetId).Name,
                    Status = ParcelStatus.Created,
                    Weight = (WeightCategories)parcel.Weight
                };
                yield return answer;
            }
            //return answer;
        }
    }
}
