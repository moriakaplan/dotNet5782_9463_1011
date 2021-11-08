using System;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// לקוח
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
                Location coordinate = new Location { Longi = Longitude, Latti = Lattitude };
                //Coordinate latti = new Coordinate { Value = Longitude, IsLongitude = false };
                return @$"customer #{Id}:
number- {Name},
phone- {Phone},
{coordinate}
";
            }
        }
    }
}