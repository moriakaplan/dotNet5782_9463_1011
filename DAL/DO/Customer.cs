using System;


namespace DO
{
    /// <summary>
    /// Customer
    /// </summary>
    public struct Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public override string ToString()
        {
            Coordinate longi = new Coordinate { Value = Longitude, IsLongitude = true };
            Coordinate latti = new Coordinate { Value = Lattitude, IsLongitude = false };
            return @$"customer #{Id}:
number- {Name},
phone- {Phone},
longitude- {longi},
lattitude- {latti}
";
        }
    }
}
