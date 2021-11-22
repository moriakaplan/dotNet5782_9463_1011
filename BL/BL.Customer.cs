using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public partial class BL : IBL.Ibl
    {
        void AddCustomer(Customer customer)
        {

        }
        void UpdateCustomer(int id, params string[] args /*name and phone*/)//לשאול אנשים
        {

        }
        Customer DisplayCustomer(int customerId)
        {
            return null;
        }
        IEnumerable<CustomerToList> DisplayListOfCustomers()
        {
            return null;
        }
    }
}
