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
    public class PlanillasSalariosMinimosController : Controller
    {
        ParametrosPlanillasManager parametrosPlanillasManager = new ParametrosPlanillasManager();

        // GET: PlanillasSalariosMinimos
        public ActionResult Index()
        {
            var model = parametrosPlanillasManager.GetAllSalariosMinimos();
            return View(model);
        }

        // GET: PlanillasSalariosMinimos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlanillasSalariosMinimos/Create
        [HttpPost]
        public ActionResult Create(SalarioMinimo salarioMinimo)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            try
            {
                if (salarioMinimo != null)
                {
                    var resultado = parametrosPlanillasManager.InsertSalarioMinimo(salarioMinimo);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: PlanillasSalariosMinimos/Edit/5
        public ActionResult Edit(int id)
        {
            var model = parametrosPlanillasManager.GetSalarioMinimo(id);

            return View(model);
        }

        // POST: PlanillasSalariosMinimos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, SalarioMinimo salarioMinimo)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var resultado = new Resultado("");
            try
            {
                if (salarioMinimo != null)
                {
                    resultado = parametrosPlanillasManager.UpdateSalarioMinimo(salarioMinimo);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: PlanillasSalariosMinimos/Delete/5
        public ActionResult Delete(short id)
        {
            try
            {
                var parametro = new SalarioMinimo { ID = id };
                var resultado = parametrosPlanillasManager.DeleteSalarioMinimo(parametro);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

    }
}
