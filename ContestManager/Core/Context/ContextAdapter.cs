using System;
using System.Data.Entity;
using System.Linq;
using Core.DataBaseEntities;
using Core.Exceptions;

namespace Core.Context
{
    public interface IContextAdapter : IDisposable
    {
        IQueryable<T> Set<T>() where T : DataBaseEntity;
        IQueryable<T> SetWithAttach<T>() where T : DataBaseEntity;

        T Find<T>(Guid id) where T : DataBaseEntity;
        T Read<T>(Guid id) where T : DataBaseEntity;

        T FindAndAttach<T>(Guid id) where T : DataBaseEntity;
        T ReadAndAttach<T>(Guid id) where T : DataBaseEntity;

        void AttachToInsert<T>(T entity) where T : DataBaseEntity;
        void AttachToUpdate<T>(T entity) where T : DataBaseEntity;
        void MarkAsDeleted<T>(Guid id) where T : DataBaseEntity;

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

        public T Find<T>(Guid id) where T : DataBaseEntity
            => Set<T>().FirstOrDefault(e => e.Id == id);

        public T Read<T>(Guid id) where T : DataBaseEntity
            => Find<T>(id) ?? throw new EntityNotFoundException<T>(id);

        public T FindAndAttach<T>(Guid id) where T : DataBaseEntity
            => SetWithAttach<T>().FirstOrDefault(e => e.Id == id);

        public T ReadAndAttach<T>(Guid id) where T : DataBaseEntity
            => FindAndAttach<T>(id) ?? throw new EntityNotFoundException<T>(id);

        public void AttachToInsert<T>(T entity) where T : DataBaseEntity
            => Attach(entity, EntityState.Added);

        public void AttachToUpdate<T>(T entity) where T : DataBaseEntity
            => Attach(entity, EntityState.Modified);

        public void MarkAsDeleted<T>(Guid id) where T : DataBaseEntity
            => Attach(ReadAndAttach<T>(id), EntityState.Deleted);

        public void SaveChanges()
            => context.SaveChanges();

        public void Dispose()
            => context.Dispose();

        private void Attach<T>(T entity, EntityState state) where T : DataBaseEntity
            => context.Entry(entity).State = state;
    }
}