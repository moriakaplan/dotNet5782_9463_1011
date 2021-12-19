using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelToList
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; } //רגיל, מהיר, חירום
        public ParcelStatus Status { get; set; } //הוגדרה(נוצרה), שויכה, נאספקה על ידי הרחפן, סופקה ללקוח
        public override string ToString()
        {
            return @$"Id: #{Id}:
sender name- {SenderName},
reciver name- {TargetName},
weight- {Weight},
priority- {Priority},
status- {Status}.
";
        }
    }
}
