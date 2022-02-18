using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    public class XMLFileException : Exception
    {
        public string FileName { get; set; } = "<Unknown>";

        public Exception Inner { get; set; }

        public XMLFileException(string fileName, string message, Exception inner):base(message)
        {
            FileName = fileName;
            Inner = inner;
        }

        public override string ToString() => Message;


        protected XMLFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}