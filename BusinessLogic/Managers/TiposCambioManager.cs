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
using TipoCambio = Cerberus.Sos.Accounting.BusinessLogic.Entities.TipoCambio;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class TiposCambioManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<TipoCambio> GetAllTiposCambio()
        {
            var tiposCambioDb = dbContext.TiposCambio.ToList();

            MapperManager.GetInstance();

            var tiposCambio = new List<TipoCambio>();
            tiposCambioDb.ForEach(p => tiposCambio.Add(Mapper.Map<DataAccess.Models.TipoCambio, TipoCambio>(p)));

            return tiposCambio;
        }

        public List<TipoCambio> GetTiposCambioByFecha(DateTime fechaTipoCambio)
        {
            var tiposCambioDb =
                dbContext.TiposCambio.Where(tc => tc.FechaTipoCambio == fechaTipoCambio).ToList();

            MapperManager.GetInstance();

            var tiposCambio = new List<TipoCambio>();
            tiposCambioDb.ForEach(p => tiposCambio.Add(Mapper.Map<DataAccess.Models.TipoCambio, TipoCambio>(p)));

            return tiposCambio;
        }

        public TipoCambio GetTipoCambio(int tipoCambioId)
        {
            var tipoCambioDb = dbContext.TiposCambio.Find(tipoCambioId);
            MapperManager.GetInstance();

            var tipoCambio = Mapper.Map<DataAccess.Models.TipoCambio, TipoCambio>(tipoCambioDb);

            return tipoCambio;
        }

        public Resultado InsertLastTipoCambio()
        {
            var fechaActual = DateTime.Now.Date;
            var ultimaFechaTipoCambio = dbContext.TiposCambio.Max(tc => tc.FechaTipoCambio);
            if (fechaActual != ultimaFechaTipoCambio)
            {
                var ultimosTipoCambio = dbContext.TiposCambio.Where(tc => tc.FechaTipoCambio == ultimaFechaTipoCambio).ToList();
                foreach (var tipoCambio in ultimosTipoCambio)
                {
                    tipoCambio.FechaTipoCambio = fechaActual;

                    dbContext.TiposCambio.Add(tipoCambio);
                    dbContext.SaveChanges();
                }

            }

            return new Resultado("OK");
        }
        
        public Resultado InsertTipoCambio(TipoCambio tipoCambio)
        {
            MapperManager.GetInstance();

            try
            {
                var tipoCambioDb = Mapper.Map<TipoCambio, DataAccess.Models.TipoCambio>(tipoCambio);

                tipoCambioDb.Activo = true;
                tipoCambioDb.FechaCreacion = DateTime.Now;
                tipoCambioDb.FechaModificacion = DateTime.Now;

                dbContext.TiposCambio.Add(tipoCambioDb);
                dbContext.SaveChanges();
                tipoCambio.Id = tipoCambioDb.Id;
                return new Resultado("La TipoCambio se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateTipoCambio(TipoCambio tipoCambio)
        {
            MapperManager.GetInstance();

            try
            {
                var tipoCambioDb = dbContext.TiposCambio.Find(tipoCambio.Id);

                tipoCambioDb.Valor = tipoCambio.Valor;
                tipoCambioDb.UsuarioModificacion = tipoCambio.UsuarioModificacion;
                tipoCambioDb.FechaModificacion = DateTime.Now;

                dbContext.Entry(tipoCambioDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("La TipoCambio se guardó correctamente.");
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
