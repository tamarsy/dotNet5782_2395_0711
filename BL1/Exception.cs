using System;
using System.Collections.Generic;
using System.Text;

namespace IBL
{
    namespace IBL
    {
        class BlException : Exception
        {
            public BlException() : base()
            {
            }

            public BlException(string? message) : base(message)
            {

            }

            public BlException(string? message, Exception? innerException) : base(message, innerException)
            {

            }
        }
    }
}
