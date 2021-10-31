using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// The class represents coordinates (for the first bonus).
        /// </summary>
        public class Coordinate
        {
            public double Value { set; get; }
            public bool IsLongitude { set; get; }
            /// <summary>
            /// Returns a Sexagesimal representation of the coordinate
            /// </summary>
            /// <returns></returns>
            private String SexagesimalCoordinates()
            {
                char direction;
                double temp = Value, minutes, seconds;
                if (IsLongitude) 
                    direction = 'N';
                else 
                    direction = 'E';
                string result;
                if (temp < 0)
                {
                    temp = -temp;
                    if (IsLongitude) 
                        direction = 'S';
                    else 
                        direction = 'W';
                }
                temp = (temp - (int)temp) * 60;
                minutes = (int)temp;
                temp -= minutes;
                seconds = temp * 60;
                result = (int)Value + "° " + minutes + "' " + seconds + "'' " + direction;
                return result;
            }
            public override string ToString()
            {
                return SexagesimalCoordinates();
            }
        }
    }
}
