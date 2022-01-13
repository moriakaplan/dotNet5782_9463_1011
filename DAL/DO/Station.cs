using System;


namespace DO
{
    /// <summary>
    /// base station
    /// </summary>
    public struct Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public int ChargeSlots { get; set; }
        public override string ToString()
        {
            Coordinate longi = new Coordinate { Value = Longitude, IsLongitude = true };
            Coordinate latti = new Coordinate { Value = Lattitude, IsLongitude = false };
            return @$"station #{Id}:
number- {Name}, 
charge slots- {ChargeSlots},
longitude- {longi},
lattitude- {latti}
";
        }
    }
}
