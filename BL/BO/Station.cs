using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// station
    /// </summary>
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int AvailableChargeSlots { get; set; }
        public List<DroneInCharge> DronesInCharge { get; set; }//IEnumerable
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
