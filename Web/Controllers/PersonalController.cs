using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;
using PagedList;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    [Authorize]
    public class PersonalController : Controller
    {
        private PersonalManager personalManager = new PersonalManager();
        private AldeasParametrosManager aldeasParametrosManager = new AldeasParametrosManager();

        // GET: Personal
        public ActionResult Index(int? pagina, string item, string nombre, string documento, string estado, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var numeroPagina = pagina ?? 1;

            var personal = BuscarPersonal(personalManager.GetAllPersonal(), item, nombre, documento, estado, fechaDesde, fechaHasta);
            var model = new PagedList<Personal>(personal, numeroPagina, 20);
            return View(model);
        }

        private List<Personal> BuscarPersonal(List<Personal> personalBase, string item, string nombre, string documento, string estado, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var resultado = personalBase;

            if (!string.IsNullOrEmpty(item))
            {
                resultado = resultado.Where(p => p.item.Contains(item)).ToList();
            }

            if (!string.IsNullOrEmpty(nombre))
            {
                resultado = resultado.Where(c => c.nombres.Contains(nombre)).ToList();
            }

            if (!string.IsNullOrEmpty(documento))
            {
                resultado = resultado.Where(c => c.numeroDocumento.Contains(documento)).ToList();
            }

            if (!string.IsNullOrEmpty(estado))
            {
                resultado = resultado.Where(c => c.estado == estado).ToList();
            }

            if (fechaDesde != null)
            {
                resultado = resultado.Where(p => p.fechaInicioCargo != null && p.fechaInicioCargo.Value.Date >= fechaDesde.Value.Date).ToList();
            }

            if (fechaHasta != null)
            {
                resultado = resultado.Where(p => p.fechaInicioCargo != null && p.fechaInicioCargo.Value.Date <= fechaHasta.Value.Date).ToList();
            }

            return resultado.OrderBy(c => c.item).ToList();
        }


        public ActionResult Details(string item)
        {
            /* Parámetros de la BD de Aldeas */
            ViewBag.codigoTipoDocumento = new SelectList(aldeasParametrosManager.GetAllTiposDocumentos(), "codigoTipoDocumento", "descripcion");
            ViewBag.codigoDepartamento = new SelectList(aldeasParametrosManager.GetAllDepartamentos(), "codigoDepartamento", "descripcion");
            ViewBag.estadoCivil = new SelectList(aldeasParametrosManager.GetAllTiposEstadoCivil(), "codigoEstadoCivil", "Descripcion");

            //ViewBag.sexo = new SelectList(aldeasParametrosManager.GetAllGeneros(), "Value", "Text");
            ViewBag.sexo = aldeasParametrosManager.GetAllGeneros();
            ViewBag.codigoEstudio = new SelectList(aldeasParametrosManager.GetAllTiposEstudios(), "codigoEstudio", "Descripcion");
            ViewBag.codigoProfesion = new SelectList(aldeasParametrosManager.GetAllProfesiones(), "codigoProfesion", "Descripcion");

            ViewBag.codigoPrograma = new SelectList(aldeasParametrosManager.GetAllProgramas(), "codigoPrograma", "Descripcion");
            ViewBag.codigoSeccion = new SelectList(aldeasParametrosManager.GetAllSecciones(), "codigoSeccion", "Descripcion");
            ViewBag.codigoCargoActual = new SelectList(aldeasParametrosManager.GetAllCargos(), "codigoCargo", "Descripcion");
            ViewBag.tipoSeguro = new SelectList(aldeasParametrosManager.GetAllSeguros(), "id", "nombreSeguro");
            ViewBag.codigoAFP = new SelectList(aldeasParametrosManager.GetAllAfps(), "codigoAfp", "descripcion");
            ViewBag.tipoInicio = new SelectList(aldeasParametrosManager.GetAllTiposInicios(), "CodigotipoInicio", "descripcion");
            ViewBag.codigoTipoContratacion = new SelectList(aldeasParametrosManager.GetAllTiposContrataciones(), "codigoTipoContratacion", "descripcion");
            ViewBag.tiempoCompleto = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Si", Value = "Si"},
                new SelectListItem {Text = "No", Value = "No"}
            };
            ViewBag.codigoEsEvaluado = new SelectList(personalManager.GetAllPersonal(), "item", "NombreCompleto");
            ViewBag.codigoEscalaSalarial = new SelectList(aldeasParametrosManager.GetAllEscalasSalariales(), "codigoEscalaSalarial", "EscalaDescripcion");
            ViewBag.codigoRecibeInstruccion = new SelectList(personalManager.GetAllPersonal(), "item", "NombreCompleto");
            ViewBag.cotizante = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Si", Value = "1"},
                new SelectListItem {Text = "No", Value = "0"}
            };
            ViewBag.area = new SelectList(aldeasParametrosManager.GetAllAreas(), "id", "area");
            ViewBag.codigoFacility = new SelectList(aldeasParametrosManager.GetAllFacilities(), "id", "Descripcion");
            ViewBag.codigoFilial = new SelectList(aldeasParametrosManager.GetAllFiliales(), "codigoFilial", "descripcion");

            var personal = personalManager.GetPersonalByItem(item);
            return View(personal);
        }

        // GET: Personal/Edit/5
        public ActionResult Edit(string item)
        {
            /* Parámetros de la BD de Aldeas */
            ViewBag.codigoTipoDocumento = new SelectList(aldeasParametrosManager.GetAllTiposDocumentos(), "codigoTipoDocumento", "descripcion");
            ViewBag.codigoDepartamento  = new SelectList(aldeasParametrosManager.GetAllDepartamentos(), "codigoDepartamento", "descripcion");
            ViewBag.estadoCivil = new SelectList(aldeasParametrosManager.GetAllTiposEstadoCivil(), "codigoEstadoCivil", "Descripcion");

            //ViewBag.sexo = new SelectList(aldeasParametrosManager.GetAllGeneros(), "Value", "Text");
            ViewBag.sexo = aldeasParametrosManager.GetAllGeneros();
            ViewBag.codigoEstudio = new SelectList(aldeasParametrosManager.GetAllTiposEstudios(), "codigoEstudio", "Descripcion");
            ViewBag.codigoProfesion = new SelectList(aldeasParametrosManager.GetAllProfesiones(), "codigoProfesion", "Descripcion");

            ViewBag.codigoPrograma = new SelectList(aldeasParametrosManager.GetAllProgramas(), "codigoPrograma", "Descripcion");
            ViewBag.codigoSeccion = new SelectList(aldeasParametrosManager.GetAllSecciones(), "codigoSeccion", "Descripcion");
            ViewBag.codigoCargoActual = new SelectList(aldeasParametrosManager.GetAllCargos(), "codigoCargo", "Descripcion");
            ViewBag.tipoSeguro = new SelectList(aldeasParametrosManager.GetAllSeguros(), "id", "nombreSeguro");
            ViewBag.codigoAFP = new SelectList(aldeasParametrosManager.GetAllAfps(), "codigoAfp", "descripcion");
            ViewBag.tipoInicio = new SelectList(aldeasParametrosManager.GetAllTiposInicios(), "CodigotipoInicio", "descripcion");
            ViewBag.codigoTipoContratacion = new SelectList(aldeasParametrosManager.GetAllTiposContrataciones(), "codigoTipoContratacion", "descripcion");
            ViewBag.tiempoCompleto = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Si", Value = "Si"},
                new SelectListItem {Text = "No", Value = "No"}
            };
            ViewBag.codigoEsEvaluado = new SelectList(personalManager.GetAllPersonal(), "item", "NombreCompleto");
            ViewBag.codigoEscalaSalarial = new SelectList(aldeasParametrosManager.GetAllEscalasSalariales(), "codigoEscalaSalarial", "EscalaDescripcion");
            ViewBag.codigoRecibeInstruccion = new SelectList(personalManager.GetAllPersonal(), "item", "NombreCompleto");
            ViewBag.cotizante = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Si", Value = "1"},
                new SelectListItem {Text = "No", Value = "0"}
            };
            ViewBag.area = new SelectList(aldeasParametrosManager.GetAllAreas(), "id", "area");
            ViewBag.codigoFacility = new SelectList(aldeasParametrosManager.GetAllFacilities(), "id", "Descripcion");
            ViewBag.codigoFilial = new SelectList(aldeasParametrosManager.GetAllFiliales(), "codigoFilial", "descripcion");

            var personal = personalManager.GetPersonalByItem(item);
            return View(personal);
        }

        public ActionResult Ingresos(string item)
        {
            var personal = personalManager.GetPersonalPorEstado("A", "P");
            return View(personal);
        }

        public ActionResult Retiros(int? pagina, string item, string nombre, string documento, string estado, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var numeroPagina = pagina ?? 1;

            var personalBase = personalManager.GetPersonalPorEstado("A", "A");
            //.Where(p => p.fechaFinContrato != null && p.fechaFinContrato.Value.Month == DateTime.Now.Month) // Personal que termina su contrato en el mes
            var personal = BuscarPersonal(personalBase, item, nombre, documento, estado, fechaDesde, fechaHasta);
            var model = new PagedList<Personal>(personal, numeroPagina, 20);
            return View(model);
        }

        public ActionResult RetirosMesActual(int? pagina, string item, string nombre, string documento, string estado, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            var numeroPagina = pagina ?? 1;

            var personalBase =
                personalManager.GetPersonalPorEstado("A", "A")
                    .Where(p => p.fechaFinContrato != null && p.fechaFinContrato.Value.Month == DateTime.Now.Month).ToList(); // Personal que termina su contrato en el mes
            var personal = BuscarPersonal(personalBase, item, nombre, documento, estado, fechaDesde, fechaHasta);
            var model = new PagedList<Personal>(personal, numeroPagina, 20);
            return View(model);
        }

        // POST: Personal/Edit/5
        [HttpPost]
        public ActionResult Edit(Personal personal)
        {
            try
            {
                var resultado = personalManager.UpdatePersonal(personal);

                return RedirectToAction("Edit", new { item = personal.item });
            }
            catch (Exception exception)
            {
                return View();
            }
        }


        public ActionResult Alta(string item)
        {
            try
            {
                var resultado = personalManager.UpdateAltaPersonal(item);

                return RedirectToAction("Edit", new { item });
            }
            catch (Exception exception)
            {
                return RedirectToAction("Edit", new { item });
            }
        }

        public ActionResult Baja(string item)
        {

            /* Parámetros de la BD de Aldeas */
            ViewBag.codigoTipoDocumento = new SelectList(aldeasParametrosManager.GetAllTiposDocumentos(), "codigoTipoDocumento", "descripcion");
            ViewBag.codigoDepartamento = new SelectList(aldeasParametrosManager.GetAllDepartamentos(), "codigoDepartamento", "descripcion");
            ViewBag.estadoCivil = new SelectList(aldeasParametrosManager.GetAllTiposEstadoCivil(), "codigoEstadoCivil", "Descripcion");

            //ViewBag.sexo = new SelectList(aldeasParametrosManager.GetAllGeneros(), "Value", "Text");
            ViewBag.sexo = aldeasParametrosManager.GetAllGeneros();
            ViewBag.codigoEstudio = new SelectList(aldeasParametrosManager.GetAllTiposEstudios(), "codigoEstudio", "Descripcion");
            ViewBag.codigoProfesion = new SelectList(aldeasParametrosManager.GetAllProfesiones(), "codigoProfesion", "Descripcion");

            ViewBag.codigoPrograma = new SelectList(aldeasParametrosManager.GetAllProgramas(), "codigoPrograma", "Descripcion");
            ViewBag.codigoSeccion = new SelectList(aldeasParametrosManager.GetAllSecciones(), "codigoSeccion", "Descripcion");
            ViewBag.codigoCargoActual = new SelectList(aldeasParametrosManager.GetAllCargos(), "codigoCargo", "Descripcion");
            ViewBag.tipoSeguro = new SelectList(aldeasParametrosManager.GetAllSeguros(), "id", "nombreSeguro");
            ViewBag.codigoAFP = new SelectList(aldeasParametrosManager.GetAllAfps(), "codigoAfp", "descripcion");
            ViewBag.tipoInicio = new SelectList(aldeasParametrosManager.GetAllTiposInicios(), "CodigotipoInicio", "descripcion");
            ViewBag.codigoTipoContratacion = new SelectList(aldeasParametrosManager.GetAllTiposContrataciones(), "codigoTipoContratacion", "descripcion");
            ViewBag.tiempoCompleto = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Si", Value = "Si"},
                new SelectListItem {Text = "No", Value = "No"}
            };
            ViewBag.codigoEsEvaluado = new SelectList(personalManager.GetAllPersonal(), "item", "NombreCompleto");
            ViewBag.codigoEscalaSalarial = new SelectList(aldeasParametrosManager.GetAllEscalasSalariales(), "codigoEscalaSalarial", "EscalaDescripcion");
            ViewBag.codigoRecibeInstruccion = new SelectList(personalManager.GetAllPersonal(), "item", "NombreCompleto");
            ViewBag.cotizante = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Si", Value = "1"},
                new SelectListItem {Text = "No", Value = "0"}
            };
            ViewBag.area = new SelectList(aldeasParametrosManager.GetAllAreas(), "id", "area");
            ViewBag.codigoFacility = new SelectList(aldeasParametrosManager.GetAllFacilities(), "id", "Descripcion");
            ViewBag.codigoFilial = new SelectList(aldeasParametrosManager.GetAllFiliales(), "codigoFilial", "descripcion");

            // Paramétricas de Tabla Baja de Personal
            ViewBag.bajaPlanillas = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Si", Value = "Si"},
                new SelectListItem {Text = "No", Value = "No"}
            };

            ViewBag.certificadoTrabajo = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Si", Value = "Si"},
                new SelectListItem {Text = "No", Value = "No"}
            };

            ViewBag.beneficiosSociales = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Si", Value = "Si"},
                new SelectListItem {Text = "No", Value = "No"}
            };

            ViewBag.codigoMotivoRetiro = new SelectList(aldeasParametrosManager.GetAllTiposRetiros(), "codigoTiporetiro", "descripcion");

            var personal = personalManager.GetPersonalByItem(item);
            return View(personal);
        }

        [HttpPost]
        public ActionResult Baja(PersonalBaja personalBaja)
        {
            try
            {
                var usuarioActual = HttpContext.User.Identity.Name;
                personalBaja.login = usuarioActual;
                var resultado = personalManager.CreateBajaPersonal(personalBaja);

                return RedirectToAction("Retiros");
            }
            catch (Exception exception)
            {
                return RedirectToAction("Retiros");
            }
        }
    }
}
