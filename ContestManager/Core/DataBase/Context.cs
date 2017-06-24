using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Core.DataBaseEntities;

namespace Core.DataBase
{
    public interface IContext : IDisposable
    {
        DbSet<T> Set<T>() where T : class;
        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        int SaveChanges();
    }

    [DbConfigurationType(typeof(NpgsqlConfiguration))]
    public class Context : DbContext, IContext
    {
        public Context() : base(nameOrConnectionString: "DataContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
            => modelBuilder.HasDefaultSchema("public");

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AuthenticationAccount> AuthenticationAccounts { get; set; }
        public virtual DbSet<EmailRegistrationRequest> EmailRegistrationRequests { get; set; }
    }
}