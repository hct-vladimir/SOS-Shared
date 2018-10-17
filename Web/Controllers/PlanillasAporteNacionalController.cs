using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Entities.ParametrosPlanillas;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    public class PlanillasAporteNacionalController : Controller
    {
        ParametrosPlanillasManager parametrosPlanillasManager = new ParametrosPlanillasManager();

        // GET: PlanillasAporteNacional
        public ActionResult Index()
        {
            var model = parametrosPlanillasManager.GetAllAportesNacionales();
            return View(model);
        }

        // GET: PlanillasAporteNacional/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlanillasAporteNacional/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AporteNacional aporteNacional)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            try
            {
                if (aporteNacional != null)
                {
                    var resultado = parametrosPlanillasManager.InsertAporteNacional(aporteNacional);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: PlanillasAporteNacional/Edit/5
        public ActionResult Edit(int id)
        {
            var model = parametrosPlanillasManager.GetAporteNacional(id);

            return View(model);
        }

        // POST: PlanillasAporteNacional/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, AporteNacional aporteNacional)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var resultado = new Resultado("");
            try
            {
                if (aporteNacional != null)
                {
                    resultado = parametrosPlanillasManager.UpdateAporteNacional(aporteNacional);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: PlanillasAporteNacional/Delete/5
        public ActionResult Delete(short id)
        {
            try
            {
                var parametro = new AporteNacional { ID = id };
                var resultado = parametrosPlanillasManager.DeleteAporteNacional(parametro);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

    }
}
