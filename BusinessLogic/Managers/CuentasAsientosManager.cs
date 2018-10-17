using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using CuentaAsiento = Cerberus.Sos.Accounting.BusinessLogic.Entities.CuentaAsiento;
using PlantillaCuenta = Cerberus.Sos.Accounting.BusinessLogic.Entities.PlantillaCuenta;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class CuentasAsientosManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<CuentaAsiento> GetAllCuentasAsientos()
        {
            var cuentasAsientosDb = dbContext.CuentasAsientos.ToList();
            MapperManager.GetInstance();

            var cuentasAsientos = new List<CuentaAsiento>();
            cuentasAsientosDb.ForEach(p => cuentasAsientos.Add(Mapper.Map<DataAccess.Models.CuentaAsiento, CuentaAsiento>(p)));

            return cuentasAsientos;
        }

        public List<CuentaAsiento> GetCuentasAsientosByCuenta(string numeroCuenta)
        {
            var cuentasAsientosDb = dbContext.CuentasAsientos.Where(c => c.CuentaContable.Numero == numeroCuenta).ToList();
            MapperManager.GetInstance();

            var cuentasAsientos = new List<CuentaAsiento>();
            cuentasAsientosDb.ForEach(p => cuentasAsientos.Add(Mapper.Map<DataAccess.Models.CuentaAsiento, CuentaAsiento>(p)));

            return cuentasAsientos;
        }

        public CuentaAsiento GetCuentasAsiento(int cuentasAsientoId)
        {
            var cuentasAsientoDb = dbContext.CuentasAsientos.Find(cuentasAsientoId);
            MapperManager.GetInstance();

            var cuentasAsiento = Mapper.Map<DataAccess.Models.CuentaAsiento, CuentaAsiento>(cuentasAsientoDb);

            return cuentasAsiento;
        }

        public List<CuentaAsiento> GetMayoresCuentasAsientos()
        {
            var cuentasAsientosDb =
                dbContext.CuentasAsientos.GroupBy(g => g.CuentaContable)
                    .Select(
                        group =>
                            new
                            {
                                CuentaContable = group.Key,
                                Debe = group.Sum(g => g.Debe),
                                Haber = group.Sum(g => g.Haber)
                            })
                    .ToList()
                    .Select(
                        s =>
                            new DataAccess.Models.CuentaAsiento
                            {
                                CuentaContable = s.CuentaContable,
                                Debe = s.Debe,
                                Haber = s.Haber
                            }).ToList();
            MapperManager.GetInstance();

            var cuentasAsientos = new List<CuentaAsiento>();
            cuentasAsientosDb.ForEach(p => cuentasAsientos.Add(Mapper.Map<DataAccess.Models.CuentaAsiento, CuentaAsiento>(p)));

            return cuentasAsientos;
        }

        public List<CuentaAsiento> GetMayoresByCuenta(string numeroCuenta)
        {
            var cuentasAsientosDb =
                dbContext.CuentasAsientos.Where(c => c.CuentaContable.Numero == numeroCuenta).GroupBy(g => g.CuentaContable)
                    .Select(
                        group =>
                            new
                            {
                                CuentaContable = group.Key,
                                Debe = group.Sum(g => g.Debe),
                                Haber = group.Sum(g => g.Haber)
                            })
                    .ToList()
                    .Select(
                        s =>
                            new DataAccess.Models.CuentaAsiento
                            {
                                CuentaContable = s.CuentaContable,
                                Debe = s.Debe,
                                Haber = s.Haber
                            }).ToList();
            MapperManager.GetInstance();

            var cuentasAsientos = new List<CuentaAsiento>();
            cuentasAsientosDb.ForEach(p => cuentasAsientos.Add(Mapper.Map<DataAccess.Models.CuentaAsiento, CuentaAsiento>(p)));

            return cuentasAsientos;
        }

        public Resultado GenerarAsiento(List<PlantillaCuenta> plantillasCuentas, int comprobanteId)
        {
            try
            {
                var cuentaAsiento = new CuentaAsiento();
                foreach (var plantillaCuenta in plantillasCuentas)
                {
                    cuentaAsiento = new CuentaAsiento
                    {
                        ComprobanteId = comprobanteId,
                        AccionNacionalId = plantillaCuenta.AccionNacionalId,
                        AnexoTributarioId = plantillaCuenta.AnexoTributarioId,
                        CodigoAuditoriaId = plantillaCuenta.CodigoAuditoriaId,
                        ContraparteId = plantillaCuenta.ContraparteId,
                        CuentaContableId = plantillaCuenta.CuentaContableId,
                        PlanProgramaticoId = plantillaCuenta.PlanProgramaticoId,
                        TerritorioId = plantillaCuenta.TerritorioId,
                        Glosa = plantillaCuenta.Glosa,
                        Debe = plantillaCuenta.Debe,
                        Haber = plantillaCuenta.Haber,
                        NotasAdicionales = plantillaCuenta.NotasAdicionales,
                        EsAjuste = plantillaCuenta.EsAjuste,
                        EsDebe = plantillaCuenta.EsDebe,
                        UsuarioCreacion = plantillaCuenta.UsuarioCreacion,
                        UsuarioModificacion = plantillaCuenta.UsuarioModificacion
                    };
                    InsertCuentaAsiento(cuentaAsiento);
                }

                return new Resultado("El asiento se generó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado InsertCuentaAsiento(CuentaAsiento cuentasAsiento)
        {
            MapperManager.GetInstance();

            try
            {
                var cuentasAsientoDb = Mapper.Map<CuentaAsiento, DataAccess.Models.CuentaAsiento>(cuentasAsiento);

                cuentasAsientoDb.Activo = true;
                cuentasAsientoDb.FechaCreacion = DateTime.Now;
                cuentasAsientoDb.FechaModificacion = DateTime.Now;
                dbContext.CuentasAsientos.Add(cuentasAsientoDb);
                dbContext.SaveChanges();

                cuentasAsiento.Id = cuentasAsientoDb.Id;
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateCuentaAsiento(CuentaAsiento cuentasAsiento)
        {
            MapperManager.GetInstance();

            try
            {
                var cuentasAsientoDb = dbContext.CuentasAsientos.Find(cuentasAsiento.Id);
                if (cuentasAsientoDb != null)
                {
                    cuentasAsientoDb.AccionNacionalId = cuentasAsiento.AccionNacionalId;
                    cuentasAsientoDb.AnexoTributarioId = cuentasAsiento.AnexoTributarioId;
                    cuentasAsientoDb.CodigoAuditoriaId = cuentasAsiento.CodigoAuditoriaId;
                    cuentasAsientoDb.ContraparteId = cuentasAsiento.ContraparteId;
                    cuentasAsientoDb.CuentaContableId = cuentasAsiento.CuentaContableId;
                    cuentasAsientoDb.PlanProgramaticoId = cuentasAsiento.PlanProgramaticoId;
                    cuentasAsientoDb.TerritorioId = cuentasAsiento.TerritorioId;
                    cuentasAsientoDb.MarcoLogicoId = cuentasAsiento.MarcoLogicoId;
                    cuentasAsientoDb.NotasAdicionales = cuentasAsiento.NotasAdicionales;
                    
                    cuentasAsientoDb.Debe = cuentasAsiento.Debe;
                    cuentasAsientoDb.Haber = cuentasAsiento.Haber;
                    cuentasAsientoDb.Glosa = cuentasAsiento.Glosa;

                    cuentasAsientoDb.UsuarioModificacion = cuentasAsiento.UsuarioModificacion;
                    cuentasAsientoDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(cuentasAsientoDb).State = EntityState.Modified;
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

        public Resultado DeleteCuentaAsiento(int id)
        {
            MapperManager.GetInstance();

            try
            {
                var cuentasAsientoDb = dbContext.CuentasAsientos.Find(id);
                cuentasAsientoDb.Activo = false;
                cuentasAsientoDb.FechaModificacion = DateTime.Now;
                dbContext.Entry(cuentasAsientoDb).State = EntityState.Modified;
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
