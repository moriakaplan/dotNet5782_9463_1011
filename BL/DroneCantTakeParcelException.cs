using System;
using System.Runtime.Serialization;

namespace IBL
{
    [Serializable]
    public class DroneCantTakeParcelException : Exception
    {
        public DroneCantTakeParcelException()
        {
        }

        public DroneCantTakeParcelException(string message) : base(message)
        {
        }

        public DroneCantTakeParcelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneCantTakeParcelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}