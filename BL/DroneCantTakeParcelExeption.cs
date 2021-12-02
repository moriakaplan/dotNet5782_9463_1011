using System;
using System.Runtime.Serialization;

namespace IBL
{
    [Serializable]
    public class DroneCantTakeParcelExeption : Exception
    {
        public DroneCantTakeParcelExeption()
        {
        }

        public DroneCantTakeParcelExeption(string message) : base(message)
        {
        }

        public DroneCantTakeParcelExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneCantTakeParcelExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}