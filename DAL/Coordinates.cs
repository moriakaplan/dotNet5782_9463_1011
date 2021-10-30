using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        internal class Coordinates
        {
            private static String LongitudeSexagesimalCoordinates(double longitude)
            {
                char direction = 'N';
                string result;
                double temp, minutes, seconds;
                if (longitude < 0)
                {
                    longitude = -longitude;
                    direction = 'S';
                }
                temp = (longitude - (int)longitude) * 60;
                minutes = (int)temp;
                temp -= minutes;
                seconds = temp * 60;
                result = (int)longitude + "° " + minutes + "' " + seconds + "'' " + direction;
                return result;
            }
            private static String LattitudeSexagesimalCoordinates(double lattitude)
            {
                char direction = 'E';
                string result;
                double temp, minutes, seconds;
                if (lattitude < 0)
                {
                    lattitude = -lattitude;
                    direction = 'W';
                }
                temp = (lattitude - (int)lattitude) * 60;
                minutes = (int)temp;
                temp -= minutes;
                seconds = temp * 60;
                result = (int)lattitude + "° " + minutes + "' " + seconds + "'' " + direction;
                return result;
            }
        }
    }
}
