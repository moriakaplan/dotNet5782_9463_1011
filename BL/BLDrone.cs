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
        public void AddDrone(Drone drone)
        {
            dl.AddDroneToTheList(new IDAL.DO.Drone
            {
                Id = drone.Id,
                MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight,
                Model = drone.Model
            });
            lstdrn.Add(new DroneToList
            {
                Id = drone.Id,
                Battery = rand.Next(20, 39) + rand.NextDouble(),
                CurrentLocation = drone.CurrentLocation,
                MaxWeight = drone.MaxWeight,
                Model = drone.Model,
                ParcelId = 0,
                Status = DroneStatus.Maintenance
            });
            //add the new station to the list in the data level
            // }
            // catch(Exception ex)
            //{
            //    throw new ExistIdException(ex.Message, ex)
            // }
        }
        public void UpdateDroneModel(int id, string model)
        {
            DroneToList drone = lstdrn.Find(item => item.Id == id);
            drone.Model = model;
            //לעדכן בדאל
        }
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
            dl.SendDroneToCharge(droneId, closestStationWithAvailableChargeSlosts(drone.CurrentLocation).Id);
        }
        public void ReleaseDroneFromeCharge(int droneId, DateTime timeInCharge)
        {
            Drone drone = DisplayDrone(droneId);
            if (drone.Status != DroneStatus.Maintenance) throw new DroneCantReleaseFromChargeExeption
            {
                message = "the drone {droneId} is not in maintenance so it can't be released from charging"
            };
            //עדכון סוללה
            //לשנות מצב רחפן לפנוי
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
    }
}
