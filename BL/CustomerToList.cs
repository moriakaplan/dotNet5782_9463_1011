using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerToList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string phone { get; set; }
        public int numOfParcelsDelivered { get; set; } //חבילות שהרחפן שלח וסופקו ללקוח
        public int numOfParcelsSentAndNotDelivered { get; set; } //חבילות שהרחפן שלח ולא סופקו ללקוח
        public int numOfParcelsInTheWay { get; set; } //חבילות שבדרך ללקוח
        public int numOfParclRecived { get; set; } //חבילות שהרחפן קיבל. סך הכל או שלא נשלחו? 
        public override string ToString()
        {
            return $@"Id #{Id}:
name- {Name},
parcels that the drone delivered to the customers- {numOfParcelsDelivered},
parcels that the drone sent and did not deliver to the customers- {numOfParcelsSentAndNotDelivered},
parcels on the way to the customer- {numOfParcelsInTheWay},
parcels the drone received- {numOfParclRecived}
";
        }
    }
}
