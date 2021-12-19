using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public IEnumerable<ParcelInCustomer> parcelFrom { get; set; }
        public IEnumerable<ParcelInCustomer> parcelTo { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id - {Id}, \n";
            result += $"Name - { Name},\n";
            result += $"Phone - { Phone},\n";
            result += $"Location - { Location},\n";
            result += $"parcels From The Customer- { parcelFrom},\n";
            result += $"parcels To The Customer- { parcelTo}.\n";

            return result;
          }
    }
}
