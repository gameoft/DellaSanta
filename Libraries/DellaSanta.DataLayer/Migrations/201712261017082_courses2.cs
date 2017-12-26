namespace DellaSanta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class courses2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseName = c.String(nullable: false, maxLength: 250, fixedLength: true),
                        TeacherId = c.Int(nullable: false),
                        CoursePathId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseId)
                .ForeignKey("dbo.CoursePaths", t => t.CoursePathId)
                .ForeignKey("dbo.Users", t => t.TeacherId)
                .Index(t => t.TeacherId)
                .Index(t => t.CoursePathId);
            
            CreateTable(
                "dbo.CoursePaths",
                c => new
                    {
                        CoursePathId = c.Int(nullable: false, identity: true),
                        CoursePathName = c.String(nullable: false, maxLength: 250, fixedLength: true),
                    })
                .PrimaryKey(t => t.CoursePathId)
                .Index(t => t.CoursePathName, unique: true, name: "AK_CoursePath_CoursePathName");
            
            CreateTable(
                "dbo.EnrolledClasses",
                c => new
                    {
                        StudentId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        StudentName = c.String(),
                        CourseName = c.String(nullable: false, maxLength: 250, fixedLength: true),
                        ExamDate = c.DateTime(nullable: false),
                        ExamGrade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentId, t.CourseId })
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .ForeignKey("dbo.Users", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Courses", "TeacherId", "dbo.Users");
            DropForeignKey("dbo.EnrolledClasses", "StudentId", "dbo.Users");
            DropForeignKey("dbo.EnrolledClasses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "CoursePathId", "dbo.CoursePaths");
            DropIndex("dbo.EnrolledClasses", new[] { "CourseId" });
            DropIndex("dbo.EnrolledClasses", new[] { "StudentId" });
            DropIndex("dbo.CoursePaths", "AK_CoursePath_CoursePathName");
            DropIndex("dbo.Courses", new[] { "CoursePathId" });
            DropIndex("dbo.Courses", new[] { "TeacherId" });
            DropTable("dbo.EnrolledClasses");
            DropTable("dbo.CoursePaths");
            DropTable("dbo.Courses");
        }
    }
}
