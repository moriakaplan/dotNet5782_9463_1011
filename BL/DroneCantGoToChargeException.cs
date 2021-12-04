using System;
using System.Runtime.Serialization;

namespace IBL
{
    [Serializable]
    public class DroneCantGoToChargeException : Exception
    {
        public DroneCantGoToChargeException()
        {
        }

        public DroneCantGoToChargeException(string message) : base(message)
        {
        }

        public DroneCantGoToChargeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneCantGoToChargeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string message { get; set; }
    }
}