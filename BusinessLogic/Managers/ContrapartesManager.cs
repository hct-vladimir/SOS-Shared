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
using Contraparte = Cerberus.Sos.Accounting.BusinessLogic.Entities.Contraparte;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class ContrapartesManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Contraparte> GetAllContrapartes()
        {
            var contrapartesDb = dbContext.Contrapartes.ToList();
            MapperManager.GetInstance();

            var contrapartes = new List<Contraparte>();
            contrapartesDb.ForEach(p => contrapartes.Add(Mapper.Map<DataAccess.Models.Contraparte, Contraparte>(p)));

            return contrapartes;
        }


        public Contraparte GetContraparte(int ContraparteId)
        {
            var contraparteDb = dbContext.Contrapartes.Find(ContraparteId);
            MapperManager.GetInstance();

            var contraparte = Mapper.Map<DataAccess.Models.Contraparte, Contraparte>(contraparteDb);

            return contraparte;
        }

        public Resultado InsertContraparte(Contraparte contraparte)
        {
            MapperManager.GetInstance();

            try
            {
                var contraparteDb = Mapper.Map<Contraparte, DataAccess.Models.Contraparte>(contraparte);
                dbContext.Contrapartes.Add(contraparteDb);
                dbContext.SaveChanges();
                return new Resultado("La contraparte se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateContraparte(Contraparte contraparte)
        {
            MapperManager.GetInstance();

            try
            {
                var contraparteDb = Mapper.Map<Contraparte, DataAccess.Models.Contraparte>(contraparte);
                dbContext.Entry(contraparteDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("La contraparte se guardó correctamente.");
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
