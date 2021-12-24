using System;


namespace DO
{
    /// <summary>
    /// drone in charge
    /// </summary>
    public struct DroneCharge
    {
        public DroneCharge(int droneId, int stationId, DateTime time)
        {
            DroneId = droneId;
            StationId = stationId;
            StartedChargeTime = time;
        }
        public int DroneId { get; set; }
        public int StationId { get; set; }
        public DateTime StartedChargeTime { get; set; }
        public override string ToString()
        {
            return @$"DroneId:{DroneId}, 
StationId:{StationId}
";
        }
    }
}

