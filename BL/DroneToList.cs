using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneToList
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public double Battery { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public DroneStatus Status { get; set; }
        public Location CurrentLocation { get; set; }
        public int ParcelId { get; set; } //אם יש חבילה מועברת
        public override string ToString()
        {
            return @$"drone #{Id}:
mode- {Model},
max weight- {MaxWeight},
battery- {Battery},
status- {Status},
current location- {CurrentLocation},
Parcel Id- {ParcelId}.
";

        }
    }
}
