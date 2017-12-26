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

            //// Configure StudentId as PK for StudentAddress
            //HasKey(c => c.StudentId);

            //// Property(c => c.StudentId).HasColumnName("StudentId");


            Property(c => c.CourseName)
                .HasMaxLength(250)
                .IsFixedLength()
                .IsRequired();


            HasRequired<User>(x => x.Student)
                .WithMany(x => x.StudentEnrollments)
                .HasForeignKey<string>(x => x.StudentId)
                .WillCascadeOnDelete(false);

        }
    }
}
