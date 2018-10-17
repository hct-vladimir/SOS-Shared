using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using Comprobante = Cerberus.Sos.Accounting.BusinessLogic.Entities.Comprobante;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class ComprobantesManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Comprobante> GetAllComprobantes()
        {
            var comprobantesDb = dbContext.Comprobantes.ToList();
            MapperManager.GetInstance();

            var comprobantes = new List<Comprobante>();
            comprobantesDb.ForEach(p => comprobantes.Add(Mapper.Map<DataAccess.Models.Comprobante, Comprobante>(p)));

            return comprobantes;
        }

        public List<Comprobante> GetComprobantesPorFacility(int facilityId)
        {
            var comprobantesDb = dbContext.Comprobantes.Where(c => c.FacilityId == facilityId && c.Activo).OrderByDescending(c => c.Id).ToList();
            MapperManager.GetInstance();

            var comprobantes = new List<Comprobante>();
            comprobantesDb.ForEach(p => comprobantes.Add(Mapper.Map<DataAccess.Models.Comprobante, Comprobante>(p)));

            return comprobantes;
        }

        public List<Comprobante> GetComprobantesPorCiudadEstado(int ciudadId, int cierreContableId, short estadoComprobanteId)
        {
            var comprobantesDb = dbContext.Comprobantes.Where(c => 
                c.CiudadId == ciudadId && 
                c.CierreContableId == cierreContableId && 
                c.EstadoComprobanteId == estadoComprobanteId && 
                c.Activo).ToList();
            MapperManager.GetInstance();

            var comprobantes = new List<Comprobante>();
            comprobantesDb.ForEach(p => comprobantes.Add(Mapper.Map<DataAccess.Models.Comprobante, Comprobante>(p)));

            return comprobantes;
        }


        public Comprobante GetComprobante(int comprobanteId)
        {
            var comprobanteDb = dbContext.Comprobantes.Find(comprobanteId);
            MapperManager.GetInstance();

            var comprobante = Mapper.Map<DataAccess.Models.Comprobante, Comprobante>(comprobanteDb);

            return comprobante;
        }

        public Resultado InsertComprobante(Comprobante comprobante)
        {
            MapperManager.GetInstance();

            try
            {
                var comprobanteDb = Mapper.Map<Comprobante, DataAccess.Models.Comprobante>(comprobante);

                comprobanteDb.NumeroComprobante = GetCodigoComprobante(comprobante.CiudadId, comprobante.TipoComprobanteId, comprobante.FechaComprobante); //"LPE180300001";
                if (comprobante.CuentaBancoId == -1)
                {
                    comprobanteDb.CuentaBancoId = null;
                }

                comprobanteDb.EstadoComprobanteId = 1;
                comprobanteDb.Activo = true;
                comprobanteDb.UsuarioCreacion = "DBO";
                comprobanteDb.FechaCreacion = DateTime.Now;
                comprobanteDb.UsuarioModificacion = "DBO";
                comprobanteDb.FechaModificacion = DateTime.Now;
                dbContext.Comprobantes.Add(comprobanteDb);
                dbContext.SaveChanges();
                comprobante.Id = comprobanteDb.Id;
                return new Resultado("El Comprobante se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        private string GetCodigoComprobante(int ciudadId, int tipoComprobanteId, DateTime fechaComprobante)
        {
            var nuevoCodigo = string.Empty;
            var prefijoCodigo = string.Format("{0}{1}{2}", "LP", tipoComprobanteId == 1 ? "I" : "E", fechaComprobante.ToString("yyMM"));
            var correlativoCodigo = 1;

            var ultimoComprobante = dbContext.Comprobantes.Count(c => c.CiudadId == ciudadId &&
                    c.TipoComprobanteId == tipoComprobanteId &&
                    c.FechaComprobante.Month == fechaComprobante.Month &&
                    c.FechaComprobante.Year == fechaComprobante.Year) > 0 ?
                dbContext.Comprobantes.Where(c =>
                    c.CiudadId == ciudadId &&
                    c.TipoComprobanteId == tipoComprobanteId &&
                    c.FechaComprobante.Month == fechaComprobante.Month &&
                    c.FechaComprobante.Year == fechaComprobante.Year).ToList().Last() : null;

            if (ultimoComprobante != null)
            {
                var codigoActual = ultimoComprobante.NumeroComprobante.Substring(7);
                var correlativoActual = int.Parse(codigoActual);
                correlativoCodigo = correlativoActual + 1;
            }

            nuevoCodigo = prefijoCodigo + correlativoCodigo.ToString().PadLeft(5, '0');
            return nuevoCodigo;
        }

        public Resultado UpdateComprobante(Comprobante comprobante)
        {
            MapperManager.GetInstance();

            try
            {
                var comprobanteDb = dbContext.Comprobantes.Find(comprobante.Id);
                comprobanteDb.FechaComprobante = comprobante.FechaComprobante;
                comprobanteDb.Beneficiario = comprobante.Beneficiario;
                comprobanteDb.Glosa = comprobante.Glosa;
                comprobanteDb.TipoComprobanteId = comprobante.TipoComprobanteId;
                comprobanteDb.TipoMonedaId = comprobante.TipoMonedaId;
                comprobanteDb.NumeroCheque = comprobante.NumeroCheque;
                comprobanteDb.CuentaBancoId = (comprobante.CuentaBancoId != null && comprobante.CuentaBancoId.Value == -1) ? null : comprobante.CuentaBancoId;
                

                dbContext.Entry(comprobanteDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El Comprobante se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado DeleteComprobante(int comprobanteId, string usuarioActual)
        {
            MapperManager.GetInstance();

            try
            {
                var comprobanteDb = dbContext.Comprobantes.Find(comprobanteId);

                comprobanteDb.Activo = false;
                comprobanteDb.UsuarioModificacion = usuarioActual;
                dbContext.Entry(comprobanteDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El Comprobante se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado CambiarEstadoComprobante(int comprobanteId, short estadoComprobanteId)
        {
            MapperManager.GetInstance();

            try
            {
                var comprobanteDb = dbContext.Comprobantes.Find(comprobanteId);
                comprobanteDb.EstadoComprobanteId = estadoComprobanteId;
                dbContext.Entry(comprobanteDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El Comprobante se guardó correctamente.");
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
