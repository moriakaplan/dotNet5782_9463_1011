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
            //creates a new station in the data level
            IDAL.DO.Customer dCustomer = new IDAL.DO.Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                Lattitude = customer.Location.Latti,
                Longitude = customer.Location.Longi,
                Phone = customer.Phone
            };
            //try
            //{
               dl.AddCustomerToTheList(dCustomer);
            //add the new station to the list in the data level
            // }
            // catch(Exception ex)
            //{
            //    throw new ExistIdException(ex.Message, ex)
            // }
        }
        void UpdateCustomer(int customerId, string name, string phone)//לשאול אנשים
        {
            IDAL.DO.Customer dCustomer = dl.DisplayCustomer(customerId);


        }
        public Customer DisplayCustomer(int customerId)
        {
            IDAL.DO.Customer dCustomer = dl.DisplayCustomer(customerId);
            Customer bCustomer = new Customer();
            bCustomer.Id = dCustomer.Id;
            bCustomer.Name = dCustomer.Name;
            bCustomer.Phone = dCustomer.Phone;
            bCustomer.Location = new Location() { Latti = dCustomer.Lattitude, Longi = dCustomer.Lattitude };
            bCustomer.parcelFrom = getCustomerParcelFrom(customerId);
            bCustomer.parcelTo = getCustomerParcelTo(customerId);
            return bCustomer;
        }
        IEnumerable<CustomerToList> DisplayListOfCustomers()
        {
            return null;
        }
    }
}
