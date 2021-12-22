using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// drone in parcel
    /// </summary>
    public class DroneInParcel
    {
        public int Id { get; set; }
        public double Battery { get; set; }
        public Location CurrentLocation { get; set; }
        public override string ToString()
        {
            return @$"
    drone #{Id}:
    battrey- {string.Format($"{Battery:0.000}")},
    current locetion- {CurrentLocation}";

        }
    }
}
