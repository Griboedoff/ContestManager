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
                "public.Contests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        OwnerId = c.Guid(nullable: false),
                        Options = c.Int(nullable: false),
                        SerializedFields = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Title, unique: true, name: "Contest_Title_Unique");
            
            CreateTable(
                "public.EmailConfirmationRequests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        Email = c.String(maxLength: 100),
                        ConfirmationCode = c.String(maxLength: 7),
                        IsUsed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.Type, t.Email, t.ConfirmationCode }, unique: true, name: "EmailConfirmationRequest_Type_Email_ConfirmationCode_Index");
            
            CreateTable(
                "public.StoredConfigs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TypeName = c.String(),
                        JsonValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropIndex("public.EmailConfirmationRequests", "EmailConfirmationRequest_Type_Email_ConfirmationCode_Index");
            DropIndex("public.Contests", "Contest_Title_Unique");
            DropIndex("public.AuthenticationAccounts", "AuthenticationAccount_Type_ServiceId_Index");
            DropTable("public.Users");
            DropTable("public.StoredConfigs");
            DropTable("public.EmailConfirmationRequests");
            DropTable("public.Contests");
            DropTable("public.AuthenticationAccounts");
        }
    }
}
