using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerToList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string phone { get; set; }
        public int numOfParcelsDelivered { get; set; } //חבילות ששלח וסופקו ללקוח
        public int numOfParcelsSentAndNotDelivered { get; set; } //חבילות ששלח ולא סופקו ללקוח
        public int numOfParclReceived { get; set; } //חבילות שקיבל 
        public int numOfParcelsInTheWay { get; set; } //חבילות שבדרך ללקוח
        public override string ToString()
        {
            return $@"Id #{Id}:
name- {Name},
parcels that the drone delivered to the customers- {numOfParcelsDelivered},
parcels that the drone sent and did not deliver to the customers- {numOfParcelsSentAndNotDelivered},
parcels on the way to the customer- {numOfParcelsInTheWay},
parcels the drone received- {numOfParclReceived}
";
        }
    }
}
