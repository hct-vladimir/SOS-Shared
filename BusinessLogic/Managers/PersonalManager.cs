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
using Personal = Cerberus.Sos.Accounting.BusinessLogic.Entities.Personal;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class PersonalManager : IDisposable
    {
        private AldeasInfantilesDBEntities dbContext = new AldeasInfantilesDBEntities();

        public List<Personal> GetAllPersonal()
        {
            var personalDB = dbContext.Personal.ToList();
            MapperManager.GetInstance();

            var personal = new List<Personal>();
            personalDB.ForEach(p => personal.Add(Mapper.Map<DataAccess.Models.Personal, Personal>(p)));

            return personal;
        }

        public List<Personal> GetPersonalPorEstado(string estado, string estadoAdm)
        {
            var personalDBQuery = dbContext.Personal.Where(p => true);
            if (!string.IsNullOrEmpty(estado))
            {
                personalDBQuery = personalDBQuery.Where(p => p.estado == estado);
            }

            if (!string.IsNullOrEmpty(estadoAdm))
            {
                personalDBQuery = personalDBQuery.Where(p => p.estadoADM == estadoAdm);
            }

            var personalDB = personalDBQuery.ToList();

            MapperManager.GetInstance();

            var personal = new List<Personal>();
            personalDB.ForEach(p => personal.Add(Mapper.Map<DataAccess.Models.Personal, Personal>(p)));

            return personal;
        }

        public Personal GetPersonalByItem(string item)
        {
            var personalDB = dbContext.Personal.FirstOrDefault(p => p.item == item);
            MapperManager.GetInstance();

            var personal = Mapper.Map<DataAccess.Models.Personal, Personal>(personalDB);

            return personal;
        }

        public Resultado UpdatePersonal(Personal personal)
        {
            MapperManager.GetInstance();

            try
            {
                var personalDb = dbContext.Personal.FirstOrDefault(p => p.item == personal.item);

                personalDb.nombres = personal.nombres;
                personalDb.apellidoPaterno = personal.apellidoPaterno;
                personalDb.apellidoMaterno = personal.apellidoMaterno;
                personalDb.apellidoCasada = personal.apellidoCasada;
                personalDb.codigoTipoDocumento = personal.codigoTipoDocumento;
                personalDb.numeroDocumento = personal.numeroDocumento;
                personalDb.codigoDepartamento = personal.codigoDepartamento;
                personalDb.estadoCivil = personal.estadoCivil;
                personalDb.sexo = personal.sexo;
                personalDb.codigoEstudio = personal.codigoEstudio;
                personalDb.codigoProfesion = personal.codigoProfesion;
                personalDb.fechaNacimiento = personal.fechaNacimiento;
                personalDb.zona = personal.zona;
                personalDb.direccion = personal.direccion;
                personalDb.telefono = personal.telefono;
                personalDb.telefonoContacto = personal.telefonoContacto;

                personalDb.codigoPrograma = personal.codigoPrograma;
                personalDb.codigoSeccion = personal.codigoSeccion;
                personalDb.fechaInicioCargo = personal.fechaInicioCargo;
                personalDb.codigoCargoActual = personal.codigoCargoActual;
                personalDb.fechaIngreso = personal.fechaIngreso;
                personalDb.tipoSeguro = personal.tipoSeguro;
                personalDb.codigoAFP = personal.codigoAFP;
                personalDb.nua = personal.nua;
                personalDb.numeroSeguro = personal.numeroSeguro;
                personalDb.salario = personal.salario;
                personalDb.numeroCuenta = personal.numeroCuenta;
                personalDb.tipoInicio = personal.tipoInicio;
                personalDb.codigoTipoContratacion = personal.codigoTipoContratacion;
                personalDb.tiempoCompleto = personal.tiempoCompleto;
                personalDb.codigoEsEvaluado = personal.codigoEsEvaluado;
                personalDb.codigoEscalaSalarial = personal.codigoEscalaSalarial;
                personalDb.codigoRecibeInstruccion = personal.codigoRecibeInstruccion;
                personalDb.correo = personal.correo;
                personalDb.usuario_correo = personal.usuario_correo;
                personalDb.area = personal.area;
                personalDb.codigoFacility = personal.codigoFacility;
                personalDb.cotizante = personal.cotizante;
                personalDb.nacionalidad = personal.nacionalidad;


                dbContext.Entry(personalDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El Recurso se guardó correctamente.");

            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateAltaPersonal(string item)
        {
            MapperManager.GetInstance();

            try
            {
                var personalDb = dbContext.Personal.FirstOrDefault(p => p.item == item);

                personalDb.estado = "A";
                personalDb.estadoADM = "A";
                personalDb.fechaInicioCargo = DateTime.Now;

                dbContext.Entry(personalDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El Recurso se guardó correctamente.");

            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado CreateBajaPersonal(PersonalBaja personalBaja)
        {
            MapperManager.GetInstance();

            try
            {
                var personalDb = dbContext.Personal.FirstOrDefault(p => p.item == personalBaja.item && p.estado == "A" && p.estadoADM == "A");

                personalDb.estado = "B";
                personalDb.estadoADM = "P";

                dbContext.Entry(personalDb).State = EntityState.Modified;

                var bajaPersonalDb = Mapper.Map<PersonalBaja, BajaPersonal>(personalBaja);
                bajaPersonalDb.codigoBaja = GetNextCodigoBaja();
                bajaPersonalDb.numeroDocumento = personalDb.numeroDocumento;
                bajaPersonalDb.itemAutorizado = personalDb.codigoEsEvaluado;
                bajaPersonalDb.fechaIngreso = personalDb.fechaIngreso.Value;
                bajaPersonalDb.fechaRetiro = DateTime.Now;
                bajaPersonalDb.transferencia = "No";
                bajaPersonalDb.fechaSistema = DateTime.Now;

                dbContext.BajaPersonals.Add(bajaPersonalDb);

                dbContext.SaveChanges();
                return new Resultado("El Recurso se guardó correctamente.");

            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        private string GetNextCodigoBaja()
        {
            var codigoString = dbContext.BajaPersonals.Max(p => p.codigoBaja);
            var codigoNumero = long.Parse(codigoString);
            var nuevoCodigo = codigoNumero + 1;
            return nuevoCodigo.ToString();
        }


        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
