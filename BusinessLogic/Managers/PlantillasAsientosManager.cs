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
using PlantillaAsiento = Cerberus.Sos.Accounting.BusinessLogic.Entities.PlantillaAsiento;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class PlantillasAsientosManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Entities.PlantillaAsiento> GetAllPlantillasAsientos()
        {
            var plantillasAsientosDb = dbContext.PlantillasAsientos.Where(p => p.Activo).ToList();
            MapperManager.GetInstance();

            var plantillasAsientos = new List<Entities.PlantillaAsiento>();
            plantillasAsientosDb.ForEach(p => plantillasAsientos.Add(Mapper.Map<DataAccess.Models.PlantillaAsiento, Entities.PlantillaAsiento>(p)));

            return plantillasAsientos;
        }

        public List<Entities.PlantillaAsiento> GetPlantillasAsientosPorUsuario(string usuarioLogin)
        {
            var plantillasAsientosDb = dbContext.PlantillasAsientos.Where(pa => pa.UsuarioCreacion == usuarioLogin && pa.Activo).ToList();
            MapperManager.GetInstance();

            var plantillasAsientos = new List<Entities.PlantillaAsiento>();
            plantillasAsientosDb.ForEach(p => plantillasAsientos.Add(Mapper.Map<DataAccess.Models.PlantillaAsiento, Entities.PlantillaAsiento>(p)));

            return plantillasAsientos;
        }

        public Entities.PlantillaAsiento GetPlantillaAsiento(int id)
        {
            var plantillaAsientoDb = dbContext.PlantillasAsientos.Find(id);
            MapperManager.GetInstance();

            var plantillaAsiento = Mapper.Map<DataAccess.Models.PlantillaAsiento, Entities.PlantillaAsiento>(plantillaAsientoDb);

            return plantillaAsiento;
        }

        public Resultado InsertPlantillaAsiento(Entities.PlantillaAsiento plantillaAsiento)
        {
            MapperManager.GetInstance();

            try
            {
                //Obtener el Id de Usuario
                var usuarioActual = dbContext.Usuarios.FirstOrDefault(u => u.Login == plantillaAsiento.UsuarioCreacion);

                var plantillaAsientoDb = Mapper.Map<Entities.PlantillaAsiento, DataAccess.Models.PlantillaAsiento>(plantillaAsiento);

                plantillaAsientoDb.UsuarioId = usuarioActual.Id;
                plantillaAsientoDb.Activo = true;
                plantillaAsientoDb.FechaCreacion = DateTime.Now;
                plantillaAsientoDb.FechaModificacion = DateTime.Now;
                dbContext.PlantillasAsientos.Add(plantillaAsientoDb);
                dbContext.SaveChanges();

                // Guardar detalle de la Plantilla
                var comprobante = dbContext.Comprobantes.Find(plantillaAsiento.ComprobanteId);
                var plantillaCuenta = new DataAccess.Models.PlantillaCuenta();
                foreach (var cuentaAsiento in comprobante.CuentasAsientos)
                {
                    plantillaCuenta = new DataAccess.Models.PlantillaCuenta
                    {
                        PlantillaAsientoId = plantillaAsientoDb.Id,
                        AccionNacionalId = cuentaAsiento.AccionNacionalId,
                        AnexoTributarioId = cuentaAsiento.AnexoTributarioId,
                        CodigoAuditoriaId = cuentaAsiento.CodigoAuditoriaId,
                        ContraparteId = cuentaAsiento.ContraparteId,
                        CuentaContableId = cuentaAsiento.CuentaContableId,
                        PlanProgramaticoId = cuentaAsiento.PlanProgramaticoId,
                        TerritorioId = cuentaAsiento.TerritorioId,
                        Glosa = cuentaAsiento.Glosa,
                        Debe = cuentaAsiento.Debe,
                        Haber = cuentaAsiento.Haber,
                        NotasAdicionales = cuentaAsiento.NotasAdicionales,
                        EsAjuste = cuentaAsiento.EsAjuste,
                        EsDebe = cuentaAsiento.EsDebe,
                        Activo = true,
                        UsuarioCreacion = plantillaAsiento.UsuarioCreacion,
                        UsuarioModificacion = plantillaAsiento.UsuarioModificacion,
                        FechaCreacion = DateTime.Now,
                        FechaModificacion = DateTime.Now
                    };
                    dbContext.PlantillasCuentas.Add(plantillaCuenta);
                }
                dbContext.SaveChanges();

                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdatePlantillaAsiento(PlantillaAsiento plantillaAsiento)
        {
            MapperManager.GetInstance();

            try
            {
                var plantillaAsientoDb = dbContext.PlantillasAsientos.Find(plantillaAsiento.Id);
                if (plantillaAsientoDb != null)
                {
                    plantillaAsientoDb.Nombre = plantillaAsiento.Nombre;
                    plantillaAsientoDb.Descripcion = plantillaAsiento.Descripcion;

                    plantillaAsientoDb.UsuarioModificacion = plantillaAsiento.UsuarioModificacion;
                    plantillaAsientoDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(plantillaAsientoDb).State = EntityState.Modified;
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

        public Resultado DeletePlantillaAsiento(int id, string usuarioActual)
        {
            MapperManager.GetInstance();

            try
            {
                var plantillaAsientoDb = dbContext.PlantillasAsientos.Find(id);
                plantillaAsientoDb.Activo = false;
                plantillaAsientoDb.UsuarioModificacion = usuarioActual;
                plantillaAsientoDb.FechaModificacion = DateTime.Now;
                dbContext.Entry(plantillaAsientoDb).State = EntityState.Modified;
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
