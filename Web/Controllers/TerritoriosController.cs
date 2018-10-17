using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Security;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    public class TerritoriosController : Controller
    {
        private TerritoriosManager territoriosManager = new TerritoriosManager();

        // GET: Territorios
        public ActionResult Index(String txtNombreTerritorio)
        {
            ViewBag.CiudadList = ObtenerCiudades(); //PARA EL ENVIO DEL CONTROLADOR A LA VISTA
            if (txtNombreTerritorio == null)
            {

                return View(territoriosManager.GetAllTerritorios());
            }
            else
            {
                return View(territoriosManager.GetAllTerritorios().Where(x => (x.Nombre).ToUpper().StartsWith(txtNombreTerritorio.ToUpper()) || txtNombreTerritorio == null));
            }


            //ViewBag.Ciudades = ObtenerCiudades();
            //    return View(territoriosManager.GetAllTerritorios());
         
        }

        // GET: Territorios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Territorio territorio = territoriosManager.GetTerritorio(id.Value);
            ViewBag.CiudadList = ObtenerCiudades();
            if (territorio == null)
            {
                return HttpNotFound();
            }
            return View(territorio);
        }

        // GET: Territorios/Create
        [HttpGet]
        public ActionResult Create(int id=0)
        {
            Territorio terr = new Territorio();
            //ViewBag.CiudadList = ObtenerCiudades();
            ViewBag.listaCiudades = ObtenerCiudades();

            return View(terr);
        }

        public List<Ciudad> ObtenerCiudades()
        {
            CiudadesManager ciudadesManager = new CiudadesManager();
            //List<SelectListItem> listaCiudades = new List<SelectListItem>();
            //foreach (var item in ciudadesManager.GetAllCiudades().ToList())
            //{
            //    listaCiudades.Add(new SelectListItem {Text=item.Nombre,Value=item.Id.ToString() });
            //}
            return ciudadesManager.GetAllCiudades();//ciudadesManager.GetAllCiudades();
        }


        // POST: Territorios/Create
        [HttpPost]
        public ActionResult Create(Territorio territorio)
        {
            if (ModelState.IsValid)
            {
                territoriosManager.InsertTerritorio(territorio);
                return RedirectToAction("Index");
            }
            ViewBag.listaCiudades = ObtenerCiudades();
            return View(territorio);
        }

        // GET: Territorios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Territorio territorio = territoriosManager.GetTerritorio(id.Value);
            ViewBag.CiudadList = ObtenerCiudades();
            if (territorio == null)
            {
                return HttpNotFound();
            }
            return View(territorio);
        }

        // POST: Territorios/Edit/5
        [HttpPost]
        public ActionResult Edit(Territorio territorio)
        {
            if (ModelState.IsValid)
            {
                territoriosManager.UpdateTerritorio(territorio);
                return RedirectToAction("Index");
            }
            ViewBag.CiudadList = ObtenerCiudades();
            return View(territorio);
        }

        // GET: Territorios/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Territorios/Delete/5
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
