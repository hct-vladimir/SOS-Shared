using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    public class PlantillasAsientosController : Controller
    {
        private PlantillasAsientosManager plantillasAsientosManager = new PlantillasAsientosManager();
        private PlantillasCuentasManager plantillasCuentasManager = new PlantillasCuentasManager();
        private CuentasAsientosManager cuentasAsientosManager = new CuentasAsientosManager();

        // GET: PlantillasAsientos
        public ActionResult Index()
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var plantillasAsientos = plantillasAsientosManager.GetPlantillasAsientosPorUsuario(usuarioActual);

            return View(plantillasAsientos);
        }

        // GET: PlantillasAsientos/Details/5
        public ActionResult Details(int id)
        {
            var plantillaAsiento = plantillasAsientosManager.GetPlantillaAsiento(id);

            ViewBag.NombrePlantilla = plantillaAsiento.Nombre;
            ViewBag.DescripcionPlantilla = plantillaAsiento.Descripcion;

            //Cuentas de la Plantilla
            var plantillasCuentas = plantillasCuentasManager.GetPlantillasCuentasPorAsiento(id);
            var plantillasCuentasDebe = plantillasCuentas.Where(c => c.Debe > 0 && c.Activo).OrderBy(c => c.CuentaContable.Numero);
            var plantillasCuentasHaber = plantillasCuentas.Where(c => c.Haber > 0 && c.Activo).OrderBy(c => c.CuentaContable.Numero);

            plantillasCuentas = plantillasCuentasDebe.Union(plantillasCuentasHaber).ToList();
            return View(plantillasCuentas);
        }

        public ActionResult Generate(int plantillaAsientoId, int comprobanteId)
        {
            var plantillasCuentas = plantillasCuentasManager.GetPlantillasCuentasPorAsiento(plantillaAsientoId);
            var resultado = cuentasAsientosManager.GenerarAsiento(plantillasCuentas, comprobanteId);
            
            return RedirectToAction("Index", "CuentasAsientos", new { id = comprobanteId });
        }

        // GET: PlantillasAsientos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlantillasAsientos/Create
        [HttpPost]
        public ActionResult Create(PlantillaAsiento plantillaAsiento)
        {
            try
            {
                var usuarioActual = HttpContext.User.Identity.Name;
                plantillaAsiento.UsuarioCreacion = usuarioActual;
                plantillaAsiento.UsuarioModificacion = usuarioActual;

                plantillasAsientosManager.InsertPlantillaAsiento(plantillaAsiento);

                return RedirectToAction("Index", "CuentasAsientos", new { id = plantillaAsiento.ComprobanteId });
            }
            catch
            {
                return View();
            }
        }

        // GET: PlantillasAsientos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlantillasAsientos/Edit/5
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

        public ActionResult Delete(int id)
        {
            try
            {
                var usuarioActual = HttpContext.User.Identity.Name;
                var resultado = plantillasAsientosManager.DeletePlantillaAsiento(id, usuarioActual);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
