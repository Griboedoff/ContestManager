using System;
using Core.DataBaseEntities;

namespace Core.Exceptions
{
    public class ConfigNotConsistentException<T> : Exception
        where T : DataBaseEntity
    {
        public ConfigNotConsistentException()
            : base($"Cannot get {typeof(T).Name} from storage")
        {
        }
    }
}