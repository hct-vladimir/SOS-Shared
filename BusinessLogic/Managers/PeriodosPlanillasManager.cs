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
using PeriodoPlanilla = Cerberus.Sos.Accounting.BusinessLogic.Entities.PeriodoPlanilla;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class PeriodosPlanillasManager : IDisposable
    {
        private PlanillasSosDBEntities dbContext = new PlanillasSosDBEntities();

        public List<PeriodoPlanilla> GetAllPeriodosPlanillas()
        {
            // TODO: Obtener planillas solo de la gestión actual
            var periodosPlanillasDb = dbContext.PERIODOS_PLANILLAS.OrderByDescending(p => p.Mes).ToList();
            MapperManager.GetInstance();

            var periodosPlanillas = new List<PeriodoPlanilla>();
            periodosPlanillasDb.ForEach(p => periodosPlanillas.Add(Mapper.Map<PERIODOS_PLANILLAS, PeriodoPlanilla>(p)));

            return periodosPlanillas;
        }

        public PeriodoPlanilla GetPeriodoPlanilla(int periodoPlanillaId)
        {
            var periodoPlanillaDb = dbContext.PERIODOS_PLANILLAS.Find(periodoPlanillaId);
            MapperManager.GetInstance();

            var periodoPlanilla = Mapper.Map<PERIODOS_PLANILLAS, PeriodoPlanilla>(periodoPlanillaDb);

            return periodoPlanilla;
        }

        public Resultado InsertPeriodoPlanilla(PeriodoPlanilla periodoPlanilla)
        {
            MapperManager.GetInstance();

            try
            {
                var periodoPlanillaDb = Mapper.Map<PeriodoPlanilla, PERIODOS_PLANILLAS>(periodoPlanilla);

                periodoPlanillaDb.UsuarioCreacion = "dbo";
                periodoPlanillaDb.FechaCreacion = DateTime.Now;
                periodoPlanillaDb.UsuarioModificacion = "dbo";
                periodoPlanillaDb.FechaModificacion = DateTime.Now;

                dbContext.PERIODOS_PLANILLAS.Add(periodoPlanillaDb);
                dbContext.SaveChanges();
                periodoPlanilla.Id = periodoPlanillaDb.Id;
                return new Resultado("El PeriodoPlanilla se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }
        
        public Resultado UpdatePeriodoPlanilla(PeriodoPlanilla periodoPlanilla)
        {
            MapperManager.GetInstance();

            try
            {
                var periodoPlanillaDb = Mapper.Map<PeriodoPlanilla, PERIODOS_PLANILLAS>(periodoPlanilla);
                dbContext.Entry(periodoPlanillaDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El PeriodoPlanilla se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado CrearPeriodoPlanilla(int gestion, int mes)
        {
            MapperManager.GetInstance();

            try
            {
                var periodoPlanillaDb = new PERIODOS_PLANILLAS
                {
                    Gestion = (short) gestion,
                    Mes = (short) mes,
                    EstadoPlanillaId = 1,
                    EsRetroactivo = false,
                    UsuarioCreacion = "dbo",
                    FechaCreacion = DateTime.Now,
                    UsuarioModificacion = "dbo",
                    FechaModificacion = DateTime.Now
                };

                dbContext.PERIODOS_PLANILLAS.Add(periodoPlanillaDb);
                dbContext.SaveChanges();
                return new Resultado("El PeriodoPlanilla se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado CerrarPeriodoPlanilla(int gestion, int mes)
        {
            var periodoPlanilla =
                dbContext.PERIODOS_PLANILLAS.FirstOrDefault(
                    p => p.Gestion == gestion && p.Mes == mes && !p.EsRetroactivo);

            try
            {
                periodoPlanilla.EstadoPlanillaId = 2;
                periodoPlanilla.UsuarioModificacion = "dbo";
                periodoPlanilla.FechaModificacion = DateTime.Now;

                dbContext.Entry(periodoPlanilla).State = EntityState.Modified;
                dbContext.SaveChanges();
                
                return new Resultado("El Recurso se guardó correctamente.");
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
