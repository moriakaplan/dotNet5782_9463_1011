using System;
using System.Runtime.Serialization;

namespace IBL
{
    [Serializable]
    public class NotExistIDExeption : Exception
    {
        private string message;
        private string v;

        public NotExistIDExeption()
        {
        }

        public NotExistIDExeption(string message) : base(message)
        {
        }

        public NotExistIDExeption(string message, string v)
        {
            this.message = message;
            this.v = v;
        }

        public NotExistIDExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotExistIDExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}