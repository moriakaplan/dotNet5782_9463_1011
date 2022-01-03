using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    public partial class DalXml
    {
        public void AddCustomerToTheList(Customer customer)
        {
            //if (DataSource.customers.Exists(item => item.Id == customer.Id))
            //    throw new CustomerException($"id: {customer.Id} already exist"); //it suppose to be this type of exception????**** 
            //DataSource.customers.Add(customer);
        }
        public Customer DisplayCustomer(int customerId)
        {
            //Customer? cus = DataSource.customers.Find(item => item.Id == customerId);
            //if (cus == null)
            //    throw new CustomerException($"id: {customerId} does not exist");
            //return (Customer)cus;
        }
        public IEnumerable<Customer> DisplayListOfCustomers(Predicate<Customer> pre)
        {
            //List<Customer> result = new List<Customer>(DataSource.customers);
            //if (pre == null) return result;
            //return result.FindAll(pre);
        }
        public void DeleteCustomer(int customerId)
        {
            //try
            //{
            //    DataSource.customers.Remove(DisplayCustomer(customerId));
            //}
            //catch (ArgumentNullException)
            //{
            //    throw new CustomerException($"id: {customerId} does not exist");
            //}
        }
    }
}
