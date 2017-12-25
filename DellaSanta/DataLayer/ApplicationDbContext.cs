using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DellaSanta.Models;

namespace DellaSanta.DataLayer
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            //To turn off lazy loading for a particular property, do not make it virtual. To turn off lazy loading for all entities in the context, set its configuration property to false
            //this.Configuration.LazyLoadingEnabled = false;
        }


        public DbSet<User> Users { get; set; }
        public DbSet<UserClaims> Claims { get; set; }
        //public DbSet<Student> Students { get; set; }
        //public DbSet<StudentAddress> StudentAddresses { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserClaimsConfiguration());


            base.OnModelCreating(modelBuilder); 
        }
    }
}