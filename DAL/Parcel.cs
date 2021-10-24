using System;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// חבילה 
        /// </summary>
        public struct Parcel
        {
            public int Id { get; set; }
            public int Senderld { get; set; }
            public int TargetId { get; set; }
            public int Droneld { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; } //רגיל, מהיר, חירום
            public DateTime Requested { get; set; } //יצירת החבילה
            public DateTime Scheduled { get; set; } //שיוך לרחפן
            public DateTime PickedUp { get; set; }  //איסוף מהשולח
            public DateTime Delivered { get; set; } //מסירה ליעד
            public override string ToString()
            {
                return $"parcel #{Id}: sender ID- {Senderld}, target ID- {TargetId}, weight- {Weight}, priority- {Priority}, requested- {Requested} ,droneld- {Droneld}, scheduled- {Scheduled}, picked up- {PickedUp}, delivered- {Delivered}";
            }
        }
    }
}