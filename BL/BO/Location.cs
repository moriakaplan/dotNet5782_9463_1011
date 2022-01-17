using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BO
{
    /// <summary>
    /// locations
    /// </summary>
    public class Location
    {
        private Coordinate longi;
        private Coordinate latti;
        public double Longi 
        { 
            get => longi.Value; 
            init => longi = new Coordinate { Value = value, IsLongitude = true }; 
        }
        public double Latti
        {
            get => latti.Value; 
            init => latti = new Coordinate { Value = value, IsLongitude = false };
        }

        public override string ToString()
        {
            //return @$"longitude- {Longi}, lattitude- {Latti}";
            //return $"{SexagesimalCoordinates(Longi, true)}, \n{SexagesimalCoordinates(Latti, false)}";
            return $"{longi},\n{latti}";
        }
    }
}
