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
using Cierre = Cerberus.Sos.Accounting.BusinessLogic.Entities.Cierre;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class CierresManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Entities.Cierre> GetCierresByGestion(int gestion)
        {
            MapperManager.GetInstance();

            var cierresDb =
                dbContext.Cierres.Where(p => p.Gestion == gestion).ToList();

            var cierres = new List<Entities.Cierre>();

            cierresDb.ForEach(p => cierres.Add(Mapper.Map<DataAccess.Models.Cierre, Entities.Cierre>(p)));

            return cierres;
        }

        ////public Entities.Cierre GetCierreByCiudadGestion(int ciudadId, int gestion)
        ////{
        ////    MapperManager.GetInstance();

        ////    var cierreDb =
        ////        dbContext.Cierres.FirstOrDefault(p => p.CiudadId == ciudadId && p.Gestion == gestion);

        ////    var cierre = Mapper.Map<DataAccess.Models.Cierre, Entities.Cierre>(cierreDb);

        ////    return cierre;
        ////}

        ////public List<CierreCiudad> GetCierresCiudades(int cierreId)
        ////{
        ////    MapperManager.GetInstance();

        ////    var cierreCiudadDb = dbContext.VCierres.Where(p => p.CierreId == cierreId).ToList();

        ////    var cierres = new List<CierreCiudad>();

        ////    cierreCiudadDb.ForEach(p => cierres.Add(Mapper.Map<VCierre, CierreCiudad>(p)));

        ////    return cierres;
        ////}

        public Resultado InsertCierre(Entities.Cierre cierre)
        {
            MapperManager.GetInstance();

            try
            {
                var cierreDb = Mapper.Map<Entities.Cierre, DataAccess.Models.Cierre>(cierre);

                cierreDb.EstadoCierreId = 1;
                cierreDb.FechaCreacion = DateTime.Now;
                cierreDb.FechaModificacion = DateTime.Now;
                dbContext.Cierres.Add(cierreDb);
                dbContext.SaveChanges();
                return new Resultado("El Cierre se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateCierre(Cierre cierre)
        {
            try
            {
                var cierreDb = dbContext.Cierres.FirstOrDefault(p => p.Id == cierre.Id);
                if (cierreDb != null)
                {
                    cierreDb.EstadoCierreId = cierre.EstadoCierreId;
                    cierreDb.UsuarioModificacion = cierre.UsuarioModificacion;
                    cierreDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(cierreDb).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return new Resultado("El registro se guardó correctamente.");
                }
                return new Resultado("No se encontró el cierre específicado");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        ////public Resultado CambiarEstado(int ciudadId, int gestion, short estadoId, string observaciones, string filasObservadas)
        ////{
        ////    try
        ////    {
        ////        var cierreDb = dbContext.Cierres.FirstOrDefault(p => p.CiudadId == ciudadId && p.Gestion == gestion);
        ////        if (cierreDb != null)
        ////        {
        ////            cierreDb.EstadoCierreId = estadoId;
        ////            if (estadoId != 3)
        ////            {
        ////                cierreDb.Observaciones = observaciones;
        ////                cierreDb.FilasObservadas = filasObservadas;
        ////            }

        ////            if (estadoId == 3)
        ////            {
        ////                var observacionesManager = new ObservacionesManager();
        ////                var resultadoObservaciones = observacionesManager.UpdateObservacionesCierre(cierreDb.Id);
        ////            }
        ////            cierreDb.FechaModificacion = DateTime.Now;

        ////            dbContext.Entry(cierreDb).State = EntityState.Modified;
        ////            dbContext.SaveChanges();
        ////            return new Resultado("El registro se guardó correctamente.");
        ////        }

        ////        return new Resultado("El cierre no existe.");
        ////    }
        ////    catch (Exception exception)
        ////    {

        ////        throw;
        ////    }
        ////}

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
