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
        public void AddDrone(int id, string model, WeightCategories weight, int stationId)
        {
            Location location = DisplayStation(stationId).Location;
            DO.Drone idalDrone = new DO.Drone
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
        public void UpdateDroneModel(int id, string model)
        {
            //update the model in the logical layer
            DroneToList drone;
            DO.Drone ddrone;
            try
            {
                drone = lstdrn.Single(item => item.Id == id);
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
        public Drone DisplayDrone(int droneId)
        {
            DroneToList droneFromList;
            try { droneFromList = lstdrn.Find(item => item.Id == droneId); }
            catch (InvalidOperationException)
            {
                throw new NotExistIDException($"id: {droneId} does not exist - drone");
            }
            if (droneFromList == null) throw new NotExistIDException($"id: {droneId} does not exist - drone");
            ParcelInTransfer parcel = null;
            if (droneFromList.Status == DroneStatus.Associated || droneFromList.Status == DroneStatus.Delivery)
            {
                DO.Parcel parcelFromFunc;
                try { parcelFromFunc = dl.DisplayParcel(droneFromList.ParcelId); }
                catch (DO.ParcelException ex) { throw new NotExistIDException(ex.Message, " - parcel"); }
                Location locOfSender = DisplayCustomer(parcelFromFunc.Senderld/*SenderId*/).Location;
                Location locOfTarget = DisplayCustomer(parcelFromFunc.TargetId).Location;
                parcel = new ParcelInTransfer
                {
                    Id = parcelFromFunc.Id,
                    InTheWay = (parcelFromFunc.PickUpTime != null && parcelFromFunc.DeliverTime == null),
                    Priority = (Priorities)parcelFromFunc.Priority,
                    Sender = new CustomerInParcel { Id = parcelFromFunc.Senderld, Name = DisplayCustomer(parcelFromFunc.Senderld).Name },
                    Target = new CustomerInParcel { Id = parcelFromFunc.TargetId, Name = DisplayCustomer(parcelFromFunc.TargetId).Name },
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
        public void SendDroneToCharge(int droneId)
        {
            //DroneToList drone= new DroneToList();
            //int i;
            //for (i = 0; i < lstdrn.Count(); i++)
            //{
            //    if (lstdrn[i].Id == droneId)
            //    {
            //        drone = lstdrn[i];
            //        break;
            //    }
            //}
            DroneToList drone;
            int index;
            try 
            { 
                index = lstdrn.FindIndex(item => item.Id == droneId); 
                drone = lstdrn.ElementAt(index); 
            }
            catch (InvalidOperationException)
            {
                throw new NotExistIDException($"id: {droneId} does not exist - drone");
            }
            if (drone.Status != DroneStatus.Available)
            {
                throw new DroneCantGoToChargeException($"the drone {droneId} is not available so it can't be sended to charging"); //if the drone is not available
            }
            Location loc = drone.CurrentLocation;
            Station st = closestStationWithChargeSlots(loc);
            double batteryNeed = minBattery(drone.Id, loc, st.Location);
            if (batteryNeed > drone.Battery)
            {
                throw new DroneCantGoToChargeException($"the battery of drone {droneId} is not enugh so it can't be sended to charging"); //if the drone dont have enough battery
            }
            try { dl.SendDroneToCharge(droneId, st.Id); } // Adds a drone charging entity and lowers the amount of available charge slots at the station
            catch (Exception) { throw new DroneCantGoToChargeException(); }
            drone.Battery -= batteryNeed;
            drone.CurrentLocation = st.Location;
            drone.Status = DroneStatus.Maintenance;

            lstdrn.RemoveAt(index);
            lstdrn.Add(drone);
        }
        public void ReleaseDroneFromeCharge(int droneId)
        {
            DroneToList drone;
            int index;
            try
            {
                index = lstdrn.FindIndex(item => item.Id == droneId);
                drone = lstdrn.ElementAt(index);
            }
            catch (InvalidOperationException)
            {
                throw new NotExistIDException($"id: {droneId} does not exist - drone");
            }
            if (drone.Status != DroneStatus.Maintenance) throw new DroneCantReleaseFromChargeException
            {
                message = $"the drone {droneId} is not in maintenance so it can't be released from charging"
            };
            try
            {
                dl.ReleaseDroneFromeCharge(droneId); //Deletes the charging entity and adds 1 to the charging slots of the station
            }
            catch (DO.DroneChargeException ex) { throw new NotExistIDException(ex.Message); }
            DO.DroneCharge dc= dl.DisplayListOfDroneCharge().Where(x => x.DroneId == droneId).Single();
            TimeSpan time =DateTime.Now - dc.StartedChargeTime;
            double b = time.TotalSeconds * ChargeRatePerHour;
            drone.Battery += (double)(b / 3600);
            if (drone.Battery > 100) drone.Battery = 100;
            drone.Status = DroneStatus.Available;
            
            lstdrn.RemoveAt(index);
            lstdrn.Add(drone);
        }
        public IEnumerable<DroneToList> DisplayListOfDrones(Func<DroneToList, bool> pre)
        {
            if (pre != null)
                return lstdrn.Where(pre);
            else
                return lstdrn.Select(x => x);
        }
    }
}
