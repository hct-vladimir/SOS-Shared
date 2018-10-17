using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Security.Entities;

namespace Cerberus.Sos.Accounting.Security.Managers
{
    public class RolesManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public string GetSessionRoles(User user)
        {
            var roles = string.Empty;
            var usuariosRoles = dbContext.UsuariosRoles.Where(ur => ur.UsuarioId == user.Id).ToList();
            if (usuariosRoles.Count > 0)
            {
                roles = string.Join("|", usuariosRoles.Select(ur => ur.Rol.Codigo).ToList());
            }
            return roles;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
