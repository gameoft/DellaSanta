namespace DellaSanta.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Claims;
    using DellaSanta.Core;
    using DellaSanta.DataLayer;
    


    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            #region users
            //User Admin
            context.Users.AddOrUpdate(
                c => c.UserId,
                new User { UserName = "admin@school.com", Password = Utils.Hash("admin"), Role = "Admin", Active = true, UserId = 1 });
            
            //User Teacher
                context.Users.AddOrUpdate(
                    c => c.UserId,
                    new User { UserName = "teacher1@school.com", Password = Utils.Hash("school"), Role = "Teacher", Active = true, UserId = 2, FirstName="T1Name", LastName="T1Surname" });

            context.Claims.AddOrUpdate(
                c => new { c.UserId, c.ClaimType },
                new UserClaims { ClaimType = "Department", ClaimValue = "Mathematics1", UserId = 2 });
            

            context.Users.AddOrUpdate(
                    c => c.UserId,
                    new User { UserName = "teacher2@school.com", Password = Utils.Hash("school"), Role = "Teacher", Active = true, UserId = 3, FirstName = "T2Name", LastName = "T2Surname" });

            context.Claims.AddOrUpdate(c => new { c.UserId, c.ClaimType }, new UserClaims { ClaimType = "Department", ClaimValue = "Mathematics2", UserId = 3 });

            context.SaveChanges();

            //User Student
            context.Users.AddOrUpdate(
                    c => c.UserId,
                    new User { UserName = "student1@school.com", Password = Utils.Hash("school"), Role = "Student", Active = true, UserId = 4, FirstName = "S1Name", LastName = "S1Surname" });

            context.Claims.AddOrUpdate(c => new { c.UserId, c.ClaimType }, new UserClaims { ClaimType = ClaimTypes.StreetAddress, ClaimValue = "address1", UserId = 4 });
            context.Claims.AddOrUpdate(c => new { c.UserId, c.ClaimType }, new UserClaims { ClaimType = "SelectedPath", ClaimValue = "1", UserId = 4 });


            context.Users.AddOrUpdate(
                    c => c.UserId,
                    new User { UserName = "student2@school.com", Password = Utils.Hash("school"), Role = "Student", Active = true, UserId = 5, FirstName = "S2Name", LastName = "S2Surname" });

            context.Claims.AddOrUpdate(c => new { c.UserId, c.ClaimType }, new UserClaims { ClaimType = ClaimTypes.StreetAddress, ClaimValue = "address2", UserId = 5 });
            context.Claims.AddOrUpdate(c => new { c.UserId, c.ClaimType }, new UserClaims { ClaimType = "SelectedPath", ClaimValue = "2", UserId = 5 });


            context.Users.AddOrUpdate(
                    c => c.UserId,
                    new User { UserName = "student3@school.com", Password = Utils.Hash("school"), Role = "Student", Active = true, UserId = 6, FirstName = "S3Name", LastName = "S3Surname" });
            context.Claims.AddOrUpdate(c => new { c.UserId, c.ClaimType }, new UserClaims { ClaimType = "SelectedPath", ClaimValue = "1", UserId = 6 });

            context.Users.AddOrUpdate(c => c.UserId, new User { UserName = "student4@school.com", Password = Utils.Hash("school"), Role = "Student", Active = true, UserId = 7, FirstName = "S4Name", LastName = "S4Surname" });
            context.Claims.AddOrUpdate(c => new { c.UserId, c.ClaimType }, new UserClaims { ClaimType = "SelectedPath", ClaimValue = "1", UserId = 7 });

            #endregion users

            #region coursepaths

            context.CoursePaths.AddOrUpdate(
                c => c.CoursePathId,
                new CoursePath
                {
                    CoursePathId = 1,
                    CoursePathName = "Applied Math"
                });


            context.CoursePaths.AddOrUpdate(
                c => c.CoursePathId,
                new CoursePath
                {
                    CoursePathId = 2,
                    CoursePathName = "Logistics"
                });

            context.SaveChanges();

            #endregion coursepaths

            #region course

            var teacher1 = context.Users.Where(x => x.UserName== "teacher1@school.com").First();

            context.Courses.AddOrUpdate(
                c => new { c.CoursePathId, c.CourseName },
                new Course
                {
                    CourseName = "Mathematics 1",
                    CoursePathId = 1,
                    TeacherId = teacher1.UserId
                });

            var teacher2 = context.Users.Where(x => x.UserName == "teacher2@school.com").First(); 

            context.Courses.AddOrUpdate(
                c => new { c.CoursePathId, c.CourseName },
                new Course
                {
                    CourseName = "Mathematics 2",
                    CoursePathId = 1,
                    TeacherId = teacher2.UserId
                });

            context.SaveChanges();

            #endregion course

            #region enrolledclasses
            var student1 = context.Users.Where(x => x.UserName == "student1@school.com").First();
            var courseenrolled = context.Courses.Where(x => x.CourseName == "Mathematics 1").First();
            

            context.EnrolledClasses.AddOrUpdate(
            c => new { c.CourseId, c.StudentId },
            new EnrolledClass
            {
                CourseId = courseenrolled.CourseId,
                CourseName = courseenrolled.CourseName,
                 StudentId = student1.UserId,
                  StudentName = student1.UserName
                 
            });

            #endregion enrolledclasses



        }
    }
}
