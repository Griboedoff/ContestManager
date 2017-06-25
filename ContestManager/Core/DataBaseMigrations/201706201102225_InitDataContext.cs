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
                "public.EmailConfigs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MailboxAddress = c.String(),
                        SmtpHost = c.String(),
                        SmtpPort = c.Int(nullable: false),
                        SmtpUser = c.String(),
                        SmtpPwd = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.EmailRegistrationRequests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100),
                        EmailAddress = c.String(maxLength: 100),
                        PasswordHash = c.Binary(),
                        Sult = c.String(maxLength: 5),
                        Secret = c.String(maxLength: 5),
                        IsUsed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.EmailAddress, t.Secret }, unique: true, name: "EmailRegistrationRequest_EmailAddress_Secret_Index");
            
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
            DropIndex("public.EmailRegistrationRequests", "EmailRegistrationRequest_EmailAddress_Secret_Index");
            DropIndex("public.AuthenticationAccounts", "AuthenticationAccount_Type_ServiceId_Index");
            DropTable("public.Users");
            DropTable("public.EmailRegistrationRequests");
            DropTable("public.EmailConfigs");
            DropTable("public.AuthenticationAccounts");
        }
    }
}
