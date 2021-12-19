using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    /// <summary>
    /// The class represents coordinates (for the first bonus).
    /// </summary>
    internal class Location
    {
        public double Longi { get; init; }
        public double Latti { get; init; }

        public override string ToString()
        {
            return @$"longitude- {SexagesimalCoordinates(Longi, true)},
lattitude- {SexagesimalCoordinates(Latti, false)}";
        }

        private String SexagesimalCoordinates(double value, bool isLongitude)
        {
            char direction;
            double minutes, seconds;
            if (isLongitude)
                direction = 'N';
            else
                direction = 'E';
            string result;
            if (value < 0)
            {
                value = -value;
                if (isLongitude)
                    direction = 'S';
                else
                    direction = 'W';
            }
            value = (value - (int)value) * 60;
            minutes = (int)value;
            value -= minutes;
            seconds = value * 60;
            result = (int)value + "° " + minutes + "' " + seconds + "'' " + direction;
            return result;
        }

        //public double Value { set; get; }
        //public bool IsLongitude { set; get; }
        /// <summary>
        /// Returns a Sexagesimal representation of the coordinate
        /// </summary>
        /// <returns></returns>


    }
}

