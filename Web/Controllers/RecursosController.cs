using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;
using PagedList;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    [Authorize]
    public class RecursosController : Controller
    {
        private RecursosManager recursosManager = new RecursosManager();
        private PlanProgramaticoManager planProgramaticoManager = new PlanProgramaticoManager();
        private CuentasContablesManager cuentasContablesManager = new CuentasContablesManager();
        private PresupuestosManager presupuestosManager = new PresupuestosManager();
        private FacilitiesManager facilitiesManager = new FacilitiesManager();
        private ObservacionesManager observacionesManager = new ObservacionesManager();
        private ReportesManager reportesManager = new ReportesManager();
        private CiudadesManager ciudadesManager = new CiudadesManager();

        // GET: Index
        public ActionResult Facilities(int? ciudadId, int id = 0)
        {
            var presupuestoActual = presupuestosManager.GetPresupuestoActual();

            ViewBag.ExistePresupuestoActivo = presupuestoActual != null;
            ViewBag.EstadoPresupuestoId = presupuestoActual != null ? presupuestoActual.EstadoPresupuestoId : 0;
            ViewBag.EstadoPresupuesto = presupuestoActual != null && presupuestoActual.EstadoPresupuesto != null ? presupuestoActual.EstadoPresupuesto.Nombre : string.Empty;
            ViewBag.CiudadOrigenId = ciudadId;
            ViewBag.PresupuestoActual = presupuestoActual;

            var esUsuarioCoordinador = User.IsInRole("CORD-PRG");
            var tieneFacilitiesCompartidos = (ciudadId == 12);

            var facilities = facilitiesManager.GetFacilitiesRecursosPorCiudad(presupuestoActual.Id, ciudadId.Value, esUsuarioCoordinador, tieneFacilitiesCompartidos);

            if (User.IsInRole("CORD-PRG") && tieneFacilitiesCompartidos)
            {
                var facilitiesCompartidos = facilitiesManager.GetFacilitiesRecursosCompartidos(presupuestoActual.Id, ciudadId.Value);
                facilities.AddRange(facilitiesCompartidos);

            }
            
            ViewBag.TotalPresupuesto = facilities.Sum(f => f.TotalRecursos);

            return View(facilities);
        }

        // GET: Index
        public ActionResult Index(int? pagina, int facilityId = 0, int ciudadId = 0, int? observacionId = 0)
        {
            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            var facilityActual = facilitiesManager.GetFacility(facilityId);

            var presupuestoFacilityActual = presupuestosManager.GetPresupuestosFacilityActual(presupuestoActual.Id, facilityActual.Id);
            ViewBag.EstadoFacilityId = presupuestoFacilityActual != null ? presupuestoFacilityActual.EstadoPresupuestoId : 0;
            ViewBag.EstadoFacility = presupuestoFacilityActual != null && presupuestoFacilityActual.EstadoPresupuesto != null ? presupuestoFacilityActual.EstadoPresupuesto.Nombre : string.Empty;

            var observaciones = observacionesManager.GetObservacionesPorEntidadFacility(1, presupuestoActual.Id, facilityId).Where(o => !o.Aprobado).ToList();
            
            // Si se trata de un facility compartido, obtener el estado en base a la ciudad correspondiente a este facility compartido
            if (facilityActual.Codigo == "R0031557")
            {
                var presupuestoCompartidoActual = presupuestosManager.GetPresupuestoCompartidoActual(presupuestoActual.Id, facilityId, ciudadId);
                if (presupuestoCompartidoActual != null)
                {
                    ViewBag.EstadoFacilityId = presupuestoCompartidoActual.EstadoPresupuestoId;
                    ViewBag.EstadoFacility = presupuestoCompartidoActual.EstadosPresupuesto != null ? presupuestoCompartidoActual.EstadosPresupuesto.Nombre : string.Empty;

                    observaciones = observaciones.Where(o => o.CiudadId == ciudadId).ToList();
                }
            }

            ViewBag.EstadoPresupuestoId = presupuestoActual != null ? presupuestoActual.EstadoPresupuestoId : 0;
            ViewBag.EstadoPresupuesto = presupuestoActual != null && presupuestoActual.EstadoPresupuesto != null ? presupuestoActual.EstadoPresupuesto.Nombre : string.Empty;
            ViewBag.Observaciones = observaciones;
            ViewBag.PresupuestoActual = presupuestoActual;
            ViewBag.FacilityActual = facilityActual;
            ViewBag.CiudadOrigenId = ciudadId;

            var listaPlanProgramatico = planProgramaticoManager.GetPlanByFacility(facilityId);
            if (!listaPlanProgramatico.Any())
            {
                listaPlanProgramatico = planProgramaticoManager.GetAllPlan();
            }

            ViewBag.PlanProgramaticoId = new SelectList(listaPlanProgramatico, "Id", "NombreLista", new object(), planProgramaticoManager.GetParentsPlanIds());
            ViewBag.CuentaContableId = new SelectList(cuentasContablesManager.GetAllCuentasContables(), "Id", "NombreDespliegue", new object(), cuentasContablesManager.GetParentsCuentasIds());

            // Parametricas
            ViewBag.FacilityId = new SelectList(facilitiesManager.GetAllFacilities(), "Id", "NombreDespliegue");
            ViewBag.ContraparteId = new SelectList(new ContrapartesManager().GetAllContrapartes(), "Id", "NombreDespliegue");
            ViewBag.AnexoTributarioId = new SelectList(new AnexosTributariosManager().GetAllAnexosTributarios(), "Id", "Descripcion");
            ViewBag.CodigoAuditoriaId = new SelectList(new CodigosAuditoriasManager().GetAllCodigosAuditoria(), "Id", "Descripcion");
            ViewBag.MarcoLogicoId = new SelectList(new CodigosAuditoriasManager().GetAllCodigosMarcoLogico(), "Id", "Codigo");
            ViewBag.AccionNacionalId = new SelectList(new AccionesNacionalesManager().GetAllAccionesNacionales(), "Id", "NombreDespliegue");
            ViewBag.TerritorioId = new SelectList(new TerritoriosManager().GetAllTerritorios(), "Id", "NombreDespliegue");

            var recursos = recursosManager.GetRecursosPorFacilityCiudad(facilityId, ciudadId);
            if (observacionId != null && observacionId.Value != 0)
            {
                var observacionActual = observacionesManager.GetObservacion(observacionId.Value);
                if (!string.IsNullOrEmpty(observacionActual.FilasObservadas))
                {
                    var filasObservadas = observacionActual.FilasObservadas.Split(',');
                    recursos = recursosManager.GetRecursosPorFacilityCiudad(facilityId, ciudadId).Where(r => filasObservadas.Contains(r.Id.ToString())).ToList();

                    ViewBag.ObservacionActual = observacionActual;
                }
            }

            @ViewBag.TotalPresupuesto = recursos.Sum(r => r.Monto);
            return View(recursos);
        }

        // GET: Revision
        public ActionResult Revision(int? pagina, int facilityId = 0, int ciudadId = 0, int? observacionId = 0, bool? esCompartido = false, int ciudadOrigenId = 0)
        {
            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            var facilityActual = facilitiesManager.GetFacility(facilityId);

            var presupuestoFacilityActual = presupuestosManager.GetPresupuestosFacilityActual(presupuestoActual.Id, facilityActual.Id);
            ViewBag.EstadoFacilityId = presupuestoFacilityActual != null ? presupuestoFacilityActual.EstadoPresupuestoId : 0;
            ViewBag.EstadoFacility = presupuestoFacilityActual != null && presupuestoFacilityActual.EstadoPresupuesto != null ? presupuestoFacilityActual.EstadoPresupuesto.Nombre : string.Empty;

            ViewBag.CiudadOrigenId = ciudadId;
            ViewBag.EsCompartido = (esCompartido != null && esCompartido.Value);
 
            // Si se trata de un facility compartido, obtener el estado en base a la ciudad correspondiente a este facility compartido
            if (esCompartido != null && esCompartido.Value)
            {
                var presupuestoCompartidoActual = presupuestosManager.GetPresupuestoCompartidoActual(presupuestoActual.Id, facilityId, ciudadId);
                if (presupuestoCompartidoActual != null)
                {
                    ViewBag.EstadoFacilityId = presupuestoCompartidoActual.EstadoPresupuestoId;
                    ViewBag.EstadoFacility = presupuestoCompartidoActual.EstadosPresupuesto != null ? presupuestoCompartidoActual.EstadosPresupuesto.Nombre : string.Empty;
                }

                var ciudadActual = ciudadesManager.GetCiudad(ciudadId);
                facilityActual.Nombre = string.Format("{0} ({1})", facilityActual.Nombre, ciudadActual.Nombre);
                if (Session["ciudadId"] != null && Session["ciudadId"].ToString() == "12")
                {
                    ViewBag.CiudadOrigenId = ciudadOrigenId;
                    ViewBag.CiudadCompartidaId = ciudadId;
                }
            }


            ViewBag.EstadoPresupuestoId = presupuestoActual != null ? presupuestoActual.EstadoPresupuestoId : 0;
            ViewBag.EstadoPresupuesto = presupuestoActual != null && presupuestoActual.EstadoPresupuesto != null ? presupuestoActual.EstadoPresupuesto.Nombre : string.Empty;
            var observaciones = observacionesManager.GetObservacionesPorEntidadFacility(1, presupuestoActual.Id, facilityId);
            ViewBag.Observaciones =  esCompartido != null && esCompartido.Value
                ? observaciones.Where(o => o.CiudadId == ciudadId).ToList()
                : observaciones;

            ViewBag.PlanProgramaticoId = new SelectList(planProgramaticoManager.GetAllPlan(), "Id", "NombreLista", new object(), planProgramaticoManager.GetParentsPlanIds());
            ViewBag.CuentaContableId = new SelectList(cuentasContablesManager.GetAllCuentasContables(), "Id", "NombreDespliegue");

            // Parametricas
            ViewBag.FacilityId = new SelectList(facilitiesManager.GetAllFacilities(), "Id", "NombreDespliegue");
            ViewBag.ContraparteId = new SelectList(new ContrapartesManager().GetAllContrapartes(), "Id", "NombreDespliegue");
            ViewBag.AnexoTributarioId = new SelectList(new AnexosTributariosManager().GetAllAnexosTributarios(), "Id", "Descripcion");
            ViewBag.CodigoAuditoriaId = new SelectList(new CodigosAuditoriasManager().GetAllCodigosAuditoria(), "Id", "Descripcion");
            ViewBag.MarcoLogicoId = new SelectList(new CodigosAuditoriasManager().GetAllCodigosMarcoLogico(), "Id", "Codigo");
            ViewBag.AccionNacionalId = new SelectList(new AccionesNacionalesManager().GetAllAccionesNacionales(), "Id", "NombreDespliegue");
            ViewBag.TerritorioId = new SelectList(new TerritoriosManager().GetAllTerritorios(), "Id", "NombreDespliegue");

            ViewBag.PresupuestoActual = presupuestoActual;
            ViewBag.FacilityActual = facilityActual;

            var recursos = recursosManager.GetRecursosPorFacilityCiudad(facilityId, ciudadId);
            if (observacionId != null && observacionId.Value != 0)
            {
                var observacionActual = observacionesManager.GetObservacion(observacionId.Value);
                if (!string.IsNullOrEmpty(observacionActual.FilasObservadas))
                {
                    var filasObservadas = observacionActual.FilasObservadas.Split(',');
                    recursos = recursosManager.GetRecursosPorFacilityCiudad(facilityId, ciudadId).Where(r => filasObservadas.Contains(r.Id.ToString())).ToList();

                    ViewBag.ObservacionActual = observacionActual;
                }
            }

            @ViewBag.TotalPresupuesto = recursos.Sum(r => r.Monto);
            return View(recursos);
        }

        public ActionResult Resumen(int facilityId, int ciudadId, int? nivelId)
        {
            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            var facilityActual = facilitiesManager.GetFacility(facilityId);

            ViewBag.FacilityActual = facilityActual;
            ViewBag.CiudadOrigenId = ciudadId;

            var resumen = reportesManager.GetReportePresupuestoResumen(presupuestoActual.Id, ciudadId, facilityId);


            var nivelesIds = new List<int>();
            if (resumen.Count > 0 && nivelId != null && nivelId.Value > 0)
            {
                switch (nivelId.Value)
                {
                    case 1:
                        nivelesIds = new List<int> { 1 };
                        break;
                    case 2:
                        nivelesIds = new List<int> { 1, 2 };
                        break;
                    case 3:
                        nivelesIds = new List<int> { 1, 2, 3 };
                        break;
                }
                resumen = resumen.Where(r => nivelesIds.Contains(r.NivelProgramaticoId.Value)).ToList();
            }

            var codigosGastos = new List<string> {"10000", "20000", "30000", "40000"};
            var resumenGastos = resumen.Where(r => r.NivelProgramaticoId == 1 && codigosGastos.Contains(r.Codigo)).ToList();
            var resumenIngresos = resumen.Where(r => r.NivelProgramaticoId == 1 && r.Codigo.StartsWith("5")).ToList();

            var totalGastos = resumenGastos.Sum(r => r.Total);
            var totalIngresos = resumenIngresos.Sum(r => r.Total);
            var totalSubsidio = totalGastos + totalIngresos;

            ViewBag.TotalGasto = totalGastos;
            ViewBag.TotalIngreso = totalIngresos;
            ViewBag.TotalSubsidio = totalSubsidio;

            return View(resumen);
        }

        public ActionResult ReporteAnual()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/R01-PresupuestoAnual.xlsx");

            var report = recursosManager.GetReporteAnual(templateDocument);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-ReporteAnual-{0}.xlsx", DateTime.Now.Year));
        }

        public ActionResult ReporteMensual()
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/R02-PresupuestoMensual.xlsx");

            var report = recursosManager.GetReporteMensual(templateDocument);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-ReporteAnual-{0}.xlsx", DateTime.Now.Year));
        }

        public ActionResult ReporteBet(int facilityId, int ciudadId)
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/E01-ReporteBET.xlsx");

            var report = recursosManager.GetReporteBet(templateDocument, facilityId, ciudadId);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-ReporteBET-{0}{1}{2}.xlsx", DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0')));
        }

        // POST: Recursos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Recurso recurso)
        {
            try
            {
                var usuarioActual = HttpContext.User.Identity.Name;
                recurso.UsuarioCreacion = usuarioActual;
                recurso.UsuarioModificacion = usuarioActual;

                recursosManager.InsertRecurso(recurso);

                return RedirectToAction("Index", new { facilityId = recurso.FacilityId, ciudadId = recurso.CiudadId });
            }
            catch
            {
                return RedirectToAction("Index", new { facilityId = recurso.FacilityId, ciudadId = recurso.CiudadId });
            }
        }

        // POST: Recursos/CreateNacional
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNacional(Recurso recurso)
        {
            try
            {
                var usuarioActual = HttpContext.User.Identity.Name;
                recurso.UsuarioCreacion = usuarioActual;
                recurso.UsuarioModificacion = usuarioActual;

                recursosManager.InsertRecurso(recurso);

                return RedirectToAction("Revision", new { facilityId = recurso.FacilityId, ciudadId = recurso.CiudadId });
            }
            catch
            {
                return RedirectToAction("Revision", new { facilityId = recurso.FacilityId, ciudadId = recurso.CiudadId });
            }
        }

        // GET: CuentasAsientos/Edit/5
        public ActionResult Edit(int id)
        {
            var recurso = recursosManager.GetRecurso(id);

            var facilityActual = facilitiesManager.GetFacility(recurso.FacilityId);

            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            ViewBag.EstadoPresupuestoId = presupuestoActual != null ? presupuestoActual.EstadoPresupuestoId : 0;
            ViewBag.EstadoPresupuesto = presupuestoActual != null && presupuestoActual.EstadoPresupuesto != null ? presupuestoActual.EstadoPresupuesto.Nombre : string.Empty;

            ViewBag.PlanProgramatico = planProgramaticoManager.GetAllPlan();
            ViewBag.CuentasContables = cuentasContablesManager.GetAllCuentasContables();

            // Parametricas
            ViewBag.Facilities = new FacilitiesManager().GetAllFacilities();
            ViewBag.Contrapartes = new ContrapartesManager().GetAllContrapartes();
            ViewBag.AnexosTributarios = new AnexosTributariosManager().GetAllAnexosTributarios();
            ViewBag.CodigosAuditoria = new CodigosAuditoriasManager().GetAllCodigosAuditoria();
            ViewBag.AccionesNacionales = new AccionesNacionalesManager().GetAllAccionesNacionales();
            ViewBag.Territorios = new TerritoriosManager().GetAllTerritorios();
            ViewBag.FacilityActual = facilityActual;

            var recursos = recursosManager.GetRecursosPorFacility(facilityActual.Id);
            ViewBag.Recursos = recursos;
            ViewBag.TotalPresupuesto = recursos.Sum(r => r.Monto);
            return View(recurso);
        }

        // POST: CuentasAsientos/Edit/5
        [HttpPost]
        public ActionResult Edit(Recurso recurso)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            try
            {
                recurso.UsuarioModificacion = usuarioActual;
                var resultado = recursosManager.UpdateRecurso(recurso);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // POST: CuentasAsientos/Delete/5
        ////[HttpPost]
        ////[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var recurso = recursosManager.GetRecurso(id);
            try
            {
                var resultado = recursosManager.DeleteRecurso(recurso.Id, usuarioActual);
                return RedirectToAction("Index", new { facilityId = recurso.FacilityId, ciudadId = recurso.CiudadId });
            }
            catch
            {
                return RedirectToAction("Index", new { facilityId = recurso.FacilityId, ciudadId = recurso.CiudadId });
            }
        }

        public ActionResult DeleteNacional(int id)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var recurso = recursosManager.GetRecurso(id);
            try
            {
                var resultado = recursosManager.DeleteRecurso(recurso.Id, usuarioActual);
                return RedirectToAction("Revision", new { facilityId = recurso.FacilityId, ciudadId = recurso.CiudadId });
            }
            catch
            {
                return RedirectToAction("Revision", new { facilityId = recurso.FacilityId, ciudadId = recurso.CiudadId });
            }
        }

        /* AJAX Actions */
        // GET: CuentasAsientos/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var resultado = recursosManager.GetRecurso(id);

                return Json(resultado);
            }
            catch (Exception exception)
            {
                return RedirectToAction("Index", new { id });
            }
        }

        // GET: CuentasAsientos/Edit/5
        public ActionResult Duplicar(int id)
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            var nuevoRecurso = recursosManager.GetRecurso(id);
            try
            {
                nuevoRecurso.Id = 0;
                nuevoRecurso.Ciudad = null;
                nuevoRecurso.Facility = null;
                nuevoRecurso.AccionesNacionale = null;
                nuevoRecurso.CodigosAuditoria = null;
                nuevoRecurso.Contraparte = null;
                nuevoRecurso.CuentaContable = null;
                nuevoRecurso.PlanProgramatico = null;
                nuevoRecurso.Presupuesto = null;
                nuevoRecurso.Territorio = null;

                nuevoRecurso.UsuarioCreacion = usuarioActual;
                nuevoRecurso.UsuarioModificacion = usuarioActual;
                var resultado = recursosManager.InsertRecurso(nuevoRecurso);
                if (HttpContext.User.IsInRole("ADMN-PRG"))
                {
                    return RedirectToAction("Index", new { facilityId = nuevoRecurso.FacilityId, ciudadId = nuevoRecurso.CiudadId });
                }
                else
                {
                    return RedirectToAction("Revision", new { facilityId = nuevoRecurso.FacilityId, ciudadId = nuevoRecurso.CiudadId });
                }
                
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { facilityId = nuevoRecurso.FacilityId, ciudadId = nuevoRecurso.CiudadId });
            }
        }

        public ActionResult Historicos()
        {
            return View();
        }


    }
}