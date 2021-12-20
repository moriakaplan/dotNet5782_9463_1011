﻿using System;
using System.Runtime.Serialization;

namespace BLApi
{
    [Serializable]
    public class TransferException : Exception
    {
        public TransferException()
        {
        }

        public TransferException(string message) : base(message)
        {
        }

        public TransferException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TransferException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}