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
        public int numOfParcelsThatGot { get; set; } //חבילות שהרחפן קיבל. סך הכל או שלא נשלחו? 
    }
}
