using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public partial class BL:IBL.Ibl
    {
        void AddDrone(Drone drone)
        {

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
            return null;
        }
        IEnumerable<DroneToList> DisplayListOfDrones()
        {
            return null;
        }
    }
}
