using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DellaSanta.Models;

namespace DellaSanta.DataLayer
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {

            Property(c => c.UserName)
                .HasMaxLength(20)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_User_UserName") { IsUnique = true }));

            Property(c => c.FirstName)
                .HasMaxLength(20);

            Property(c => c.LastName)
                .HasMaxLength(20);

            Property(c => c.Password)
             .IsRequired();
            
            Property(c => c.Password)
              .IsRequired();

            Property(c => c.Role)
              .HasMaxLength(10)
              .IsRequired();

        
            //HasMany<UserClaims>(g => g.Claims)
            //  .WithRequired(s => s.User)
            //  .HasForeignKey<int>(s => s.UserId)
            //  .WillCascadeOnDelete(false);

            //Ignore(au => au.Claims);

        }
    }
}