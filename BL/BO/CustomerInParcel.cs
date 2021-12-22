using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// customer in parcel
    /// </summary>
    public class CustomerInParcel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $@"
    Id #{Id}:
    name- {Name}";
        }
    }
}
