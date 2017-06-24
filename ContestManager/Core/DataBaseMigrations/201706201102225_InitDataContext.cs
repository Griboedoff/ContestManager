namespace Core.DataBaseMigrations
{
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
                        ServiceId = c.String(maxLength: 100),
                        ServiceToken = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.Type, t.ServiceId }, unique: true, name: "AuthenticationAccount_Type_ServiceId_Index");
            
            CreateTable(
                "public.EmailRegistrationRequests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        PasswordHash = c.Binary(),
                        Sult = c.String(maxLength: 5),
                        Secret = c.String(maxLength: 5),
                        IsUsed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true, name: "EmailRegistrationRequest_EmailIndex");
            
            CreateTable(
                "public.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropIndex("public.EmailRegistrationRequests", "EmailRegistrationRequest_EmailIndex");
            DropIndex("public.AuthenticationAccounts", "AuthenticationAccount_Type_ServiceId_Index");
            DropTable("public.Users");
            DropTable("public.EmailRegistrationRequests");
            DropTable("public.AuthenticationAccounts");
        }
    }
}
