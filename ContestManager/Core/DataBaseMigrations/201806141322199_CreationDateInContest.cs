using System.Data.Entity.Migrations;

namespace Core.DataBaseMigrations
{
    public partial class CreationDateInContest : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Contests", "CreationDate", c => c.DateTime(nullable: false));
        }

        public override void Down()
        {
            DropColumn("public.Contests", "CreationDate");
        }
    }
}