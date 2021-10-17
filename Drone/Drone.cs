using System;

namespace IDAL
{
    namespace DO
    {
            public struct Drone
            {
                public int Id { get; set; }
                public string model { get; set; }
                public WeightCategories MaxWeight { get; set; }
                public DroneStatuses Status { get; set; }
                public Double Battery { get; set; }
            }
    }
}

