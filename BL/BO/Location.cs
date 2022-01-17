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
            return $"{longi},\n{Latti}";
        }

        ///// <summary>
        ///// Returns a Sexagesimal representation of the coordinate
        ///// </summary>
        ///// <returns></returns>
        //private String SexagesimalCoordinates(double value, bool isLongitude)
        //{
        //    char direction;
        //    double minutes, seconds;
        //    if (isLongitude)
        //        direction = 'N';
        //    else
        //        direction = 'E';
        //    string result;
        //    if (value < 0)
        //    {
        //        value = -value;
        //        if (isLongitude)
        //            direction = 'S';
        //        else
        //            direction = 'W';
        //    }
        //    value = (value - (int)value) * 60;
        //    minutes = (int)value;
        //    value -= minutes;
        //    seconds = value * 60;
        //    result = (int)value + "° " + minutes + "' " + string.Format($"{seconds:0.000}") + "'' " + direction;
        //    return result;
        //}
    }
}
