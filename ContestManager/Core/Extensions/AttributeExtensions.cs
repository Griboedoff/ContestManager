using System;
using System.Reflection;
using Core.Exceptions;

namespace Core.Extensions
{
    public static class AttributeExtensions
    {
        public static TAttr GetAttribute<TAttr>(this Enum structValue) where TAttr : Attribute
        {
            var attributes = structValue.GetAllAttributes<TAttr>();
            if (attributes.Length > 1)
                throw new AmbiguousAttributesException(typeof (TAttr));
            if (attributes.Length == 1)
                return attributes[0];
            throw new AttributeNotFoundException(typeof (TAttr), structValue.GetType());
        }

        private static TAttr[] GetAllAttributes<TAttr>(this Enum value) where TAttr : Attribute
        {
            foreach (
                var field in
                value.GetType().GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                if (field.GetValue(null).Equals(value))
                    return (TAttr[]) (field.GetCustomAttributes(typeof (TAttr), false));
            }
            return new TAttr[0];
        }
    }
}