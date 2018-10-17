using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Security.Entities;
using AutoMapper;
using Cerberus.Sos.Accounting.Log;

namespace Cerberus.Sos.Accounting.Security.Managers
{
    public class UsersManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<User> GetAllUsuarios()
        {
            var usuariosDb = dbContext.Usuarios.ToList();

            var usuarios = new List<User>();
            usuariosDb.ForEach(p => usuarios.Add(Mapper.Map<Usuario, User>(p)));

            return usuarios;
        }

        public User GetUsuario(int usuarioId)
        {
            var usuarioDb = dbContext.Usuarios.Find(usuarioId);

            var usuario = Mapper.Map<Usuario, User>(usuarioDb);

            return usuario;
        }

        public Result InsertUsuario(User usuario)
        {
            try
            {
                var usuarioDb = Mapper.Map<User, DataAccess.Models.Usuario>(usuario);
                dbContext.Usuarios.Add(usuarioDb);
                dbContext.SaveChanges();
                return new Result("El Usuario se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Result("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Result UpdateUsuario(User usuario)
        {
            try
            {
                var usuarioDb = Mapper.Map<User, DataAccess.Models.Usuario>(usuario);
                dbContext.Entry(usuarioDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Result("El Usuario se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Result("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public User GetUserByUsuarioPassword(string usuario, string password)
        {
            User usuarioValido = null;
            var usuarioDb = dbContext.Usuarios.FirstOrDefault(u => u.Login == usuario && u.Password == password && u.Activo);

            if (usuarioDb != null)
            {
                usuarioValido = Mapper.Map<DataAccess.Models.Usuario, User>(usuarioDb);
            }

            return usuarioValido;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
