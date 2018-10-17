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
    public class ContrapartesController : Controller
    {
        private ContrapartesManager contrapartesManager = new ContrapartesManager();
        // GET: Contrapartes
        public ActionResult Index(string txtNombreContraparte)
        {
            if (txtNombreContraparte == null)
            {
                return View(contrapartesManager.GetAllContrapartes());
            }
            else
            {
                return View(contrapartesManager.GetAllContrapartes().Where(x => ((x.Nombre).ToUpper()).Contains(txtNombreContraparte.ToUpper()) || txtNombreContraparte == null));
            }

        }

        // GET: Contrapartes/Details/5
        public ActionResult Details(int? id)
        {
            // return View();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contraparte contraparte = contrapartesManager.GetContraparte(id.Value);
            if (contraparte == null)
            {
                return HttpNotFound();
            }
            return View(contraparte);
        }

        // GET: Contrapartes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contrapartes/Create
        [HttpPost]
        public ActionResult Create(Contraparte contraparte)
        {
            if (ModelState.IsValid)
            {
                contrapartesManager.InsertContraparte(contraparte);
                return RedirectToAction("Index");
            }

            return View(contraparte);
        }

        // GET: Contrapartes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contraparte contraparte = contrapartesManager.GetContraparte(id.Value);
            if (contraparte == null)
            {
                return HttpNotFound();
            }
            return View(contraparte);
        }

        // POST: Contrapartes/Edit/5
        [HttpPost]
        public ActionResult Edit(Contraparte contraparte)
        {
            if (ModelState.IsValid)
            {
                contrapartesManager.UpdateContraparte(contraparte);
                return RedirectToAction("Index");
            }
            return View(contraparte);
        }

      
    }
}
