using System;


namespace DO
{
    /// <summary>
    /// parcels 
    /// </summary>
    public struct Parcel
    {
        public int Id { get; set; }
        public int Senderld { get; set; }
        public int TargetId { get; set; }
        public int Droneld { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; } 
        public DateTime? CreateTime { get; set; } 
        public DateTime? AssociateTime { get; set; } 
        public DateTime? PickUpTime { get; set; }  
        public DateTime? DeliverTime { get; set; }  
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
