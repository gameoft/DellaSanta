namespace DellaSanta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class courses4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EnrolledClasses", "ExamDate", c => c.DateTime());
            AlterColumn("dbo.EnrolledClasses", "ExamGrade", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EnrolledClasses", "ExamGrade", c => c.Int(nullable: false));
            AlterColumn("dbo.EnrolledClasses", "ExamDate", c => c.DateTime(nullable: false));
        }
    }
}
