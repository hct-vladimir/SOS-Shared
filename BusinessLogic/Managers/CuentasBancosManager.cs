using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using CuentaBanco = Cerberus.Sos.Accounting.BusinessLogic.Entities.CuentaBanco;


namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class CuentasBancosManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<CuentaBanco> GetAllCuentasBancos()
        {
            var CuentasBancoDb = dbContext.CuentasBancos.ToList();
            MapperManager.GetInstance();

            var cuentasBanco = new List<CuentaBanco>();
            CuentasBancoDb.ForEach(p => cuentasBanco.Add(Mapper.Map<DataAccess.Models.CuentaBanco, CuentaBanco>(p)));

            return cuentasBanco;
        }

        public CuentaBanco GetCuentasBanco(int CuentasBancoId)
        {
            var CuentasBancoDb = dbContext.CuentasBancos.Find(CuentasBancoId);
            MapperManager.GetInstance();

            var cuentasBanco = Mapper.Map<DataAccess.Models.CuentaBanco, CuentaBanco>(CuentasBancoDb);

            return cuentasBanco;
        }

        public Resultado InsertCuentasBanco(CuentaBanco cuentasBanco)
        {
            MapperManager.GetInstance();

            try
            {
                var CuentasBancoDb = Mapper.Map<CuentaBanco, DataAccess.Models.CuentaBanco>(cuentasBanco);
                dbContext.CuentasBancos.Add(CuentasBancoDb);
                dbContext.SaveChanges();
                return new Resultado("La cuenta banco se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateCuentasBanco(CuentaBanco cuentasBanco)
        {
            MapperManager.GetInstance();

            try
            {
                var CuentasBancoDb = Mapper.Map<CuentaBanco, DataAccess.Models.CuentaBanco>(cuentasBanco);
                dbContext.Entry(CuentasBancoDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("La cuenta banco se guardó correctamente.");
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
