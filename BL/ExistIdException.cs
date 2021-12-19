using System;
using System.Runtime.Serialization;

namespace BLApi
{
    [Serializable]
    public class ExistIdException : Exception
    {
        private string message;
        private string v;

        public ExistIdException()
        {
        }

        public ExistIdException(string message) : base(message)
        {
        }

        public ExistIdException(string message, string v)
        {
            this.message = message;
            this.v = v;
        }

        public ExistIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExistIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}