using System;

namespace ImmGate.Base.Exceptions
{
    public class TypeNotFoundException : Exception
    {
        public TypeNotFoundException(string message) : base(message)
        {

        }
    }
}