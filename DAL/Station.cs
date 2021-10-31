﻿using System;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// תחנות בסיס
        /// </summary>
        public struct Station
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            //private Coordinate longi;
            //private Coordinate latti;
            //public double Longitude//bonus 1
            //{
            //    get { return longi.Value; }
            //    set { 
            //        longi = new Coordinate();
            //        longi.Value = value;
            //        longi.IsLongitude = false;
            //    }
            //}
            //public double Lattitude//bonus 1
            //{
            //    get { return latti.Value; }
            //    set
            //    {
            //        latti = new Coordinate();
            //        latti.Value = value;
            //        latti.IsLongitude = false;
            //    }
            //}
            public int ChargeSlots { get; set; }
            public override string ToString()
            {
                Coordinate longi = new Coordinate { Value = Longitude, IsLongitude = true };
                Coordinate latti = new Coordinate { Value = Longitude, IsLongitude = false };
                return @$"station #{Id}:
number- {Name}, 
longitude- {longi},
lattitude- {latti}, 
charge slots- {ChargeSlots}";
            }
        }
    }
}