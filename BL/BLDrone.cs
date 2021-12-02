using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IBL;
using IDAL;

namespace IBL
{
    public partial class BL
    {
        /// <summary>
        /// adding drone
        /// </summary>
        /// <param name="drone"></param>
        public void AddDrone(Drone drone)
        {
            IDAL.DO.Drone idalDrone=new IDAL.DO.Drone //create new drone for data layer
            {
                Id = drone.Id,
                MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight,
                Model = drone.Model
            };
            try
            {
                dl.AddDroneToTheList(idalDrone);
                //add the new customer to the list in the data level
            }
            catch (IDAL.DO.DroneException ex)
            {
                throw new ExistIdException(ex.Message, "-drone");
            }
            lstdrn.Add(new DroneToList
            {
                Id = drone.Id,
                Battery = random.Next(20, 39) + random.NextDouble(),
                CurrentLocation = drone.CurrentLocation,
                MaxWeight = drone.MaxWeight,
                Model = drone.Model,
                ParcelId = 0,
                Status = DroneStatus.Maintenance
            });//add drone to the list of the drone in the logical layer
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
            IDAL.DO.Drone ddrone;
            try
            {
                drone = lstdrn.Find(item => item.Id == id);
                drone.Model = model;
                //update the model in the data layer
                ddrone = dl.DisplayDrone(id);
                dl.DeleteDrone(id);
            }
            catch (ArgumentNullException) { throw new NotExistIDExeption($"id: {id} does not exist - drone"); }
            catch (IDAL.DO.DroneException ex) { throw new NotExistIDExeption(ex.Message, " - drone"); }
            ddrone.Model = model;
            dl.AddDroneToTheList(ddrone);
        }
        /// <summary>
        /// send the drone to charge
        /// </summary>
        /// <param name="droneId"></param>
        public void SendDroneToCharge(int droneId)
        {
            DroneToList drone = lstdrn.Find(item => item.Id == droneId);
            double lo = drone.CurrentLocation.Longi, la = drone.CurrentLocation.Latti;
            if (drone.Status != DroneStatus.Available) throw new DroneCantGoToChargeExeption
            {
                message = "the drone {droneId} is not available so it can't be sended to charging"
            };
            Station st = closestStationWithChargeSlots(drone.CurrentLocation);
            double batteryNeed = minBattery(drone.Id, drone.CurrentLocation, st.Location);
            if (batteryNeed>drone.Battery) throw new DroneCantGoToChargeExeption//if the drone dont have enough battery
            {
                message = "the drone {droneId} is not available so it can't be sended to charging"
            };
            drone.Battery -= batteryNeed;
            drone.CurrentLocation = st.Location;
            drone.Status = DroneStatus.Maintenance;
            dl.SendDroneToCharge(droneId, st.Id); // Adds a drone charging entity and lowers the amount of available charge slots at the station
        }
        /// <summary>
        /// Release the Drone Frome Charge
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="timeInCharge"></param>
        public void ReleaseDroneFromeCharge(int droneId, DateTime timeInCharge)
        {
            Drone drone = DisplayDrone(droneId);
            if (drone.Status != DroneStatus.Maintenance) throw new DroneCantReleaseFromChargeExeption
            {
                message = "the drone {droneId} is not in maintenance so it can't be released from charging"
            };
            double time = convertDateTimeToDoubleInHours(timeInCharge);
            DroneToList droneFromList = lstdrn.Find(item => item.Id == droneId);
            droneFromList.Battery += time * ChargeRatePerHour;
            droneFromList.Status = DroneStatus.Available;
            dl.ReleaseDroneFromeCharge(droneId); //Deletes the charging entity and adds 1 to the charging slots of the station
        }
        /// <summary>
        ///  Returns the drone with the requested ID
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        public Drone DisplayDrone(int droneId)
        {
            DroneToList droneFromList = lstdrn.Find(item => item.Id == droneId);
            ParcelInTransfer parcel = null;
            if (droneFromList.Status == DroneStatus.Associated || droneFromList.Status == DroneStatus.Delivery)
            {
                Parcel parcelFromFunc = DisplayParcel(droneFromList.ParcelId);
                Location locOfSender = DisplayCustomer(parcelFromFunc.Sender.Id).Location;
                Location locOfTarget = DisplayCustomer(parcelFromFunc.Target.Id).Location;
                parcel = new ParcelInTransfer
                {
                    Id = parcelFromFunc.Id,
                    InTheWay = (parcelFromFunc.PickUpTime != DateTime.MinValue && parcelFromFunc.DeliverTime == DateTime.MinValue),
                    Priority = parcelFromFunc.Priority,
                    Sender = parcelFromFunc.Sender,
                    Target = parcelFromFunc.Target,
                    PickingPlace = locOfSender,
                    TargetPlace = locOfTarget,
                    TransportDistance = distance(locOfSender, locOfTarget),
                    Weight = parcelFromFunc.Weight
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
        ///  returns the list of the drones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DroneToList> DisplayListOfDrones()
        {
            foreach(DroneToList drone in lstdrn)
            {
                yield return drone;
            }
        }
        /// <summary>
        /// convert Date Time To Double In Hours
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private double convertDateTimeToDoubleInHours(DateTime dateTime)
        {
            double result = 0;
            result += dateTime.Year * 365 * 24;
            //result += dateTime.Month * 30/*that not good*/* 24;
            //result += dateTime.Day * 24;
            result += dateTime.DayOfYear * 24;
            result += dateTime.Hour;
            result += dateTime.Minute * (1 / 60);
            result += dateTime.Minute * (1 / 360);
            return result;
        }
    }
}
