﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using System.Xml.Linq;

//with linq to xml

namespace Dal
{
    public partial class DalXml
    {
        public void AddCustomerToTheList(Customer cus)
        {
            if (customersRoot.Elements().Any(item => item.Element("Id").Value == cus.Id.ToString()))
                throw new CustomerException($"id: {cus.Id} already exist"); //it suppose to be this type of exception????**** 
            customersRoot.Add(ConvertSomething(cus, "customer"));
        }
        public Customer DisplayCustomer(int customerId)
        {
            XElement cus;
            try { cus = customersRoot.Elements().Where(item => item.Element("Id").Value == customerId.ToString()).Single(); }
            catch (InvalidOperationException)
            {
                throw new CustomerException($"id: {customerId} does not exist");
            }
            return ConvertCus(cus);
        }

        public IEnumerable<Customer> DisplayListOfCustomers(Predicate<Customer> pre)
        {
            return customersRoot.Elements().Select(x=>(Customer)ConvertSomething(x, typeof(Customer))).ToList().FindAll(pre);
        }
        public void DeleteCustomer(int customerId)
        {
            if (customersRoot.Elements().Any(item => item.Element("Id").Value == customerId.ToString()))
            {
                throw new CustomerException($"id: {customerId} does not exist");
            }
            customersRoot.Elements().Where(item => item.Element("Id").Value == customerId.ToString()).Remove();
        }
    }
}
