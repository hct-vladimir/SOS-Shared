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
using TraspasosPrograma = Cerberus.Sos.Accounting.BusinessLogic.Entities.TraspasosPrograma;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class TraspasosProgramasManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<TraspasosPrograma> GetAllTraspasosProgramas()
        {
            var traspasosProgramasDb = dbContext.TraspasosProgramas.ToList();
            MapperManager.GetInstance();

            var traspasosProgramas = new List<Entities.TraspasosPrograma>();
            traspasosProgramasDb.ForEach(p => traspasosProgramas.Add(Mapper.Map<DataAccess.Models.TraspasosPrograma, Entities.TraspasosPrograma>(p)));

            return traspasosProgramas;
        }

        public TraspasosPrograma GetCuentasTraspaso(int traspasosProgramaId)
        {
            var traspasosProgramaDb = dbContext.TraspasosProgramas.Find(traspasosProgramaId);
            MapperManager.GetInstance();

            var traspasosPrograma = Mapper.Map<DataAccess.Models.TraspasosPrograma, Entities.TraspasosPrograma>(traspasosProgramaDb);

            return traspasosPrograma;
        }

        public Resultado InsertTraspasosPrograma(TraspasosPrograma traspasosPrograma)
        {
            MapperManager.GetInstance();

            try
            {
                var traspasosProgramaDb = Mapper.Map<TraspasosPrograma, DataAccess.Models.TraspasosPrograma>(traspasosPrograma);

                traspasosProgramaDb.Activo = true;
                traspasosProgramaDb.FechaCreacion = DateTime.Now;
                traspasosProgramaDb.FechaModificacion = DateTime.Now;
                dbContext.TraspasosProgramas.Add(traspasosProgramaDb);
                dbContext.SaveChanges();
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateTraspasosPrograma(TraspasosPrograma traspasosPrograma)
        {
            MapperManager.GetInstance();

            try
            {
                var traspasosProgramaDb = dbContext.TraspasosProgramas.Find(traspasosPrograma.Id);
                if (traspasosProgramaDb != null)
                {

                    traspasosProgramaDb.UsuarioModificacion = traspasosPrograma.UsuarioModificacion;
                    traspasosProgramaDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(traspasosProgramaDb).State = EntityState.Modified;
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

        public Resultado DeleteTraspasosPrograma(int id)
        {
            MapperManager.GetInstance();

            try
            {
                var traspasosProgramaDb = dbContext.TraspasosProgramas.Find(id);
                traspasosProgramaDb.Activo = false;
                traspasosProgramaDb.FechaModificacion = DateTime.Now;
                dbContext.Entry(traspasosProgramaDb).State = EntityState.Modified;
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
