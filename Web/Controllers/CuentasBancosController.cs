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
    public class CuentasBancosController : Controller
    {
        private CuentasBancosManager cuentasBancosManager = new CuentasBancosManager();

        // GET: CuentasBancos
        public ActionResult Index(String txtNumeroCuentaBanco)
        {
            ViewBag.Bancos = ObtenerBancos(); //PARA EL ENVIO DEL CONTROLADOR A LA VISTA
            ViewBag.Facilities = ObtenerFacilities();
            if (txtNumeroCuentaBanco == null)
            {

                return View(cuentasBancosManager.GetAllCuentasBancos());
            }
            else
            {
                return View(cuentasBancosManager.GetAllCuentasBancos().Where(x => (x.NumeroCuenta).ToUpper().StartsWith(txtNumeroCuentaBanco.ToUpper()) || txtNumeroCuentaBanco == null));
            }

        }

        public List<Banco> ObtenerBancos()
        {
            BancosManager bancosManager = new BancosManager();
            return bancosManager.GetAllBancos();
        }

        public List<Facility> ObtenerFacilities()
        {
            FacilitiesManager facilitiesManager = new FacilitiesManager();
            return facilitiesManager.GetAllFacilities();
        }

        // GET: CuentasBancos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuentaBanco cuentasBanco = cuentasBancosManager.GetCuentasBanco(id.Value);
            ViewBag.BancoList = ObtenerBancos();
            ViewBag.FacilityList = ObtenerFacilities();

            if (cuentasBanco == null)
            {
                return HttpNotFound();
            }
            return View(cuentasBanco);
        }

        // GET: CuentasBancos/Create
        public ActionResult Create()
        {
            CuentaBanco cuentasBanco = new CuentaBanco();
            ViewBag.BancoList = ObtenerBancos();
            ViewBag.FacilityList = ObtenerFacilities();

            return View(cuentasBanco);
        }

        // POST: CuentasBancos/Create
        [HttpPost]
        public ActionResult Create(CuentaBanco cuentasBanco)
        {
            if (ModelState.IsValid)
            {
                cuentasBancosManager.InsertCuentasBanco(cuentasBanco);
                return RedirectToAction("Index");
            }
            ViewBag.BancoList = ObtenerBancos();
            ViewBag.FacilityList = ObtenerFacilities();
            return View(cuentasBanco);
        }

        // GET: CuentasBancos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuentaBanco cuentasBanco = cuentasBancosManager.GetCuentasBanco(id.Value);
            ViewBag.BancoList = ObtenerBancos();
            ViewBag.FacilityList = ObtenerFacilities();
            if (cuentasBanco == null)
            {
                return HttpNotFound();
            }
            return View(cuentasBanco);
        }

        // POST: CuentasBancos/Edit/5
        [HttpPost]
        public ActionResult Edit(CuentaBanco cuentasBanco)
        {
            if (ModelState.IsValid)
            {
                cuentasBancosManager.UpdateCuentasBanco(cuentasBanco);
                return RedirectToAction("Index");
            }
            ViewBag.BancoList = ObtenerBancos();
            ViewBag.FacilityList = ObtenerFacilities();
            return View(cuentasBanco);
        }

        // GET: CuentasBancos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CuentasBancos/Delete/5
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
