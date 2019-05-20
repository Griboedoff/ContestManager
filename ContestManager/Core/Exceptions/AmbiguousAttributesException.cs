using System;

namespace Core.Exceptions
{
    public class AmbiguousAttributesException : Exception
    {
        public AmbiguousAttributesException(Type type) : base($"Ambiguous attributes number for property {type}.")
        {
        }
    }
}