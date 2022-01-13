using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;


namespace BL
{
    class Simulator
    {
        const double velocity = 2; //kilometers per second.
        const int stepTimer = 500; //miliseconds, half of second.
        public Simulator(BL blObject, int droneId, Action UpdateDisplayDelegate, Func<bool> checkStop)
        {
            while(!checkStop())
            {

            }
        }
    }
}
