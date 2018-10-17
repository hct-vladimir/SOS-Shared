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
    public class CodigosAuditoriasController : Controller
    {
        private CodigosAuditoriasManager codigosAuditoriasManager = new CodigosAuditoriasManager();

        // GET: CodigosAuditorias
        public ActionResult Index(string txtCodigo, string txtDescripcion)
        {
            if (txtCodigo == null && txtDescripcion == null)
            {
                return View(codigosAuditoriasManager.GetAllCodigosAuditoria());
            }
            else
            {
                if (String.IsNullOrEmpty(txtCodigo)  && !String.IsNullOrEmpty(txtDescripcion))
                {
                    return View(codigosAuditoriasManager.GetAllCodigosAuditoria().Where(x => ((x.Descripcion).ToUpper()).Contains(txtDescripcion.ToUpper()) || txtDescripcion == null));
                }
                else
                {
                    if (!String.IsNullOrEmpty(txtCodigo) && String.IsNullOrEmpty(txtDescripcion))
                    {
                        return View(codigosAuditoriasManager.GetAllCodigosAuditoria().Where(x => ((x.Codigo).ToUpper()).Contains(txtCodigo.ToUpper()) || txtCodigo == null));
                    }
                    else
                    {   return View(codigosAuditoriasManager.GetAllCodigosAuditoria().Where(x => ((x.Codigo).ToUpper()).Contains(txtCodigo.ToUpper()) && ((x.Descripcion).ToUpper()).Contains(txtDescripcion.ToUpper())));
                    }
                }



            }
        }

        // GET: CodigosAuditorias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodigosAuditoria codigosAuditoria = codigosAuditoriasManager.GetCodigoAuditoria(id.Value);
            if (codigosAuditoria == null)
            {
                return HttpNotFound();
            }
            return View(codigosAuditoria);
        }

        // GET: CodigosAuditorias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CodigosAuditorias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CodigosAuditoria codigosAuditoria)
        {
            if (ModelState.IsValid)
            {
                codigosAuditoriasManager.InsertCodigoAuditoria(codigosAuditoria);
                return RedirectToAction("Index");
            }

            return View(codigosAuditoria);
        }

        // GET: CodigosAuditorias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodigosAuditoria codigosAuditoria = codigosAuditoriasManager.GetCodigoAuditoria(id.Value);
            if (codigosAuditoria == null)
            {
                return HttpNotFound();
            }
            return View(codigosAuditoria);
        }

        // POST: CodigosAuditorias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CodigosAuditoria codigosAuditoria)
        {
            if (ModelState.IsValid)
            {
                codigosAuditoriasManager.UpdateCodigoAuditoria(codigosAuditoria);
                return RedirectToAction("Index");
            }
            return View(codigosAuditoria);
        }
    }
}
