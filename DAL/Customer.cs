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
            private Coordinate longi;
            private Coordinate latti;
            
            public double Longitude//bonus 1
            {
                get { return longi.Value; }
                set
                {
                    longi = new Coordinate();
                    longi.Value = value;
                    longi.IsLongitude = true;
                }
            }
            public double Lattitude//bonus 1
            {
                get { return latti.Value; }
                set
                {
                    latti = new Coordinate();
                    latti.Value = value;
                    latti.IsLongitude = false;
                }
            }
            public override string ToString()
            {
                return @$"customer #{Id}:
number- {Name},
phone- {Phone},
longitude- {longi},
lattitude- {latti}
";
            }
        }
    }
}