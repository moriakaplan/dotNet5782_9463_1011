﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Location
    {
        public double Longitude { get; init; }
        public double Lattitude { get; init; }

        public override string ToString()
        {
            return @$"longitude- {Longitude},
lattitude- {Lattitude}.
";


        }
    }
}
