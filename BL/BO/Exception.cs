using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;


namespace BO
{
    [Serializable]
    public class ObjectNotExistException : Exception
    {
        public ObjectNotExistException() : base()
        {
        }
        public ObjectNotExistException(string message) : base(message)
        {
        }
        public ObjectNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected ObjectNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public override string ToString()
        {
            return (Message != null)? Message:"The specific object is not exist";
        }
    }




    public class ObjectAlreadyExistException : Exception
    {
        public ObjectAlreadyExistException() : base()
        {
        }
        public ObjectAlreadyExistException(string message) : base(message)
        {
        }
        public ObjectAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected ObjectAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public override string ToString()
        {
            return (Message != null) ? Message : "The specific object is already exist";
        }
    }



    public class NoChangesToUpdateException : Exception
    {
        public NoChangesToUpdateException() : base()
        {
        }
        public NoChangesToUpdateException(string message) : base(message)
        {
        }
        public NoChangesToUpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected NoChangesToUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public override string ToString()
        {
            return (Message != null) ? Message : "No Changes To Update";
        }
    }



    public class ObjectNotAvailableForActionException : Exception
    {
        public ObjectNotAvailableForActionException() : base()
        {
        }
        public ObjectNotAvailableForActionException(string message) : base(message)
        {
        }
        public ObjectNotAvailableForActionException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected ObjectNotAvailableForActionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public override string ToString()
        {
            return (Message != null) ? Message : "Object Not Available For Action";
        }
    }
}
