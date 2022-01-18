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
    class Simulator
    {
        private const double velocity = 5; //kilometers per second.
        private const int delayMS = 500; //miliseconds, half of second.
        private static Drone/*ToList*/ drone;

        enum status { fly, charge, wait, toCharge };//charge-בטיענה
                                                    //toCharge-כשהוא מוצא את התחנה שהוא הולך להיטען בה
                                                    //wait- אם אין לו לאן ללכת בטעינה הוא מחכה עד שיתפנה מקום
                                                    //fly-כשהוא נוסע
        private status droneStatus = status.charge;

        private Location targetLocation;
        private double distanceFromTarget = 0;
        private double batteryUsage;



        public Simulator(BL bl, int droneId, Action updateDisplay, Func<bool> stop)
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

        /// <summary>
        /// כשהמצב של הרחפן הוא טעינה
        /// </summary>
        /// <param name="bl"></param>
        private void chargedDrone(BL bl)
        {
            if (delay())//פונקציה 
            {
                switch (droneStatus)
                {
                    case status.toCharge://אם הרחפן צריך לחפש תחנה שבה הוא צריך להיטען
                        try
                        {
                            lock (bl)
                            {
                                Location currentLoc = drone.CurrentLocation;
                                double currentBattery = drone.Battery;
                                drone.Status = DroneStatus.Available;//שינוי הסטטוס של הרחפן לזמין
                                bl.SendDroneToCharge(drone.Id);//שליחת הרחפן לטעינה
                                drone.CurrentLocation = currentLoc;//שינוי מיקום הרחפן להיות המיקום המקורי שלו
                                drone.Battery = currentBattery;//שינוי בטרית הרפן להיות הבטריה המקורית שלו
                                droneStatus = status.charge;
                                int stationId = 0;//איפוס התחנה שבה נמצא הרחפן
                                foreach (StationToList station in bl.GetStationsList())
                                {
                                    if (bl.GetDroneChargesList(station.Id).Any(x => x.DroneId == drone.Id))//לממש תפונקציה
                                        stationId = station.Id;
                                }
                                Location stationLoc = bl.GetStation(stationId).Location;
                                targetLocation = stationLoc;
                                
                                distanceFromTarget = bl.distance(drone.CurrentLocation, stationLoc);//לממד תפונקציה
                                batteryUsage = bl.batteryForAvailable;

                            }
                        }
                        catch (Exception ex) when (/*ex is TimeException ||*/ ex is NotExistIDException)
                        {
                            //if the closest station did not have open charging slots
                            drone.Status = DroneStatus.Maintenance;
                            droneStatus = status.wait;
                        }
                        break;
                    case status.fly:
                        lock (bl)
                        {
                            calculateDistance(bl);
                        }
                        if (distanceFromTarget == 0)
                        {
                            droneStatus = status.charge;
                        }
                        break;
                    case status.charge:
                        double timePassed = (double)delayMS / 1000;//****
                        drone.Battery += bl.chargeRatePerMinute * timePassed;
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
                                batteryUsage = bl.batteryForLight;
                                break;
                            case WeightCategories.Medium:
                                batteryUsage = bl.batteryForMedium;
                                break;
                            case WeightCategories.Heavy:
                                batteryUsage = bl.batteryForHeavy;
                                break;
                        }
                    }
                    else
                        batteryUsage = bl.batteryForAvailable;
                    calculateDistance(bl);
                    if (distanceFromTarget == 0)
                    {
                        if (!pickedUp)
                            bl.PickParcelByDrone(drone.Id);
                        else
                        {
                            bl.DeliverParcelByDrone(drone.Id);
                            batteryUsage = bl.batteryForAvailable;
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
                    catch (NotExistIDException ex)
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

        /// <summary>
        /// calculate the updated distance between the drone and the target and update the field distanceFromTarget
        /// </summary>
        /// <param name="bl"></param>
        private void calculateDistance(BL bl) 
        {
            lock (bl)
            {
                distanceFromTarget = bl.distance(drone.CurrentLocation, targetLocation);
                //if (distanceFromTarget < 0.0001)
                //{
                //    distanceFromTarget = 0;
                //    drone.CurrentLocation = targetLocation;
                //    return;
                //}
                double change =        velocity * delayMS           / 1000; //calculate the change in the distance, according to the delay- the time that passed since the previous calculation.
                //              (זמן במילי שניות)*(מהירות לשנייה) 
                //change = Min(change, distanceFromTarget); //in case the drone has theoretically passed the target
                if(change > distanceFromTarget) //in case the drone has theoretically passed the target
                {
                    distanceFromTarget = 0;
                    drone.CurrentLocation = targetLocation;
                    return;
                }
                double proportionalChange = change / distanceFromTarget;
                drone.Battery = Max(0.0, drone.Battery - distanceFromTarget * batteryUsage);
                Location loc = drone.CurrentLocation;
                drone.CurrentLocation = new Location //update the location of the drone
                { 
                    Longi = loc.Latti + ((targetLocation.Latti - loc.Latti) * proportionalChange), //ignore the shipua of earth 
                    Latti = loc.Longi + ((targetLocation.Longi - loc.Longi) * proportionalChange)
                };
                distanceFromTarget = bl.distance(drone.CurrentLocation, targetLocation);
            }

        }
    }
}
