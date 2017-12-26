using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DellaSanta.Core;

namespace DellaSanta.DataLayer
{
    public class CoursePathConfiguration : EntityTypeConfiguration<CoursePath>
    {
        public CoursePathConfiguration()
        {
            //Property(c => c.StudentId).HasColumnName("StudentId");


            Property(c => c.CoursePathName)
                .HasMaxLength(250)
                .IsFixedLength()
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_CoursePath_CoursePathName") { IsUnique = true }));

            HasMany<Course>(g => g.Courses)
                .WithRequired(s => s.CoursePath)
                .HasForeignKey<int>(s => s.CoursePathId)
                .WillCascadeOnDelete(false);


            //set StudentName column as concurrency column so that it will be included in the where clause in update and delete commands.
            //You can also use IsRowVersion() method for byte[] property to make it as a concurrency column.
            //Property(p => p.StudentName)
            //        .IsConcurrencyToken();


            //Property(p => p.DateOfBirth)
            //       .HasColumnName("DoB")
            //       .HasColumnOrder(3)
            //       .HasColumnType("datetime2");

            //Property(p => p.Height)
            //        .IsOptional();

            //Property(p => p.Height)
            //        .HasPrecision(2, 2);

            //Property(p => p.Weight)
            //        .IsRequired();

            //HasOptional(c => c.Parent)
            //    .WithMany(c => c.Children)
            //    .HasForeignKey(c => c.ParentCategoryId)
            //    .WillCascadeOnDelete(false);

            ////One - to - Zero - or - One relationship using Fluent API
            //// Configure Student & StudentAddress entity
            //HasOptional(s => s.Address) // Mark Address property optional in Student entity
            //            .WithRequired(ad => ad.Student); // mark Student property as required in StudentAddress entity. Cannot save StudentAddress without Student

            // Configure StudentId as FK for StudentAddress
            //HasOptional(s => s.Address)
            //            .WithRequired(ad => ad.Student);


            //Configure One-to - One relationship using Fluent API:


            // Configure StudentId as FK for StudentAddress
            //HasRequired(s => s.Address)
            //            .WithRequiredPrincipal(ad => ad.Student);




            // configures one-to-many relationship
            //HasRequired<Grade>(s => s.CurrentGrade)
            //    .WithMany(g => g.Students)
            //    .HasForeignKey<int>(s => s.CurrentGradeId);

            //alternatively i can configure the grade
            //modelBuilder.Entity<Grade>()
            //    .HasMany<Student>(g => g.Students)
            //    .WithRequired(s => s.CurrentGrade)
            //    .WillCascadeOnDelete(false);
            //    .HasForeignKey<int>(s => s.CurrentGradeId);


            //Configure Many-to - Many Relationship using Fluent API:
            //HasMany<Course>(s => s.Courses)
            //    .WithMany(c => c.Students)
            //    .Map(cs =>
            //    {
            //        cs.MapLeftKey("StudentRefId");
            //        cs.MapRightKey("CourseRefId");
            //        cs.ToTable("StudentCourse");
            //    });


            //The ConcurrencyCheck attribute can be applied to one or more properties in an entity class in EF 6 and EF Core. When applied to a property, 
            //the corresponding column in the database table will be used in the optimistic concurrency check using where clause.
            //EF will include StudentName column in UPDATE statement to check for optimistic concurrency.

        }
    }
}
