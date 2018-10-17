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
using EstadoCuenta = Cerberus.Sos.Accounting.BusinessLogic.Entities.EstadoCuenta;
using CuentaAsiento = Cerberus.Sos.Accounting.BusinessLogic.Entities.CuentaAsiento;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class EstadosCuentasManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<EstadoCuenta> GetAllEstadosCuentas()
        {
            var estadosCuentasDb = dbContext.EstadosCuentas.ToList();
            MapperManager.GetInstance();

            var estadosCuentas = new List<EstadoCuenta>();
            estadosCuentasDb.ForEach(p => estadosCuentas.Add(Mapper.Map<DataAccess.Models.EstadoCuenta, EstadoCuenta>(p)));

            return estadosCuentas;
        }

        public EstadoCuenta GetEstadoCuenta(int estadoCuentaId)
        {
            var estadoCuentaDb = dbContext.EstadosCuentas.Find(estadoCuentaId);
            MapperManager.GetInstance();

            var estadoCuenta = Mapper.Map<DataAccess.Models.EstadoCuenta, EstadoCuenta>(estadoCuentaDb);

            return estadoCuenta;
        }

        public List<EstadoCuenta> GetEstadosCuentasByTipo(int tipoEstadoCuentaId)
        {
            // Se obtienen los estados de cuenta pendientes para el tipo enviado por parámetro.
            var estadosCuentas = dbContext.EstadosCuentas.Where(c => c.TipoEstadoCuentaId == tipoEstadoCuentaId && (c.DebeCuentaAsientoId == null || c.HaberCuentaAsientoId == null)).ToList();
            var estadosCuentasIds = estadosCuentas.Select(e => e.DebeCuentaAsientoId).Union(estadosCuentas.Select(e => e.HaberCuentaAsientoId )).ToList();
            var estadosCuentasAnonimo =
                estadosCuentas.Select(e => new {e.Id, CuentaAsientoId = e.DebeCuentaAsientoId})
                    .Union(estadosCuentas.Select(e => new {e.Id, CuentaAsientoId = e.HaberCuentaAsientoId}))
                    .ToList();

            var cuentasRelacionadasDb = dbContext.CuentasAsientos.Where(c => estadosCuentasIds.Contains(c.Id)).ToList();

            MapperManager.GetInstance();
            var cuentasRelacionadas = new List<CuentaAsiento>();
            cuentasRelacionadasDb.ForEach(p => cuentasRelacionadas.Add(Mapper.Map<DataAccess.Models.CuentaAsiento, CuentaAsiento>(p)));

            var estadosCuentasRelacionados = cuentasRelacionadas.Join(estadosCuentasAnonimo,
                    cuentasAsientosKey => cuentasAsientosKey.Id,
                    anonimo => anonimo.CuentaAsientoId,
                    (cuentaAsiento, anonimo) => new EstadoCuenta
                    {
                        Id = anonimo.Id,
                        CuentaAsiento = cuentaAsiento
                    }).ToList();

            return estadosCuentasRelacionados;
        }

        public Resultado InsertEstadoCuenta(EstadoCuenta estadoCuenta)
        {
            MapperManager.GetInstance();

            try
            {
                var estadoCuentaDb = Mapper.Map<EstadoCuenta, DataAccess.Models.EstadoCuenta>(estadoCuenta);

                estadoCuentaDb.Activo = true;
                estadoCuentaDb.FechaCreacion = DateTime.Now;
                estadoCuentaDb.FechaModificacion = DateTime.Now;


                dbContext.EstadosCuentas.Add(estadoCuentaDb);
                dbContext.SaveChanges();
                estadoCuenta.Id = estadoCuentaDb.Id;
                return new Resultado("La EstadoCuenta se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateEstadoCuenta(EstadoCuenta estadoCuenta)
        {
            MapperManager.GetInstance();

            try
            {
                var estadoCuentaDb = dbContext.EstadosCuentas.Find(estadoCuenta.Id);
                if (estadoCuentaDb != null)
                {
                    estadoCuentaDb.DebeCiudadId = estadoCuenta.DebeCiudadId;
                    estadoCuentaDb.DebeFacilityId = estadoCuenta.DebeFacilityId;
                    estadoCuentaDb.DebeCuentaAsientoId = estadoCuenta.DebeCuentaAsientoId;
                    estadoCuentaDb.HaberCiudadId = estadoCuenta.HaberCiudadId;
                    estadoCuentaDb.HaberFacilityId = estadoCuenta.HaberFacilityId;
                    estadoCuentaDb.HaberCuentaAsientoId = estadoCuenta.HaberCuentaAsientoId;

                    estadoCuentaDb.UsuarioModificacion = estadoCuenta.UsuarioModificacion;
                    estadoCuentaDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(estadoCuentaDb).State = EntityState.Modified;
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

        public Resultado UpdateEstadoCuentaDebe(EstadoCuenta estadoCuenta)
        {
            MapperManager.GetInstance();

            try
            {
                var estadoCuentaDb = dbContext.EstadosCuentas.Find(estadoCuenta.Id);
                if (estadoCuentaDb != null)
                {
                    estadoCuentaDb.DebeCiudadId = estadoCuenta.DebeCiudadId;
                    estadoCuentaDb.DebeFacilityId = estadoCuenta.DebeFacilityId;
                    estadoCuentaDb.DebeCuentaAsientoId = estadoCuenta.DebeCuentaAsientoId;

                    estadoCuentaDb.UsuarioModificacion = estadoCuenta.UsuarioModificacion;
                    estadoCuentaDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(estadoCuentaDb).State = EntityState.Modified;
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

        public Resultado UpdateEstadoCuentaHaber(EstadoCuenta estadoCuenta)
        {
            MapperManager.GetInstance();

            try
            {
                var estadoCuentaDb = dbContext.EstadosCuentas.Find(estadoCuenta.Id);
                if (estadoCuentaDb != null)
                {
                    estadoCuentaDb.HaberCiudadId = estadoCuenta.HaberCiudadId;
                    estadoCuentaDb.HaberFacilityId = estadoCuenta.HaberFacilityId;
                    estadoCuentaDb.HaberCuentaAsientoId = estadoCuenta.HaberCuentaAsientoId;

                    estadoCuentaDb.UsuarioModificacion = estadoCuenta.UsuarioModificacion;
                    estadoCuentaDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(estadoCuentaDb).State = EntityState.Modified;
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

        public Resultado DeleteEstadoCuenta(int id)
        {
            MapperManager.GetInstance();

            try
            {
                var estadoCuentaDb = dbContext.EstadosCuentas.Find(id);
                estadoCuentaDb.Activo = false;
                estadoCuentaDb.FechaModificacion = DateTime.Now;
                dbContext.Entry(estadoCuentaDb).State = EntityState.Modified;
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
