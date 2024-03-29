﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace DO
{
    /// <summary>
    ///drone 
    /// </summary>
    public struct Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }

        public override string ToString()
        {
            return @$"drone #{Id}:
model- {Model},
max weight- {MaxWeight}
";
            //                status - { Status},
            //battery - { Battery}
        }
    }
}


