using System;
using System.Threading.Tasks;
using Core.DataBaseEntities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Core.DataBase
{
    public static class ContextExtensions
    {
        public static async Task<T> Read<T>(this DbSet<T> set, Guid id) where T : DataBaseEntity
        {
            var value = await set.FindAsync(id);
            if (value == default(T))
                throw new EntityNotFoundException<T>(id);

            return value;
        }
    }
}