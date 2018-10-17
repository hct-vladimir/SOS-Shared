using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes;
using Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes.Presupuesto;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Security.Entities;
using AccionesNacionale = Cerberus.Sos.Accounting.BusinessLogic.Entities.AccionesNacionale;
using AnexosTributario = Cerberus.Sos.Accounting.BusinessLogic.Entities.AnexosTributario;
using Banco = Cerberus.Sos.Accounting.BusinessLogic.Entities.Banco;
using Ciudad = Cerberus.Sos.Accounting.BusinessLogic.Entities.Ciudad;
using CodigosAuditoria = Cerberus.Sos.Accounting.BusinessLogic.Entities.CodigosAuditoria;
using Comprobante = Cerberus.Sos.Accounting.BusinessLogic.Entities.Comprobante;
using Contraparte = Cerberus.Sos.Accounting.BusinessLogic.Entities.Contraparte;
using CuentaAsiento = Cerberus.Sos.Accounting.BusinessLogic.Entities.CuentaAsiento;
using PlantillaCuenta = Cerberus.Sos.Accounting.BusinessLogic.Entities.PlantillaCuenta;
using PlantillaAsiento = Cerberus.Sos.Accounting.BusinessLogic.Entities.PlantillaAsiento;
using CuentaBanco = Cerberus.Sos.Accounting.BusinessLogic.Entities.CuentaBanco;
using CuentaContable = Cerberus.Sos.Accounting.BusinessLogic.Entities.CuentaContable;
using EstadoCuenta = Cerberus.Sos.Accounting.BusinessLogic.Entities.EstadoCuenta;
using TraspasosPrograma = Cerberus.Sos.Accounting.BusinessLogic.Entities.TraspasosPrograma;
using Facility = Cerberus.Sos.Accounting.BusinessLogic.Entities.Facility;
using Factura = Cerberus.Sos.Accounting.BusinessLogic.Entities.Factura;
using FacturaComprador = Cerberus.Sos.Accounting.BusinessLogic.Entities.FacturaComprador;
using Personal = Cerberus.Sos.Accounting.BusinessLogic.Entities.Personal;
using PersonalBaja = Cerberus.Sos.Accounting.BusinessLogic.Entities.PersonalBaja;
using PlanProgramatico = Cerberus.Sos.Accounting.BusinessLogic.Entities.PlanProgramatico;
using Presupuesto = Cerberus.Sos.Accounting.BusinessLogic.Entities.Presupuesto;
using Recurso = Cerberus.Sos.Accounting.BusinessLogic.Entities.Recurso;
using Observacion = Cerberus.Sos.Accounting.BusinessLogic.Entities.Observacion;
using Territorio = Cerberus.Sos.Accounting.BusinessLogic.Entities.Territorio;
using Usuario = Cerberus.Sos.Accounting.BusinessLogic.Entities.Usuario;


using PlanillaSueldo = Cerberus.Sos.Accounting.BusinessLogic.Entities.PlanillaSueldo;

using ListaArea = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaArea;
using ListaCiudad = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaCiudad;
using ListaDepartamento = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaDepartamento;
using ListaFacility = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaFacility;
using ListaFilial = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaFilial;
using ListaProfesion = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaProfesion;
using ListaPrograma = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaPrograma;
using ListaSeccion = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.ListaSeccion;
using TipoDocumento = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.TipoDocumento;
using TipoEstadoCivil = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.TipoEstadoCivil;
using TipoRetiro = Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas.TipoRetiro;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public sealed class MapperManager
    {
        private static MapperManager instance = null;
        private static readonly object padlock = new object();

        private MapperManager()
        {
            Mapper.Initialize(config =>
            {

                config.CreateMap<DataAccess.Models.Usuario, User>();
                config.CreateMap<User, DataAccess.Models.Usuario>();

                config.CreateMap<DataAccess.Models.Usuario, Usuario>();
                config.CreateMap<Usuario, DataAccess.Models.Usuario>();

                config.CreateMap<DataAccess.Models.CuentaContable, CuentaContable>();
                config.CreateMap<CuentaContable, DataAccess.Models.CuentaContable>();

                config.CreateMap<DataAccess.Models.Comprobante, Comprobante>();
                config.CreateMap<Comprobante, DataAccess.Models.Comprobante>()
                    .ForMember(dest => dest.CuentasAsientos, opt => opt.Ignore())
                    .ForMember(dest => dest.CuentaBanco, opt => opt.Ignore())
                    .ForMember(dest => dest.Facility, opt => opt.Ignore())
                    .ForMember(dest => dest.TiposComprobante, opt => opt.Ignore());

                config.CreateMap<VCierreContable, CierreContableCiudad>();
                config.CreateMap<CierreContableCiudad, VCierreContable>();

                config.CreateMap<DataAccess.Models.EstadoCuenta, EstadoCuenta>();
                config.CreateMap<EstadoCuenta, DataAccess.Models.EstadoCuenta>();

                config.CreateMap<DataAccess.Models.TraspasosPrograma, TraspasosPrograma>();
                config.CreateMap<TraspasosPrograma, DataAccess.Models.TraspasosPrograma>();

                config.CreateMap<DataAccess.Models.CuentaAsiento, CuentaAsiento>();
                config.CreateMap<CuentaAsiento, DataAccess.Models.CuentaAsiento>()
                    .ForMember(dest => dest.AccionesNacionale, opt => opt.Ignore())
                    .ForMember(dest => dest.AnexosTributario, opt => opt.Ignore())
                    .ForMember(dest => dest.CodigosAuditoria, opt => opt.Ignore())
                    .ForMember(dest => dest.PlanProgramatico, opt => opt.Ignore())
                    .ForMember(dest => dest.Comprobante, opt => opt.Ignore())
                    .ForMember(dest => dest.Contraparte, opt => opt.Ignore())
                    .ForMember(dest => dest.CuentaContable, opt => opt.Ignore())
                    .ForMember(dest => dest.Territorio, opt => opt.Ignore());

                config.CreateMap<DataAccess.Models.PlantillaAsiento, PlantillaAsiento>();
                config.CreateMap<PlantillaAsiento, DataAccess.Models.PlantillaAsiento>();

                config.CreateMap<DataAccess.Models.PlantillaCuenta, PlantillaCuenta>();
                config.CreateMap<PlantillaCuenta, DataAccess.Models.PlantillaCuenta>()
                    .ForMember(dest => dest.AccionesNacionale, opt => opt.Ignore())
                    .ForMember(dest => dest.AnexosTributario, opt => opt.Ignore())
                    .ForMember(dest => dest.CodigosAuditoria, opt => opt.Ignore())
                    .ForMember(dest => dest.PlanProgramatico, opt => opt.Ignore())
                    .ForMember(dest => dest.PlantillaAsiento, opt => opt.Ignore())
                    .ForMember(dest => dest.Contraparte, opt => opt.Ignore())
                    .ForMember(dest => dest.CuentaContable, opt => opt.Ignore())
                    .ForMember(dest => dest.Territorio, opt => opt.Ignore());

                config.CreateMap<DataAccess.Models.PlanProgramatico, PlanProgramatico>();
                config.CreateMap<PlanProgramatico, DataAccess.Models.PlanProgramatico>();

                config.CreateMap<DataAccess.Models.Presupuesto, Presupuesto>();
                config.CreateMap<Presupuesto, DataAccess.Models.Presupuesto>();

                config.CreateMap<DataAccess.Models.Recurso, Recurso>();
                config.CreateMap<Recurso, DataAccess.Models.Recurso>()
                    .ForMember(dest => dest.RecursosMeses, opt => opt.Ignore());

                config.CreateMap<DataAccess.Models.Observacion, Observacion>()
                    .ForMember(dest => dest.EsCompartido , opt => opt.Ignore())
                    .ForMember(dest => dest.CiudadOrigenId, opt => opt.Ignore());
                config.CreateMap<Observacion, DataAccess.Models.Observacion>();

                config.CreateMap<DataAccess.Models.Factura, Factura>();
                config.CreateMap<Factura, DataAccess.Models.Factura>();

                config.CreateMap<DataAccess.Models.FacturaComprador, FacturaComprador>();
                config.CreateMap<FacturaComprador, DataAccess.Models.FacturaComprador>();

                config.CreateMap<DataAccess.Models.AccionesNacionale, AccionesNacionale>();
                config.CreateMap<AccionesNacionale, DataAccess.Models.AccionesNacionale>().ForMember(dest => dest.CuentasAsientos, opt => opt.Ignore()).ForMember(dest => dest.Recursos, opt => opt.Ignore());

                config.CreateMap<DataAccess.Models.Ciudad, Ciudad>();
                config.CreateMap<Ciudad, DataAccess.Models.Ciudad>();

                config.CreateMap<DataAccess.Models.Banco, Banco>();
                config.CreateMap<Banco, DataAccess.Models.Banco>();

                config.CreateMap<DataAccess.Models.Territorio, Territorio>();
                config.CreateMap<Territorio, DataAccess.Models.Territorio>();

                config.CreateMap<DataAccess.Models.Contraparte, Contraparte>();
                config.CreateMap<Contraparte, DataAccess.Models.Contraparte>();

                config.CreateMap<DataAccess.Models.Facility, Facility>();
                config.CreateMap<Facility, DataAccess.Models.Facility>();

                config.CreateMap<DataAccess.Models.CuentaBanco, CuentaBanco>();
                config.CreateMap<CuentaBanco, DataAccess.Models.CuentaBanco>();

                config.CreateMap<DataAccess.Models.AnexosTributario, AnexosTributario>();
                config.CreateMap<AnexosTributario, DataAccess.Models.AnexosTributario>();

                config.CreateMap<DataAccess.Models.CodigosAuditoria, CodigosAuditoria>();
                config.CreateMap<CodigosAuditoria, DataAccess.Models.CodigosAuditoria>();

                /* MAPEOS PARA PLANILLAS */
                config.CreateMap<DataAccess.Models.Personal, Personal>();
                config.CreateMap<Personal, DataAccess.Models.Personal>();

                config.CreateMap<DataAccess.Models.BajaPersonal, PersonalBaja>();
                config.CreateMap<PersonalBaja, DataAccess.Models.BajaPersonal>();

                config.CreateMap<PERIODOS_PLANILLAS, PeriodoPlanilla>()
                    .ForMember(dest => dest.EstadoPlanilla, opt => opt.MapFrom(src => src.ESTADOS_PLANILLAS));
                config.CreateMap<PeriodoPlanilla, PERIODOS_PLANILLAS>();

                config.CreateMap<RETROACTIVO, Retroactivo>()
                    .ForMember(dest => dest.EstadoPlanilla, opt => opt.MapFrom(src => src.ESTADOS_PLANILLAS));
                config.CreateMap<Retroactivo, RETROACTIVO>();

                config.CreateMap<ESTADOS_PLANILLAS, EstadoPlanilla>();
                config.CreateMap<EstadoPlanilla, ESTADOS_PLANILLAS>();
               
                config.CreateMap<PLANILLA_SUELDOS, PlanillaSueldo>();
                config.CreateMap<PlanillaSueldo, PLANILLA_SUELDOS>();

                config.CreateMap<PLANILLA_RCIVA, PlanillaRcIva>();
                config.CreateMap<PlanillaRcIva, PLANILLA_RCIVA>();

                config.CreateMap<PARAMETRO, PlanillaParametro>();
                config.CreateMap<PlanillaParametro, PARAMETRO>();

                config.CreateMap<VPLANILLA_SUELDOS_GENERAL, RptPlanillaSueldosGeneral>();
                config.CreateMap<VPLANILLA_SUELDOS, RptPlanillaSueldos>();
                config.CreateMap<VPLANILLA_RCIVA, RptPlanillaRcIva>();
                /* ECC 13/10/18 mapeo de datos de las planillas*/
                config.CreateMap<VPLANILLA_AFP_FUTURO, RptPlanillaAfpFuturo>();
                config.CreateMap<VPLANILLA_AFP_PREVISION, RptPlanillaAfpPrevision>();
                config.CreateMap<VPLANILLA_APORTES_SALUD, RptPlanillaAportesSalud>();

                /* MAPEOS PARA PARAMETROS BD DE ALDEAS */
                config.CreateMap<ListaArea, DataAccess.Models.ListaArea>();
                config.CreateMap<DataAccess.Models.ListaArea, ListaArea>();

                config.CreateMap<ListaCiudad, DataAccess.Models.ListaCiudad>();
                config.CreateMap<DataAccess.Models.ListaCiudad, ListaCiudad>();

                config.CreateMap<ListaDepartamento, DataAccess.Models.ListaDepartamento>();
                config.CreateMap<DataAccess.Models.ListaDepartamento, ListaDepartamento>();

                config.CreateMap<ListaFacility, DataAccess.Models.ListaFacility>();
                config.CreateMap<DataAccess.Models.ListaFacility, ListaFacility>();

                config.CreateMap<ListaFilial, DataAccess.Models.ListaFilial>();
                config.CreateMap<DataAccess.Models.ListaFilial, ListaFilial>();

                config.CreateMap<ListaProfesion, DataAccess.Models.ListaProfesion>();
                config.CreateMap<DataAccess.Models.ListaProfesion, ListaProfesion>();

                config.CreateMap<ListaPrograma, DataAccess.Models.ListaPrograma>();
                config.CreateMap<DataAccess.Models.ListaPrograma, ListaPrograma>();

                config.CreateMap<ListaSeccion, DataAccess.Models.ListaSeccion>();
                config.CreateMap<DataAccess.Models.ListaSeccion, ListaSeccion>();

                config.CreateMap<TipoDocumento, DataAccess.Models.TipoDocumento>();
                config.CreateMap<DataAccess.Models.TipoDocumento, TipoDocumento>();

                config.CreateMap<TipoEstadoCivil, DataAccess.Models.TipoEstadoCivil>();
                config.CreateMap<DataAccess.Models.TipoEstadoCivil, TipoEstadoCivil>();

                config.CreateMap<TipoRetiro, DataAccess.Models.TipoRetiro>();
                config.CreateMap<DataAccess.Models.TipoRetiro, TipoRetiro>();

                /* MAPEOS PARA REPORTES */
                config.CreateMap<RptPresupuestoResumen_Result, ReporteResumen>();
                config.CreateMap<ReporteResumen, RptPresupuestoResumen_Result>();

                config.CreateMap<VComprobante, ReporteComprobante>();
                config.CreateMap<ReporteComprobante, VComprobante>();

                config.CreateMap<ReporteBalanceComprobacion, BalanceComprobacion_Result>();
                config.CreateMap<BalanceComprobacion_Result, ReporteBalanceComprobacion>();

                config.CreateMap<BalanceRangoCuentas_Result, ReporteBalanceComprobacion>();

                config.CreateMap<ReporteMayores, Mayores_Result>();
                config.CreateMap<Mayores_Result, ReporteMayores>();

                config.CreateMap<ReporteBalanceGeneral, BalanceGeneral_Result>();
                config.CreateMap<BalanceGeneral_Result, ReporteBalanceGeneral>();

                config.CreateMap<ReporteEstadoResultados, EstadoResultado_Result>();
                config.CreateMap<EstadoResultado_Result, ReporteEstadoResultados>();

                config.CreateMap<ReporteLibroCompras, LibroCompras_Result>();
                config.CreateMap<LibroCompras_Result, ReporteLibroCompras>();

                config.CreateMap<ReporteLibroVentas, LibroVentas_Result>();
                config.CreateMap<LibroVentas_Result, ReporteLibroVentas>();
            });
        }

        public static void GetInstance()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new MapperManager();
                }
            }
        }
    }
}
