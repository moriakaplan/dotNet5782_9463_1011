using System;
using System.Runtime.Serialization;

namespace IBL
{
    [Serializable]
    public class DroneCantReleaseFromChargeExeption : Exception
    {
        public DroneCantReleaseFromChargeExeption()
        {
        }

        public DroneCantReleaseFromChargeExeption(string message) : base(message)
        {
        }

        public DroneCantReleaseFromChargeExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneCantReleaseFromChargeExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string message { get; set; }
    }
}