using System;
using System.Runtime.Serialization;

namespace BLApi
{
    [Serializable]
    public class NotExistIDException : Exception
    {
        private string message;
        private string v;

        public NotExistIDException()
        {
        }

        public NotExistIDException(string message) : base(message)
        {
        }

        public NotExistIDException(string message, string v)
        {
            this.message = message;
            this.v = v;
        }

        public NotExistIDException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotExistIDException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}