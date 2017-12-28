namespace DellaSanta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_department : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CoursePaths", "Department", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CoursePaths", "Department");
        }
    }
}
