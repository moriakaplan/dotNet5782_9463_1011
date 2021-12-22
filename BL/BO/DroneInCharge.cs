using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// drone in charge
    /// </summary>
    public class DroneInCharge
    {
        public int Id { get; set; }
        public double Battery { get; set; }
        public override string ToString()
        {
            return @$"drone #{Id}:
battery- {string.Format($"{Battery:0.000}")}.
";
        }
    }
}
