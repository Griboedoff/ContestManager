namespace Core.DataBaseMigrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Results : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Participants", "SerializedResults", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("public.Participants", "SerializedResults");
        }
    }
}
