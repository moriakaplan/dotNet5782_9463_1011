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
    public class Coordinate
    {
        public double Value { get; init; }
        public bool IsLongitude { get; init; }
        public override string ToString()
        {
            return SexagesimalCoordinates();
        }

        /// <summary>
        /// Returns a Sexagesimal representation of the coordinate
        /// </summary>
        /// <returns></returns>
        private String SexagesimalCoordinates()
        {
            double val;
            char direction;
            double minutes, seconds;
            if (IsLongitude)
                direction = 'N';
            else
                direction = 'E';
            string result;
            if (Value < 0)
            {
                val = -Value;
                if (IsLongitude)
                    direction = 'S';
                else
                    direction = 'W';
            }
            else { val = Value; }
            val = (val - (int)val) * 60;
            minutes = (int)val;
            val -= minutes;
            seconds = val * 60;
            //result = (int)val + "° " + minutes + "' " + string.Format($"{seconds:0.000}") + "'' " + direction;
            result = $"{(int)val}° {minutes}' {seconds:0.000}'' {direction}";
            return result;
        }



    }
}

