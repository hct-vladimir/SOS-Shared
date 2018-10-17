using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas;
using Cerberus.Sos.Accounting.DataAccess.Models;
using ListaArea = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaArea;
using ListaCargo = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaCargo;
using ListaCiudad = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaCiudad;
using ListaDepartamento = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaDepartamento;
using ListaEscalaSalarial = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaEscalaSalarial;
using ListaFacility = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaFacility;
using ListaFilial = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaFilial;
using ListaProfesion = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaProfesion;
using ListaPrograma = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaPrograma;
using ListaSeccion = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaSeccion;
using TipoDocumento = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.TipoDocumento;
using TipoEstadoCivil = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.TipoEstadoCivil;
using TipoEstudio = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.TipoEstudio;
using TipoAfp = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.TipoAfp;
using TipoContratacion = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.TipoContratacion;
using TipoInicio = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.TipoInicio;
using TipoRetiro = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.TipoRetiro;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class AldeasParametrosManager : IDisposable
    {
        private AldeasInfantilesDBEntities dbContext = new AldeasInfantilesDBEntities();

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<ListaArea> GetAllAreas()
        {
            var parametroDb = dbContext.ListaAreas.ToList();
            MapperManager.GetInstance();

            var parametro = new List<ListaArea>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.ListaArea, ListaArea>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<ListaCiudad> GetAllCiudades()
        {
            var parametroDb = dbContext.ListaCiudades.ToList();
            MapperManager.GetInstance();

            var parametro = new List<ListaCiudad>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.ListaCiudad, ListaCiudad>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<ListaDepartamento> GetAllDepartamentos()
        {
            var parametroDb = dbContext.ListaDepartamentoes.ToList();
            MapperManager.GetInstance();

            var parametro = new List<ListaDepartamento>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.ListaDepartamento, ListaDepartamento>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<ListaFacility> GetAllFacilities()
        {
            var parametroDb = dbContext.ListaFacilities.ToList();
            MapperManager.GetInstance();

            var parametro = new List<ListaFacility>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.ListaFacility, ListaFacility>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<ListaFilial> GetAllFiliales()
        {
            var parametroDb = dbContext.ListaFilials.ToList();
            MapperManager.GetInstance();

            var parametro = new List<ListaFilial>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.ListaFilial, ListaFilial>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<ListaProfesion> GetAllProfesiones()
        {
            var parametroDb = dbContext.ListaProfesions.ToList();
            MapperManager.GetInstance();

            var parametro = new List<ListaProfesion>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.ListaProfesion, ListaProfesion>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<ListaPrograma> GetAllProgramas()
        {
            var parametroDb = dbContext.ListaProgramas.ToList();
            MapperManager.GetInstance();

            var parametro = new List<ListaPrograma>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.ListaPrograma, ListaPrograma>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<ListaSeccion> GetAllSecciones()
        {
            var parametroDb = dbContext.ListaSeccions.ToList();
            MapperManager.GetInstance();

            var parametro = new List<ListaSeccion>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.ListaSeccion, ListaSeccion>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<TipoDocumento> GetAllTiposDocumentos()
        {
            var parametroDb = dbContext.TipoDocumentoes.ToList();
            MapperManager.GetInstance();

            var parametro = new List<TipoDocumento>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.TipoDocumento, TipoDocumento>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<TipoEstadoCivil> GetAllTiposEstadoCivil()
        {
            var parametroDb = dbContext.TipoEstadoCivils.ToList();
            MapperManager.GetInstance();

            var parametro = new List<TipoEstadoCivil>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.TipoEstadoCivil, TipoEstadoCivil>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<TipoEstudio> GetAllTiposEstudios()
        {
            var parametroDb = dbContext.TipoEstudios.ToList();
            MapperManager.GetInstance();

            var parametro = new List<TipoEstudio>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.TipoEstudio, TipoEstudio>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetAllGeneros()
        {
            var generosItems = new List<SelectListItem>
            {
                new SelectListItem() {Text = "Femenino", Value = "F"},
                new SelectListItem() {Text = "Masculino", Value = "M"}
            };

            return generosItems;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<ListaCargo> GetAllCargos()
        {
            var parametroDb = dbContext.ListaCargoes.ToList();
            MapperManager.GetInstance();

            var parametro = new List<ListaCargo>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.ListaCargo, ListaCargo>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<Entities.Aldeas.SeguroMedico> GetAllSeguros()
        {
            var parametroDb = dbContext.Seguros.ToList();
            MapperManager.GetInstance();

            var parametro = new List<Entities.Aldeas.SeguroMedico>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.Seguro, Entities.Aldeas.SeguroMedico>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<TipoAfp> GetAllTiposAfps()
        {
            var parametroDb = dbContext.TipoAFPs.ToList();
            MapperManager.GetInstance();

            var parametro = new List<TipoAfp>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.TipoAFP, TipoAfp>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<ListaAfp> GetAllAfps()
        {
            var parametroDb = dbContext.ListaAFPs.ToList();
            MapperManager.GetInstance();

            var parametro = new List<ListaAfp>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.ListaAFP, ListaAfp>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<TipoInicio> GetAllTiposInicios()
        {
            var parametroDb = dbContext.TipoInicios.ToList();
            MapperManager.GetInstance();

            var parametro = new List<TipoInicio>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.TipoInicio, TipoInicio>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<TipoContratacion> GetAllTiposContrataciones()
        {
            var parametroDb = dbContext.TipoContratacions.ToList();
            MapperManager.GetInstance();

            var parametro = new List<TipoContratacion>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.TipoContratacion, TipoContratacion>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<ListaEscalaSalarial> GetAllEscalasSalariales()
        {
            var parametroDb = dbContext.ListaEscalaSalarials.ToList();
            MapperManager.GetInstance();

            var parametro = new List<ListaEscalaSalarial>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.ListaEscalaSalarial, ListaEscalaSalarial>(p)));

            return parametro;
        }

        /// <summary>
        /// Devuelve una lista del parámetro.
        /// </summary>
        /// <returns></returns>
        public List<TipoRetiro> GetAllTiposRetiros()
        {
            var parametroDb = dbContext.TipoRetiroes.ToList();
            MapperManager.GetInstance();

            var parametro = new List<TipoRetiro>();
            parametroDb.ForEach(p => parametro.Add(Mapper.Map<DataAccess.Models.TipoRetiro, TipoRetiro>(p)));

            return parametro;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
