using System;
using System.Reflection;

namespace Core.Exceptions
{
    public class AttributeNotFoundException : Exception
    {
        public AttributeNotFoundException(MemberInfo attributeType, MemberInfo classType)
            : base($"Attribute {attributeType.Name} not found for type {classType.Name}.")
        {
        }
    }
}