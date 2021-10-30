using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public class Coordinate
        {
            public double Value { set; get; }
            public bool IsLongitude { set; get; }
            //public Coordinate(double val, bool isLongitude)
            //{
            //    Value = val;
            //    IsLongitude = isLongitude;
            //}

            //private String LongitudeSexagesimalCoordinates(double longitude)
            //{
            //    char direction = 'N';
            //    string result;
            //    double temp, minutes, seconds;
            //    if (longitude < 0)
            //    {
            //        longitude = -longitude;
            //        direction = 'S';
            //    }
            //    temp = (longitude - (int)longitude) * 60;
            //    minutes = (int)temp;
            //    temp -= minutes;
            //    seconds = temp * 60;
            //    result = (int)longitude + "° " + minutes + "' " + seconds + "'' " + direction;
            //    return result;
            //}
            //private String LattitudeSexagesimalCoordinates(double lattitude)
            //{
            //    char direction = 'E';
            //    string result;
            //    double temp, minutes, seconds;
            //    if (lattitude < 0)
            //    {
            //        lattitude = -lattitude;
            //        direction = 'W';
            //    }
            //    temp = (lattitude - (int)lattitude) * 60;
            //    minutes = (int)temp;
            //    temp -= minutes;
            //    seconds = temp * 60;
            //    result = (int)lattitude + "° " + minutes + "' " + seconds + "'' " + direction;
            //    return result;
            //}
            private String SexagesimalCoordinates()
            {
                char direction;
                if (IsLongitude) 
                    direction = 'N';
                else 
                    direction = 'E';
                string result;
                double temp, minutes, seconds;
                if (Value < 0)
                {
                    Value = -Value;
                    if (IsLongitude) 
                        direction = 'S';
                    else 
                        direction = 'W';
                }
                temp = (Value - (int)Value) * 60;
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
