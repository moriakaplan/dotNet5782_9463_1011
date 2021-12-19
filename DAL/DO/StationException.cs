using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    public class StationException : Exception
    {
        public StationException()
        {
        }

        public StationException(string message) : base(message)
        {
        }

        public StationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}