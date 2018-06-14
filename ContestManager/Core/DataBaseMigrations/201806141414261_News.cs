namespace Core.DataBaseMigrations
{
    using System.Data.Entity.Migrations;
    
    public partial class News : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.News",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ContestId = c.Guid(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        MdContent = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Contests", t => t.ContestId, cascadeDelete: true)
                .Index(t => t.ContestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.News", "ContestId", "public.Contests");
            DropIndex("public.News", new[] { "ContestId" });
            DropTable("public.News");
        }
    }
}
