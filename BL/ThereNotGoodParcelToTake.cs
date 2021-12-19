using System;
using System.Runtime.Serialization;

namespace BLApi
{
    [Serializable]
    public class ThereNotGoodParcelToTake : Exception
    {
        public ThereNotGoodParcelToTake()
        {
        }

        public ThereNotGoodParcelToTake(string message) : base(message)
        {
        }

        public ThereNotGoodParcelToTake(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ThereNotGoodParcelToTake(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}