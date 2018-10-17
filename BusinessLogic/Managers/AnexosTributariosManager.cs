using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using AnexosTributario = Cerberus.Sos.Accounting.BusinessLogic.Entities.AnexosTributario;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class AnexosTributariosManager
    {

        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<AnexosTributario> GetAllAnexosTributarios()
        {
            var anexosTributariosDb = dbContext.AnexosTributarios.ToList();
            MapperManager.GetInstance();

            var anexosTributarios = new List<AnexosTributario>();
            anexosTributariosDb.ForEach(p => anexosTributarios.Add(Mapper.Map<DataAccess.Models.AnexosTributario, AnexosTributario>(p)));

            return anexosTributarios;
        }

        public AnexosTributario GetAnexoTributario(int anexoTributarioId)
        {
            var anexoTributarioDb = dbContext.AnexosTributarios.Find(anexoTributarioId);
            MapperManager.GetInstance();

            var anexoTributario = Mapper.Map<DataAccess.Models.AnexosTributario, AnexosTributario>(anexoTributarioDb);

            return anexoTributario;
        }

        public Resultado InsertAnexoTributario(AnexosTributario anexoTributario)
        {
            MapperManager.GetInstance();

            try
            {
                var anexoTributarioDb = Mapper.Map<AnexosTributario, DataAccess.Models.AnexosTributario>(anexoTributario);
                dbContext.AnexosTributarios.Add(anexoTributarioDb);
                dbContext.SaveChanges();
                return new Resultado("El Anexo Tributario se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateAnexoTributario(AnexosTributario anexoTributario)
        {
            MapperManager.GetInstance();

            try
            {
                var anexoTributarioDb = Mapper.Map<AnexosTributario, DataAccess.Models.AnexosTributario>(anexoTributario);
                dbContext.Entry(anexoTributarioDb).State = EntityState.Modified;
                dbContext.SaveChanges();


                  return new Resultado("El Anexo Tributario se guardó correctamente.");
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
