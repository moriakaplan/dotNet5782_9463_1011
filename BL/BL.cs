using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL
    {
        public List<DroneToList> lstdrn;
        public IDal dl = new DalObject.DalObject();
        public BL()
        {
            dl = new DalObject.DalObject();
            lstdrn = (List<DroneToList>)dl.DisplayListOfDrones();
            initializeDrone();
        }
        private void initializeDrone()
        {
            foreach (IDAL.DO.Drone drone in dl.DisplayListOfDrones())
            {
                lstdrn.Add(new DroneToList { Id = drone.Id, MaxWeight = (WeightCategories)drone.MaxWeight, Model = drone.Model });
            }
            foreach (DroneToList drone in lstdrn)
            {
                //if()//אם יש חבילה שעוד לא סופקה אבל הרחפן שוייך
                //{

                //}
                foreach(IDAL.DO.Parcel parcel in dl.DisplayListOfParcels())
                {

                }
            }


        }
    }
}
