using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public DroneInParcel Drone { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; } //רגיל, מהיר, חירום
        public DateTime Requested { get; set; } //יצירת החבילה
        public DateTime Scheduled { get; set; } //שיוך לרחפן
        public DateTime PickedUp { get; set; }  //איסוף מהשולח
        public DateTime Delivered { get; set; } //מסירה ליעד
        public override string ToString()
        {

        }
    }
}
