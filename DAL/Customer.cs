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
            //public Customer(int id, string name, string phone, double longitude, double lattitude)
            //{
            //    Id = id;
            //    Name = name;
            //    Phone = phone;
            //    Longitude = longitude;
            //    Lattitude = lattitude;
            //}
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            private Coordinate longi;
            private Coordinate latti;
            public double Longitude
            {
                get { return longi.Value; }
                set
                {
                    longi = new Coordinate();
                    longi.Value = value;
                    longi.IsLongitude = true;
                }
            }
            public double Lattitude
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
                return $"customer #{Id}: number- {Name}, phone- {Phone}, " +
                    $"longitude- {longi}, lattitude- {latti}";
            }
        }
    }
}