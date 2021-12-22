using DO;
using DalApi;
using System;
using System.Collections.Generic;
namespace Dal
{
    internal partial class DalObject
    {
       
        public void AddCustomerToTheList(Customer customer)
        {
            if (DataSource.customers.Exists(item => item.Id == customer.Id)) 
                throw new CustomerException($"id: {customer.Id} already exist"); //it suppose to be this type of exception????**** 
            DataSource.customers.Add(customer);
        }
       
        public Customer DisplayCustomer(int customerId)
        {
            foreach (Customer item in DataSource.customers)
            {
                if (item.Id == customerId)
                    return item;
            }
            throw new CustomerException($"id: {customerId} does not exist");
            //Customer? cu= DataSource.customers.Find(item => item.Id == customerId);
            //if(cu==null) throw new CustomerException($"id: {customerId} does not exist");
            //return (Customer)cu;
        }
        
        public IEnumerable<Customer> DisplayListOfCustomers(Predicate<Customer> pre)
        {
            List<Customer> result = new List<Customer>(DataSource.customers);
            if (pre == null) return result;
            return result.FindAll(pre);
        }

        public void DeleteCustomer(int customerId)
        {
            try
            {
                DataSource.customers.Remove(DisplayCustomer(customerId));
            }
            catch (ArgumentNullException)
            {
                throw new CustomerException($"id: {customerId} does not exist");
            }
        }
    }
}