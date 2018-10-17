using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using Observacion = Cerberus.Sos.Accounting.BusinessLogic.Entities.Observacion;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class ObservacionesManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Observacion> GetAllObservaciones()
        {
            var observacionesDb = dbContext.Observaciones.ToList();
            MapperManager.GetInstance();

            var observaciones = new List<Observacion>();
            observacionesDb.ForEach(p => observaciones.Add(Mapper.Map<DataAccess.Models.Observacion, Entities.Observacion>(p)));

            return observaciones;
        }

        public List<Observacion> GetObservacionesPorEntidadFacility(short tipoObservacionId, int entidadId, int facilityId)
        {
            var observacionesDb = dbContext.Observaciones
                .Where(o => o.TipoObservacionId == tipoObservacionId 
                && o.EntidadId == entidadId 
                && o.FacilityId == facilityId
                && o.Activo).ToList();
            MapperManager.GetInstance();

            var observaciones = new List<Observacion>();
            observacionesDb.ForEach(p => observaciones.Add(Mapper.Map<DataAccess.Models.Observacion, Entities.Observacion>(p)));

            return observaciones;
        }

        public Observacion GetObservacion(int id)
        {
            var observacionDb = dbContext.Observaciones.Find(id);
            MapperManager.GetInstance();

            var observacion = Mapper.Map<DataAccess.Models.Observacion, Observacion>(observacionDb);

            return observacion;
        }

        public Resultado InsertObservacion(Observacion observacion)
        {
            MapperManager.GetInstance();

            try
            {
                var observacionDb = Mapper.Map<Observacion, DataAccess.Models.Observacion>(observacion);

                observacionDb.Activo = true;
                observacionDb.FechaCreacion = DateTime.Now;
                observacionDb.FechaModificacion = DateTime.Now;
                dbContext.Observaciones.Add(observacionDb);
                dbContext.SaveChanges();
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateObservacion(Observacion observacion)
        {
            MapperManager.GetInstance();

            try
            {
                var observacionDb = dbContext.Observaciones.Find(observacion.Id);
                if (observacionDb != null)
                {
                    observacionDb.Descripcion = observacion.Descripcion;
                    observacionDb.FilasObservadas = observacion.FilasObservadas;
                    observacionDb.EsNacional = observacion.EsNacional;
                    observacionDb.Aprobado = observacion.Aprobado;

                    observacionDb.UsuarioModificacion = observacion.UsuarioModificacion;
                    observacionDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(observacionDb).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return new Resultado("El registro se guardó correctamente.");
                }
                return new Resultado("No se encontró el registro especificado");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado DeleteObservacion(int id, string usuarioActual)
        {
            MapperManager.GetInstance();

            try
            {
                var observacionDb = dbContext.Observaciones.Find(id);
                observacionDb.Activo = false;
                observacionDb.UsuarioModificacion = usuarioActual;
                observacionDb.FechaModificacion = DateTime.Now;
                dbContext.Entry(observacionDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El registro se eliminó correctamente.");
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
