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
using Factura = Cerberus.Sos.Accounting.BusinessLogic.Entities.Factura;
using FacturaComprador = Cerberus.Sos.Accounting.BusinessLogic.Entities.FacturaComprador;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class FacturasManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Factura> GetAllFacturas()
        {
            var facturasDb = dbContext.Facturas.ToList();
            MapperManager.GetInstance();

            var facturas = new List<Entities.Factura>();
            facturasDb.ForEach(p => facturas.Add(Mapper.Map<DataAccess.Models.Factura, Factura>(p)));

            return facturas;
        }

        public Factura GetFactura(int facturaId)
        {
            var facturaDb = dbContext.Facturas.Find(facturaId);
            MapperManager.GetInstance();

            var factura = Mapper.Map<DataAccess.Models.Factura, Factura>(facturaDb);

            return factura;
        }

        public Resultado InsertFactura(Factura factura)
        {
            MapperManager.GetInstance();

            try
            {
                var facturaDb = Mapper.Map<Factura, DataAccess.Models.Factura>(factura);

                // Se inserta datos del comprador si no existe


                facturaDb.TipoMonedaId = 1;
                facturaDb.Activo = true;
                facturaDb.FechaCreacion = DateTime.Now;
                facturaDb.FechaModificacion = DateTime.Now;

                if (facturaDb.FacturaComprador != null)
                {
                    facturaDb.FacturaComprador.Activo = true;
                }

                dbContext.Facturas.Add(facturaDb);
                dbContext.SaveChanges();
                factura.Id = facturaDb.Id;
                return new Resultado("La Factura se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateFactura(Factura factura)
        {
            MapperManager.GetInstance();

            try
            {
                var facturaDb = Mapper.Map<Factura, DataAccess.Models.Factura>(factura);
                dbContext.Entry(facturaDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("La Factura se guardó correctamente.");
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
