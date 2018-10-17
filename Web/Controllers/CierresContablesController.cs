using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    public class CierresContablesController : Controller
    {
        private CierresContablesManager cierresContablesManager = new CierresContablesManager();
        private ComprobantesManager comprobantesManager = new ComprobantesManager();
        private UsuariosManager usuariosManager = new UsuariosManager();
        private ObservacionesManager observacionesManager = new ObservacionesManager();

        // GET: CierresContables
        public ActionResult Index()
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            //Obtener Periodo Contable Activo
            var periodoActivo = cierresContablesManager.GetPeriodoActivo();

            //Si no existe un periodo activo no mostrar nada en la pantalla
            if (periodoActivo == null)
            {
                ViewBag.PeriodoActivo = string.Empty;
                return View(new List<CierreContableCiudad>());
            }

            ViewBag.PeriodoActivoId = periodoActivo.Id;
            ViewBag.PeriodoActivo = string.Format("{0}/{1}", periodoActivo.Mes.ToString().PadLeft(2, '0'), periodoActivo.Gestion);

            var cierresContables = new List<CierreContableCiudad>();

            if (HttpContext.User.IsInRole("CTBD-CLR"))
            {
                cierresContables = cierresContablesManager.GetCierresContablesCiudades(periodoActivo.Id);
            }
            else
            {
                var ciudadesId = usuariosManager.GetCiudadesUsuario(usuarioActual);
                cierresContables = cierresContablesManager.GetCierresContablesCiudades(ciudadesId, periodoActivo.Id);
            }

            ViewBag.TotalPresupuesto = 0;

            return View(cierresContables);
        }

        // GET: Presupuestos/CambiarEstado/2018
        public ActionResult CambiarEstado(int ciudadId, int periodoId, short estadoId)
        {
            var cierreActual = cierresContablesManager.GetCierreContableByCiudadPeriodo(ciudadId, periodoId);

            // Verificación de Comprobantes Pendientes
            var comprobantesPendientes = comprobantesManager.GetComprobantesPorCiudadEstado(ciudadId, cierreActual.Id, 1);

            if (comprobantesPendientes.Count > 0)
            {
                TempData["FlashMessage"] = "Existen Comprobantes Pendientes. Favor verificar.";
                return RedirectToAction("Facilities", "Comprobantes", new { ciudadId });
            }

            if (HttpContext.User.IsInRole("ADMN-PRG"))
            {
                // Verificación de Comprobantes Observados
                var comprobantesObservados = comprobantesManager.GetComprobantesPorCiudadEstado(ciudadId, cierreActual.Id, 3);

                if (comprobantesObservados.Count > 0)
                {
                    TempData["FlashMessage"] = "Existen Comprobantes Observados. Favor Finalizar todos los Comprobantes en estado Observado.";
                    return RedirectToAction("Facilities", "Comprobantes", new { ciudadId });
                } 
            }


            var resultado = cierresContablesManager.CambiarEstado(ciudadId, periodoId, estadoId);

            return RedirectToAction("Facilities", "Comprobantes", new { ciudadId });
        }

        // POST: CuentasAsientos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateObservacion(Observacion observacion)
        {
            try
            {
                var usuarioActual = HttpContext.User.Identity.Name;
                observacion.UsuarioCreacion = usuarioActual;
                observacion.UsuarioModificacion = usuarioActual;

                observacion.TipoObservacionId = 2; // Observación de Comprobante

                observacion.EsNacional = HttpContext.User.IsInRole("PPTO-CLR");

                observacionesManager.InsertObservacion(observacion);

                //Luego de guardar la Observación, se coloca al Comprobante en estado OBSERVADO
                var resultado = comprobantesManager.CambiarEstadoComprobante(observacion.EntidadId.Value, 3);

                return RedirectToAction("Index", "CuentasAsientos", new { id = observacion.EntidadId });
            }
            catch
            {
                return RedirectToAction("Index", "CuentasAsientos", new { id = observacion.EntidadId });
            }
        }

        // GET: CierresContables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CierresContables/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CierresContables/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CierresContables/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CierresContables/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CierresContables/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
