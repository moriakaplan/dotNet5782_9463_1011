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
    public partial class BL: Ibl
    {
        void AddDrone(Drone drone)
        {
            //creates a new station in the data level
            IDAL.DO.Drone dalDrone = new IDAL.DO.Drone
            {
                Id = drone.Id,
                Model=drone.Model, MaxWeight=drone.MaxWeight, 
            };
            //try
            //{
            dl.AddCustomerToTheList(dCustomer);
            //add the new station to the list in the data level
            // }
            // catch(Exception ex)
            //{
            //    throw new ExistIdException(ex.Message, ex)
            // }
        }
        void UpdateDroneModel(int id, string model)
        {

        }
        void SendDroneToCharge(int droneId)
        {

        }
        void ReleaseDroneFromeCharge(int droneId, DateTime timeInCharge)
        {

        }
        Drone DisplayDrone(int droneId)
        {
            DroneToList droneFromList = drones.Find(item => item.Id == droneId);
            Parcel parcelFromFunc = DisplayParcel(droneFromList.ParcelId);
            ParcelInTransfer parcel = new ParcelInTransfer
            {
                Id = parcelFromFunc.Id,
                InTheWay = (parcelFromFunc.PickedUp != DateTime.MinValue && parcelFromFunc.Delivered == DateTime.MinValue),
                /*PickingPlace= , PickingPlace= ,*/
                Priority = parcelFromFunc.Priority,
                Sender = parcelFromFunc.Sender,
                Target = parcelFromFunc.Target,
                TransportDistance = dl.Distance(PickingPlace.longitude, PickingPlace.lattitude, TargetPlace.longitude, TargetPlace.lattitude), 
                Weight=parcelFromFunc.Weight
            };
            return new Drone { 
                Id = droneFromList.Id, 
                Battery = droneFromList.Battery, 
                CurrentLocation = droneFromList.CurrentLocation, 
                MaxWeight = droneFromList.MaxWeight, 
                Model = droneFromList.Model,
                Status = droneFromList.Status,
                ParcelInT = parcel };
        }
        IEnumerable<DroneToList> DisplayListOfDrones(Predicate<DroneToList> pre)
        {
            if (pre != null)
            {
                List<DroneToList> result = new List<DroneToList>();
                foreach(DroneToList item in drones)
                {
                    if (pre(item)) result.Add(item);
                }
                return result;
            }
            else
            {
                return drones;
            }
        }

        //void Ibl.AddDrone(Drone drone)
        //{
        //    throw new NotImplementedException();
        //}
        //void Ibl.AddCustomer(Customer customer)
        //{
        //    throw new NotImplementedException();
        //}

        //void Ibl.AddParcelToDelivery(Parcel parcel)
        //{
        //    throw new NotImplementedException();
        //}

        //void Ibl.UpdateDroneModel(int id, string model)
        //{
        //    throw new NotImplementedException();
        //}

        //void Ibl.UpdateCustomer(int id, params string[] args)
        //{
        //    throw new NotImplementedException();
        //}

        //void Ibl.SendDroneToCharge(int droneId)
        //{
        //    throw new NotImplementedException();
        //}

        //void Ibl.ReleaseDroneFromeCharge(int droneId, DateTime timeInCharge)
        //{
        //    throw new NotImplementedException();
        //}

        //void Ibl.AssignParcelToDrone(int parcelId, int droneId)
        //{
        //    throw new NotImplementedException();
        //}

        //void Ibl.AssignParcelToDrone(int droneId)
        //{
        //    throw new NotImplementedException();
        //}

        //void Ibl.PickParcelByDrone(int parcelId)
        //{
        //    throw new NotImplementedException();
        //}

        //void Ibl.DeliverParcelByDrone(int droneId)
        //{
        //    throw new NotImplementedException();
        //}

        //Customer Ibl.DisplayCustomer(int customerId)
        //{
        //    throw new NotImplementedException();
        //}

        //Drone Ibl.DisplayDrone(int droneId)
        //{
        //    throw new NotImplementedException();
        //}

        //Parcel Ibl.DisplayParcel(int parcelId)
        //{
        //    throw new NotImplementedException();
        //}

        //IEnumerable<DroneToList> Ibl.DisplayListOfDrones()
        //{
        //    throw new NotImplementedException();
        //}

        //IEnumerable<CustomerToList> Ibl.DisplayListOfCustomers()
        //{
        //    throw new NotImplementedException();
        //}

        //IEnumerable<ParcelToList> Ibl.DisplayListOfParcels()
        //{
        //    throw new NotImplementedException();
        //}

        //IEnumerable<ParcelToList> Ibl.DisplayListOfUnassignedParcels()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
