using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;


namespace BL
{
    [Serializable]
    class BlException : Exception
    {
        public BlException() : base()
        {

        }

        public BlException(string message) : base(message)
        {

        }

        public BlException(string message, Exception innerException) : base(message, innerException)
        {

        }
        protected BlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
        public override string ToString()
        {
            return "";
        }
    }
}
