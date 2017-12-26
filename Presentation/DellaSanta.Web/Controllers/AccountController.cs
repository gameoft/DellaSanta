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

namespace DellaSanta.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        public AccountController()
        {
        }


        private bool ValidateCredentials(LoginViewModel model)
        {
            var user = _applicationDbContext.Users.Where(x => x.UserName == model.Email).FirstOrDefault();
            if (null != user)
            {
                if (Utils.Hash(model.Password) == user.Password)
                {
                    return true;
                }
            }

            return false;

        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                bool result = ValidateCredentials(model); // _activeDirectoryService.ValidateCredentials(model.Domain, model.UserName, model.Password);
                if (result)
                {
                    //var user = await _userService.GetUserByUserNameAsync(model.UserName);
                    var user = _applicationDbContext.Users.First(x => x.UserName == model.Email);


                    if (user != null && user.Active)
                    {
                        //var roleNames = user.Roles.Select(r => r.Name).ToList();
                        var role = user.Role;
                        //        _authenticationService.SignIn(user, roleNames);

                        //sign in
                        var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, model.Email),
                            new Claim(ClaimTypes.Email, model.Email),
                            new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                            new Claim(ClaimTypes.GivenName, user.FirstName ?? "Admin"),
                            new Claim(ClaimTypes.Surname, user.LastName ?? "Admin"),
                            new Claim(ClaimTypes.Role, user.Role),
                        };

                        foreach (var item in user.Claims)
                        {
                            claims.Add(new Claim(item.ClaimType, item.ClaimValue));
                        }

                        var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                        var context = Request.GetOwinContext();
                        var authManager = context.Authentication;

                        authManager.SignIn(new AuthenticationProperties
                        { IsPersistent = model.RememberMe }, identity);

                        //        _log.Info($"Login Successful: {user.UserName}");

                        // Redirect to return URL
                        if (!string.IsNullOrEmpty(returnUrl) && !string.Equals(returnUrl, "/") && Url.IsLocalUrl(returnUrl))
                            return RedirectToLocal(returnUrl);

                        //        // User is in a role, so redirect to Administration area
                        //        if (roleNames.Contains(Constants.RoleNames.Developer) ||
                        //            roleNames.Contains(Constants.RoleNames.ApplicationManager))
                        //            return RedirectToRoute("Dashboard");

                        return RedirectToAction("Index", "Home");
                        //sign in
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

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}

            //return View(model);
            return View("Login", model);
        }



        //
        // GET: /Account/Register
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
                User user = null;
                using (var identitydbContextTransaction = _applicationDbContext.Database.BeginTransaction())
                {
                    try
                    {

                        user = new User { UserName = model.Email, Active = true, FirstName = model.FirstName, LastName = model.LastName, Password = Utils.Hash(model.Password), Role = "Teacher" };
                        _applicationDbContext.Users.Add(user);

                        if (!string.IsNullOrEmpty(model.MobilePhone))
                            _applicationDbContext.Claims.Add(new UserClaims { ClaimType = ClaimTypes.MobilePhone, ClaimValue = model.MobilePhone, User = user });
                        if (!string.IsNullOrEmpty(model.Department))
                            _applicationDbContext.Claims.Add(new UserClaims { ClaimType = "Department", ClaimValue = model.Department, User = user });

                        var result = _applicationDbContext.SaveChangesAsync();

                        if (result.Result > 0)
                        {
                            identitydbContextTransaction.Commit();
                        }
                        else
                           ModelState.AddModelError("", "Operation failed.");

                    }
                    catch (Exception)
                    {
                        identitydbContextTransaction.Rollback();
                        //    ModelState.AddModelError("", "Unhandled exception. Please retry in 5 minutes.");

                        //_log.Info($"Login Fail: {model.UserName}");
                        ModelState.AddModelError("", "Operation failed.");
                        //throw;
                    }
                }
                //sign in
                var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, user.UserName),
                            new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                            new Claim(ClaimTypes.GivenName, user.FirstName),
                            new Claim(ClaimTypes.Surname, user.LastName),
                            new Claim(ClaimTypes.Role, user.Role),
                            };

                foreach (var item in user.Claims)
                {
                    claims.Add(new Claim(item.ClaimType, item.ClaimValue));
                }

                var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                var context = Request.GetOwinContext();
                var authManager = context.Authentication;

                authManager.SignIn(new AuthenticationProperties
                { IsPersistent = false }, identity);

                //        _log.Info($"Login Successful: {user.UserName}");

                return RedirectToAction("Index", "Home");
                //sign in
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

            //var appDbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            User user = null;
            if (ModelState.IsValid)
            {
                using (var identitydbContextTransaction = _applicationDbContext.Database.BeginTransaction())
                {

                    try
                    {
                        user = new User { UserName = model.Email, Active = true, FirstName = model.FirstName, LastName = model.LastName, Password = Utils.Hash(model.Password), Role = "Student" };
                        _applicationDbContext.Users.Add(user);
                        _applicationDbContext.Claims.Add(new UserClaims { ClaimType = ClaimTypes.StreetAddress, ClaimValue = model.Address, User = user });
                        var result = _applicationDbContext.SaveChangesAsync();

                        if (result.Result > 0)
                        {
                            identitydbContextTransaction.Commit();

                        }
                        else
                            ModelState.AddModelError("", "Operation failed.");


                    }
                    catch (Exception)
                    {
                        identitydbContextTransaction.Rollback();
                        //_log.Info($"Login Fail: {model.UserName}");
                        ModelState.AddModelError("", "Operation failed.");
                        //throw;
                    }
                }


                //sign in
                var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, user.UserName),
                            new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                            new Claim(ClaimTypes.GivenName, user.FirstName),
                            new Claim(ClaimTypes.Surname, user.LastName),
                            new Claim(ClaimTypes.Role, user.Role),
                            };

                foreach (var item in user.Claims)
                {
                    claims.Add(new Claim(item.ClaimType, item.ClaimValue));
                }

                var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                var context = Request.GetOwinContext();
                var authManager = context.Authentication;

                authManager.SignIn(new AuthenticationProperties
                { IsPersistent = false }, identity);

                //        _log.Info($"Login Successful: {user.UserName}");

                return RedirectToAction("Index", "Home");
                //sign in


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