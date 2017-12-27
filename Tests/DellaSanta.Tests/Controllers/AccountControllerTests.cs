using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Dellasanta.Web.Common.Security;
using DellaSanta.Controllers;
using DellaSanta.Core;
using DellaSanta.Models;
using DellaSanta.Services;
using NSubstitute;
using NUnit.Framework;

namespace DellaSanta.Tests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
    
        private IUserService  _userService;
        private IAuthenticationService _authenticationService;

        [SetUp]
        public void SetUp()
        {
        
            _authenticationService = Substitute.For<IAuthenticationService>();
            _userService = Substitute.For<IUserService>();
        }

        [Test]
        public void Login_ReturnViewResult()
        {
            // Arrange
            var sut = new AccountController(null, null);

            // Act
            var result = sut.Login("") as ViewResult;
            

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.ViewName);
            var model = (LoginViewModel)result.Model;
            Assert.IsNull(model.Email);
            Assert.IsNull(model.Password);
        }

        [Test]
        public async Task Login_PostInvalidAccount_ReturnViewWithModelError()
        {
            // Arrange
            _userService.ValidateCredentialsAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(false);
         
            var sut = new AccountController(_userService, _authenticationService);

            var model = new LoginViewModel();

            // Act
            var result = await sut.Login(model, "") as ViewResult;

            // Assert
            await _userService.Received(1).ValidateCredentialsAsync(Arg.Any<string>(), Arg.Any<string>());
            
            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.ViewName);
            model = (LoginViewModel)result.Model;
          
            Assert.True(!result.ViewData.ModelState.IsValid);
            Assert.AreEqual(result.ViewData.ModelState[""].Errors.Count, 1);
            Assert.AreEqual(result.ViewData.ModelState[""].Errors[0].ErrorMessage, "Incorrect username or password.");
        }

        [Test]
        public async Task Login_PostValidUser_RedirectToHome()
        {
            // Arrange
            _userService.ValidateCredentialsAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            var user = new User { Active = true };
            _userService.GetUserByUserNameAsync(Arg.Any<string>()).Returns(user);

            var sut = new AccountController(_userService, _authenticationService);

            // Act
            var result = await sut.Login(new LoginViewModel(), "") as RedirectToRouteResult;

            // Assert
            await _userService.Received(1).ValidateCredentialsAsync(Arg.Any<string>(), Arg.Any<string>());

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Permanent);
            Assert.AreEqual(result.RouteValues["controller"], "Home");
            Assert.AreEqual(result.RouteValues["action"], "Index");
        }

      
    }
}