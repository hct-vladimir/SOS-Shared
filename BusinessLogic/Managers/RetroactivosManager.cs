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

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class RetroactivosManager : IDisposable
    {
        private PlanillasSosDBEntities dbContext = new PlanillasSosDBEntities();

        public List<Retroactivo> GetAllRetroactivos()
        {
            // TODO: Obtener planillas solo de la gestión actual
            var retroactivosDb = dbContext.RETROACTIVOS.OrderByDescending(p => p.Mes).ToList();
            MapperManager.GetInstance();

            var retroactivos = new List<Retroactivo>();
            retroactivosDb.ForEach(p => retroactivos.Add(Mapper.Map<RETROACTIVO, Retroactivo>(p)));

            return retroactivos;
        }

        public Retroactivo GetRetroactivo(int retroactivoId)
        {
            var retroactivoDb = dbContext.RETROACTIVOS.Find(retroactivoId);
            MapperManager.GetInstance();

            var retroactivo = Mapper.Map<RETROACTIVO, Retroactivo>(retroactivoDb);

            return retroactivo;
        }

        public Resultado InsertRetroactivo(Retroactivo retroactivo)
        {
            MapperManager.GetInstance();

            try
            {
                var retroactivoDb = Mapper.Map<Retroactivo, RETROACTIVO>(retroactivo);

                retroactivoDb.EstadoPlanillaId = 1;
                retroactivoDb.FechaCreacion = DateTime.Now;
                retroactivoDb.FechaModificacion = DateTime.Now;

                dbContext.RETROACTIVOS.Add(retroactivoDb);
                dbContext.SaveChanges();
                retroactivo.Id = retroactivoDb.Id;
                return new Resultado("El Retroactivo se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateRetroactivo(Retroactivo retroactivo)
        {
            MapperManager.GetInstance();

            try
            {
                var retroactivoDb = Mapper.Map<Retroactivo, RETROACTIVO>(retroactivo);
                dbContext.Entry(retroactivoDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El Retroactivo se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        ////public Resultado CrearRetroactivo(int gestion, int mes)
        ////{
        ////    MapperManager.GetInstance();

        ////    try
        ////    {
        ////        var retroactivoDb = new RETROACTIVO
        ////        {
        ////            Gestion = (short)gestion,
        ////            Mes = (short)mes,
        ////            EstadoPlanillaId = 1,
        ////            UsuarioCreacion = "dbo",
        ////            FechaCreacion = DateTime.Now,
        ////            UsuarioModificacion = "dbo",
        ////            FechaModificacion = DateTime.Now
        ////        };

        ////        dbContext.RETROACTIVOS.Add(retroactivoDb);
        ////        dbContext.SaveChanges();
        ////        return new Resultado("El Retroactivo se guardó correctamente.");
        ////    }
        ////    catch (Exception excepcion)
        ////    {
        ////        LogHelper.RegisterError(excepcion.Message);
        ////        return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
        ////    }
        ////}

        public Resultado CerrarRetroactivo(int id)
        {
            var retroactivo = dbContext.RETROACTIVOS.Find(id);

            try
            {
                retroactivo.EstadoPlanillaId = 2;
                retroactivo.UsuarioModificacion = "dbo";
                retroactivo.FechaModificacion = DateTime.Now;

                dbContext.Entry(retroactivo).State = EntityState.Modified;
                dbContext.SaveChanges();

                return new Resultado("El Retroactivo se guardó correctamente.");
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
