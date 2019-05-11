    using System.Data.Entity.Migrations;
namespace Core.DataBaseMigrations
{

    public partial class FieldsInUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Users", "SerializedFields", c => c.String());
        }

        public override void Down()
        {
            DropColumn("public.Users", "SerializedFields");
        }
    }
}
