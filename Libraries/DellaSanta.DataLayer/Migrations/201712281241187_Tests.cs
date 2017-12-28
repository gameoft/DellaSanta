namespace DellaSanta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tests : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LogEntries", "Logger", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LogEntries", "Logger", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
