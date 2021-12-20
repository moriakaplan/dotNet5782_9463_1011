using System;
using System.Runtime.Serialization;

namespace BLApi
{
    [Serializable]
    public class ThereNotGoodParcelToTakeException : Exception
    {
        public ThereNotGoodParcelToTakeException()
        {
        }

        public ThereNotGoodParcelToTakeException(string message) : base(message)
        {
        }

        public ThereNotGoodParcelToTakeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ThereNotGoodParcelToTakeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}