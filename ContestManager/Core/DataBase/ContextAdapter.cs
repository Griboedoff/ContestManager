using System;
using System.Linq;
using Core.DataBaseEntities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Core.DataBase
{
    public interface IContextAdapter : IDisposable
    {
        IQueryable<T> Set<T>() where T : DataBaseEntity;
        IQueryable<T> SetWithAttach<T>() where T : DataBaseEntity;

        T[] GetAll<T>() where T : DataBaseEntity;
        T Find<T>(Guid id) where T : DataBaseEntity;
        T Read<T>(Guid id) where T : DataBaseEntity;

        T FindAndAttach<T>(Guid id) where T : DataBaseEntity;
        T ReadAndAttach<T>(Guid id) where T : DataBaseEntity;

        void SaveChanges();
    }

    public class ContextAdapter : IContextAdapter
    {
        private readonly IContext context;

        public ContextAdapter(IContext context)
        {
            this.context = context;
        }

        public IQueryable<T> Set<T>() where T : DataBaseEntity
            => context.Set<T>().AsNoTracking();

        public IQueryable<T> SetWithAttach<T>() where T : DataBaseEntity
            => context.Set<T>();

        public T[] GetAll<T>() where T : DataBaseEntity
            => context.Set<T>().ToArray();

        public T Find<T>(Guid id) where T : DataBaseEntity
            => Set<T>().FirstOrDefault(e => e.Id == id);

        public T Read<T>(Guid id) where T : DataBaseEntity
            => Find<T>(id) ?? throw new EntityNotFoundException<T>(id);

        public T FindAndAttach<T>(Guid id) where T : DataBaseEntity
            => SetWithAttach<T>().FirstOrDefault(e => e.Id == id);

        public T ReadAndAttach<T>(Guid id) where T : DataBaseEntity
            => FindAndAttach<T>(id) ?? throw new EntityNotFoundException<T>(id);

        public void SaveChanges()
            => context.SaveChanges();

        public void Dispose()
            => context.Dispose();
    }
}