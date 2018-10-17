using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using CodigosAuditoria = Cerberus.Sos.Accounting.BusinessLogic.Entities.CodigosAuditoria;
using CodigoMarcoLogico = Cerberus.Sos.Accounting.BusinessLogic.Entities.CodigoMarcoLogico;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class CodigosAuditoriasManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<CodigosAuditoria> GetAllCodigosAuditoria()
        {
            var CodigosAuditoriaDb = dbContext.CodigosAuditorias.ToList();
            MapperManager.GetInstance();

            var CodigosAuditoria = new List<CodigosAuditoria>();
            CodigosAuditoriaDb.ForEach(p => CodigosAuditoria.Add(Mapper.Map<DataAccess.Models.CodigosAuditoria, CodigosAuditoria>(p)));

            return CodigosAuditoria;
        }

        public List<CodigoMarcoLogico> GetAllCodigosMarcoLogico()
        {
            var codigosMarcoLogicoDb = dbContext.CodigosMarcoLogicos.ToList();
            MapperManager.GetInstance();

            var codigosMarcoLogico = new List<CodigoMarcoLogico>();
            codigosMarcoLogicoDb.ForEach(p => codigosMarcoLogico.Add(Mapper.Map<DataAccess.Models.CodigoMarcoLogico, CodigoMarcoLogico>(p)));

            return codigosMarcoLogico;
        }

        public CodigosAuditoria GetCodigoAuditoria(int codigoAuditoriaId)
        {
            var CodigoAuditoriaDb = dbContext.CodigosAuditorias.Find(codigoAuditoriaId);
            MapperManager.GetInstance();

            var codigoAuditoria = Mapper.Map<DataAccess.Models.CodigosAuditoria, CodigosAuditoria>(CodigoAuditoriaDb);

            return codigoAuditoria;
        }

        public Resultado InsertCodigoAuditoria(CodigosAuditoria codigoAuditoria)
        {
            MapperManager.GetInstance();

            try
            {
                var CodigoAuditoriaDb = Mapper.Map<CodigosAuditoria, DataAccess.Models.CodigosAuditoria>(codigoAuditoria);

                if (string.IsNullOrEmpty(CodigoAuditoriaDb.Codigo))
                {
                    CodigoAuditoriaDb.Codigo = null;
                }

                dbContext.CodigosAuditorias.Add(CodigoAuditoriaDb);
                dbContext.SaveChanges();
                return new Resultado("El CodigosAuditorias se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateCodigoAuditoria(CodigosAuditoria codigoAuditoria)
        {
            MapperManager.GetInstance();

            try
            {
                var CodigoAuditoriaDb = Mapper.Map<CodigosAuditoria, DataAccess.Models.CodigosAuditoria>(codigoAuditoria);
                dbContext.Entry(CodigoAuditoriaDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El CodigosAuditorias se guardó correctamente.");
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
