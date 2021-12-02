using System;
using System.Runtime.Serialization;

namespace IBL
{
    [Serializable]
    public class TransferExeption : Exception
    {
        public TransferExeption()
        {
        }

        public TransferExeption(string message) : base(message)
        {
        }

        public TransferExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TransferExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}