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
    [Authorize]
    public class FacilitiesController : Controller
    {
        private FacilitiesManager facilitiesManager = new FacilitiesManager();

        // GET: Facilities
        public ActionResult Index(String txtNombreFacility)
        {
            ViewBag.Ciudades = ObtenerCiudades(); //PARA EL ENVIO DEL CONTROLADOR A LA VISTA
            if (txtNombreFacility == null)
            {

                return View(facilitiesManager.GetAllFacilities());
            }
            else
            {
                return View(facilitiesManager.GetAllFacilities().Where(x =>(x.Nombre).ToUpper().StartsWith(txtNombreFacility.ToUpper()) || txtNombreFacility == null));
            }

        }

        public List<Ciudad> ObtenerCiudades()
        {
            CiudadesManager ciudadesManager = new CiudadesManager();
              return ciudadesManager.GetAllCiudades();//ciudadesManager.GetAllCiudades();
        }

        // GET: Facilities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = facilitiesManager.GetFacility(id.Value);
            //facility.CiudadList = ObtenerCiudades();
            ViewBag.CiudadList = ObtenerCiudades();
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        // GET: Facilities/Create
        public ActionResult Create()
        {
            Facility facility = new Facility();
            ViewBag.CiudadList = ObtenerCiudades();
            ViewBag.listaCiudades = ObtenerCiudades();

            return View(facility);
        }

        // POST: Facilities/Create
        [HttpPost]
        public ActionResult Create(Facility facility)
        {
            if (ModelState.IsValid)
            {
                facilitiesManager.InsertFacility(facility);
                return RedirectToAction("Index");
            }
             ViewBag.listaCiudades = ObtenerCiudades();
            return View(facility);
        }

        // GET: Facilities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = facilitiesManager.GetFacility(id.Value);
            ViewBag.CiudadList = ObtenerCiudades();
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        // POST: Facilities/Edit/5
        [HttpPost]
        public ActionResult Edit(Facility facility)
        {
            if (ModelState.IsValid)
            {
                facilitiesManager.UpdateFacility(facility);
                return RedirectToAction("Index");
            }
            ViewBag.CiudadList = ObtenerCiudades();
            return View(facility);
        }

        // GET: Facilities/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Facilities/Delete/5
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
