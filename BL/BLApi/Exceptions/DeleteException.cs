using System;
using System.Runtime.Serialization;

namespace BLApi
{
    [Serializable]
    public class DeleteException : Exception
    {
        private string message;
        private string v;

        public DeleteException()
        {
        }

        public DeleteException(string message) : base(message)
        {
        }

        public DeleteException(string message, string v)
        {
            this.message = message;
            this.v = v;
        }

        public DeleteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}