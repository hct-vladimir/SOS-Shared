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
    public class AnexosTributariosController : Controller
    {
        private AnexosTributariosManager anexosTributariosManager = new AnexosTributariosManager();

        // GET: AnexosTributarios
        public ActionResult Index(string txtCodigo, string txtDescripcion, string txtGrupo)
        {
            if (txtCodigo == null && txtDescripcion == null && txtGrupo== null)
            {
                return View(anexosTributariosManager.GetAllAnexosTributarios());
            }
            else
            {
                //primer nivel
                if (!String.IsNullOrEmpty(txtCodigo) && String.IsNullOrEmpty(txtDescripcion) && String.IsNullOrEmpty(txtGrupo))
                {
                    return View(anexosTributariosManager.GetAllAnexosTributarios().Where(x => ((x.Codigo).ToUpper()).Contains(txtCodigo.ToUpper()) || txtCodigo == null));

                }
                else {
                    if (String.IsNullOrEmpty(txtCodigo) && !String.IsNullOrEmpty(txtDescripcion) && String.IsNullOrEmpty(txtGrupo))
                    {
                        return View(anexosTributariosManager.GetAllAnexosTributarios().Where(x => ((x.Descripcion).ToUpper()).Contains(txtDescripcion.ToUpper()) || txtDescripcion == null));
                    }
                    else {
                        if (String.IsNullOrEmpty(txtCodigo) && String.IsNullOrEmpty(txtDescripcion) && !String.IsNullOrEmpty(txtGrupo))
                        {
                            return View(anexosTributariosManager.GetAllAnexosTributarios().Where(x => ((x.Grupo).ToUpper()).Contains(txtGrupo.ToUpper()) || txtGrupo == null));
                        }
                        else {
                            //2 nivel
                            if (!String.IsNullOrEmpty(txtCodigo) && !String.IsNullOrEmpty(txtDescripcion) && String.IsNullOrEmpty(txtGrupo))
                            {
                                return View(anexosTributariosManager.GetAllAnexosTributarios().Where(x => ((x.Codigo).ToUpper()).Contains(txtCodigo.ToUpper()) && ((x.Descripcion).ToUpper()).Contains(txtDescripcion.ToUpper())));
                            }
                            else {
                                if (!String.IsNullOrEmpty(txtCodigo) && String.IsNullOrEmpty(txtDescripcion) && !String.IsNullOrEmpty(txtGrupo))
                                {
                                    return View(anexosTributariosManager.GetAllAnexosTributarios().Where(x => ((x.Codigo).ToUpper()).Contains(txtCodigo.ToUpper()) && ((x.Grupo).ToUpper()).Contains(txtGrupo.ToUpper())));
                                }
                                else {
                                    if (String.IsNullOrEmpty(txtCodigo) && !String.IsNullOrEmpty(txtDescripcion) && !String.IsNullOrEmpty(txtGrupo))
                                    {
                                        return View(anexosTributariosManager.GetAllAnexosTributarios().Where(x => ((x.Descripcion).ToUpper()).Contains(txtDescripcion.ToUpper()) && ((x.Grupo).ToUpper()).Contains(txtGrupo.ToUpper())));
                                    }
                                    else {
                                        //3er nivel
                                        return View(anexosTributariosManager.GetAllAnexosTributarios().Where(x => ((x.Codigo).ToUpper()).Contains(txtCodigo.ToUpper()) && ((x.Grupo).ToUpper()).Contains(txtGrupo.ToUpper()) && ((x.Grupo).ToUpper()).Contains(txtGrupo.ToUpper())));
                                    }
                                }
                            }
                        }

                    }


                }



            }
        }

        // GET: AnexosTributarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnexosTributario anexoTributario = anexosTributariosManager.GetAnexoTributario(id.Value);
            if (anexoTributario == null)
            {
                return HttpNotFound();
            }
            return View(anexoTributario);
        }

        // GET: AnexosTributarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AnexosTributarios/Create

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Create(AnexosTributario anexoTributario)
        {
            if (ModelState.IsValid)
            {
                anexosTributariosManager.InsertAnexoTributario(anexoTributario);
                return RedirectToAction("Index");
            }

            return View(anexoTributario);
         }

        // GET: AnexosTributarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnexosTributario anexosTributario = anexosTributariosManager.GetAnexoTributario(id.Value);
            if (anexosTributario == null)
            {
                return HttpNotFound();
            }
            return View(anexosTributario);
        }

        // POST: AnexosTributarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(AnexosTributario anexosTributario)
        {
            if (ModelState.IsValid)
            {
                anexosTributariosManager.UpdateAnexoTributario(anexosTributario);
                return RedirectToAction("Index");
            }
            return View(anexosTributario);
        }
    }
}
