using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    public class ErroresController : Controller
    {
        // GET: Errores
        public ActionResult Index(int codigo = 0)
        {
            ViewBag.CodigoError = codigo;
            switch (codigo)
            {
                case 500:
                case 505:
                    ViewBag.Title = "Se produjo un Error inesperado";
                    ViewBag.Description = "Favor intente ingresar nuevamente al sistema, si el problema persiste contáctese con el Administrador.";
                    break;

                case 404:
                    ViewBag.Title = "Página no encontrada";
                    ViewBag.Description = "La Página a la que intentó ingresar no existe";
                    break;
                case 999:
                    ViewBag.CodigoError = 403;
                    ViewBag.Title = "La Sesión expiró";
                    ViewBag.Description = "Favor ingrese nuevamente al sistema, si el problema persiste contáctese con el Administrador.";
                    break;
                default:
                    ViewBag.Title = "Se produjo un Error desconocido";
                    ViewBag.Description = "Favor intente más tarde, si el problema persiste contáctese con el Administrador.";
                    break;
            }

            return View();
        }
    }
}