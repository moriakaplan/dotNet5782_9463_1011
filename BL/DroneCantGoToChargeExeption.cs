using System;
using System.Runtime.Serialization;

namespace IBL
{
    [Serializable]
    internal class DroneCantGoToChargeExeption : Exception
    {
        public DroneCantGoToChargeExeption()
        {
        }

        public DroneCantGoToChargeExeption(string message) : base(message)
        {
        }

        public DroneCantGoToChargeExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneCantGoToChargeExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string message { get; set; }
    }
}