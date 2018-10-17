using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Usuario = Cerberus.Sos.Accounting.BusinessLogic.Entities.Usuario;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class UsuariosManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Usuario> GetAllUsuarios()
        {
            var usuariosDb = dbContext.Usuarios.ToList();
            MapperManager.GetInstance();

            var usuarios = new List<Usuario>();
            usuariosDb.ForEach(p => usuarios.Add(Mapper.Map<DataAccess.Models.Usuario, Usuario>(p)));

            return usuarios;
        }

        public Usuario GetUsuario(int usuarioId)
        {
            var usuarioDb = dbContext.Usuarios.Find(usuarioId);
            MapperManager.GetInstance();

            var usuario = Mapper.Map<DataAccess.Models.Usuario, Usuario>(usuarioDb);

            return usuario;
        }
        public Usuario GetUsuarioByLogin(string usuarioLogin)
        {
            var usuarioDb = dbContext.Usuarios.FirstOrDefault(u => u.Login == usuarioLogin);
            MapperManager.GetInstance();

            var usuario = Mapper.Map<DataAccess.Models.Usuario, Usuario>(usuarioDb);

            return usuario;
        }

        public List<int> GetCiudadesUsuario(string usuarioLogin)
        {
            var usuarioActual = dbContext.Usuarios.FirstOrDefault(u => u.Login == usuarioLogin && u.Activo);

            var ciudadesIds =
                dbContext.UsuariosCiudades.Where(uc => uc.UsuarioId == usuarioActual.Id && uc.Activo)
                    .Select(uc => uc.CiudadId)
                    .ToList();

            return ciudadesIds;
        }

        public Resultado InsertUsuario(Usuario usuario)
        {
            MapperManager.GetInstance();

            try
            {
                var usuarioDb = Mapper.Map<Usuario, DataAccess.Models.Usuario>(usuario);
                dbContext.Usuarios.Add(usuarioDb);
                dbContext.SaveChanges();
                return new Resultado("El Usuario se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateUsuario(Usuario usuario)
        {
            MapperManager.GetInstance();

            try
            {
                var usuarioDb = Mapper.Map<Usuario, DataAccess.Models.Usuario>(usuario);
                dbContext.Entry(usuarioDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El Usuario se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Usuario Login(string usuario, string password)
        {
            MapperManager.GetInstance();

            Usuario usuarioValido = null;
            var usuarioDb = dbContext.Usuarios.FirstOrDefault(u => u.Login == usuario && u.Password == password && u.Activo);

            if (usuarioDb != null)
            {
                usuarioValido = Mapper.Map<DataAccess.Models.Usuario, Usuario>(usuarioDb);
            }

            return usuarioValido;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
