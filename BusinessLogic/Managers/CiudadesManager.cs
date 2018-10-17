using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using Ciudad = Cerberus.Sos.Accounting.BusinessLogic.Entities.Ciudad;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class CiudadesManager:IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Ciudad> GetAllCiudades()
        {
            var ciudadesDb = dbContext.Ciudades.Where(c => c.Activo).ToList();
            MapperManager.GetInstance();

            var ciudades = new List<Ciudad>();
            ciudadesDb.ForEach(p => ciudades.Add(Mapper.Map<DataAccess.Models.Ciudad, Ciudad>(p)));

            return ciudades;
        }

        public List<Ciudad> GetCiudadesByNombre(string nomCiudad)
        {
            var ciudadesDb = dbContext.Ciudades.ToList().Where(item=>item.Nombre== nomCiudad).ToList();
            MapperManager.GetInstance();

            var ciudades = new List<Ciudad>();
            ciudadesDb.ForEach(p => ciudades.Add(Mapper.Map<DataAccess.Models.Ciudad, Ciudad>(p)));

            return ciudades;
        }


        public Ciudad GetCiudad(int ciudadId)
        {
            var ciudadDb = dbContext.Ciudades.Find(ciudadId);
            MapperManager.GetInstance();

            var ciudad = Mapper.Map<DataAccess.Models.Ciudad, Ciudad>(ciudadDb);

            return ciudad;
        }

        // mio
        public Ciudad GetCiudadNombre(string ciudadNombre)
        {
            var ciudadDb = dbContext.Ciudades.Find(ciudadNombre);
            MapperManager.GetInstance();

            var ciudad = Mapper.Map<DataAccess.Models.Ciudad, Ciudad>(ciudadDb);

            return ciudad;
        }

        public Resultado InsertCiudad(Ciudad ciudad)
        {
            MapperManager.GetInstance();

            try
            {
                var ciudadDb = Mapper.Map<Ciudad, DataAccess.Models.Ciudad>(ciudad);
                dbContext.Ciudades.Add(ciudadDb);
                dbContext.SaveChanges();
                return new Resultado("La ciudad se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateCiudad(Ciudad ciudad)
        {
            MapperManager.GetInstance();

            try
            {
                var ciudadDb = Mapper.Map<Ciudad, DataAccess.Models.Ciudad>(ciudad);
                dbContext.Entry(ciudadDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("La ciudad se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }


        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
