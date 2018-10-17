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
using Banco = Cerberus.Sos.Accounting.BusinessLogic.Entities.Banco;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class BancosManager:IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Banco> GetAllBancos()
        {
            var bancosDb = dbContext.Bancos.ToList();
            MapperManager.GetInstance();

            var bancos = new List<Banco>();
            bancosDb.ForEach(p => bancos.Add(Mapper.Map<DataAccess.Models.Banco, Banco>(p)));

            return bancos;
        }

        public Banco GetBanco(int bancoId)
        {
            var bancoDb = dbContext.Bancos.Find(bancoId);
            MapperManager.GetInstance();
            var banco = Mapper.Map<DataAccess.Models.Banco, Banco>(bancoDb);
            return banco;
        }

        public Resultado InsertBanco(Banco banco)
        {
            MapperManager.GetInstance();

            try
            {
                var bancoDb = Mapper.Map<Banco, DataAccess.Models.Banco>(banco);
                dbContext.Bancos.Add(bancoDb);
                dbContext.SaveChanges();
                return new Resultado("El banco se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateBanco(Banco banco)
        {
            MapperManager.GetInstance();

            try
            {
                var bancoDb = Mapper.Map<Banco, DataAccess.Models.Banco>(banco);
                dbContext.Entry(bancoDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El banco se guardó correctamente.");
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
