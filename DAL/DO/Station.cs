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
            Location coordinate = new Location { Longi = Longitude, Latti = Lattitude };
            return @$"station #{Id}:
number- {Name}, 
{coordinate}, 
charge slots- {ChargeSlots}
";
        }
    }
}
