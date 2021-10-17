using System;

namespace Parcel
{
    struct Parcel
    {
        public int Id { get; set; }
        public int Senderld { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DateTime Requested { get; set; }
        public int Droneld { get; set; }
        public DateTime Scheduled { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }


    }
}
