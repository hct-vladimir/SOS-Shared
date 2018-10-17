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
    public class PlanillasParametrosController : Controller
    {
        ParametrosPlanillasManager parametrosPlanillasManager = new ParametrosPlanillasManager();


        // GET: PlanillasParametros
        public ActionResult Index()
        {
            var model = parametrosPlanillasManager.GetAllParametros();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Parametro parametro)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            try
            {
                if (parametro != null)
                {
                    var resultado = parametrosPlanillasManager.InsertParametro(parametro);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int id)
        {
            var model = parametrosPlanillasManager.GetParametro(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Parametro parametro)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var resultado = new Resultado("");
            try
            {
                if (parametro!= null)
                {
                    resultado = parametrosPlanillasManager.UpdateParametro(parametro);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(short id)
        {
            try
            {
                var parametro = new Parametro {ID = id};
                var resultado = parametrosPlanillasManager.DeleteParametro(parametro);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}