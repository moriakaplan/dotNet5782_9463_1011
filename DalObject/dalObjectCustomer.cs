using DO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Dal
{
    internal partial class DalObject
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer customer)
        {
            if (DataSource.customers.Exists(item => item.Id == customer.Id)) 
                throw new CustomerException($"id: {customer.Id} already exist"); 
            DataSource.customers.Add(customer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerId)
        {
            if (!DataSource.customers.Exists(item => item.Id == customerId)) 
                throw new CustomerException($"id: {customerId} does not exist");
            return DataSource.customers.Find(item => item.Id == customerId);
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomersList(Predicate<Customer> pre)
        {
            List<Customer> result = new List<Customer>(DataSource.customers);
            if (pre == null) return result;
            return result.FindAll(pre);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(int customerId)
        {
            try
            {
                DataSource.customers.Remove(GetCustomer(customerId));
            }
            catch (ArgumentNullException)
            {
                throw new CustomerException($"id: {customerId} does not exist");
            }
        }
    }
}