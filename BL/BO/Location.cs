using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Location
    {
        public double Longi { get; init; }
        public double Latti { get; init; }

        public override string ToString()
        {
            return @$"longitude- {Longi}, lattitude- {Latti}";
        }
    }
}
