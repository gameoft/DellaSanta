namespace DellaSanta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class selectedpath2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "SelectedPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "SelectedPath", c => c.Int(nullable: false));
        }
    }
}
