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
    public class CiudadesController : Controller
    {
        private CiudadesManager ciudadesManager = new CiudadesManager();

        // GET: Ciudades
        public ActionResult Index(string txtNombreCiudad)
        {
            if (txtNombreCiudad == null)
            {
                return View(ciudadesManager.GetAllCiudades());
            }
            else
            {
                return View(ciudadesManager.GetAllCiudades().Where(x => (x.Nombre).ToUpper().StartsWith(txtNombreCiudad.ToUpper()) || txtNombreCiudad == null));
            }

        }

        //public ActionResult Index()
        //{

        //    return View(ciudadesManager.GetAllCiudades());
        //    //return View(ciudadesManager.GetAllCiudades().Where(x => x.Nombre.StartsWith(txtNombreCiudad) || txtNombreCiudad == null));
        //}

        //// POST: Ciudades/Create
        //[HttpPost]
        //public ActionResult Index(Ciudad ciudad)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ciudadesManager.(ciudad);
        //        return RedirectToAction("Index");
        //    }

        //    return View(ciudad);
        //}

        // GET: Ciudades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ciudad ciudad = ciudadesManager.GetCiudad(id.Value);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
            //return View();
        }

        // GET: Ciudades/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ciudades/Create
        [HttpPost]
        public ActionResult Create(Ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                //if (ciudad.Nombre != null )
                // {
                    
                    ciudadesManager.InsertCiudad(ciudad);
                    return RedirectToAction("Index");
                //}
            }

            return View(ciudad);
        }

        // GET: Ciudades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ciudad ciudad = ciudadesManager.GetCiudad(id.Value);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // POST: Ciudades/Edit/5
        [HttpPost]
        public ActionResult Edit(Ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                ciudadesManager.UpdateCiudad(ciudad);
                return RedirectToAction("Index");
            }
            return View(ciudad);
        }

        // GET: Ciudades/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Ciudades/Delete/5
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
