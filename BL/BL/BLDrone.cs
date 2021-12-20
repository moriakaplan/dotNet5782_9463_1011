﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLApi;

namespace BL
{
    public partial class BL
    {
        /// <summary>
        /// adding drone
        /// </summary>
        /// <param name="drone"></param>
        public void AddDrone(int id, string model, WeightCategories weight, int stationId)
        {
            Location location = DisplayStation(stationId).Location;
            DO.Drone idalDrone=new DO.Drone
            {
                Id = id,
                MaxWeight = (DO.WeightCategories)weight,
                Model = model
            };
            try
            {
                dl.AddDroneToTheList(idalDrone);
                //add the new customer to the list in the data level
            }
            catch (DO.DroneException ex)
            {
                throw new ExistIdException(ex.Message, "-drone");
            }
            lstdrn.Add(new DroneToList
            {
                Id = id,
                Battery = random.Next(20, 39) + random.NextDouble(),
                CurrentLocation = location,
                MaxWeight = weight,
                Model = model,
                ParcelId = 0,
                Status = DroneStatus.Maintenance
            });//add drone to the list of the drone in the logical layer
            dl.SendDroneToCharge(id, stationId);
        }
        /// <summary>
        /// update the drone model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        public void UpdateDroneModel(int id, string model)
        {
            //update the model in the logical layer
            DroneToList drone;
            DO.Drone ddrone;
            try
            {
                drone = lstdrn.Find(item => item.Id == id);
                drone.Model = model;
                //update the model in the data layer
                ddrone = dl.DisplayDrone(id);
                dl.DeleteDrone(id);
            }
            catch (ArgumentNullException) { throw new NotExistIDException($"id: {id} does not exist - drone"); }
            catch (DO.DroneException ex) { throw new NotExistIDException(ex.Message, " - drone"); }
            ddrone.Model = model;
            dl.AddDroneToTheList(ddrone);
        }
        /// <summary>
        ///  Returns the drone with the requested ID
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Drone DisplayDrone(int droneId)
        {
            DroneToList droneFromList=null;
            foreach (DroneToList item in lstdrn)
            {
                if (item.Id == droneId)
                    droneFromList = item;
            }
            if (droneFromList == null) throw new NotExistIDException($"id: {droneId} does not exist - drone");
            ParcelInTransfer parcel = null;
            if (droneFromList.Status == DroneStatus.Associated || droneFromList.Status == DroneStatus.Delivery)
            {
                DO.Parcel parcelFromFunc;
                try { parcelFromFunc = dl.DisplayParcel(droneFromList.ParcelId); }
                catch(DO.ParcelException ex) { throw new NotExistIDException(ex.Message, " - parcel"); }
                Location locOfSender = DisplayCustomer(parcelFromFunc.Senderld/*SenderId*/).Location;
                Location locOfTarget = DisplayCustomer(parcelFromFunc.TargetId).Location;
                parcel = new ParcelInTransfer
                {
                    Id = parcelFromFunc.Id,
                    InTheWay = (parcelFromFunc.PickUpTime != null && parcelFromFunc.DeliverTime == null),
                    Priority = (Priorities)parcelFromFunc.Priority,
                    Sender = new CustomerInParcel { Id = parcelFromFunc.Senderld, Name = DisplayCustomer(parcelFromFunc.Senderld/*SenderId*/).Name },/*DisplayCustomer(parcelFromFunc.Senderld/*SenderId*/
                    Target = new CustomerInParcel { Id = parcelFromFunc.TargetId, Name = DisplayCustomer(parcelFromFunc.TargetId/*SenderId*/).Name },
                    PickingPlace = locOfSender,
                    TargetPlace = locOfTarget,
                    TransportDistance = distance(locOfSender, locOfTarget),
                    Weight = (WeightCategories)parcelFromFunc.Weight
                };
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
        /// <summary>
         /// send the drone to charge
         /// </summary>
         /// <param name="droneId"></param>
        public void SendDroneToCharge(int droneId)
        {
            //if (!lstdrn.Exists(item => item.Id == droneId)) throw;
            //DroneToList drone = lstdrn.Find(item => item.Id == droneId);
            //Drone drone = DisplayDrone(droneId);
            //DroneToList drone = lstdrn.Find(x => x.Id == droneId);
            DroneToList drone= new DroneToList();
            int i;
            for (i = 0; i < lstdrn.Count; i++)
            {
                if (lstdrn[i].Id == droneId)
                {
                    drone = lstdrn[i];
                    break;
                }
            }
            if (i == lstdrn.Count) throw new NotExistIDException($"id: {droneId} does not exist - drone");
            double lo = drone.CurrentLocation.Longi, la = drone.CurrentLocation.Latti;
            if (drone.Status != DroneStatus.Available) 
                throw new DroneCantGoToChargeException($"the drone {droneId} is not available so it can't be sended to charging"); //if the drone is not available
            Station st = closestStationWithChargeSlots(drone.CurrentLocation);
            double batteryNeed = minBattery(drone.Id, drone.CurrentLocation, st.Location);
            if (batteryNeed > drone.Battery) 
                throw new DroneCantGoToChargeException($"the battery of drone {droneId} is not enugh so it can't be sended to charging"); //if the drone dont have enough battery
            try { dl.SendDroneToCharge(droneId, st.Id); } // Adds a drone charging entity and lowers the amount of available charge slots at the station
            catch (Exception) { throw new DroneCantGoToChargeException(); }
            drone.Battery -= batteryNeed;
            drone.CurrentLocation = st.Location;
            drone.Status = DroneStatus.Maintenance;
            lstdrn[i] = drone;
        }
        /// <summary>
        /// Release the Drone Frome Charge
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="timeInCharge"></param>
        public void ReleaseDroneFromeCharge(int droneId, TimeSpan timeInCharge)
        {
            Drone drone = DisplayDrone(droneId);
            if (drone.Status != DroneStatus.Maintenance) throw new DroneCantReleaseFromChargeException
            {
                message = $"the drone {droneId} is not in maintenance so it can't be released from charging"
            };
            for(int i=0;i< lstdrn.Count;i++)
            {
                if(lstdrn[i].Id== droneId)
                {
                    DroneToList dronetolist = lstdrn[i];
                    double b = timeInCharge.TotalSeconds * ChargeRatePerHour;
                    dronetolist.Battery = dronetolist.Battery+(double)(b/3600);
                    if (dronetolist.Battery > 100) dronetolist.Battery = 100;
                    dronetolist.Status = DroneStatus.Available;
                    lstdrn[i] = dronetolist;
                }
            }
            try
            {
                dl.ReleaseDroneFromeCharge(droneId); //Deletes the charging entity and adds 1 to the charging slots of the station
            }
            catch (DO.DroneChargeException ex) { throw new NotExistIDException(ex.Message); }
        }
        /// <summary>
        ///  returns the list of the drones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneToList> DisplayListOfDrones(Predicate<DroneToList> pre)
       {
            foreach(DroneToList drone in lstdrn)
            {
                if (pre == null || pre(drone))
                    yield return drone;
            }
        }
    }
}