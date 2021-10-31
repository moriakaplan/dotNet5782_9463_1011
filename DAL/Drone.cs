using System;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        ///רחפנים 
        /// </summary>
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status { get; set; }
            public double Battery { get; set; }
            public override string ToString()
            {
                return @$"drone #{Id}:
model- {Model},
max weight- {MaxWeight},
status- {Status},
battery- {Battery}";
            }
        }
    }
}

