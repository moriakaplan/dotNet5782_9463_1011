using System;


namespace DO
{
    /// <summary>
    /// חבילות 
    /// </summary>
    public struct Parcel
    {
        public int Id { get; set; }
        public int Senderld { get; set; }
        public int TargetId { get; set; }
        public int Droneld { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; } //רגיל, מהיר, חירום
        public DateTime? CreateTime { get; set; } //יצירת החבילה
        public DateTime? AssociateTime { get; set; } //שיוך לרחפן
        public DateTime? PickUpTime { get; set; }  //איסוף מהשולח
        public DateTime? DeliverTime { get; set; } //מסירה ליעד
        public override string ToString()
        {
            return @$"parcel #{Id}: 
sender ID- {Senderld},
target ID- {TargetId},
weight- {Weight},
priority- {Priority},
requested- {CreateTime},
droneld- {Droneld}, 
created- {CreateTime},
scheduled- {AssociateTime}, 
picked up- {PickUpTime},
delivered- {DeliverTime}
";
        }
    }
}
