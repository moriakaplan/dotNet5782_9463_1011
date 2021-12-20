using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLApi
{
    /// <summary>
    /// give an object of the class BL (that implement the interface IBL).
    /// </summary>
    public static class BLFactory
    {
        /// <summary>
        /// return the BL object, that implement the interface IBL.
        /// </summary>
        /// <returns></returns>
        public static IBL GetBl()
        {
            return BL.BL.Instance;
        }
    }
}
