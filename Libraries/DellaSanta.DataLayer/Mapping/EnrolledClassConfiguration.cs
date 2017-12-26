using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DellaSanta.Core;

namespace DellaSanta.DataLayer
{
    public class EnrolledClassConfiguration : EntityTypeConfiguration<EnrolledClass>
    {
        public EnrolledClassConfiguration()
        {
            
            HasKey(c => new { c.StudentId, c.CourseId });
            
            Property(c => c.CourseName)
                .HasMaxLength(250)
                .IsFixedLength()
                .IsRequired();

            Property(c => c.ExamDate)
              .IsOptional();

            Property(c => c.ExamGrade)
              .IsOptional();
            
            HasRequired<User>(x => x.Student)
                .WithMany(x => x.StudentEnrollments)
                .HasForeignKey<int>(x => x.StudentId)
                .WillCascadeOnDelete(false);
            
            //HasRequired<Course>(x => x.Course)
            // .WithMany(x => x.EnrolledClasses)
            // .HasForeignKey<int>(x => x.CourseId)
            // .WillCascadeOnDelete(false);

        }
    }
}
