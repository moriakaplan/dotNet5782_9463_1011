////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
//using BO;
////using System.Threading;
//using static BL.BL;
//using BLApi;

//namespace BL
//{
//    class Simulator
//    {
//        const double VELOCITY = 2; //kilometers per second.
//        const int DELAY = 500; //miliseconds, half of second.
//        private static Drone drone;
//        public Simulator(BL bl, int droneId, Action UpdateDisplayDelegate, Func<bool> checkStop)
//        {
//            lock (bl)
//            {
//                 drone = bl.GetDrone(droneId); //ממשיך לזרוק. זה בסדר?
//            }
//            while (!checkStop())
//            {
//                if(drone.Status==DroneStatus.Available)
//                {

//                }
//                if(drone.Status == DroneStatus.Delivery)
//                {

//                }
//                if(drone.Status==DroneStatus.Maintenance)
//                {

//                }
//                //Thread.Sleep(DELAY);
//                //lock (bl)
//                //{
//                //    Drone drone = bl.GetDrone(droneId); //ממשיך לזרוק. זה בסדר?
//                //    if (drone.Status == DroneStatus.Available)
//                //    {
//                //        try
//                //        {
//                //            bl.AssignParcelToDrone(droneId);
//                //        }
//                //        catch(ThereNotGoodParcelToTakeException)
//                //        {
//                //            if(drone.Battery<100)
//                //            {

//                //                try
//                //                {
//                //                    bl.SendDroneToCharge(droneId);
//                //                }
//                //                catch(DroneCantGoToChargeException)
//                //                {

//                //                   drone.CurrentLocation = bl.closestStation(drone.CurrentLocation);//לא נכון!!!!!
//                //                   //לקרוא לפונקציה moveDrone
//                //                }
//                //            }
//                //            //להחליט מה לעשות
//                //            //שיחכה עד שיש חבילה?
//                //        }
//                //    }
//                //    if (drone.Status == DroneStatus.Maintenance)
//                //    {
//                //        //if(drone.Battery+bl.batteryToAdd(droneId)>=100)
//                //        //{
//                //        //    bl.ReleaseDroneFromeCharge(droneId);
//                //        //}                                                                                                                                                                  
//                //        if(drone.Battery==100)
//                //        {

//                //        }
//                //    }

//                //}

//            }
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using static System.Math;
//using System.Diagnostics;
using BO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Threading;
using static BL.BL;
using BLApi;


namespace BL
{
    class Simulation
    {
        private static double V = 50;
        private static int delayMS = 500;
        private static double accuracy = 0.0001;
        private static BO.Drone/*ToList*/ drone;

        enum status { deliver, charge, wait, toCharge };
        private status droneStatus = status.charge;

        private Location targetLocation;
        private double distanceFromTarget = 0;
        private double batteryUsage;



        public Simulation(BL bl, int droneId, Action updateDisplay, Func<bool> stop)
        {
            lock (bl)
            {
                //מציאת הרחפו שאיתו נעבוד
                //drone = bl.DisplayDroneList(d => d.id == dId).FirstOrDefault();
                drone = bl.GetDrone(droneId); //ממשיך לזרוק. זה בסדר?

            }
            while (!stop())//כל עוד לא רצו להפסיק את הסימולציה
            {
                if (drone.Status == DroneStatus.Available)//אם הרחפן זמין
                {
                    //availableDrone(bl);
                }
                else if (drone.Status == DroneStatus.Delivery)//אם הרחפו במשלוח
                {
                    //deliveryDrone(bl);
                }
                else if (drone.Status == DroneStatus.Maintenance)//אם הרחפו בטעינה
                {
                   // chargedDrone(bl);
                }
                updateDisplay();
            }
        }

        private void chargedDrone(BL bl)
        {
            if (delay())//אם לא נגמרה ההשהייה
            {
                switch (droneStatus)
                {
                    case status.toCharge:
                        try
                        {
                            lock (bl)
                            {
                                Location currentLoc = drone.CurrentLocation;
                                double currentBattery = drone.Battery;
                                drone.Status = DroneStatus.Available;
                                bl.SendDroneToCharge(drone.Id);//שליחת הרחפן לטעינה
                                drone.CurrentLocation = currentLoc;
                                drone.Battery = currentBattery;
                                droneStatus = status.charge;
                                int stationId = 0;//איפוס התחנה שבה נמצא הרחפן
                                foreach (StationToList station in bl.GetStationList)//לממש תפונקציה
                                {
                                   
                                    if (bl.displayDronesInCharge(station.Id).Any(d => d == drone.Id))//לממש תפונקציה
                                        stationId = station.Id;
                                }
                                Location stationLoc = bl.GetStation(stationId).location;
                                targetLocation = stationLoc;
                                
                                distanceFromTarget = bl.calcDistance(drone.CurrentLocation, stationLoc);//לממד תפונקציה
                                batteryUsage = bl.availablePK;

                            }
                        }
                        catch (Exception ex) when (ex is BO.exceptions.TimeException || ex is BO.exceptions.NotFoundException)
                        {
                            //if the closest station did not have open charging slots
                            drone.Status = DroneStatus.Maintenance;
                            droneStatus = status.wait;
                        }
                        break;
                    case status.deliver:
                        lock (bl)
                        {
                            calculate(bl);
                        }
                        if (distanceFromTarget == 0)
                        {
                            droneStatus = status.charge;
                        }
                        break;
                    case status.charge:
                        double timePassed = (double)delayMS / 1000;
                        drone.Battery += bl.chargingPH * timePassed;
                        drone.Battery = Min(drone.Battery, 100);
                        if (drone.Battery == 100)
                            lock (bl)
                            {
                                bl.ReleaseDroneFromeCharge(drone.Id);
                            }
                        break;
                    case status.wait: //try sending drone to charge - waiting until a station close enough has empty charge slots
                        droneStatus = status.toCharge;
                        break;
                    default:
                        break;
                }

            }
        }

        private void deliveryDrone(BL bl)
        {
            if (delay())
            {
                lock (bl)
                {
                    Parcel parcel = bl.GetParcel(drone.ParcelInT.Id);
                    //bool pickedUp = parcel.pickup is not null;
                    bool pickedUp = parcel.PickUpTime is not null;
                    
                    //targetLocation = pickedUp ? bl.targetLocation(parcel.Id) : bl.senderLocation(parcel.Id);
                    targetLocation = pickedUp ? bl.GetCustomer(parcel.Target.Id).Location: bl.GetCustomer(parcel.Sender.Id).Location;//לא יודעת אם עובד!!!
                    if (pickedUp)
                    {
                        switch (parcel.Weight)
                        {
                            case WeightCategories.Light:
                                batteryUsage = bl.lightPK;
                                break;
                            case WeightCategories.Medium:
                                batteryUsage = bl.mediumPK;
                                break;
                            case WeightCategories.Heavy:
                                batteryUsage = bl.heavyPK;
                                break;
                        }
                    }
                    else
                        batteryUsage = bl.AvailablePK;
                    calculate(bl);
                    if (distanceFromTarget == 0)
                    {
                        if (!pickedUp)
                            bl.PickParcelByDrone(drone.Id);
                        else
                        {
                            bl.DeliverParcelByDrone(drone.Id);
                            batteryUsage = bl.availablePK;
                        }
                    }
                }
            }
        }

        private void availableDrone(BL bl)
        {
            if (delay())
            {
                lock (bl)
                {
                    try
                    {
                        bl.AssignParcelToDrone(drone.Id);
                    }
                    catch (BO.exceptions.NotFoundException ex)
                    {
                        if (drone.Battery == 100) //drone cant collect any parcel at all
                            return;
                        else if (ex.Message.Equals("no parcel can be matched to the drone"))
                        {
                            drone.Status = DroneStatus.Maintenance;
                            droneStatus = status.toCharge;
                        } //drone cant collect any parcel because of his battery
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }
        private static bool delay()
        {
            try
            {
                Thread.Sleep(delayMS);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void calculate(IBL bl)
        {
            lock (bl)
            {
                distanceFromTarget = bl.calcDistance(drone.CurrentLocation, targetLocation);//לממש תפונקציה
                if (distanceFromTarget < accuracy)
                {
                    distanceFromTarget = 0;
                    drone.CurrentLocation = targetLocation;
                    return;
                }
                double timePassed = (double)delayMS / 1000;
                double distanceChange = V * timePassed;
                double change = Min(distanceChange, distanceFromTarget); //in case the drone has theoretically passed the target
                double proportionalChange = change / distanceFromTarget;
                drone.Battery = Max(0.0, drone.Battery - change * batteryUsage);
                double droneLat = drone.CurrentLocation.Latti;
                double droneLong = drone.CurrentLocation.Longi;
                double targetLat = targetLocation.Latti;
                double targetLong = targetLocation.Longi;
                double lat = droneLat + (targetLat - droneLat) * proportionalChange; //we ignore the shipua of earth
                double lon = droneLong + (targetLong - droneLong) * proportionalChange;
                drone.CurrentLocation = new Location { Longi = lon, Latti = lat };
                distanceFromTarget = bl.calcDistance(drone.currentLocation, targetLocation);//לממש תפונקציה
            }

        }
    }
}
