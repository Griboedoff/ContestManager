using System;
using Core.Models.Configs;

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