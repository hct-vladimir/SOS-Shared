using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;
using Cerberus.Sos.Accounting.Security.Entities;
using Cerberus.Sos.Accounting.Security.Managers;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private UsersManager usersManager = new UsersManager();
        private SecurityManager securityManager = new SecurityManager();
        private CiudadesManager ciudadesManager = new CiudadesManager();

        // GET: Usuarios
        public ActionResult Index()
        {
            return View(usersManager.GetAllUsuarios());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User usuario = usersManager.GetUsuario(id.Value);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User usuario)
        {
            if (ModelState.IsValid)
            {
                usersManager.InsertUsuario(usuario);
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User usuario = usersManager.GetUsuario(id.Value);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User usuario)
        {
            if (ModelState.IsValid)
            {
                usersManager.UpdateUsuario(usuario);
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model, string returnUrl)
        {
            MapperManager.GetInstance();

            var validUser = securityManager.Login(model);

            if (validUser != null)
            {
                if (!validUser.Activo)
                {
                    ModelState.AddModelError("", "El usuario no se encuentra activo.");
                    return View(model);
                }

                Response.Cookies.Add(securityManager.GetAuthCookie(validUser));
                var ciudad = ciudadesManager.GetCiudad(validUser.CiudadId);
                Session["ciudadId"] = validUser.CiudadId;
                Session["ciudadNombre"] = ciudad.Nombre;

                return RedirectToAction("Index", "Home");
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            ModelState.AddModelError("", "El nombre de usuario o la contraseña especificados son incorrectos.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Usuarios");
        }
    }
}
