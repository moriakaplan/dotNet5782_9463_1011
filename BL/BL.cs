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
                foreach (IDAL.DO.Parcel parcel in dl.DisplayListOfParcels())
                {
                    IDAL.DO.Drone tempDrone = dl.DisplayDrone(parcel.Droneld);//מציאת הרחפן שהחבילה משוייכת אליו, צריך לבדוק אם הוא קיים(אם החבילה משוייכת)
                    if ((parcel.Droneld == drone.Id) && (parcel.Delivered == DateTime.MinValue) && (tempDrone.Status == DroneStatus.associated))//אם יש חבילה שעוד לא סופקה אבל אבל הרחפן שוייך
                    {
                        drone.Status = DroneStatus.delivery;
                        if ((parcel.Scheduled != DateTime.MinValue) && (parcel.PickedUp == DateTime.MinValue))//אם החבילה שויכה אבל לא נאספה
                        {
                            // drone.CurrentLocation;המיקום צריך להיות בתחנה הקרובה לשולח
                        }
                        if ((parcel.PickedUp != DateTime.MinValue) && (parcel.Delivered == DateTime.MinValue))//אם החבילה נאספה אבל לא סופקה
                        {
                            IDAL.DO.Customer tempCustomer = dl.DisplayCustomer(parcel.Senderld);//לבדוק אם הוא קיים
                            drone.CurrentLocation = new Location() { Longi = tempCustomer.Longitude, Latti = tempCustomer.Lattitude };//מיקום הרחפן הוא במיקום השולח
                        }
                        drone.Battery = 0;//יוגרל בין טעינה מינימאלית שתאפשר לרחפן לבצע את המשלוח ולהגיע לתחנה הקרובה לבין טעינה מלאה
                    }
                    if ()//אם הרחפן לא מבצע משלוח
                    {
                        //drone.Status=מוגרל בין תחזוקה לפנוי
                    }

                }
            }
        }
    }
}
