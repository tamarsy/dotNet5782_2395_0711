using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    public class XMLFileException : Exception
    {
        public string FileName { get; set; } = "<Unknown>";

        public string Message { get; set; } 

        public Exception Inner { get; set; }

        public XMLFileException(string fileName, string message, Exception inner)
        {
            FileName = fileName;
            Message = message;
            Inner = inner;
        }

        public override string ToString() => Message;


        protected XMLFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}