using System;
using Core.DataBaseEntities;
using Microsoft.EntityFrameworkCore;

namespace Core.DataBase
{
    public interface IContext : IDisposable
    {
        DbSet<T> Set<T>() where T : class;

        int SaveChanges();
    }

    public class Context : DbContext, IContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "User ID=contest_manager;Password=ya_admin;Host=localhost;Port=5432;Database=GreatOlympicTries;Pooling=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<AuthenticationAccount>().HasIndex(p => new { p.Type, p.ServiceId }).IsUnique();
            modelBuilder.Entity<Contest>().HasIndex(p => p.Title).IsUnique();
            modelBuilder.Entity<Invite>()
                .HasIndex(p => new { p.Type, p.Email, p.ConfirmationCode })
                .IsUnique();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Contest> Contests { get; set; }
        public virtual DbSet<StoredConfig> StoredConfigs { get; set; }
        public virtual DbSet<AuthenticationAccount> AuthenticationAccounts { get; set; }
        public virtual DbSet<Invite> EmailConfirmationRequests { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<QualificationTask> QualificationTasks { get; set; }
    }
}