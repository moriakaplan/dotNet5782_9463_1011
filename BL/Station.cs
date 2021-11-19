using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int AvailableChargeSlots { get; set; }
        public IEnumerable<DroneInCharge> DronesInCharge { get; set; }
        public override string ToString()
        {
            return @$"Station Id: #{Id}:
name- {Name},
location- {Location},
Available Charge Slots- {AvailableChargeSlots},
Drones In Charge- {DronesInCharge}.
";
        }
    }
}
