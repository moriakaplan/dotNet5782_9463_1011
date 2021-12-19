using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
            string mode = InTheWay ? "in the way to the destination" : "wait for picking";
            return @$"Id: #{Id}:
sender- {Sender},
parcel mode- {mode},
sender- {Sender},
reciver- {Target},
weight- {Weight},
priority- {Priority},
picking up place- {PickingPlace},
delivering place- {TargetPlace},
Transport Distance- {TransportDistance}.
";

        }
    }
}
