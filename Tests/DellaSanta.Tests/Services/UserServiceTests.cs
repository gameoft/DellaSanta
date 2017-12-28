using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using DellaSanta.Core;
using DellaSanta.DataLayer;
using DellaSanta.Services;
using DellaSanta.Logging;

namespace DellaSanta.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private User _user1;
        private User _user2;
        private User _user3;
        private IQueryable<User> _users;
        private ApplicationDbContext _userRepository;
        private ILogManager _log;

        [SetUp]
        public void SetUp()
        {
            _user1 = new User
            {
                UserId = 1,
                UserName = "johndoe",
                FirstName = "John",
                LastName = "Doe",
                Active = true,
                Password = "70374248fd7129088fef42b8f568443f6dce3a48", // "xxxxxxxxx",
                Role = "Student"
            };
            _user2 = new User
            {
                UserId = 2,
                UserName = "janetdoe",
                FirstName = "John",
                LastName = "Doe",
                Active = true,
                Password = "70374248fd7129088fef42b8f568443f6dce3a48", // "xxxxxxxxx",
                Role = "Student"
               
            };
            _user3 = new User
            {
                UserId = 3,
                UserName = "123456789",
                FirstName = "Eric",
                LastName = "Newton",
                Active = false,
                Password = "70374248fd7129088fef42b8f568443f6dce3a48", // "xxxxxxxxx",
                Role = "Student"
            };
            _users = new List<User>
            {
                _user1,
                _user2,
                _user3
            }.AsQueryable();

            var mockSet = NSubstituteHelper.CreateMockDbSet(_users);
            var mockRepository = Substitute.For<ApplicationDbContext>();
            mockRepository.Users.Returns(mockSet);
            _userRepository = mockRepository;
            _log = Substitute.For<ILogManager>();
        }


        [Test]
        public async Task GetUserByUserName_ValidUserName_Return1User()
        {
            var sut = new UserService(_userRepository, _log);
            var user = await sut.GetUserByUserNameAsync("johndoe");
            Assert.AreEqual(_user1, user);
        }

        [Test]
        public async Task GetUserByUserName_InvalidUsername_ReturnNull()
        {
            var sut = new UserService(_userRepository, _log);
            var user = await sut.GetUserByUserNameAsync("dummynonexistant");
            Assert.IsNull(user);
        }

        [Test]
        public async Task ValidateCredentials_ValidPassword_ReturnTrue()
        {
            var sut = new UserService(_userRepository, _log);
            var isValid = await sut.ValidateCredentialsAsync("janetdoe", "xxxxxxxxx");
            Assert.IsTrue(isValid);
        }


    }
}
