using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// parcel
    /// </summary>
    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public DroneInParcel Drone { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; } // Regular, Fast, Emergency
        public DateTime? CreateTime { get; set; } //creating time of the parcel
        public DateTime? AssociateTime { get; set; } //the time when the parcel associated to the drone
        public DateTime? PickUpTime { get; set; }  //when the drone pick the parcel
        public DateTime? DeliverTime { get; set; } //when the drone deliver the parcel
        public override string ToString()
        {
            return @$"Id: #{Id}:
sender- {Sender},
reciver- {Target},
drone- {Drone},
weight- {Weight},
priority- {Priority},
created- {CreateTime},
scheduled- {AssociateTime}, 
picked up- {PickUpTime},
delivered- {DeliverTime}.
";
        }
    }
}
