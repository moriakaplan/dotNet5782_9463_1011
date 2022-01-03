using System;
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
            customersRoot.Add(ConvertCus(cus));
            customersRoot.Save(customersPath);
        }
        public Customer DisplayCustomer(int customerId)
        {
            XElement? cus = customersRoot.Elements().Where(item => int.Parse(item.Element("Id").Value) == customerId).SingleOrDefault(); 
            if (cus==null)
            {
                throw new CustomerException($"id: {customerId} does not exist");
            }
            return ConvertCus(cus);
        }

        public IEnumerable<Customer> DisplayListOfCustomers(Predicate<Customer> pre)
        {
            if (pre != null)
                return customersRoot.Elements().Select(x => ConvertCus(x)).ToList().FindAll(pre);
            else
                return customersRoot.Elements().Select(x => ConvertCus(x));
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
