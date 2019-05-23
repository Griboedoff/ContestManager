using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.DataBaseEntities;
using Microsoft.EntityFrameworkCore;

namespace Core.DataBase
{
    public interface IAsyncRepository<T> where T : DataBaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> WhereAsync(Expression<Func<T, bool>> selector);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> selector);
        Task<bool> AnyAsync(Expression<Func<T, bool>> selector);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }

    public class Repository<T> : IAsyncRepository<T> where T : DataBaseEntity
    {
        private readonly Context dbContext;

        public Repository(Context dbContext) => this.dbContext = dbContext;

        public async Task<T> GetByIdAsync(Guid id) => await dbContext.Set<T>().FindAsync(id);
        public async Task<IReadOnlyList<T>> ListAllAsync() => await dbContext.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> WhereAsync(Expression<Func<T, bool>> selector)
            => await dbContext.Set<T>().Where(selector).ToListAsync();

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> selector)
            => await dbContext.Set<T>().FirstOrDefaultAsync(selector);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> selector)
            => await dbContext.Set<T>().AnyAsync(selector);

        public async Task<T> AddAsync(T entity)
        {
            dbContext.Set<T>().Add(entity);
            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}