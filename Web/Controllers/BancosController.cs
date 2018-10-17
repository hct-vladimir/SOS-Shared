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
    public class BancosController : Controller
    {
        private BancosManager bancosManager = new BancosManager();
        // GET: Bancos
        public ActionResult Index(string txtNombreBanco)
        {
            if (txtNombreBanco == null)
            {
                return View(bancosManager.GetAllBancos());
            }
            else
            {
                return View(bancosManager.GetAllBancos().Where(x => (x.Nombre).ToUpper().Contains(txtNombreBanco.ToUpper()) || txtNombreBanco == null));
            }
            // return View(bancosManager.GetAllBancos());
        }

        // GET: Bancos/Details/5
        public ActionResult Details(int? id)
        {
            //return View();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banco banco = bancosManager.GetBanco(id.Value);
            if (banco == null)
            {
                return HttpNotFound();
            }
            return View(banco);
        }

        // GET: Bancos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bancos/Create
        [HttpPost]
        public ActionResult Create(Banco banco)
        {
            if (ModelState.IsValid)
            {
                bancosManager.InsertBanco(banco);
                return RedirectToAction("Index");
            }

            return View(banco);
        }

        // GET: Bancos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banco banco= bancosManager.GetBanco(id.Value);
            if (banco == null)
            {
                return HttpNotFound();
            }
            return View(banco);
        }

        // POST: Bancos/Edit/5
        [HttpPost]
        public ActionResult Edit(Banco banco)
        {
            if (ModelState.IsValid)
            {
                bancosManager.UpdateBanco(banco);
                return RedirectToAction("Index");
            }
            return View(banco);
        }

        // GET: Bancos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Bancos/Delete/5
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
