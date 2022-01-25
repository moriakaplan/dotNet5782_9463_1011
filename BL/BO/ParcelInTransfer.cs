using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// parcel in transfer
    /// </summary>
    public class ParcelInTransfer
    {
        public int Id { get; set; }
        public bool InTheWay { get; set; }//in the way to the destination or wating for pick up
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; } //Regular, Fast, Emergency
        public Location PickingPlace { get; set; } 
        public Location TargetPlace { get; set; } 
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
