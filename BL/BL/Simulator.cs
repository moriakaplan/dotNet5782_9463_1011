using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using static System.Math;
using BO;
using static BL.BL;
using BLApi;


namespace BL
{
    /// <summary>
    /// simulation for one drone- manage the drone actions.
    /// </summary>
    class Simulator
    {
        private const double velocity = 30; //kilometers per second.
        private const int delayMS = 500; //miliseconds, half of second.
        private DroneToList drone;

        enum status { fly, inCharge, wait, toCharge };
        //charge- now in charge
        //toCharge- When he finds the station he is going to charge at
        //wait- If he has nowhere to go for charging
        //fly- when the rrone fly
        private status droneStatus = status.inCharge;

        private Location targetLocation;
        private double distanceFromTarget = 0;
        private double batteryUsage;

        /// <summary>
        /// constructor, start the simulation
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="droneId"></param>
        /// <param name="updateDisplay"></param>
        /// <param name="stop"></param>
        public Simulator(BL bl, int droneId, Action updateDisplay, Func<bool> stop)
        {
            lock (bl)
            {
                drone = bl.GetDronesList(x => x.Id == droneId).SingleOrDefault();
                if (drone == null) throw new NotExistIDException();
            }
            while (!stop()) //As long as the simolation didnt stop
            {
                switch (drone.Status)
                {
                    case (DroneStatus.Maintenance):
                        chargedDrone(bl);
                        break;
                    case (DroneStatus.Available):
                        availableDrone(bl);
                        break;
                    case (DroneStatus.Associated):
                    case (DroneStatus.Delivery):
                        deliveryDrone(bl);
                        break;
                }
                updateDisplay();
            }
        }

        /// <summary>
        /// when the drone is Maintenance
        /// </summary>
        /// <param name="bl"></param>
        private void chargedDrone(BL bl)
        {
            if (delay()) 
            {
                switch (droneStatus)
                {
                    case status.toCharge://If the drone needs to look for a station where it can be charged
                        try
                        {
                            lock (bl)
                            {
                                Location currentLoc = drone.CurrentLocation;
                                double currentBattery = drone.Battery;
                                drone.Status = DroneStatus.Available;//Change the status of the drone to available
                                bl.SendDroneToCharge(drone.Id);//Sending the drone for charging
                                drone.CurrentLocation = currentLoc;//Change the location of the drone to be its original location
                                drone.Battery = currentBattery;//Changing the drone battery to be its original battery
                                droneStatus = status.inCharge;
                                int stationId = 0;//Reset the station where the drone is located
                                foreach (StationToList station in bl.GetStationsList())
                                {
                                    if (bl.GetDroneChargesList(station.Id).Any(x => x.DroneId == drone.Id))//Checks if the drone is charged at the station
                                        stationId = station.Id;
                                }
                                Location stationLoc = bl.GetStation(stationId).Location;
                                targetLocation = stationLoc;

                                distanceFromTarget = bl.distance(drone.CurrentLocation, stationLoc);//Finds the distance between the location of the drone and the station where it needs to be charged
                                batteryUsage = bl.batteryForAvailable;
                            }
                        }
                        catch ( NotExistIDException) 
                        {
                            //if the closest station did not have open charging slots
                            drone.Status = DroneStatus.Maintenance;
                            droneStatus = status.wait;
                        }
                        break;
                    case status.fly://if the drone is fling to the station
                        lock (bl)
                        {
                            calculateDistance(bl);
                        }
                        if (distanceFromTarget == 0)//If he reached his destination (to the station he needed to be charge at)
                        {
                            droneStatus = status.inCharge;
                        }
                        break;
                    case status.inCharge://if the drone is in charge
                        double timePassed = (double)delayMS / 1000;
                        drone.Battery += bl.chargeRatePerMinute * timePassed / 60;
                        drone.Battery = Min(drone.Battery, 100);
                        if (drone.Battery == 100)
                        lock (bl)
                        {
                            bl.ReleaseDroneFromeCharge(drone.Id);//If the skimmer has finished charging then it is released from charge
                            drone.Status = DroneStatus.Available;
                        }
                        break;
                    case status.wait: //Trying to send the drone to charging, if if he does not succeed then he has to wait until space becomes available so he is in this position.
                        droneStatus = status.toCharge;
                        break;
                    default:
                        break;
                }

            }
        }


        /// <summary>
        /// if the drone in delivery
        /// </summary>
        /// <param name="bl"></param>
        private void deliveryDrone(BL bl)
        {
            if (delay())
            {
                lock (bl)
                {
                    Parcel parcel = bl.GetParcel(drone.ParcelId);//Finding the parcel the drone is delivering
                    bool pickedUp = parcel.PickUpTime is not null;//If the parcel was collected

                    targetLocation = pickedUp ? bl.GetCustomer(parcel.Target.Id).Location : bl.GetCustomer(parcel.Sender.Id).Location;//Finds whether the parcel has been collected and updates the location
                    if (pickedUp)//If the parcel has been collected 
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
                    if (distanceFromTarget == 0)//If the drone reached its destination
                    {
                        if (!pickedUp)//If the parcel has not yet been collected
                        {
                            bl.PickParcelByDrone(drone.Id);//Collects the package
                        }
                        else
                        {
                            bl.DeliverParcelByDrone(drone.Id);//Collects the package
                            batteryUsage = bl.batteryForAvailable;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// if the drone is available
        /// </summary>
        /// <param name="bl"></param>
        private void availableDrone(BL bl)
        {
            if (delay())
            {
                lock (bl)
                {
                    try
                    {
                        bl.AssignParcelToDrone(drone.Id);//accosiate the drone to the parcel
                    }
                    catch (ThereNotGoodParcelToTakeException)//NotExistIDException
                    {
                        if (drone.Battery == 100)
                            return;
                        else
                        {
                            drone.Status = DroneStatus.Maintenance;
                            droneStatus = status.toCharge;
                        }
                    }
                    

                }
            }
        }

        /// <summary>
        /// stop the simolator for half a second
        /// </summary>
        /// <returns></returns>
        private static bool delay()
        {
            try
            {
                Thread.Sleep(delayMS);
            }
            catch (ArgumentOutOfRangeException)
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
                double change = velocity * delayMS / 1000; //calculate the change in the distance, according to the delay- the time that passed since the previous calculation.
                // (time in milliseconds) * (speed per second)
                if (change > distanceFromTarget) //if the drone already passed the target in this half of decond
                {
                    distanceFromTarget = 0;
                    drone.CurrentLocation = targetLocation;

                    return;
                }
                double proportionalChange = change / distanceFromTarget;
                drone.Battery = Max(0.0, drone.Battery - distanceFromTarget * batteryUsage);
                Location loc = drone.CurrentLocation;
                drone.CurrentLocation = new Location
                {
                    Latti = loc.Latti + ((targetLocation.Latti - loc.Latti) * proportionalChange),
                    Longi = loc.Longi + ((targetLocation.Longi - loc.Longi) * proportionalChange)
                };
                distanceFromTarget = bl.distance(drone.CurrentLocation, targetLocation);
            }
        }
    }
}
