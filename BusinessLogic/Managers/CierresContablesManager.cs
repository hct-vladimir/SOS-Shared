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
using CierreContable = Cerberus.Sos.Accounting.BusinessLogic.Entities.CierreContable;
using PeriodoCierre = Cerberus.Sos.Accounting.BusinessLogic.Entities.PeriodoCierre;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class CierresContablesManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Entities.CierreContable> GetCierresContablesByPeriodo(int periodoId)
        {
            MapperManager.GetInstance();

            var cierresContablesDb =
                dbContext.CierresContables.Where(p => p.PeriodoCierreId == periodoId).ToList();

            var cierresContables = new List<Entities.CierreContable>();

            cierresContablesDb.ForEach(p => cierresContables.Add(Mapper.Map<DataAccess.Models.CierreContable, Entities.CierreContable>(p)));

            return cierresContables;
        }

        public PeriodoCierre GetPeriodoActivo()
        {
            MapperManager.GetInstance();

            var periodoActivoDb = dbContext.PeriodosCierres.FirstOrDefault(p => p.Activo);
            var periodoActivo = Mapper.Map<DataAccess.Models.PeriodoCierre, PeriodoCierre>(periodoActivoDb);

            return periodoActivo;
        }


        public Entities.CierreContable GetCierreContableByCiudadPeriodo(int ciudadId, int periodoId)
        {
            MapperManager.GetInstance();

            var cierreDb =
                dbContext.CierresContables.FirstOrDefault(p => p.CiudadId == ciudadId && p.PeriodoCierreId == periodoId);

            var cierreContable = Mapper.Map<DataAccess.Models.CierreContable, Entities.CierreContable>(cierreDb);

            return cierreContable;
        }

        public List<CierreContableCiudad> GetCierresContablesCiudades(int periodoId)
        {
            return GetCierresContablesCiudades(null, periodoId);
        }

        public List<CierreContableCiudad> GetCierresContablesCiudades(List<int> ciudadesIds, int periodoId)
        {
            MapperManager.GetInstance();
            var cierreContableCiudadDb = new List<VCierreContable>();

            if (ciudadesIds != null)
            {
                cierreContableCiudadDb = dbContext.VCierresContables.Where(c => ciudadesIds.Contains(c.CiudadId) && c.PeriodoCierreId == periodoId).ToList();
            }
            else
            {
                cierreContableCiudadDb = dbContext.VCierresContables.Where(c => c.PeriodoCierreId == periodoId).ToList();
            }

            var cierresContables = new List<CierreContableCiudad>();

            cierreContableCiudadDb.ForEach(c => cierresContables.Add(Mapper.Map<VCierreContable, CierreContableCiudad>(c)));

            return cierresContables;
        }



        public Resultado InsertCierreContable(Entities.CierreContable cierreContable)
        {
            MapperManager.GetInstance();

            try
            {
                var cierreDb = Mapper.Map<Entities.CierreContable, DataAccess.Models.CierreContable>(cierreContable);

                cierreDb.EstadoCierreId = 1;
                cierreDb.FechaCreacion = DateTime.Now;
                cierreDb.FechaModificacion = DateTime.Now;
                dbContext.CierresContables.Add(cierreDb);
                dbContext.SaveChanges();
                return new Resultado("El CierreContable se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateCierreContable(CierreContable cierreContable)
        {
            try
            {
                var cierreDb = dbContext.CierresContables.FirstOrDefault(p => p.Id == cierreContable.Id);
                if (cierreDb != null)
                {
                    cierreDb.EstadoCierreId = cierreContable.EstadoCierreId;
                    cierreDb.UsuarioModificacion = cierreContable.UsuarioModificacion;
                    cierreDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(cierreDb).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return new Resultado("El registro se guardó correctamente.");
                }
                return new Resultado("No se encontró el cierreContable específicado");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado CambiarEstado(int ciudadId, int periodoId, short estadoId)
        {
            try
            {
                var presupuestoDb = dbContext.CierresContables.FirstOrDefault(p => p.CiudadId == ciudadId && p.PeriodoCierreId == periodoId);
                if (presupuestoDb != null)
                {
                    presupuestoDb.EstadoCierreId = estadoId;
                    presupuestoDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(presupuestoDb).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return new Resultado("El registro se guardó correctamente.");
                }

                return new Resultado("El presupuesto no existe.");
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
