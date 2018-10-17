using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using Territorio = Cerberus.Sos.Accounting.BusinessLogic.Entities.Territorio;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class TerritoriosManager:IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Territorio> GetAllTerritorios()
        {
            var territoriosDb = dbContext.Territorios.ToList();
            MapperManager.GetInstance();

            var territorios = new List<Territorio>();
            territoriosDb.ForEach(p => territorios.Add(Mapper.Map<DataAccess.Models.Territorio, Territorio>(p)));

            return territorios;
        }

        public Territorio GetTerritorio(int territorioId)
        {
            var territorioDb = dbContext.Territorios.Find(territorioId);
            MapperManager.GetInstance();

            var territorio = Mapper.Map<DataAccess.Models.Territorio, Territorio>(territorioDb);

            return territorio;
        }

        public Resultado InsertTerritorio(Territorio territorio)
        {
            MapperManager.GetInstance();

            try
            {
                var territorioDb = Mapper.Map<Territorio, DataAccess.Models.Territorio>(territorio);
                dbContext.Territorios.Add(territorioDb);
                dbContext.SaveChanges();
                return new Resultado("El territorio se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateTerritorio(Territorio territorio)
        {
            MapperManager.GetInstance();

            try
            {
                var territorioDb = Mapper.Map<Territorio, DataAccess.Models.Territorio>(territorio);
                dbContext.Entry(territorioDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El territorio se guardó correctamente.");
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
