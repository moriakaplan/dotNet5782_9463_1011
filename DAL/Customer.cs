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
                return $"customer #{Id}: number- {Name},phone- {Phone}, longitude- {Longitude}, lattitude- {Lattitude}";
            }
        }
    }
}