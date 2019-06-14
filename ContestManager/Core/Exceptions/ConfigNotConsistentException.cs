using System;
using Core.Configs;

namespace Core.Exceptions
{
    public class ConfigNotConsistentException<T> : Exception
        where T : class, IConfig
    {
        public ConfigNotConsistentException()
            : base($"Cannot get {typeof(T).Name} from storage")
        {
        }
    }
}