﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DellaSanta.Core;


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

        public ApplicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public DbSet<UserClaims> Claims { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CoursePath> CoursePaths { get; set; }
        public DbSet<EnrolledClass> EnrolledClasses { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserClaimsConfiguration());
            modelBuilder.Configurations.Add(new CourseConfiguration());
            modelBuilder.Configurations.Add(new CoursePathConfiguration());
            modelBuilder.Configurations.Add(new EnrolledClassConfiguration());
            modelBuilder.Configurations.Add(new LogEntryConfiguration());

            base.OnModelCreating(modelBuilder); 
        }
    }
}