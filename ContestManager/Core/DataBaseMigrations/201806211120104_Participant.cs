using System.Data.Entity.Migrations;

namespace Core.DataBaseMigrations
{
    public partial class Participant : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "public.Participants",
                    c => new
                    {
                        Id = c.Guid(nullable: false),
                        ContestId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("public.Participants");
        }
    }
}