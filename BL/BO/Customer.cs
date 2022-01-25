using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// customer
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public IEnumerable<ParcelInCustomer> ParcelsFrom { get; set; }
        public IEnumerable<ParcelInCustomer> ParcelsTo { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id - {Id}, \n";
            result += $"Name - { Name},\n";
            result += $"Phone - { Phone},\n";
            result += $"Location - { Location},\n";
            result += $"parcels From The Customer- { ParcelsFrom},\n";
            result += $"parcels To The Customer- { ParcelsTo}.\n";

            return result;
          }
    }
}
