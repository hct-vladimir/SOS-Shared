using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Cerberus.Sos.Accounting.Security.Entities;

namespace Cerberus.Sos.Accounting.Security.Managers
{
    public class SecurityManager
    {
        private UsersManager usersManager = new UsersManager();
        private RolesManager rolesManager = new RolesManager();

        public User Login(User loginUser)
        {
            var validUser = usersManager.GetUserByUsuarioPassword(loginUser.Login, loginUser.Password);
            return validUser;
        }

        public HttpCookie GetAuthCookie(User validUser)
        {
            var sessionRoles = rolesManager.GetSessionRoles(validUser);
            DateTime expires = DateTime.Now.AddMinutes(30);
            var authTicket = new FormsAuthenticationTicket(
                1,                  // version
                validUser.Login,        // user name
                DateTime.Now,       // created
                expires,            // expires
                validUser.RememberMe,   // persistent?
                sessionRoles        // roles for use in IsInRole() method
                );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            return authCookie;
        }
    }
}
