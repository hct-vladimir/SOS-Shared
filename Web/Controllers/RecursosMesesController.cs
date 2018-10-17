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
    public class RecursosMesesController : Controller
    {
        private RecursosMesesManager recursosMesesManager = new RecursosMesesManager();
        private PlanProgramaticoManager planProgramaticoManager = new PlanProgramaticoManager();
        private CuentasContablesManager cuentasContablesManager = new CuentasContablesManager();
        private PresupuestosManager presupuestosManager = new PresupuestosManager();
        private FacilitiesManager facilitiesManager = new FacilitiesManager();

        // GET: RecursosMeses
        public ActionResult Index(bool tieneCobertura = false, int facilityId = 0, int ciudadId = 0)
        {
            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            var facilityActual = facilitiesManager.GetFacility(facilityId);

            var presupuestoFacilityActual = presupuestosManager.GetPresupuestosFacilityActual(presupuestoActual.Id, facilityActual.Id);
            ViewBag.EstadoFacilityId = presupuestoFacilityActual != null ? presupuestoFacilityActual.EstadoPresupuestoId : 0;
            ViewBag.EstadoFacility = presupuestoFacilityActual != null && presupuestoFacilityActual.EstadoPresupuesto != null ? presupuestoFacilityActual.EstadoPresupuesto.Nombre : string.Empty;

            // Si se trata de un facility compartido, obtener el estado en base a la ciudad correspondiente a este facility compartido
            if (facilityActual.Codigo == "R0031557")
            {
                var presupuestoCompartidoActual = presupuestosManager.GetPresupuestoCompartidoActual(presupuestoActual.Id, facilityId, ciudadId);
                if (presupuestoCompartidoActual != null)
                {
                    ViewBag.EstadoFacilityId = presupuestoCompartidoActual.EstadoPresupuestoId;
                    ViewBag.EstadoFacility = presupuestoCompartidoActual.EstadosPresupuesto != null ? presupuestoCompartidoActual.EstadosPresupuesto.Nombre : string.Empty;
                }
            }

            ViewBag.EstadoPresupuestoId = presupuestoActual != null ? presupuestoActual.EstadoPresupuestoId : 0;
            ViewBag.EstadoPresupuesto = presupuestoActual != null && presupuestoActual.EstadoPresupuesto != null ? presupuestoActual.EstadoPresupuesto.Nombre : string.Empty;

            ////ViewBag.PlanProgramaticoId = new SelectList(planProgramaticoManager.GetAllPlan(), "Id", "NombreDespliegue");
            ////ViewBag.CuentaContableId = new SelectList(cuentasContablesManager.GetAllCuentasContables(), "Id", "NombreDespliegue");

            ////// Parametricas
            ////ViewBag.FacilityId = new SelectList(facilitiesManager.GetAllFacilities(), "Id", "NombreDespliegue");
            ////ViewBag.ContraparteId = new SelectList(new ContrapartesManager().GetAllContrapartes(), "Id", "Nombre");
            ////ViewBag.AnexoTributarioId = new SelectList(new AnexosTributariosManager().GetAllAnexosTributarios(), "Id", "Descripcion");
            ////ViewBag.CodigoAuditoriaId = new SelectList(new CodigosAuditoriasManager().GetAllCodigosAuditoria(), "Id", "Descripcion");
            ////ViewBag.AccionNacionalId = new SelectList(new AccionesNacionalesManager().GetAllAccionesNacionales(), "Id", "Descripcion");
            ViewBag.PresupuestoActual = presupuestoActual;
            ViewBag.FacilityActual = facilityActual;
            ViewBag.CiudadOrigenId = ciudadId;

            var recursosMeses = recursosMesesManager.GetRecursosMesesPorFacilityCiudad(facilityId, ciudadId);

            recursosMeses = tieneCobertura ? 
                recursosMeses.Where(r => r.Recurso.Cobertura.HasValue && r.Recurso.Cobertura.Value > 0).ToList() : 
                recursosMeses.Where(r => !r.Recurso.Cobertura.HasValue || r.Recurso.Cobertura.Value == 0).ToList();

            ViewBag.TotalPresupuesto = recursosMeses.Sum(r => r.Recurso.Monto);
            ViewBag.TieneCobertura = tieneCobertura;
            return View(recursosMeses);
        }

        public ActionResult ReporteBet(int facilityId, int ciudadId)
        {
            // Template File
            string templateDocument =
                System.Web.HttpContext.Current.Server.MapPath("~/Templates/E01-ReporteBET.xlsx");

            var report = recursosMesesManager.GetReporteBet(templateDocument, facilityId, ciudadId);

            return File(report.ToArray(), "application/octet-stream", string.Format("SOS-ReporteBET-{0}{1}{2}.xlsx", DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2, '0'), DateTime.Now.Day.ToString().PadLeft(2, '0')));
        }


        // GET: RecursosMeses/Edit/5
        public ActionResult Edit(int id)
        {
            var presupuestoActual = presupuestosManager.GetPresupuestoActual();
            ViewBag.EstadoPresupuestoId = presupuestoActual != null ? presupuestoActual.EstadoPresupuestoId : 0;
            ViewBag.EstadoPresupuesto = presupuestoActual != null && presupuestoActual.EstadoPresupuesto != null ? presupuestoActual.EstadoPresupuesto.Nombre : string.Empty;

            var recursosMeses = recursosMesesManager.GetAllRecursosMeses();
            ViewBag.RecursosMeses = recursosMeses;
            ViewBag.TotalPresupuesto = recursosMeses.Sum(r => r.Recurso.Monto);

            var recursoMes = recursosMesesManager.GetRecursoMes(id);
            return View(recursoMes);
        }

        // POST: RecursosMeses/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, RecursoMes recursoMes)
        {
            var resultado = new Resultado(string.Empty);
            var usuarioActual = HttpContext.User.Identity.Name;
            try
            {
                recursoMes.UsuarioModificacion = usuarioActual;
                resultado = recursosMesesManager.UpdateRecursoMes(recursoMes);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(resultado);
            }

        }

        // POST: RecursosMeses/Edit/5
        [HttpPost]
        public ActionResult EditCobertura(int id, RecursoMes recursoMes)
        {
            var resultado = new Resultado(string.Empty);
            var usuarioActual = HttpContext.User.Identity.Name;
            try
            {
                recursoMes.UsuarioModificacion = usuarioActual;
                resultado = recursosMesesManager.UpdateCoberturaRecursoMes(recursoMes);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(resultado);
            }

        }
    }
}
