using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DellaSanta.Core;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Dellasanta.Web.Common.Security
{
    public class OwinAuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase _context;
        private const string AuthenticationType = "ApplicationCookie";

        public OwinAuthenticationService(HttpContextBase context)
        {
            _context = context;
        }

        public void SignIn(User user)
        {
            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };
            if (!string.IsNullOrEmpty(user.FirstName))
                claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));

            if (!string.IsNullOrEmpty(user.LastName))
                claims.Add(new Claim(ClaimTypes.Surname, user.LastName));

            foreach (var item in user.Claims)
            {
                claims.Add(new Claim(item.ClaimType, item.ClaimValue));
            }

            ClaimsIdentity identity = new ClaimsIdentity(claims, AuthenticationType);

            IOwinContext context = _context.Request.GetOwinContext();
            IAuthenticationManager authenticationManager = context.Authentication;
            //var context = Request.GetOwinContext();
            //var authManager = context.Authentication;

            authenticationManager.SignIn(identity);
            //authManager.SignIn(new AuthenticationProperties
            //{ IsPersistent = model.RememberMe }, identity);
            
        }

        public void SignOut()
        {
            IOwinContext context = _context.Request.GetOwinContext();
            IAuthenticationManager authenticationManager = context.Authentication;

            authenticationManager.SignOut(AuthenticationType);
        }
    }
}
