using System;
using System.Runtime.Serialization;

namespace IBL
{
    [Serializable]
    public class DroneCantPickTheParcel : Exception
    {
        public DroneCantPickTheParcel()
        {
        }

        public DroneCantPickTheParcel(string message) : base(message)
        {
        }

        public DroneCantPickTheParcel(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneCantPickTheParcel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}