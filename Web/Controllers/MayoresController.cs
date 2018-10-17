using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    public class MayoresController : Controller
    {
        private CuentasAsientosManager cuentasAsientosManager = new CuentasAsientosManager();

        // GET: Mayores
        public ActionResult Index(string numeroCuenta, string mes, string gestion)
        {
            var cuentas = new List<CuentaAsiento>();
            if (string.IsNullOrEmpty(numeroCuenta))
            {
                cuentas = cuentasAsientosManager.GetMayoresCuentasAsientos();
            }
            else
            {
                cuentas = cuentasAsientosManager.GetMayoresByCuenta(numeroCuenta);
            }
            return View(cuentas);
        }

    }
}
