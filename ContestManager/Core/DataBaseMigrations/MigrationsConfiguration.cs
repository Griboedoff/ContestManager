using System.Data.Entity.Migrations;

namespace Core.DataBaseMigrations
{
    internal sealed class MigrationsConfiguration : DbMigrationsConfiguration<DataBase.Context>
    {
        public MigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataBaseMigrations";
        }
    }
}
