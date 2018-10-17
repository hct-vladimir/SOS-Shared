using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AccountingSos.Models;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    public class AccionesNacionalesController : Controller
    {
        private AccionesNacionalesManager accionesNacionalesManager = new AccionesNacionalesManager();

        // GET: AccionesNacionales
        public ActionResult Index(string txtCodigo, string txtDescripcion)
        {
           
            if (txtCodigo == null && txtDescripcion == null)
            {
                return View(accionesNacionalesManager.GetAllAccionesNacionales());
            }
            else
            {
                if (String.IsNullOrEmpty(txtCodigo) && !String.IsNullOrEmpty(txtDescripcion))
                {
                    return View(accionesNacionalesManager.GetAllAccionesNacionales().Where(x => ((x.Descripcion).ToUpper()).Contains(txtDescripcion.ToUpper()) || txtDescripcion == null));
                }
                else
                {
                    if (!String.IsNullOrEmpty(txtCodigo) && String.IsNullOrEmpty(txtDescripcion))
                    {
                        return View(accionesNacionalesManager.GetAllAccionesNacionales().Where(x => ((x.Codigo).ToUpper()).Contains(txtCodigo.ToUpper()) || txtCodigo == null));
                    }
                    else
                    {
                        return View(accionesNacionalesManager.GetAllAccionesNacionales().Where(x => ((x.Codigo).ToUpper()).Contains(txtCodigo.ToUpper()) && ((x.Descripcion).ToUpper()).Contains(txtDescripcion.ToUpper())));
                    }
                }




            }
        }

        // GET: AccionesNacionales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccionesNacionale accionNacional = accionesNacionalesManager.GetAccionNacional(id.Value);
            if (accionNacional == null)
            {
                return HttpNotFound();
            }
            return View(accionNacional);
        }

        // GET: AccionesNacionales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccionesNacionales/Create
        [HttpPost]
        public ActionResult Create(AccionesNacionale accionNacional)
        {
            if (ModelState.IsValid)
            {
                accionesNacionalesManager.InsertAccionNacional(accionNacional);
                return RedirectToAction("Index");
            }

            return View(accionNacional);
        }

        // GET: AccionesNacionales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccionesNacionale accionNacional = accionesNacionalesManager.GetAccionNacional(id.Value);
            if (accionNacional == null)
            {
                return HttpNotFound();
            }
            return View(accionNacional);
        }

        // POST: AccionesNacionales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(AccionesNacionale accionNacional)
        {
            if (ModelState.IsValid)
            {
                accionesNacionalesManager.UpdateAccionNacional(accionNacional);
                return RedirectToAction("Index");
            }
            return View(accionNacional);
        }
    }
}
