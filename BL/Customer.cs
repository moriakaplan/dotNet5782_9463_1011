using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
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
            return @$"Id #{Id}:
Name- {Name},
Phone- {Phone},
Location- {Location},
parcel In  customer- From The Customer- {parcelFrom},
parcel In  customer- To The Customer- {parcelTo}.
";

        }
    }
}
