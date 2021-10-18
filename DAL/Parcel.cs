using System;

namespace IDAL
{
    namespace DO
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
            public override string ToString()
            {
<<<<<<< HEAD
                return $"parcel #{Id}: ";
=======
                return $"parcel #{Id}: sender ID- {Senderld}, target ID- {TargetId}, weight- {Weight}, priority- {Priority}, requested- {Requested} ,droneld- {Droneld}, scheduled- {Scheduled}, picked up- {PickedUp}, delivered- {Delivered}";
>>>>>>> f939132c4cf4741de2253fbd47212e9502de9821
            }
        }
    }
}