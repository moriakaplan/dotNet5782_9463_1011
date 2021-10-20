using System;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// טעינת סוללת רחפן
        /// </summary>
        public struct DroneCharge
        {
            public int DroneId { get; set; }
            public int StationId { get; set; }
            public override string ToString()
            {
                return $"DroneId:{DroneId}, StationId:{StationId}";
            }
        }
    }
}
