using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLApi;
using System.Runtime.CompilerServices;

namespace BL
{
    internal partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int id)
        {

            Parcel pa = GetParcel(id);
            if (pa.AssociateTime == null)//the parcel can deleted only if in not accosiated to drone
            {
                try { lock (dl) { dl.DeleteParcel(id); } }
                catch (DO.ParcelException) { throw new NotExistIDException(""); }
            }
            else throw new DeleteException($"parcel {id} can't be deleted");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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
                lock (dl)
                {
                    return dl.AddParcel(idalParcel);
                    //add the new Parcel to the list in the data level
                }
            }
            catch (DO.ParcelException ex)
            {
                throw new ExistIdException(ex.Message, "-parcel");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AssignParcelToDrone(int droneId)
        {
            Drone bdrone;
            bdrone = GetDrone(droneId);
            if (bdrone.Status != DroneStatus.Available)//if the drone is not available it cant accosiated
            {
                throw new DroneCantTakeParcelException($"drone {droneId} is not available so it cant take a new parcel");
            }
            Parcel parcel = findClosestParcel(droneId);//find the closest parcel to the drone
            Location locOfSender = GetCustomer(parcel.Sender.Id).Location;
            Location locOfTarget = GetCustomer(parcel.Target.Id).Location;
            double batteryNeeded =
                minBattery(droneId, bdrone.CurrentLocation, locOfSender) +
                minBattery(droneId, locOfSender, locOfTarget) +
                minBattery(droneId, locOfTarget, closestStationWithChargeSlots(locOfTarget).Location);
            if (batteryNeeded > bdrone.Battery)//if for the delivery of that parcel the drone need more battery then the battery the drone have
            {
                throw new ThereNotGoodParcelToTakeException($"we did not found a good parcel that the drone" + "can take");
            }
            try
            {
                lock (dl)
                {
                    dl.AssignParcelToDrone(parcel.Id, droneId); //update drone and scheduled time in parcel
                }
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickParcelByDrone(int droneId)
        {
            Parcel parcelToPick = GetParcel(GetDrone(droneId).ParcelInT.Id);
            if (!(parcelToPick.AssociateTime != null && parcelToPick.Drone.Id == droneId && parcelToPick.PickUpTime == null))
            {
                throw new TransferException($"drone {droneId} can't pick up the parcel");
            }
            foreach (DroneToList drone in lstdrn)
            {
                if (drone.Id == droneId)
                {
                    double batteryNeeded = minBattery(droneId, drone.CurrentLocation, GetCustomer(parcelToPick.Sender.Id).Location);
                    if (batteryNeeded > drone.Battery) throw new DroneCantTakeParcelException("the battery of the drone is not enugh for pick the parcel");
                    try { lock (dl) { dl.PickParcelByDrone(parcelToPick.Id); } } //update pick up time in the parcel
                    catch (DO.ParcelException ex) { throw new NotExistIDException(ex.Message, " - parcel"); }
                    drone.Battery -= batteryNeeded;
                    drone.CurrentLocation = GetCustomer(parcelToPick.Sender.Id).Location;
                    drone.Status = DroneStatus.Delivery;
                    return;
                }
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliverParcelByDrone(int droneId)
        {
            Parcel parcelToDeliver = GetParcel(GetDrone(droneId).ParcelInT.Id);
            if ((parcelToDeliver.Drone.Id != droneId || parcelToDeliver.PickUpTime == null || parcelToDeliver.DeliverTime != null))
            {
                throw new TransferException($"drone {droneId} can't deliver the parcel");
            }
            foreach (DroneToList drone in lstdrn)
            {
                if (drone.Id == droneId)
                {
                    double batteryNeeded = minBattery(droneId, drone.CurrentLocation, GetCustomer(parcelToDeliver.Target.Id).Location);
                    if (batteryNeeded > drone.Battery) throw new DroneCantTakeParcelException("the battery of the drone is not enugh for deliver the parcel");
                    try
                    {
                        lock (dl)
                        {
                            dl.DeliverParcelToCustomer(GetDrone(droneId).ParcelInT.Id);//update the deliver time in the data layer
                        }
                    }
                    catch (DO.ParcelException ex) { throw new NotExistIDException(ex.Message, " - parcel"); }
                    drone.Battery -= batteryNeeded;
                    drone.CurrentLocation = GetCustomer(parcelToDeliver.Target.Id).Location;
                    drone.Status = DroneStatus.Available;
                    drone.ParcelId = 0; return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int parcelId)
        {
            DO.Parcel parcelFromDal;
            Drone droneFromFunc = null;
            DroneInParcel drone = null;
            try
            {
                lock (dl)
                {
                    parcelFromDal = dl.GetParcel(parcelId);
                }
            }
            catch (DO.ParcelException ex)
            {
                throw new NotExistIDException(ex.Message, "- parcel");
            }
            if (parcelFromDal.AssociateTime != null) //if the parcel is associated (also pickup or delivery)
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetParcelsList()
        {
            lock (dl)
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

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetListOfUnassignedParcels()
        {
            lock (dl)
            {
                return from parcel in dl.GetParcelsList(x => x.AssociateTime == null)
                       select new ParcelToList
                       {
                           Id = parcel.Id,
                           Priority = (Priorities)parcel.Priority,
                           SenderName = GetCustomer(parcel.Senderld).Name,
                           TargetName = GetCustomer(parcel.TargetId).Name,
                           Status = ParcelStatus.Created,
                           Weight = (WeightCategories)parcel.Weight
                       };
            }
        }

        /// <summary>
        /// Returns all parcels with the highest priority
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ParcelToList> findParcelsWithHighesPrioritiy(Priorities maxPriority)
        {
            IEnumerable<ParcelToList> parcelsList = GetListOfUnassignedParcels();
            if (parcelsList.Count() == 0) throw new ThereNotGoodParcelToTakeException("we did not found a good parcel that the drone" + "can take");
            Priorities max = parcelsList.Max(p => p.Priority <= maxPriority ? p.Priority : Priorities.Regular); //find the highest priority that is not bigger than the parameter we got.
            return from parcel in parcelsList
                   where parcel.Priority == max
                   select parcel;
        }

        /// <summary>
        /// Returns all parcels with the highest weight the drone can take
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        private IEnumerable<Parcel> findParcelsWithHighesWeight(WeightCategories weight)
        {
            IEnumerable<ParcelToList> parcelsList = findParcelsWithHighesPrioritiy(Priorities.Emergency);//find the parcel eith the highest priority
            WeightCategories max = parcelsList.Max(p => p.Weight <= weight ? p.Weight : WeightCategories.Light);//find the max weight that the drone can take
            IEnumerable<Parcel> result = from parcel in parcelsList//find the parcel with the max weight
                                         where parcel.Weight == max
                                         select GetParcel(parcel.Id);
            if (result.Count() == 0)//if there are no parcels in that weight (all the parcel are hevy then the weight of the drone weight)
            {
                parcelsList = findParcelsWithHighesPrioritiy(Priorities.Fast);//find all the parcels that there priority is fast
                max = parcelsList.Max(p => p.Weight <= weight ? p.Weight : WeightCategories.Light);//find the max weight that the drone can take
                result = from parcel in parcelsList//find the parcel with the max weight
                         where parcel.Weight == max
                         select GetParcel(parcel.Id);
                if (result.Count() == 0)//if there are no parcels in that weight 
                {
                    parcelsList = findParcelsWithHighesPrioritiy(Priorities.Regular);//find all the parcels that there priority is ragular
                    max = parcelsList.Max(p => p.Weight <= weight ? p.Weight : WeightCategories.Light);//find the max weight that the drone can take
                    result = from parcel in parcelsList//find the parcel with the max weight
                             where parcel.Weight == max
                             select GetParcel(parcel.Id);
                }
            }
            if (result.Count() == 0) throw new ThereNotGoodParcelToTakeException("we did not found a good parcel that the drone" + "can take");
            return result;
        }

        /// <summary>
        /// find Closest Pacel
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        private Parcel findClosestParcel(int droneId)
        {
            IEnumerable<Parcel> parcelsList = findParcelsWithHighesWeight(GetDrone(droneId).MaxWeight);//all the parcels that in the highest weight and priority
            Parcel result = parcelsList.First();
            Location DroneLocation = GetDrone(droneId).CurrentLocation;
            double minDistance = distance(GetCustomer(result.Sender.Id).Location, DroneLocation);
            foreach (Parcel parcel in parcelsList)//fid the closest parcelto the drone
            {
                double dis = distance(GetCustomer(parcel.Sender.Id).Location, DroneLocation);
                if (dis < minDistance)
                {
                    minDistance = dis;
                    result = parcel;
                }
            }
            return result;
        }
    }
}
