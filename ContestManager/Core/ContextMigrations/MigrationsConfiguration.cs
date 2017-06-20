using System.Data.Entity.Migrations;

namespace Core.ContextMigrations
{
    internal sealed class MigrationsConfiguration : DbMigrationsConfiguration<Context.Context>
    {
        public MigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"ContextMigrations";
        }
    }
}
