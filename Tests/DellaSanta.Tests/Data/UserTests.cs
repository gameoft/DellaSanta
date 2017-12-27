using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DellaSanta.Core;
using DellaSanta.DataLayer;
using NUnit.Framework;

namespace DellaSanta.Tests.Data
{
    [TestFixture]
    public class UserTests : PersistenceTest
    {
        [Test]
        public void CanSaveAndLoadUser()
        {
            var sut = new User
            {
                UserName = "johndoe",
                FirstName = "John",
                LastName = "Doe",
                Active = true,
                Password = "xxxxxxxxx",
                Role = "Student"
            };

            var fromDb = SaveAndLoadEntity(sut);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.UserName, "johndoe");
            Assert.AreEqual(fromDb.FirstName, "John");
            Assert.AreEqual(fromDb.LastName, "Doe");
            Assert.AreEqual(fromDb.Active, true);
            Assert.AreEqual(fromDb.Password, "xxxxxxxxx");
            Assert.AreEqual(fromDb.Role, "Student");

        }

        private User SaveAndLoadEntity(User sut, bool disposeContext = true)
        {
            context.Users.Add(sut);
            var result = context.SaveChanges();

            int id = sut.UserId;

            if (disposeContext)
            {
                context.Dispose();
                context = new ApplicationDbContext(GetTestDbName());
            }

            try
            {
                return context.Users.Where(x => x.UserId == id).FirstOrDefault();
            }
            catch
            {
                // If a record cannot be find by id, it must be composite primary key. 
                // So we just return the very first record. 
                return context.Users.FirstOrDefault();
            }
        }

    }
}
