using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public double Battery { get; set; }
        public DroneStatus Status { get; set; }
        public ParcelInTransfer ParcelInT { get; set; } 
        public Location CurrentLocation { get; set; }
        public override string ToString()
        {
            string parcel = "";
            if (ParcelInT != null) parcel = $"parcel in transfer-{ParcelInT},'\n'";
            return @$"drone #{Id}:
model- {Model},
max weight- {MaxWeight},
battery-{Battery},
ststus-{Status},
parsels in transfer-{ParcelInT},
current locetion-{CurrentLocation}.
";

        }
    }
}
