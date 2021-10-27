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
            //public Station(int id, string name, double longitude, double lattitude, int chargeSlots)
            //{
            //    Id = id;
            //    Name = name;
            //    Longitude = longitude;
            //    Lattitude = lattitude;
            //    ChargeSlots = chargeSlots;
            //}
            public int Id { get; set; }
            public string Name { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public int ChargeSlots { get; set; }
            public override string ToString()
            {
                return $"station #{Id}: number- {Name}, longitude- {Longitude}, lattitude- {Lattitude}, charge slots- {ChargeSlots}";
            }
        }
    }
}