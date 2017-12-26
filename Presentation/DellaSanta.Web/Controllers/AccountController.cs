using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using DellaSanta.Models;
using DellaSanta.DataLayer;
using System.Security.Cryptography;
using System.Text;
using DellaSanta.Core;
using DellaSanta.Services;
using System.Collections.Generic;
using Dellasanta.Web.Common.Security;

namespace DellaSanta.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public AccountController(
             IUserService userService,
             IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }
        
        
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                bool result = await _userService.ValidateCredentialsAsync(model.Email, model.Password);
                if (result)
                {
                    var user = await _userService.GetUserByUserNameAsync(model.Email);
                    if (user != null && user.Active)
                    {
                        _authenticationService.SignIn(user);
                        //        _log.Info($"Login Successful: {user.UserName}");

                        // Redirect to return URL
                        if (!string.IsNullOrEmpty(returnUrl) && !string.Equals(returnUrl, "/") && Url.IsLocalUrl(returnUrl))
                            return RedirectToLocal(returnUrl);

                        //        // User is in a role, so redirect to Administration area
                        //        if (roleNames.Contains(Constants.RoleNames.Developer) ||
                        //            roleNames.Contains(Constants.RoleNames.ApplicationManager))
                        //            return RedirectToRoute("Dashboard");

                        return RedirectToAction("Index", "Home");
                    }
                    //_log.Info($"Authorization Fail: {model.UserName}");
                    ModelState.AddModelError("", Constants.Messages.NotAuthorized);
                }
                else
                {
                    //_log.Info($"Login Fail: {model.UserName}");
                    ModelState.AddModelError("", "Incorrect username or password.");
                }
            }
            
            return View("Login", model);
        }
        
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RegisterTeacher()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterTeacher(RegisterTeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                 
                User user = new User { UserName = model.Email, Active = true, FirstName = model.FirstName, LastName = model.LastName, Password = Utils.Hash(model.Password), Role = "Teacher" };
                if (!string.IsNullOrEmpty(model.MobilePhone)) 
                    user.Claims.Add(new UserClaims { ClaimType = ClaimTypes.MobilePhone, ClaimValue = model.MobilePhone });
                if (!string.IsNullOrEmpty(model.Department))
                    user.Claims.Add(new UserClaims { ClaimType = "Department", ClaimValue = model.Department });
                
                var result = await _userService.AddUserAsync(user);
                if (result > 0)
                {
                    //_authenticationService.SignIn(user);
                    ModelState.AddModelError("", "User successfully saved.");
                    return View(new RegisterTeacherViewModel());
                    //return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "User not saved. Please retry in 5 minutes or contact the administrator.");
                
            }

            return View(model);

        }



        //try
        //{
        //    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName };
        //    var result = await UserManager.CreateAsync(user, model.Password);
        //    if (result.Succeeded)
        //    {
        //        IdentityResult result1 = null;
        //        IdentityResult result2 = null;
        //        if (0 == user.Claims.Where(c => c.ClaimType == "Department").Count())
        //        {
        //            result1 = await UserManager.AddClaimAsync(user.Id, new Claim("Department", model.Department));
        //        }
        //        if (0 == user.Claims.Where(c => c.ClaimType == "MobilePhone").Count())
        //        {
        //            result2 = await UserManager.AddClaimAsync(user.Id, new Claim("MobilePhone", model.MobilePhone));
        //        }

        //        if (result1.Succeeded && result2.Succeeded)
        //        {
        //            identitydbContextTransaction.Commit();

        //            return RedirectToAction("Index", "Home");
        //        }
        //        if (!result1.Succeeded)
        //            AddErrors(result1);
        //        if (!result2.Succeeded)
        //            AddErrors(result2);

        //    }
        //    else
        //        ModelState.AddModelError("", "User not saved. Please retry in 5 minutes.");

        //}
        //catch (Exception)
        //{
        //    identitydbContextTransaction.Rollback();
        //    ModelState.AddModelError("", "Unhandled exception. Please retry in 5 minutes.");
        //}






        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.Email, Active = true, FirstName = model.FirstName, LastName = model.LastName, Password = Utils.Hash(model.Password), Role = "Student" };
                user.Claims.Add(new UserClaims { ClaimType = ClaimTypes.StreetAddress, ClaimValue = model.Address });
                var res = await _userService.AddUserAsync(user);
                if (res > 0)
                {
                    _authenticationService.SignIn(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "User not saved. Please retry in 5 minutes or contact the administrator.");
                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }




        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //return RedirectToAction("Index", "Home");

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Login");


        }



        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (_userManager != null)
        //        {
        //            _userManager.Dispose();
        //            _userManager = null;
        //        }

        //        if (_signInManager != null)
        //        {
        //            _signInManager.Dispose();
        //            _signInManager = null;
        //        }
        //    }

        //    base.Dispose(disposing);
        //}

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        //internal class ChallengeResult : HttpUnauthorizedResult
        //{
        //    public ChallengeResult(string provider, string redirectUri)
        //        : this(provider, redirectUri, null)
        //    {
        //    }

        //    public ChallengeResult(string provider, string redirectUri, string userId)
        //    {
        //        LoginProvider = provider;
        //        RedirectUri = redirectUri;
        //        UserId = userId;
        //    }

        //    public string LoginProvider { get; set; }
        //    public string RedirectUri { get; set; }
        //    public string UserId { get; set; }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
        //        if (UserId != null)
        //        {
        //            properties.Dictionary[XsrfKey] = UserId;
        //        }
        //        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        //    }
        //}
        #endregion
    }
}