using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInTransfer
    {
        public int Id { get; set; }
        public bool InTheWay { get; set; } //((משהו פה מוזר מאוד (צריך להיות: מצב חבילה(בולאני- ממתין לאיסוף/בדרך ליעד****
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; } //רגיל, מהיר, חירום
        public Location PickingPlace { get; set; } //מקום איסוף
        public Location TargetPlace { get; set; } //מקום אספקה
        public double TransportDistance { get; set; }
        public override string ToString()
        {

        }
    }
}
