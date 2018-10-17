using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.BusinessLogic.Managers;
using HttpContextExtensions = System.Web.WebPages.HttpContextExtensions;

namespace Cerberus.Sos.Accounting.Web.Controllers
{
    [Authorize]
    public class CuentasAsientosController : Controller
    {
        private CuentasAsientosManager cuentasAsientosManager = new CuentasAsientosManager();
        private TraspasosProgramasManager traspasosProgramasManager = new TraspasosProgramasManager();
        private ComprobantesManager comprobantesManager = new ComprobantesManager();
        private CuentasContablesManager cuentasContablesManager = new CuentasContablesManager();
        private PlanProgramaticoManager planProgramaticoManager = new PlanProgramaticoManager();
        private TiposCambioManager tiposCambioManager = new TiposCambioManager();
        private PlantillasAsientosManager plantillasAsientosManager = new PlantillasAsientosManager();
        private ObservacionesManager observacionesManager = new ObservacionesManager();
        private EstadosCuentasManager estadosCuentasManager = new EstadosCuentasManager();
        private FacilitiesManager facilitiesManager = new FacilitiesManager();

        // GET: CuentasAsientos
        public ActionResult Index(int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Comprobantes");
            }

            var comprobante = comprobantesManager.GetComprobante(id);
            if (comprobante != null)
            {
                ViewBag.ComprobanteId = comprobante.Id;
                ViewBag.FechaComprobante = comprobante.FechaComprobante.ToString("dd'/'MM'/'yyyy");
                ViewBag.NumeroComprobante = comprobante.NumeroComprobante;
                ViewBag.Facility = string.Format("{0} - {1}", comprobante.Facility.Codigo, comprobante.Facility.Nombre); // "R0031514 - PAF CV";
                ViewBag.FacilityId = comprobante.FacilityId;
                ViewBag.TipoComprobanteId = comprobante.TipoComprobanteId;
                ViewBag.TipoComprobante = comprobante.TiposComprobante.Nombre;
                ViewBag.FichaBanco = comprobante.CuentaBanco != null ? comprobante.CuentaBanco.FichaBanco : string.Empty; // "4020BK1177-C1108001-MAESTRO CARE EL ALTO";
                ViewBag.TipoMoneda = comprobante.TiposMoneda.Moneda;
                var tipoCambioActual = tiposCambioManager.GetTiposCambioByFecha(DateTime.Now.Date).FirstOrDefault(tc => tc.TipoMonedaId == 2);
                ViewBag.TipoCambio = tipoCambioActual != null ? tipoCambioActual.Valor.ToString("N2") : "0";

                ViewBag.TotalDebe = comprobante.CuentasAsientos.Where(c => c.Activo).Sum(c => c.Debe);
                ViewBag.TotalHaber = comprobante.CuentasAsientos.Where(c => c.Activo).Sum(c => c.Haber);

                ViewBag.FacilityActual = comprobante.Facility;
                ViewBag.EstadoComprobante = comprobante.EstadosComprobante.Nombre;
                ViewBag.CiudadIdComprobante = comprobante.CiudadId;
            }

            ViewBag.PlanProgramaticoId = new SelectList(planProgramaticoManager.GetAllPlan(), "Id", "NombreDespliegue");
            ViewBag.CuentaContableId = new SelectList(cuentasContablesManager.GetAllCuentasContables(), "Id", "NombreDespliegue");

            // Parametricas
            ViewBag.TerritorioId = new SelectList(new TerritoriosManager().GetAllTerritorios(), "Id", "NombreDespliegue");
            ViewBag.ContraparteId = new SelectList(new ContrapartesManager().GetAllContrapartes(), "Id", "Nombre");
            ViewBag.AnexoTributarioId = new SelectList(new AnexosTributariosManager().GetAllAnexosTributarios(), "Id", "Descripcion");
            ViewBag.CodigoAuditoriaId = new SelectList(new CodigosAuditoriasManager().GetAllCodigosAuditoria(), "Id", "Descripcion");
            ViewBag.AccionNacionalId = new SelectList(new AccionesNacionalesManager().GetAllAccionesNacionales(), "Id", "NombreDespliegue");
            ViewBag.MarcoLogicoId = new SelectList(new CodigosAuditoriasManager().GetAllCodigosMarcoLogico(), "Id", "Codigo");

            //////Transferencias
            ////var comprobantesANIds = traspasosProgramasManager.GetAllTraspasosProgramas().Where(c => c.CuentaTransitoriaNumero == "88110" && c.ComprobanteProgramaId == null).Select(c => c.ComprobanteProgramaId).ToList();

            ////var comprobantesPendientesANIds = comprobantesManager.GetComprobantesPorFacility(comprobante.FacilityId).Where(c => comprobantesANIds.Contains(c.Id)).Select(c => c.Id).ToList();

            ////var cuentasTraspasosManutencion = cuentasAsientosManager.GetAllCuentasAsientos().Where(c => comprobantesPendientesANIds.Contains(c.ComprobanteId) && c.CuentaContable.Numero == "88110").ToList();

            ////ViewBag.TransfManutencion = cuentasTraspasosManutencion;
            ////ViewBag.TransfManutencionTotalDebe = cuentasTraspasosManutencion.Sum(c => c.Debe);
            ////ViewBag.TransfManutencionTotalHaber = cuentasTraspasosManutencion.Sum(c => c.Haber);

            //Plantillas de Asientos del Usuario Actual
            var usuarioActual = HttpContext.User.Identity.Name;
            var plantillasAsientos = plantillasAsientosManager.GetPlantillasAsientosPorUsuario(usuarioActual);
            ViewBag.PlantillasAsientos = plantillasAsientos;

            //Observaciones
            var observaciones = observacionesManager.GetObservacionesPorEntidadFacility(2, comprobante.Id, comprobante.Facility.Id);
            if (HttpContext.User.IsInRole("ADMN-PRG"))
            {
                observaciones = observaciones.Where(o => !o.Aprobado).ToList();
                ViewBag.ExisteObservacion = observaciones.Any();
            }

            var historialObservaciones = observaciones.Aggregate(string.Empty, (current, observacion) => current + string.Format("{0} - {1} : {2}\n", observacion.UsuarioCreacion, observacion.FechaCreacion.ToString("dd'/'MM'/'yyyy", null), observacion.Descripcion));
            ViewBag.HistorialObservaciones = historialObservaciones;

            //Cuentas del Asiento Actual
            var cuentasAsientos = new List<CuentaAsiento>();
            var cuentasAsientosDebe = comprobante.CuentasAsientos.Where(c => c.Debe > 0 && c.Activo).OrderBy(c => c.CuentaContable.Numero);
            var cuentasAsientosHaber = comprobante.CuentasAsientos.Where(c => c.Haber > 0 && c.Activo).OrderBy(c => c.CuentaContable.Numero);

            cuentasAsientos = cuentasAsientosDebe.Union(cuentasAsientosHaber).ToList();

            return View(cuentasAsientos);
        }

        // POST: CuentasAsientos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CuentaAsiento cuentaAsiento)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            try
            {
                if (cuentaAsiento != null)
                {
                    cuentaAsiento.UsuarioCreacion = usuarioActual;
                    cuentaAsiento.UsuarioModificacion = usuarioActual;
                    var resultado = cuentasAsientosManager.InsertCuentaAsiento(cuentaAsiento);
                }

                return RedirectToAction("Index", new { id = cuentaAsiento.ComprobanteId });
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", new { id = cuentaAsiento.ComprobanteId });
            }
        }

        // GET: CuentasAsientos/Edit/5
        public ActionResult Edit(int id)
        {
            var cuentaAsiento = cuentasAsientosManager.GetCuentasAsiento(id);
            var comprobante = comprobantesManager.GetComprobante(cuentaAsiento.ComprobanteId);
            if (comprobante != null)
            {
                ViewBag.Comprobante = comprobante;

                ViewBag.ComprobanteId = comprobante.Id;
                ViewBag.FechaComprobante = comprobante.FechaComprobante.ToString("dd'/'MM'/'yyyy");
                ViewBag.NumeroComprobante = comprobante.NumeroComprobante;
                ViewBag.Facility = comprobante.Facility.NombreDespliegue;
                ViewBag.TipoComprobante = comprobante.TiposComprobante.Nombre;
                ViewBag.FichaBanco = comprobante.CuentaBanco != null ? comprobante.CuentaBanco.FichaBanco : string.Empty; // "4020BK1177-C1108001-MAESTRO CARE EL ALTO";

                ViewBag.TotalDebe = comprobante.CuentasAsientos.Where(c => c.Activo).Sum(c => c.Debe);
                ViewBag.TotalHaber = comprobante.CuentasAsientos.Where(c => c.Activo).Sum(c => c.Haber);
                ViewBag.CuentasAsientos = comprobante.CuentasAsientos.Where(c => c.Activo).ToList();
            }
            ViewBag.PlanProgramatico = planProgramaticoManager.GetAllPlan();
            ViewBag.CuentasContables = cuentasContablesManager.GetAllCuentasContables();

            // Parametricas
            ViewBag.Contrapartes = new ContrapartesManager().GetAllContrapartes();
            ViewBag.AnexosTributarios = new AnexosTributariosManager().GetAllAnexosTributarios();
            ViewBag.CodigosAuditoria = new CodigosAuditoriasManager().GetAllCodigosAuditoria();
            ViewBag.AccionesNacionales = new AccionesNacionalesManager().GetAllAccionesNacionales();
            ViewBag.Territorios = new TerritoriosManager().GetAllTerritorios();

            return View(cuentaAsiento);
        }

        // POST: CuentasAsientos/Edit/5
        [HttpPost]
        public ActionResult Edit(CuentaAsiento cuentaAsiento)
        {
            var usuarioActual = HttpContext.User.Identity.Name;
            var resultado = new Resultado("");
            try
            {
                if (cuentaAsiento != null)
                {
                    cuentaAsiento.UsuarioModificacion = usuarioActual;
                    resultado = cuentasAsientosManager.UpdateCuentaAsiento(cuentaAsiento);
                }

                return Json(resultado);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: CuentasAsientos/Duplicar/5
        public ActionResult Duplicar(int id)
        {
            var nuevaCuentaAsiento = cuentasAsientosManager.GetCuentasAsiento(id);
            try
            {
                nuevaCuentaAsiento.Id = 0;
                nuevaCuentaAsiento.AccionesNacionale = null;
                nuevaCuentaAsiento.CodigosAuditoria = null;
                nuevaCuentaAsiento.Contraparte = null;
                nuevaCuentaAsiento.CuentaContable = null;
                nuevaCuentaAsiento.PlanProgramatico = null;
                nuevaCuentaAsiento.Territorio = null;
                var resultado = cuentasAsientosManager.InsertCuentaAsiento(nuevaCuentaAsiento);

                return RedirectToAction("Index", new { id = nuevaCuentaAsiento.ComprobanteId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { id = nuevaCuentaAsiento.ComprobanteId });
            }


        }

        // POST: CuentasAsientos/Delete/5
        // [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(CuentaAsiento cuentaAsiento)
        {
            try
            {
                var resultado = cuentasAsientosManager.DeleteCuentaAsiento(cuentaAsiento.Id);
                var cuentaAsientoEntity = cuentasAsientosManager.GetCuentasAsiento(cuentaAsiento.Id);
                return RedirectToAction("Index", new { id = cuentaAsientoEntity.ComprobanteId });
            }
            catch
            {
                return RedirectToAction("Index", new { id = cuentaAsiento.ComprobanteId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdjuntarRespaldos()
        {
            return RedirectToAction("Index");
        }

        public ActionResult EstadosCuentas(int comprobanteId, int tipoEstado)
        {
            var comprobante = comprobantesManager.GetComprobante(comprobanteId);
            if (comprobante != null)
            {
                ViewBag.ComprobanteId = comprobante.Id;
                ViewBag.FechaComprobante = comprobante.FechaComprobante.ToString("dd'/'MM'/'yyyy");
                ViewBag.NumeroComprobante = comprobante.NumeroComprobante;
                ViewBag.TipoComprobanteId = comprobante.TipoComprobanteId;
                ViewBag.TipoComprobante = comprobante.TiposComprobante.Nombre;
                ViewBag.FichaBanco = comprobante.CuentaBanco != null ? comprobante.CuentaBanco.FichaBanco : string.Empty; // "4020BK1177-C1108001-MAESTRO CARE EL ALTO";
                ViewBag.TipoMoneda = comprobante.TiposMoneda.Moneda;
                ViewBag.TipoCambio = tiposCambioManager.GetTiposCambioByFecha(DateTime.Now.Date).FirstOrDefault(tc => tc.TipoMonedaId == 2).Valor.ToString("N2");

                ViewBag.FacilityActual = comprobante.Facility;
                ViewBag.EstadoComprobante = comprobante.EstadosComprobante.Nombre;
                ViewBag.CiudadIdComprobante = comprobante.CiudadId;
            }

            // Datos particulares por cada tipo de Estado de Cuentas
            var cuentasContablesTodas = cuentasContablesManager.GetAllCuentasContables();
            var cuentasContablesEstados = cuentasContablesManager.GetCuentasContablesEstados(tipoEstado);
            var estadosCuentasRelacionados = estadosCuentasManager.GetEstadosCuentasByTipo(tipoEstado);

            var tituloEstado = string.Empty;
            var etiquetaCuenta = string.Empty;
            var cuentasContablesEstado = new SelectList(cuentasContablesEstados, "Id", "NombreDespliegue");
            var cuentasContables = new SelectList(cuentasContablesTodas, "Id", "NombreDespliegue");
            var localidades = new List<Ciudad>();
            var facilities = new List<Facility>();
            switch (tipoEstado)
            {
                case 1:
                    tituloEstado = "Transferencias 88000";
                    etiquetaCuenta = "Cuenta de Transferencia";
                    break;
                case 2:
                    tituloEstado = "Cuentas por Pagar";
                    etiquetaCuenta = "Cuenta por Pagar";
                    break;
                case 3:
                    tituloEstado = "Cuentas por Cobrar";
                    etiquetaCuenta = "Cuenta por Cobrar";
                    break;
            }

            ViewBag.TipoEstado = tipoEstado;
            ViewBag.TituloEstado = tituloEstado;
            ViewBag.EtiquetaCuenta = etiquetaCuenta;
            ViewBag.CuentaContableEstadoId = cuentasContablesEstado;
            ViewBag.CuentaContableId = cuentasContables;
            ViewBag.CiudadId = localidades;
            ViewBag.FacilityId = facilities;

            return View(estadosCuentasRelacionados);
        }

        [HttpPost]
        public ActionResult EstadosCuentas(EstadoCuenta estadoCuenta)
        {
            GenerarAsientoEstadoCuenta(estadoCuenta);
            return RedirectToAction("Index", new { id = estadoCuenta.ComprobanteId });
        }

        private void GenerarAsientoEstadoCuenta(EstadoCuenta estadoCuenta)
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            var cuentaEstadoCuenta = new CuentaAsiento //Debito
            {
                ComprobanteId = estadoCuenta.ComprobanteId,
                CuentaContableId = estadoCuenta.CuentaContableEstadoId,
                Glosa = estadoCuenta.Glosa,
                Debe = estadoCuenta.Debe,
                Haber = estadoCuenta.Haber,
                TerritorioId = 1,
                PlanProgramaticoId = 6,
                ContraparteId = 1,
                EsDebe = true,
                EsAjuste = false,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cuentasAsientosManager.InsertCuentaAsiento(cuentaEstadoCuenta);

            var cuentaPrograma = new CuentaAsiento
            {
                ComprobanteId = estadoCuenta.ComprobanteId,
                CuentaContableId = estadoCuenta.CuentaContableId,
                Glosa = estadoCuenta.Glosa,
                Debe = estadoCuenta.Haber,
                Haber = estadoCuenta.Debe,
                TerritorioId = 1,
                PlanProgramaticoId = 6,
                ContraparteId = 1,
                EsDebe = false,
                EsAjuste = false,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cuentasAsientosManager.InsertCuentaAsiento(cuentaPrograma);

            var comprobanteActual = comprobantesManager.GetComprobante(estadoCuenta.ComprobanteId);
            // Insertar en la Tabla de Estados de Cuentas
            if (!estadoCuenta.EstadoCuentaRelacionado)
            {
                
                if (estadoCuenta.Debe > 0)
                {
                    var estadoCuentaDebe = new EstadoCuenta
                    {
                        TipoEstadoCuentaId = estadoCuenta.TipoEstadoCuentaId,
                        DebeCiudadId = comprobanteActual.CiudadId,
                        DebeFacilityId = comprobanteActual.FacilityId,
                        DebeCuentaAsientoId = cuentaEstadoCuenta.Id,
                        UsuarioCreacion = usuarioActual,
                        UsuarioModificacion = usuarioActual
                    };

                    estadosCuentasManager.InsertEstadoCuenta(estadoCuentaDebe);
                }

                if (estadoCuenta.Haber > 0)
                {
                    var estadoCuentaHaber = new EstadoCuenta
                    {
                        TipoEstadoCuentaId = estadoCuenta.TipoEstadoCuentaId,
                        HaberCiudadId = comprobanteActual.CiudadId,
                        HaberFacilityId = comprobanteActual.FacilityId,
                        HaberCuentaAsientoId = cuentaEstadoCuenta.Id,
                        UsuarioCreacion = usuarioActual,
                        UsuarioModificacion = usuarioActual
                    };

                    estadosCuentasManager.InsertEstadoCuenta(estadoCuentaHaber);
                } 
            }

            // Si es estado de Cuentas Relacionado Actualizar el Estado de Cuentas Correspondiente
            if (estadoCuenta.EstadoCuentaRelacionado)
            {
                var estadoCuentaRelacionado = estadosCuentasManager.GetEstadoCuenta(estadoCuenta.EstadoCuentaRelacionadoId);
                estadoCuentaRelacionado.UsuarioModificacion = usuarioActual;

                var completarDebe = (estadoCuentaRelacionado.DebeCuentaAsientoId == null);
                if (completarDebe)
                {
                    estadoCuentaRelacionado.DebeCiudadId = comprobanteActual.CiudadId;
                    estadoCuentaRelacionado.DebeFacilityId = comprobanteActual.FacilityId;
                    estadoCuentaRelacionado.DebeCuentaAsientoId = cuentaEstadoCuenta.Id;

                    estadosCuentasManager.UpdateEstadoCuentaDebe(estadoCuentaRelacionado);
                }
                else
                {
                    estadoCuentaRelacionado.HaberCiudadId = comprobanteActual.CiudadId;
                    estadoCuentaRelacionado.HaberFacilityId = comprobanteActual.FacilityId;
                    estadoCuentaRelacionado.HaberCuentaAsientoId = cuentaEstadoCuenta.Id;

                    estadosCuentasManager.UpdateEstadoCuentaHaber(estadoCuentaRelacionado);
                }
            }

        }

        [HttpPost]
        public ActionResult Traspaso(Traspaso traspaso)
        {
            if (traspaso.AnPrograma)
            {
                switch (traspaso.CuentaTransitoriaNro)
                {
                    case 88110:
                        GenerarAsientoAnPrograma(traspaso, CuentaTransitoria.Manutencion);
                        break;
                    case 88130:
                        GenerarAsientoAnPrograma(traspaso, CuentaTransitoria.Sueldos);
                        break;
                    case 88120:
                        GenerarAsientoAnPrograma(traspaso, CuentaTransitoria.Resto);
                        break;
                    case 88210:
                        GenerarAsientoAnPrograma(traspaso, CuentaTransitoria.Construccion);
                        break;
                }

            }
            else
            {
                switch (traspaso.CuentaTransitoriaNro)
                {
                    case 88110:
                        GenerarAsientoProgramaAn(traspaso, CuentaTransitoria.Manutencion);
                        break;
                    case 88130:
                        GenerarAsientoProgramaAn(traspaso, CuentaTransitoria.Sueldos);
                        break;
                    case 88120:
                        GenerarAsientoProgramaAn(traspaso, CuentaTransitoria.Resto);
                        break;
                    case 88210:
                        GenerarAsientoProgramaAn(traspaso, CuentaTransitoria.Construccion);
                        break;
                }
            }

            return RedirectToAction("Index", new { id = traspaso.ComprobanteId });
        }

        private void GenerarAsientoAnPrograma(Traspaso traspaso, CuentaTransitoria cuentaTransitoria)
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            var cuentaTransferencia = new CuentaAsiento //Debito
            {
                ComprobanteId = traspaso.ComprobanteId,
                CuentaContableId = (int) cuentaTransitoria,
                Glosa = traspaso.Glosa,
                Debe = traspaso.Monto,
                Haber = 0,
                TerritorioId = 1,
                PlanProgramaticoId = 6,
                ContraparteId = 1,
                EsDebe = true,
                EsAjuste = false,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cuentasAsientosManager.InsertCuentaAsiento(cuentaTransferencia);

            var cuentaPrograma = new CuentaAsiento
            {
                ComprobanteId = traspaso.ComprobanteId,
                CuentaContableId = traspaso.CuentaContableId,
                Glosa = traspaso.Glosa,
                Debe = 0,
                Haber = traspaso.Monto,
                TerritorioId = 1,
                PlanProgramaticoId = 6,
                ContraparteId = 1,
                EsDebe = false,
                EsAjuste = false,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cuentasAsientosManager.InsertCuentaAsiento(cuentaPrograma);

            // Tabla de Cuentas Traspaso
            var traspasoPrograma = new TraspasosPrograma
            {
                ComprobanteProgramaId = cuentaTransferencia.ComprobanteId,
                MontoTraspaso = cuentaTransferencia.Debe,
                CuentaTransitoriaNumero = traspaso.CuentaTransitoriaNro.ToString(),
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            traspasosProgramasManager.InsertTraspasosPrograma(traspasoPrograma);
        }

        private void GenerarAsientoProgramaAn(Traspaso traspaso, CuentaTransitoria cuentaTransitoria)
        {
            var usuarioActual = HttpContext.User.Identity.Name;

            var cuentaPersonalizada = new CuentaAsiento
            {
                ComprobanteId = traspaso.ComprobanteId,
                CuentaContableId = traspaso.CuentaContableId,
                Glosa = traspaso.Glosa,
                Debe = traspaso.Monto,
                Haber = 0,
                TerritorioId = 1,
                PlanProgramaticoId = 6,
                ContraparteId = 1,
                EsDebe = false,
                EsAjuste = false,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cuentasAsientosManager.InsertCuentaAsiento(cuentaPersonalizada);

            var cuentaTransferencia = new CuentaAsiento //Debito
            {
                ComprobanteId = traspaso.ComprobanteId,
                CuentaContableId = (int) cuentaTransitoria, //TODO Colocar el ID de la Cuenta 8
                Glosa = traspaso.Glosa,
                Debe = 0,
                Haber = traspaso.Monto,
                TerritorioId = 1,
                PlanProgramaticoId = 6,
                ContraparteId = 1,
                EsDebe = true,
                EsAjuste = false,
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            cuentasAsientosManager.InsertCuentaAsiento(cuentaTransferencia);
            // Tabla de Cuentas Traspaso
            var traspasoPrograma = new TraspasosPrograma
            {
                ComprobanteProgramaId = cuentaTransferencia.ComprobanteId,
                MontoTraspaso = cuentaTransferencia.Haber,
                CuentaTransitoriaNumero = traspaso.CuentaTransitoriaNro.ToString(),
                UsuarioCreacion = usuarioActual,
                UsuarioModificacion = usuarioActual
            };

            traspasosProgramasManager.InsertTraspasosPrograma(traspasoPrograma);
        }
    }
}
