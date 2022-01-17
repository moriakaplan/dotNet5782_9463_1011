using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum WeightCategories { Light, Medium, Heavy };
    public enum Priorities { Regular, Fast, Emergency };
    public enum ParcelStatus { Created, Associated, PickedUp, Delivered };
    public enum DroneStatus 
    {
        Maintenance,
        Available,
        Associated, 
        Delivery
    };
}
