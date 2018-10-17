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
using PlantillaCuenta = Cerberus.Sos.Accounting.BusinessLogic.Entities.PlantillaCuenta;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class PlantillasCuentasManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Entities.PlantillaCuenta> GetAllPlantillasCuentas()
        {
            var plantillasCuentasDb = dbContext.PlantillasCuentas.Where(p => p.Activo).ToList();
            MapperManager.GetInstance();

            var plantillasCuentas = new List<Entities.PlantillaCuenta>();
            plantillasCuentasDb.ForEach(p => plantillasCuentas.Add(Mapper.Map<DataAccess.Models.PlantillaCuenta, Entities.PlantillaCuenta>(p)));

            return plantillasCuentas;
        }

        public List<Entities.PlantillaCuenta> GetPlantillasCuentasPorAsiento(int plantillaAsientoId)
        {
            var plantillasCuentasDb = dbContext.PlantillasCuentas.Where(pa => pa.PlantillaAsientoId == plantillaAsientoId && pa.Activo).ToList();
            MapperManager.GetInstance();

            var plantillasCuentas = new List<Entities.PlantillaCuenta>();
            plantillasCuentasDb.ForEach(p => plantillasCuentas.Add(Mapper.Map<DataAccess.Models.PlantillaCuenta, Entities.PlantillaCuenta>(p)));

            return plantillasCuentas;
        }

        public Entities.PlantillaCuenta GetPlantillaCuenta(int id)
        {
            var plantillaCuentaDb = dbContext.PlantillasCuentas.Find(id);
            MapperManager.GetInstance();

            var plantillaCuenta = Mapper.Map<DataAccess.Models.PlantillaCuenta, Entities.PlantillaCuenta>(plantillaCuentaDb);

            return plantillaCuenta;
        }

        public Resultado InsertPlantillaCuenta(Entities.PlantillaCuenta plantillaCuenta)
        {
            MapperManager.GetInstance();

            try
            {
                var plantillaCuentaDb = Mapper.Map<Entities.PlantillaCuenta, DataAccess.Models.PlantillaCuenta>(plantillaCuenta);

                plantillaCuentaDb.Activo = true;
                plantillaCuentaDb.FechaCreacion = DateTime.Now;
                plantillaCuentaDb.FechaModificacion = DateTime.Now;
                dbContext.PlantillasCuentas.Add(plantillaCuentaDb);
                dbContext.SaveChanges();
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdatePlantillaCuenta(PlantillaCuenta plantillaCuenta)
        {
            MapperManager.GetInstance();

            try
            {
                var plantillaCuentaDb = dbContext.PlantillasCuentas.Find(plantillaCuenta.Id);
                if (plantillaCuentaDb != null)
                {
                    plantillaCuentaDb.AccionNacionalId = plantillaCuenta.AccionNacionalId;
                    plantillaCuentaDb.AnexoTributarioId = plantillaCuenta.AnexoTributarioId;
                    plantillaCuentaDb.CodigoAuditoriaId = plantillaCuenta.CodigoAuditoriaId;
                    plantillaCuentaDb.ContraparteId = plantillaCuenta.ContraparteId;
                    plantillaCuentaDb.CuentaContableId = plantillaCuenta.CuentaContableId;
                    plantillaCuentaDb.PlanProgramaticoId = plantillaCuenta.PlanProgramaticoId;
                    plantillaCuentaDb.TerritorioId = plantillaCuenta.TerritorioId;

                    plantillaCuentaDb.Debe = plantillaCuenta.Debe;
                    plantillaCuentaDb.Haber = plantillaCuenta.Haber;
                    plantillaCuentaDb.Glosa = plantillaCuenta.Glosa;

                    plantillaCuentaDb.UsuarioModificacion = plantillaCuenta.UsuarioModificacion;
                    plantillaCuentaDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(plantillaCuentaDb).State = EntityState.Modified;
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

        public Resultado DeletePlantillaCuenta(int id, string usuarioActual)
        {
            MapperManager.GetInstance();

            try
            {
                var plantillaCuentaDb = dbContext.PlantillasCuentas.Find(id);
                plantillaCuentaDb.Activo = false;
                plantillaCuentaDb.UsuarioModificacion = usuarioActual;
                plantillaCuentaDb.FechaModificacion = DateTime.Now;
                dbContext.Entry(plantillaCuentaDb).State = EntityState.Modified;
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
