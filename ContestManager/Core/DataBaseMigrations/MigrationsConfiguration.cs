using System.Data.Entity.Migrations;
using Core.DataBase;

namespace Core.DataBaseMigrations
{
    internal sealed class MigrationsConfiguration : DbMigrationsConfiguration<Context>
    {
        public MigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataBaseMigrations";
        }
    }
}
