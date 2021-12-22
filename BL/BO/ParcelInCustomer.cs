using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// parcel in customer
    /// </summary>
    public class ParcelInCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; } // Regular, Fast, Emergency
        public ParcelStatus Status { get; set; } //Created, Associated, PickedUp, Delivered
        public CustomerInParcel SenderOrTarget { get; set; }
        public override string ToString()
        {
            return @$"Id: #{Id}:
weight- {Weight},
priority- {Priority},
status- {Status},
Customer In Parcel- {SenderOrTarget}.
";//*מה זה אמור להיות השדה האחרון?
        }
    }
}
