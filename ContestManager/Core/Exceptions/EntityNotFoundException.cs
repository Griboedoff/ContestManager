using System;
using Core.DataBaseEntities;

namespace Core.Exceptions
{
    public class EntityNotFoundException<T> : Exception
        where T : DataBaseEntity
    {
        public EntityNotFoundException(Guid id)
            : base($"Entity of type {typeof(T).Name} with id {id} not found")
        {
        }
    }
}