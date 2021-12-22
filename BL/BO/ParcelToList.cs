using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// parcek to the list
    /// </summary>
    public class ParcelToList
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string TargetName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; } //Regular, Fast, Emergency
        public ParcelStatus Status { get; set; } 
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
