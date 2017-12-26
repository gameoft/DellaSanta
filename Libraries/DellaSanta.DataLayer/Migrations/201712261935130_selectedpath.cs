namespace DellaSanta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class selectedpath : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "SelectedPath", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "SelectedPath");
        }
    }
}
