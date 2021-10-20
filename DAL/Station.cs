using System;

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
            public Double Longitude { get; set; }
            public Double Lattitude { get; set; }
            public int ChargeSlots { get; set; }
            public override string ToString()
            {
                return $"station #{Id}: number- {Name}, longitude- {Longitude}, lattitude- {Lattitude}, charge slots- {ChargeSlots}";
            }
        }
    }
}