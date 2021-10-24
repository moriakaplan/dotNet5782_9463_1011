using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject
    {
        public void addStation(Station st)
        {
            DataSource.stations.Add(st);
        }
    }
}
