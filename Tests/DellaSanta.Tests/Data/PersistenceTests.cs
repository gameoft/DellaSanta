using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DellaSanta.DataLayer;
using NUnit.Framework;

namespace DellaSanta.Tests.Data
{
    [TestFixture]
    public abstract class PersistenceTest
    {
        protected ApplicationDbContext context;

        [SetUp]
        public void SetUp()
        {
            //TODO fix compilation warning (below)
#pragma warning disable 0618
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            context = new ApplicationDbContext(GetTestDbName());
            context.Database.Delete();
            context.Database.Create();

           
        }

        protected string GetTestDbName()
        {
            string testDbName = "Data Source=" +
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +
                @"\\DellaSanta.Data.Tests.Db.sdf;Persist Security Info=False";
            return testDbName;
        }

    
    }
}
