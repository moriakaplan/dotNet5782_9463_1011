﻿using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    public class CustomerException : Exception
    {
        public CustomerException()
        {
        }

        public CustomerException(string message) : base(message)
        {
        }

        public CustomerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}