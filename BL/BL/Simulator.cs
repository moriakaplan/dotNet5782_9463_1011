using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;
using BLApi;

namespace BL
{
    class Simulator
    {
        const double VELOCITY = 2; //kilometers per second.
        const int DELAY = 500; //miliseconds, half of second.
        public Simulator(BL bl, int droneId, Action UpdateDisplayDelegate, Func<bool> checkStop)
        {
            while (!checkStop())
            {
                Thread.Sleep(DELAY);
                lock (bl)
                {
                    Drone drone = bl.GetDrone(droneId); //ממשיך לזרוק. זה בסדר?
                    if (drone.Status == DroneStatus.Available)
                    {
                        try
                        {
                            bl.AssignParcelToDrone(droneId);
                        }
                        catch(ThereNotGoodParcelToTakeException)
                        {
                            if(drone.Battery<100)
                            {

                                try
                                {
                                    bl.SendDroneToCharge(droneId);
                                }
                                catch(DroneCantGoToChargeException)
                                {

                                   drone.CurrentLocation = bl.closestStation(drone.CurrentLocation);//לא נכון!!!!!
                                   //לקרוא לפונקציה moveDrone
                                }
                            }
                            //להחליט מה לעשות
                            //שיחכה עד שיש חבילה?
                        }
                    }
                    if (drone.Status == DroneStatus.Maintenance)
                    {
                        //if(drone.Battery+bl.batteryToAdd(droneId)>=100)
                        //{
                        //    bl.ReleaseDroneFromeCharge(droneId);
                        //}
                        if(drone.Battery==100)
                        {
                            
                        }
                    }

                }
            }
        }
    }
}
