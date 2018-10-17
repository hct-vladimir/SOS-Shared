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
using Retencion = Cerberus.Sos.Accounting.BusinessLogic.Entities.Retencion;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class RetencionesManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Entities.Retencion> GetAllRetenciones()
        {
            var retencionesDb = dbContext.Retenciones.ToList();
            MapperManager.GetInstance();

            var retenciones = new List<Entities.Retencion>();
            retencionesDb.ForEach(p => retenciones.Add(Mapper.Map<DataAccess.Models.Retencion, Entities.Retencion>(p)));

            return retenciones;
        }

        public List<Entities.Retencion> GetRetencionesByMesGestion(int mes, int gestion)
        {
            var retencionesDb = dbContext.Retenciones.Where(r=>r.FechaComprobante.Value.Month == mes && r.FechaComprobante.Value.Year == gestion).ToList();
            MapperManager.GetInstance();

            var retenciones = new List<Entities.Retencion>();
            retencionesDb.ForEach(p => retenciones.Add(Mapper.Map<DataAccess.Models.Retencion, Entities.Retencion>(p)));

            return retenciones;
        }

        public Entities.Retencion GetRetencion(int retencionId)
        {
            var retencionDb = dbContext.Retenciones.Find(retencionId);
            MapperManager.GetInstance();

            var retencion = Mapper.Map<DataAccess.Models.Retencion, Entities.Retencion>(retencionDb);

            return retencion;
        }

        public Resultado InsertRetencion(Entities.Retencion retencion)
        {
            MapperManager.GetInstance();

            try
            {
                var retencionDb = Mapper.Map<Entities.Retencion, DataAccess.Models.Retencion>(retencion);

                // Se inserta datos del comprador si no existe
                
                retencionDb.Activo = true;
                retencionDb.FechaCreacion = DateTime.Now;
                retencionDb.FechaModificacion = DateTime.Now;

                dbContext.Retenciones.Add(retencionDb);
                dbContext.SaveChanges();
                retencion.Id = retencionDb.Id;
                return new Resultado("La Retencion se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateRetencion(Entities.Retencion retencion)
        {
            MapperManager.GetInstance();

            try
            {
                var retencionDb = Mapper.Map<Retencion, DataAccess.Models.Retencion>(retencion);
                dbContext.Entry(retencionDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("La Retencion se guardó correctamente.");
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
