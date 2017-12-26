using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using DellaSanta.Core;


namespace DellaSanta.DataLayer
{
    public class UserClaimsConfiguration : EntityTypeConfiguration<UserClaims>
    {

        public UserClaimsConfiguration()
        {
            // Configure StudentId as PK for StudentAddress
            HasKey(c => new { c.UserId, c.ClaimType } );

            Property(c => c.UserId)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_UserClaim", 1) { IsUnique = true })); 
                

            Property(c => c.ClaimType)
                .HasMaxLength(256)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("AK_UserClaim", 2) { IsUnique = true }));

            
            Property(c => c.ClaimValue)
                .HasMaxLength(256)
                .IsRequired();

            HasRequired<User>(x => x.User)
               .WithMany(x => x.Claims)
               .HasForeignKey<int>(x => x.UserId )
               .WillCascadeOnDelete(false);


        }


    }
}