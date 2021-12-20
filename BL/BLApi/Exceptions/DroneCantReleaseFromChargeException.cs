using System;
using System.Runtime.Serialization;

namespace BLApi
{
    [Serializable]
    public class DroneCantReleaseFromChargeException : Exception
    {
        public DroneCantReleaseFromChargeException()
        {
        }

        public DroneCantReleaseFromChargeException(string message) : base(message)
        {
        }

        public DroneCantReleaseFromChargeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneCantReleaseFromChargeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string message { get; set; }
    }
}