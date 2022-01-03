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
            customersRoot.Add(ConvertSomething(cus, "customer"));
        }

        public Customer DisplayCustomer(int customerId)
        {
            XElement cus = customersRoot.Elements().Where(item => item.Element("Id").Value == customerId.ToString()).FirstOrDefault();
            if (cus == null)
                throw new CustomerException($"id: {customerId} does not exist");
            return (Customer)ConvertSomething(cus, typeof(Customer));
        }
        public IEnumerable<Customer> DisplayListOfCustomers(Predicate<Customer> pre)
        {
            if (pre != null)
                return customersRoot.Elements().Select(x => (Customer)ConvertSomething(x, typeof(Customer))).ToList().FindAll(pre);
            else
                return customersRoot.Elements().Select(x => (Customer)ConvertSomething(x, typeof(Customer))).ToList();
        }

        public void DeleteCustomer(int customerId)
        {
            if (customersRoot.Elements().Any(item => item.Attribute("id").Value == customerId.ToString()))
            {
                throw new CustomerException($"id: {customerId} does not exist");
            }
            customersRoot.Elements().Where(item => item.Attribute("id").Value == customerId.ToString()).Remove();
        }
    }
}
