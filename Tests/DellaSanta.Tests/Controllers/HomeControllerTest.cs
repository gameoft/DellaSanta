using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dellasanta.Web.Common.Security;
using DellaSanta.Controllers;
using DellaSanta.Core;
using DellaSanta.Models;
using DellaSanta.Services;
using NSubstitute;
using NUnit.Framework;
using System.Web;
using DellaSanta.Logging;

namespace DellaSanta.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private User _user1;
        private IUserService _userService;
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

            _userService = Substitute.For<IUserService>();
            _log = Substitute.For<ILogManager>();
        }

     
        [Test]
        public async Task CourseEnrollment_PostValidData_ReturnOK()
        {
            // Arrange
            _userService.AddClassAsync(Arg.Any<EnrolledClass>()).Returns(1);
            
            var sut = new HomeController(_userService, _log);

            var identity = Substitute.For<ClaimsIdentity>();
            identity.Claims.Returns(new List<Claim> { new Claim(ClaimTypes.Sid, "1") } );
            
            var userMock = Substitute.For<IPrincipal>();
            userMock.Identity.Returns(identity);

            //var contextMock = Substitute.For<HttpContextBase>();
            //contextMock.User.Returns(userMock);
                        
            var webCtx = Substitute.For<ControllerContext>();
            webCtx.HttpContext.User.Returns(userMock);
            webCtx.HttpContext.Request.IsAuthenticated.Returns(true);
            
            sut.ControllerContext = webCtx;

            var model = new CourseEnrollmentViewModel { CourseId = "1", UserId = "1" };

            // Act
            var result = await sut.CourseEnrollment(model) as JsonResult;


            // Assert

            await _userService.Received(1).AddClassAsync(Arg.Any<EnrolledClass>());

            Assert.IsNotNull(result);
            Assert.IsTrue((bool)result.Data);
         
         
        }

     


    }
}