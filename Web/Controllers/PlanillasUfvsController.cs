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
    public class PlanillasUfvsController : Controller
    {
        ParametrosPlanillasManager parametrosPlanillasManager = new ParametrosPlanillasManager();

        // GET: PlanillasUfvs
        public ActionResult Index()
        {
            var model = parametrosPlanillasManager.GetAllRegistrosUfvs();

            return View(model);
        }


        // GET: PlanillasUfvs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlanillasUfvs/Create
        [HttpPost]
        public ActionResult Create(RegistroUfv registroUfv)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            try
            {
                if (registroUfv != null)
                {
                    var resultado = parametrosPlanillasManager.InsertRegistroUfv(registroUfv);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: PlanillasUfvs/Edit/5
        public ActionResult Edit(int id)
        {
            var model = parametrosPlanillasManager.GetRegistroUfv(id);

            return View(model);
        }

        // POST: PlanillasUfvs/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, RegistroUfv registroUfv)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var resultado = new Resultado("");
            try
            {
                if (registroUfv != null)
                {
                    resultado = parametrosPlanillasManager.UpdateRegistroUfv(registroUfv);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: PlanillasUfvs/Delete/5
        public ActionResult Delete(short id)
        {
            try
            {
                var parametro = new RegistroUfv { ID = id };
                var resultado = parametrosPlanillasManager.DeleteRegistroUfv(parametro);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

    }
}
