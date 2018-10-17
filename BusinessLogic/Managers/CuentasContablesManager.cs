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
using CuentaContable = Cerberus.Sos.Accounting.BusinessLogic.Entities.CuentaContable;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class CuentasContablesManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<CuentaContable> GetAllCuentasContables()
        {
            var cuentasContablesDb = dbContext.CuentasContables.Where(c => c.Activo).ToList();
            MapperManager.GetInstance();

            var cuentasContables = new List<CuentaContable>();
            cuentasContablesDb.ForEach(p => cuentasContables.Add(Mapper.Map<DataAccess.Models.CuentaContable, CuentaContable>(p)));

            return cuentasContables;
        }

        public List<CuentaContable> GetCuentasContablesByCuenta(string numeroCuenta)
        {
            var cuentasContablesDb = dbContext.CuentasContables.Where(c => c.Numero == numeroCuenta).ToList();
            MapperManager.GetInstance();

            var cuentasContables = new List<CuentaContable>();
            cuentasContablesDb.ForEach(p => cuentasContables.Add(Mapper.Map<DataAccess.Models.CuentaContable, CuentaContable>(p)));

            return cuentasContables;
        }

        public List<CuentaContable> GetCuentasContablesByCuentas(List<string> numerosCuentas)
        {
            var cuentasContablesDb = dbContext.CuentasContables.Where(c => numerosCuentas.Contains(c.Numero) ).ToList();
            MapperManager.GetInstance();

            var cuentasContables = new List<CuentaContable>();
            cuentasContablesDb.ForEach(p => cuentasContables.Add(Mapper.Map<DataAccess.Models.CuentaContable, CuentaContable>(p)));

            return cuentasContables;
        }

        public List<CuentaContable> GetCuentasContablesEstados(int tipoEstadoCuenta)
        {
            var numerosCuentas = new List<string>();

            switch (tipoEstadoCuenta)
            {
                case 1: // Transferencias
                    numerosCuentas = new List<string> { "88110", "88120", "88130", "88210" };
                    break;
                case 2:
                    var cuentasContablesPorPagar = dbContext.CuentasContables.Where(c => c.Numero.StartsWith("9") && c.NivelCuentaId == 4).ToList();
                    numerosCuentas = cuentasContablesPorPagar.Select(c => c.Numero).ToList();
                    break;
                case 3:
                    var cuentasContablesPorCobrar = dbContext.CuentasContables.Where(c => c.Numero.StartsWith("06") && c.NivelCuentaId == 4).ToList();
                    numerosCuentas = cuentasContablesPorCobrar.Select(c => c.Numero).ToList();
                    break;
            }

            var cuentasContablesEstados = GetCuentasContablesByCuentas(numerosCuentas);

            return cuentasContablesEstados;
        }

        public CuentaContable GetCuentasContable(int cuentasContableId)
        {
            var cuentasContableDb = dbContext.CuentasContables.Find(cuentasContableId);
            MapperManager.GetInstance();

            var cuentasContable = Mapper.Map<DataAccess.Models.CuentaContable, CuentaContable>(cuentasContableDb);

            return cuentasContable;
        }

        public List<int> GetParentsCuentasIds()
        {
            var cuentasContablesDb = dbContext.CuentasContables.Where(c => !c.Elegible).ToList();
            MapperManager.GetInstance();

            var cuentasContables = new List<CuentaContable>();
            cuentasContablesDb.ForEach(p => cuentasContables.Add(Mapper.Map<DataAccess.Models.CuentaContable, CuentaContable>(p)));

            return cuentasContables.Select(p => p.Id).ToList();
        }

        public Resultado InsertCuentaContable(CuentaContable cuentasContable)
        {
            MapperManager.GetInstance();

            try
            {
                var cuentasContableDb = Mapper.Map<CuentaContable, DataAccess.Models.CuentaContable>(cuentasContable);

                cuentasContableDb.Activo = true;
                cuentasContableDb.UsuarioCreacion = "DBO";
                cuentasContableDb.FechaCreacion = DateTime.Now;
                cuentasContableDb.UsuarioModificacion = "DBO";
                cuentasContableDb.FechaModificacion = DateTime.Now;
                dbContext.CuentasContables.Add(cuentasContableDb);
                dbContext.SaveChanges();
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateCuentaContable(CuentaContable cuentasContable)
        {
            MapperManager.GetInstance();

            try
            {
                var cuentasContableDb = Mapper.Map<CuentaContable, DataAccess.Models.CuentaContable>(cuentasContable);
                dbContext.Entry(cuentasContableDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El registro se guardó correctamente.");
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
