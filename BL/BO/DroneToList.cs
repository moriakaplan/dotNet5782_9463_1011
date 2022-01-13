using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// drone for the list
    /// </summary>
    public class DroneToList
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Battery { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public DroneStatus Status { get; set; }
        public Location CurrentLocation { get; set; }
        public int ParcelId { get; set; } //אם יש חבילה מועברת
        public override string ToString()
        {
            return $"   drone #{Id}: \n" +
                $"mode- {Model}, " +
                $"max weight- {MaxWeight}, " +
                $"battery- {string.Format($"{Battery:0.000}")}, " +
                $"status- {Status}, " +
                $"\ncurrent location- {CurrentLocation}, " +
                $"\nParcel Id- {ParcelId}.";

        }
    }
}
