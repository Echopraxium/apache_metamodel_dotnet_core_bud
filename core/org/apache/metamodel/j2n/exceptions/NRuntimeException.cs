using System;

namespace org.apache.metamodel.j2n.exceptions
{
    public class NRuntimeException : NSystemException
    {
        public NRuntimeException()
        {
        } // constructor

        public NRuntimeException(string msg) : base(msg)
        {
        } // constructor

        public NRuntimeException(string msg, Exception inner) : base(msg, inner)
        {
        } // constructor
    } // NParseException class
} // org.apache.metamodel.j2n.exceptions namespace
