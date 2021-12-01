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
        //public List<DroneToList> lstdrn;
        //public IDal dl = new DalObject.DalObject();

        internal static Random rand = new Random();
        /// <summary>
        /// adding drone
        /// </summary>
        /// <param name="drone"></param>
        public void AddDrone(Drone drone)
        {
            dl.AddDroneToTheList(new IDAL.DO.Drone
            {
                Id = drone.Id,
                MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight,
                Model = drone.Model
            });//add drone to the data layer
            lstdrn.Add(new DroneToList
            {
                Id = drone.Id,
                Battery = rand.Next(20, 39) + rand.NextDouble(),
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
            DroneToList drone = lstdrn.Find(item => item.Id == id);
            drone.Model = model;
            //update the model in the data layer
            IDAL.DO.Drone ddrone = dl.DisplayDrone(id);
            dl.DeleteDrone(id);
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
            //IEnumerable<IDAL.DO.Station> stationsWithAvailable = dl.DisplayListOfStationsWithAvailableCargeSlots();
            //double min = dl.DistanceForStation(lo, la, stationsWithAvailable..Id);
            //double distance;
            //IDAL.DO.Station closerStation;
            //foreach (IDAL.DO.Station station in stationsWithAvailable)
            //{
            //    distance = dl.DistanceForStation(lo, la, station.Id);
            //    if (distance < min) min = distance;
            //}
            //Parcel pa = DisplayParcel(drone.ParcelId);
            //ParcelInTransfer parcel= new ParcelInTransfer { i }
            //Drone DroneNotFromList = new Drone
            //{
            //    Id = drone.Id,
            //    Battery = drone.Battery,
            //    CurrentLocation = drone.CurrentLocation,
            //    MaxWeight = drone.MaxWeight,
            //    Model = drone.Model,
            //    ParcelInT = parcel,
            //    Status = drone.Status
            //};
            Station st = closestStationWithAvailableChargeSlosts(drone.CurrentLocation);
            double batteryNeed = minBattery(drone.Id, drone.CurrentLocation, st.Location);
            if (batteryNeed>drone.Battery) throw new DroneCantGoToChargeExeption
            {
                message = "the drone {droneId} is not available so it can't be sended to charging"
            };
            drone.Battery -= batteryNeed;
            drone.CurrentLocation = st.Location;
            //לעדכן בדאל את מה שצריך(מיקום?)
            drone.Status = DroneStatus.Maintenance;
            dl.SendDroneToCharge(droneId, st.Id); // מוסיף ישות טעינת רחפן ומוריד את כמות העמדות הפנויות בתחנה
        }
        public void ReleaseDroneFromeCharge(int droneId, DateTime timeInCharge)
        {
            Drone drone = DisplayDrone(droneId);
            if (drone.Status != DroneStatus.Maintenance) throw new DroneCantReleaseFromChargeExeption
            {
                message = "the drone {droneId} is not in maintenance so it can't be released from charging"
            };
            //עקרונית צריך לעדכן בדאל אבל אין שם בטריה ומצב סוללה
            double time = convertDateTimeToDoubleInHours(timeInCharge);//צריך להמיר איכשהו את ה-dataTime
            //IDAL.DO.Drone updateDrone = dl.DisplayDrone(droneId);
            //dl.DeleteDrone(droneId);
            //updateDrone.Battery=
            DroneToList droneFromList = lstdrn.Find(item => item.Id == droneId);
            double rate = dl.AskBattery(dl.DisplayDrone(droneId))[4];
            droneFromList.Battery += time * rate;
            droneFromList.Status = DroneStatus.Available;
            dl.ReleaseDroneFromeCharge(droneId); //מוחק את הישות טעינה ומוסיף 1 לעמדות טעינה של התחנה
        }
        public Drone DisplayDrone(int droneId)
        {
            DroneToList droneFromList = lstdrn.Find(item => item.Id == droneId);
            Parcel parcelFromFunc = DisplayParcel(droneFromList.ParcelId);
            ParcelInTransfer parcel = new ParcelInTransfer
            {
                Id = parcelFromFunc.Id,
                InTheWay = (parcelFromFunc.PickedUp != DateTime.MinValue && parcelFromFunc.Delivered == DateTime.MinValue),
                Priority = parcelFromFunc.Priority,
                Sender = parcelFromFunc.Sender,
                Target = parcelFromFunc.Target,
                PickingPlace = DisplayCustomer(parcelFromFunc.Sender.Id).Location,
                //PickingPlace= ,*/
                TransportDistance = dl.Distance(PickingPlace.longitude, PickingPlace.lattitude, TargetPlace.longitude, TargetPlace.lattitude),
                Weight = parcelFromFunc.Weight
            };
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
        public IEnumerable<DroneToList> DisplayListOfDrones()
        {
            //if (pre != null)
            //{
            //    List<DroneToList> result = new List<DroneToList>();
            //    foreach (DroneToList item in drones)
            //    {
            //        if (pre(item)) result.Add(item);
            //    }
            //    return result;
            //}
            //else
            //{
            //    return drones;
            //}
            return lstdrn;
        }
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
