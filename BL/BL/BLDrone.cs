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
        public void AddDrone(int id, string model, WeightCategories weight, int stationId)
        {
            lock (dl)
            {
                Location location = GetStation(stationId).Location;
                DO.Drone idalDrone = new DO.Drone
                {
                    Id = id,
                    MaxWeight = (DO.WeightCategories)weight,
                    Model = model
                };

                try
                {
                    dl.AddDrone(idalDrone);
                    //add the new customer to the list in the data level
                }
                catch (DO.DroneException ex)
                {
                    throw new ExistIdException(ex.Message, "-drone");
                }
                lstdrn.Add(new DroneToList  //add drone to the list of the drone in the logical layer
                {
                    Id = id,
                    Battery = random.Next(20, 40),
                    CurrentLocation = location,
                    MaxWeight = weight,
                    Model = model,
                    ParcelId = 0,
                    Status = DroneStatus.Maintenance
                });
                dl.SendDroneToCharge(id, stationId);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneModel(int id, string model)
        {
            //update the model in the logical layer
            DroneToList drone;
            DO.Drone ddrone;
            lock (dl)
            {
                try
                {
                    drone = lstdrn.Single(item => item.Id == id);
                    drone.Model = model;
                    //update the model in the data layer
                    ddrone = dl.GetDrone(id);
                    dl.DeleteDrone(id);
                }
                catch (ArgumentNullException) { throw new NotExistIDException($"id: {id} does not exist - drone"); }
                catch (DO.DroneException ex) { throw new NotExistIDException(ex.Message, " - drone"); }
                ddrone.Model = model;
                dl.AddDrone(ddrone);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneId)
        {
            DroneToList droneFromList;
            try { droneFromList = lstdrn.Find(item => item.Id == droneId); }
            catch (InvalidOperationException)
            {
                throw new NotExistIDException($"id: {droneId} does not exist - drone");
            }
            if (droneFromList == null) throw new NotExistIDException($"id: {droneId} does not exist - drone");
            ParcelInTransfer parcel = null;
            if (droneFromList.Status == DroneStatus.Associated || droneFromList.Status == DroneStatus.Delivery)//if the drone is associated or delivers
            {
                DO.Parcel parcelFromFunc;
                Location locOfSender, locOfTarget;
                lock (dl)
                {
                    try { parcelFromFunc = dl.GetParcel(droneFromList.ParcelId); }
                    catch (DO.ParcelException ex) { throw new NotExistIDException(ex.Message, " - parcel"); }
                    locOfSender = GetCustomer(parcelFromFunc.Senderld).Location;
                    locOfTarget = GetCustomer(parcelFromFunc.TargetId).Location;
                    double dis = droneFromList.Status == DroneStatus.Associated ? //find the distance to the next stop of the drone.
                        distance(droneFromList.CurrentLocation, locOfSender) 
                        : distance(locOfSender, locOfTarget); 
                    parcel = new ParcelInTransfer//initialize the parcel that the drone delivers
                    {
                        Id = parcelFromFunc.Id,
                        InTheWay = (parcelFromFunc.PickUpTime != null && parcelFromFunc.DeliverTime == null),
                        Priority = (Priorities)parcelFromFunc.Priority,
                        Sender = new CustomerInParcel { Id = parcelFromFunc.Senderld, Name = GetCustomer(parcelFromFunc.Senderld).Name },
                        Target = new CustomerInParcel { Id = parcelFromFunc.TargetId, Name = GetCustomer(parcelFromFunc.TargetId).Name },
                        PickingPlace = locOfSender,
                        TargetPlace = locOfTarget,
                        TransportDistance = dis,
                        Weight = (WeightCategories)parcelFromFunc.Weight
                    };
                }
            }
            return new Drone
            {
                Id = droneFromList.Id,
                Battery = droneFromList.Battery,
                CurrentLocation = droneFromList.CurrentLocation,
                MaxWeight = droneFromList.MaxWeight,
                Model = droneFromList.Model,
                Status = droneFromList.Status,
                ParcelInT = parcel
            };
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendDroneToCharge(int droneId)
        {
            DroneToList drone;
            int index;
            try
            {
                //find the reqeuested drone
                index = lstdrn.FindIndex(item => item.Id == droneId);
                drone = lstdrn.ElementAt(index);
            }
            catch (InvalidOperationException)
            {
                throw new NotExistIDException($"id: {droneId} does not exist - drone");
            }
            if (drone.Status != DroneStatus.Available)//if the drone is not available it cant go to charge
            {
                throw new DroneCantGoToChargeException($"the drone {droneId} is not available so it can't be sended to charging"); //if the drone is not available
            }
            Location loc = drone.CurrentLocation;
            lock (dl)
            {
                Station st = closestStationWithChargeSlots(loc);
                int batteryNeed = (int)minBattery(drone.Id, loc, st.Location);
                if (batteryNeed > drone.Battery)//if the drone dont have enugh battery to go to the station
                {
                    throw new DroneCantGoToChargeException($"the battery of drone {droneId} is not enugh so it can't be sended to charging"); //if the drone dont have enough battery
                }
                try
                {
                    // Adds a drone charging entity and lowers the amount of available charge slots at the station
                    dl.SendDroneToCharge(droneId, st.Id);
                }
                catch (DO.DroneException) { throw new DroneCantGoToChargeException(); }
                catch (DO.StationException) { throw new DroneCantGoToChargeException(); }
                drone.Battery -= batteryNeed;
                drone.CurrentLocation = st.Location;
                drone.Status = DroneStatus.Maintenance;
            }
            lstdrn.RemoveAt(index);
            lstdrn.Add(drone);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDroneFromeCharge(int droneId)
        {
            DroneToList drone;
            int index;
            try
            {
                //find the reqeuested drone
                index = lstdrn.FindIndex(item => item.Id == droneId);
                drone = lstdrn.ElementAt(index);
            }
            catch (InvalidOperationException)
            {
                throw new NotExistIDException($"id: {droneId} does not exist - drone");
            }
            if (drone.Status != DroneStatus.Maintenance) throw new DroneCantReleaseFromChargeException//if the drone id not in charge it cant be released from charge
            {
                message = $"the drone {droneId} is not in maintenance so it can't be released from charging"
            };
            try
            {
                lock (dl)
                {
                    drone.Battery += (int)batteryToAdd(droneId);
                    if (drone.Battery > 100) drone.Battery = 100;
                    dl.ReleaseDroneFromeCharge(droneId); //Deletes the charging entity and adds 1 to the charging slots of the station
                }
            }
            catch (DO.DroneChargeException ex) { throw new NotExistIDException(ex.Message); }
            catch (ArgumentNullException ex) { throw new NotExistIDException(ex.Message); }
            catch (InvalidOperationException ex) { throw new NotExistIDException(ex.Message); }
            drone.Status = DroneStatus.Available;
            lstdrn.RemoveAt(index);
            lstdrn.Add(drone);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetDronesList(Func<DroneToList, bool> pre = null)
        {
            if (pre != null)
                return lstdrn.Where(pre)
                    .OrderBy(x => x.Id)
                    .OrderBy(x => x.Status);
            else
                return lstdrn.Select(x => x)
                    .OrderBy(x => x.Id)
                    .OrderBy(x => x.Status);
        }


        /// <summary>
        /// return the drone that charge in the requested station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        internal IEnumerable<DO.DroneCharge> GetDroneChargesList(int stationId)
        {
            return dl.GetDroneChargesList(x => x.StationId == stationId);
        }


        /// <summary>
        /// Calculates the battery to add in charge
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        internal double batteryToAdd(int droneId)
        {
            DO.DroneCharge dc;
            try
            {
                dc = dl.GetDroneChargesList().Where(x => x.DroneId == droneId).Single();
            }
            catch (InvalidOperationException) { throw new NotExistIDException(); }
            TimeSpan time = DateTime.Now - dc.StartedChargeTime;
            return time.TotalSeconds * chargeRatePerMinute / 60.0;
        }

    }
}
