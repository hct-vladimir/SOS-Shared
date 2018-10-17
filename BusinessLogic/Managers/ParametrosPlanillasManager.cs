using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Entities.ParametrosPlanillas;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using TipoCambio = Cerberus.Sos.Accounting.DataAccess.Models.TipoCambio;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class ParametrosPlanillasManager : IDisposable
    {
        private PlanillasSosDBEntities dbContext = new PlanillasSosDBEntities();

        #region Parametros
        public List<Parametro> GetAllParametros()
        {
            var listaDb = dbContext.PARAMETROS_PLANILLAS.ToList();

            MapperManager.GetInstance();

            var listaEntidades = new List<Parametro>();
            listaDb.ForEach(p => listaEntidades.Add(Mapper.Map<PARAMETROS_PLANILLAS, Parametro>(p)));

            return listaEntidades;
        }

        public Parametro GetParametro(int entidadId)
        {
            var entidadDb = dbContext.PARAMETROS_PLANILLAS.Find(entidadId);
            MapperManager.GetInstance();

            var entidad = Mapper.Map<PARAMETROS_PLANILLAS, Parametro>(entidadDb);

            return entidad;
        }

        public Resultado InsertParametro(Parametro parametro)
        {
            MapperManager.GetInstance();

            try
            {
                var parametroDb = Mapper.Map<Parametro, PARAMETROS_PLANILLAS>(parametro);

                dbContext.PARAMETROS_PLANILLAS.Add(parametroDb);
                dbContext.SaveChanges();
                parametro.ID = parametroDb.ID;
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateParametro(Parametro parametro)
        {
            MapperManager.GetInstance();

            try
            {
                var parametroDb = dbContext.PARAMETROS_PLANILLAS.Find(parametro.ID);

                parametroDb.NOMBRE = parametro.NOMBRE;
                parametroDb.DESCRIPCION = parametro.DESCRIPCION;
                parametroDb.VALOR = parametro.VALOR;

                dbContext.Entry(parametroDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado DeleteParametro(Parametro parametro)
        {
            MapperManager.GetInstance();

            try
            {
                var parametroDb = dbContext.PARAMETROS_PLANILLAS.Find(parametro.ID);

                dbContext.Entry(parametroDb).State = EntityState.Deleted;
                dbContext.SaveChanges();
                return new Resultado("El registro fue borrado con éxito.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        #endregion

        #region RegistroUfv

        public List<RegistroUfv> GetAllRegistrosUfvs()
        {
            var listaDb = dbContext.REGISTRO_UFVS.ToList();

            MapperManager.GetInstance();

            var listaEntidades = new List<RegistroUfv>();
            listaDb.ForEach(p => listaEntidades.Add(Mapper.Map<REGISTRO_UFVS, RegistroUfv>(p)));

            return listaEntidades;
        }

        public RegistroUfv GetRegistroUfv(int entidadId)
        {
            var entidadDb = dbContext.REGISTRO_UFVS.Find(entidadId);
            MapperManager.GetInstance();

            var entidad = Mapper.Map<REGISTRO_UFVS, RegistroUfv>(entidadDb);

            return entidad;
        }

        public Resultado InsertRegistroUfv(RegistroUfv registroUfv)
        {
            MapperManager.GetInstance();

            try
            {
                var registroUfvDb = Mapper.Map<RegistroUfv, REGISTRO_UFVS>(registroUfv);

                dbContext.REGISTRO_UFVS.Add(registroUfvDb);
                dbContext.SaveChanges();
                registroUfv.ID = registroUfvDb.ID;
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateRegistroUfv(RegistroUfv registroUfv)
        {
            MapperManager.GetInstance();

            try
            {
                var registroUfvDb = dbContext.REGISTRO_UFVS.Find(registroUfv.ID);

                registroUfvDb.FECHA_UFV = registroUfv.FECHA_UFV;
                registroUfvDb.UFV_ANTERIOR = registroUfv.UFV_ANTERIOR;
                registroUfvDb.UFV_ACTUAL = registroUfv.UFV_ACTUAL;

                dbContext.Entry(registroUfvDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado DeleteRegistroUfv(RegistroUfv registroUfv)
        {
            MapperManager.GetInstance();

            try
            {
                var registroUfvDb = dbContext.REGISTRO_UFVS.Find(registroUfv.ID);

                dbContext.Entry(registroUfvDb).State = EntityState.Deleted;
                dbContext.SaveChanges();
                return new Resultado("El registro fue borrado con éxito.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }


        #endregion

        #region SalarioMinimo
        public List<SalarioMinimo> GetAllSalariosMinimos()
        {
            var listaDb = dbContext.SALARIOS_MINIMOS.ToList();

            MapperManager.GetInstance();

            var listaEntidades = new List<SalarioMinimo>();
            listaDb.ForEach(p => listaEntidades.Add(Mapper.Map<SALARIOS_MINIMOS, SalarioMinimo>(p)));

            return listaEntidades;
        }

        public SalarioMinimo GetSalarioMinimo(int entidadId)
        {
            var entidadDb = dbContext.SALARIOS_MINIMOS.Find(entidadId);
            MapperManager.GetInstance();

            var entidad = Mapper.Map<SALARIOS_MINIMOS, SalarioMinimo>(entidadDb);

            return entidad;
        }

        public Resultado InsertSalarioMinimo(SalarioMinimo salarioMinimo)
        {
            MapperManager.GetInstance();

            try
            {
                var salarioMinimoDb = Mapper.Map<SalarioMinimo, SALARIOS_MINIMOS>(salarioMinimo);

                dbContext.SALARIOS_MINIMOS.Add(salarioMinimoDb);
                dbContext.SaveChanges();
                salarioMinimo.ID = salarioMinimoDb.ID;
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateSalarioMinimo(SalarioMinimo salarioMinimo)
        {
            MapperManager.GetInstance();

            try
            {
                var salarioMinimoDb = dbContext.SALARIOS_MINIMOS.Find(salarioMinimo.ID);

                salarioMinimoDb.FECHA_SALARIO_MINIMO = salarioMinimo.FECHA_SALARIO_MINIMO;
                salarioMinimoDb.SALARIO_MINIMO = salarioMinimo.SALARIO_MINIMO;

                dbContext.Entry(salarioMinimoDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado DeleteSalarioMinimo(SalarioMinimo salarioMinimo)
        {
            MapperManager.GetInstance();

            try
            {
                var salarioMinimoDb = dbContext.SALARIOS_MINIMOS.Find(salarioMinimo.ID);

                dbContext.Entry(salarioMinimoDb).State = EntityState.Deleted;
                dbContext.SaveChanges();
                return new Resultado("El registro fue borrado con éxito.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }
        #endregion

        #region AporteNacionalSolidario
        public List<AporteNacional> GetAllAportesNacionales()
        {
            var listaDb = dbContext.PORCENTAJE_APORTE_NACIONAL_SOL.ToList();

            MapperManager.GetInstance();

            var listaEntidades = new List<AporteNacional>();
            listaDb.ForEach(p => listaEntidades.Add(Mapper.Map<PORCENTAJE_APORTE_NACIONAL_SOL, AporteNacional>(p)));

            return listaEntidades;
        }

        public AporteNacional GetAporteNacional(int entidadId)
        {
            var entidadDb = dbContext.PORCENTAJE_APORTE_NACIONAL_SOL.Find(entidadId);
            MapperManager.GetInstance();

            var entidad = Mapper.Map<PORCENTAJE_APORTE_NACIONAL_SOL, AporteNacional>(entidadDb);

            return entidad;
        }

        public Resultado InsertAporteNacional(AporteNacional aporteNacional)
        {
            MapperManager.GetInstance();

            try
            {
                var aporteNacionalDb = Mapper.Map<AporteNacional, PORCENTAJE_APORTE_NACIONAL_SOL>(aporteNacional);

                dbContext.PORCENTAJE_APORTE_NACIONAL_SOL.Add(aporteNacionalDb);
                dbContext.SaveChanges();
                aporteNacional.ID = aporteNacionalDb.ID;
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateAporteNacional(AporteNacional aporteNacional)
        {
            MapperManager.GetInstance();

            try
            {
                var aporteNacionalDb = dbContext.PORCENTAJE_APORTE_NACIONAL_SOL.Find(aporteNacional.ID);

                aporteNacionalDb.INTERVALO_INICIAL = aporteNacional.INTERVALO_INICIAL;
                aporteNacionalDb.INTERVALO_FINAL = aporteNacional.INTERVALO_FINAL;
                aporteNacionalDb.PORCENTAJE = aporteNacional.PORCENTAJE;
                aporteNacionalDb.TIPO = aporteNacional.TIPO;

                dbContext.Entry(aporteNacionalDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado DeleteAporteNacional(AporteNacional aporteNacional)
        {
            MapperManager.GetInstance();

            try
            {
                var aporteNacionalDb = dbContext.PORCENTAJE_APORTE_NACIONAL_SOL.Find(aporteNacional.ID);

                dbContext.Entry(aporteNacionalDb).State = EntityState.Deleted;
                dbContext.SaveChanges();
                return new Resultado("El registro fue borrado con éxito.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }
        #endregion

        #region BonoAntiguedad
        public List<BonoAntiguedad> GetAllBonosAntiguedad()
        {
            var listaDb = dbContext.PORCENTAJE_BONO_ANTIGUEDAD.ToList();

            MapperManager.GetInstance();

            var listaEntidades = new List<BonoAntiguedad>();
            listaDb.ForEach(p => listaEntidades.Add(Mapper.Map<PORCENTAJE_BONO_ANTIGUEDAD, BonoAntiguedad>(p)));

            return listaEntidades;
        }

        public BonoAntiguedad GetBonoAntiguedad(int entidadId)
        {
            var entidadDb = dbContext.PORCENTAJE_BONO_ANTIGUEDAD.Find(entidadId);
            MapperManager.GetInstance();

            var entidad = Mapper.Map<PORCENTAJE_BONO_ANTIGUEDAD, BonoAntiguedad>(entidadDb);

            return entidad;
        }

        public Resultado InsertBonoAntiguedad(BonoAntiguedad bonoAntiguedad)
        {
            MapperManager.GetInstance();

            try
            {
                var bonoAntiguedadDb = Mapper.Map<BonoAntiguedad, PORCENTAJE_BONO_ANTIGUEDAD>(bonoAntiguedad);

                dbContext.PORCENTAJE_BONO_ANTIGUEDAD.Add(bonoAntiguedadDb);
                dbContext.SaveChanges();
                bonoAntiguedad.ID = bonoAntiguedadDb.ID;
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateBonoAntiguedad(BonoAntiguedad bonoAntiguedad)
        {
            MapperManager.GetInstance();

            try
            {
                var bonoAntiguedadDb = dbContext.PORCENTAJE_BONO_ANTIGUEDAD.Find(bonoAntiguedad.ID);

                bonoAntiguedadDb.ANIOS_INICIAL = bonoAntiguedad.ANIOS_INICIAL;
                bonoAntiguedadDb.ANIOS_FINAL = bonoAntiguedad.ANIOS_FINAL;
                bonoAntiguedadDb.PORCENTAJE = bonoAntiguedad.PORCENTAJE;

                dbContext.Entry(bonoAntiguedadDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El registro se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado DeleteBonoAntiguedad(BonoAntiguedad bonoAntiguedad)
        {
            MapperManager.GetInstance();

            try
            {
                var bonoAntiguedadDb = dbContext.PORCENTAJE_BONO_ANTIGUEDAD.Find(bonoAntiguedad.ID);

                dbContext.Entry(bonoAntiguedadDb).State = EntityState.Deleted;
                dbContext.SaveChanges();
                return new Resultado("El registro fue borrado con éxito.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }
        #endregion

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
