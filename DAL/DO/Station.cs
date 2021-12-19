using System;


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
        public int ChargeSlots { get; set; }

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
