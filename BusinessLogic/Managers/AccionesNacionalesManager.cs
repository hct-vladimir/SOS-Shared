using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using AccionesNacionale = Cerberus.Sos.Accounting.BusinessLogic.Entities.AccionesNacionale;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class AccionesNacionalesManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<AccionesNacionale> GetAllAccionesNacionales()
        {
            var accionesNacionalesDb = dbContext.AccionesNacionales.ToList();
            MapperManager.GetInstance();

            var accionesNacionales = new List<AccionesNacionale>();
            accionesNacionalesDb.ForEach(p => accionesNacionales.Add(Mapper.Map<DataAccess.Models.AccionesNacionale, AccionesNacionale>(p)));

            return accionesNacionales;
        }

        public AccionesNacionale GetAccionNacional(int accionNacionalId)
        {
            var accionNacionalDb = dbContext.AccionesNacionales.Find(accionNacionalId);
            MapperManager.GetInstance();

            var accionNacional = Mapper.Map<DataAccess.Models.AccionesNacionale, AccionesNacionale>(accionNacionalDb);

            return accionNacional;
        }

        public Resultado InsertAccionNacional(AccionesNacionale accionNacional)
        {
            MapperManager.GetInstance();

            try
            {
                var AccionNacionalDb = Mapper.Map<AccionesNacionale, DataAccess.Models.AccionesNacionale>(accionNacional);
                dbContext.AccionesNacionales.Add(AccionNacionalDb);
                dbContext.SaveChanges();
                return new Resultado("La Accion Nacional se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateAccionNacional(AccionesNacionale accionNacional)
        {
            MapperManager.GetInstance();

            try
            {
                var AccionNacionalDb = Mapper.Map<AccionesNacionale, DataAccess.Models.AccionesNacionale>(accionNacional);
                dbContext.Entry(AccionNacionalDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("La Accion Nacional se guardó correctamente.");
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
