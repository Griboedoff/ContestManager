namespace Core.DataBaseMigrations
{
    using System.Data.Entity.Migrations;

    public partial class ContestState : DbMigration
    {
        public override void Up()
        {
            AddColumn("public.Contests", "State", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("public.Contests", "State");
        }
    }
}
