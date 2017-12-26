namespace DellaSanta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class courses3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EnrolledClasses", "CourseName", c => c.String(maxLength: 250, fixedLength: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EnrolledClasses", "CourseName", c => c.String(nullable: false, maxLength: 250, fixedLength: true));
        }
    }
}
