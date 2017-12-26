using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DellaSanta.Core;

namespace Dellasanta.Web.Common.Security
{
    public interface IAuthenticationService
    {
        void SignIn(User user);
        void SignOut();
    }
}
