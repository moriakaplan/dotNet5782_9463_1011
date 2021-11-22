using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL
    {
        public List<DroneToList> lstdrn;
        public IDal dl; 
        public BL()
        {
            dl= new DalObject.DalObject();
            lstdrn= (List<DroneToList>)dl.DisplayListOfDrones();
            initializeDrone();
        }
        private void initializeDrone()
        {

        }
    }
}
