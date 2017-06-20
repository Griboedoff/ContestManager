namespace Core.ContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDataContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.AuthenticationAccounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        ServiceId = c.String(),
                        ServiceToken = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("public.Users");
            DropTable("public.AuthenticationAccounts");
        }
    }
}
