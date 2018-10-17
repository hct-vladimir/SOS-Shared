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
    public class PlanillasBonoAntiguedadController : Controller
    {

        ParametrosPlanillasManager parametrosPlanillasManager = new ParametrosPlanillasManager();

        // GET: PlanillasBonoAntiguedad
        public ActionResult Index()
        {
            var model = parametrosPlanillasManager.GetAllBonosAntiguedad();
            return View(model);
        }

        // GET: PlanillasBonoAntiguedad/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlanillasBonoAntiguedad/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BonoAntiguedad bonoAntiguedad)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            try
            {
                if (bonoAntiguedad != null)
                {
                    var resultado = parametrosPlanillasManager.InsertBonoAntiguedad(bonoAntiguedad);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: PlanillasBonoAntiguedad/Edit/5
        public ActionResult Edit(int id)
        {
            var model = parametrosPlanillasManager.GetBonoAntiguedad(id);

            return View(model);
        }

        // POST: PlanillasBonoAntiguedad/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, BonoAntiguedad bonoAntiguedad)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var resultado = new Resultado("");
            try
            {
                if (bonoAntiguedad != null)
                {
                    resultado = parametrosPlanillasManager.UpdateBonoAntiguedad(bonoAntiguedad);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        // GET: PlanillasBonoAntiguedad/Delete/5
        public ActionResult Delete(short id)
        {
            try
            {
                var parametro = new BonoAntiguedad { ID = id };
                var resultado = parametrosPlanillasManager.DeleteBonoAntiguedad(parametro);
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

    }
}
