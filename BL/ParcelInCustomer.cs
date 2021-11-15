using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; } //רגיל, מהיר, חירום
        public ParcelStatus Status { get; set; } //הוגדרה(נוצרה), שויכה, נאספקה על ידי הרחפן, סופקה ללקוח
        public CustomerInParcel SenderOrTarget { get; set; }
        public override string ToString()
        {

        }
    }
}
