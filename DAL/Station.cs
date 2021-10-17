using System;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id { get; set; }
            public int Name { get; set; }
            public Double Longitude { get; set; }
            public Double Lattitude { get; set; }
            public int ChargeSlots { get; set; }
        }
    }
}