using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;

namespace AccountingSos.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private TiposCambioManager tiposCambioManager = new TiposCambioManager();
        private PresupuestosManager presupuestosManager = new PresupuestosManager();

        public ActionResult Index()
        {
            Session["presupuestoId"] = 0;

            if (!HttpContext.User.IsInRole("PPTO-CLR"))
            {
                var presupuestoActual = presupuestosManager.GetPresupuestoActual();
                ViewBag.PresupuestoActualId = presupuestoActual != null ? presupuestoActual.Id : 0;
                Session["presupuestoId"] = presupuestoActual != null ? presupuestoActual.Id : 0;
            }

            if (HttpContext.User.IsInRole("CTBD-CLR"))
            {
                var resultado = tiposCambioManager.InsertLastTipoCambio();
                var tiposCambio = tiposCambioManager.GetTiposCambioByFecha(DateTime.Now.Date);
                ViewBag.TiposCambio = tiposCambio;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult EditTipoCambio(TipoCambio tipoCambio)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            try
            {
                tipoCambio.UsuarioModificacion = usuarioActual;
                var resultado = tiposCambioManager.UpdateTipoCambio(tipoCambio);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }



    }
}