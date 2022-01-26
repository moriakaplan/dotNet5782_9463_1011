using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using System.Xml.Linq;
using System.Runtime.CompilerServices;


//with linq to xml

namespace Dal
{
    public partial class DalXml
    {
        /// <summary>
        /// add customer
        /// </summary>
        /// <param name="cus"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer cus)
        {
            if (customersRoot.Elements().Any(item => item.Element("Id").Value == cus.Id.ToString()))
                throw new CustomerException($"id: {cus.Id} already exist"); 
            customersRoot.Add(ConvertCus(cus));
            customersRoot.Save(customersPath);
        }

        /// <summary>
        /// return the customer with the requested id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerId)
        {
            XElement cus = customersRoot.Elements().Where(item => int.Parse(item.Element("Id").Value) == customerId).SingleOrDefault(); 
            if (cus==null)
            {
                throw new CustomerException($"id: {customerId} does not exist");
            }
            return ConvertCus(cus);
        }

        /// <summary>
        /// returns all the customers
        /// </summary>
        /// <param name="pre"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomersList(Predicate<Customer> pre)
        {
            if (pre != null)
                return customersRoot.Elements().Select(x => ConvertCus(x)).ToList().FindAll(pre);
            else
                return customersRoot.Elements().Select(x => ConvertCus(x));
        }

        /// <summary>
        /// delete a customer
        /// </summary>
        /// <param name="customerId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
