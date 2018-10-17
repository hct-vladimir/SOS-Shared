using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes;
using Cerberus.Sos.Accounting.BusinessLogic.Entities.Reportes.Presupuesto;
using Cerberus.Sos.Accounting.DataAccess.Models;
using OfficeOpenXml;
using Recurso = Cerberus.Sos.Accounting.BusinessLogic.Entities.Recurso;
using RecursoMes = Cerberus.Sos.Accounting.BusinessLogic.Entities.RecursoMes;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class ReportesManager
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        #region Reportes Presupuesto

        public List<ReporteResumen> GetReportePresupuestoResumen(int presupuestoId, int? ciudadId, int? facilityId)
        {
            var resultado = dbContext.RptPresupuestoResumen(presupuestoId, ciudadId, facilityId).ToList();
             MapperManager.GetInstance();

            var reporte = new List<ReporteResumen>();
            resultado.ForEach(r => reporte.Add(Mapper.Map<RptPresupuestoResumen_Result, ReporteResumen>(r)));

            return reporte;
        }

        public MemoryStream ExportReportePresupuestoResumen(string templateDocument, List<ReporteResumen> resumen, decimal? totalGastos = 0, decimal? totalIngresos = 0, decimal? totalSubsidio = 0)
        {
            try
            {

                // Results Output
                MemoryStream output = new MemoryStream();

                // Read Template
                using (FileStream templateDocumentStream = File.OpenRead(templateDocument))
                {
                    // Create Excel EPPlus Package based on template stream
                    using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                    {
                        // Grab the sheet with the template, sheet name is "BOL".
                        ExcelWorksheet sheet = package.Workbook.Worksheets["ReporteGeneral"];

                        //Datos de Cabecera
                        sheet.Cells[1, 2].Value = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                        sheet.Cells[1, 9].Value = totalGastos.Value.ToString("N2");
                        sheet.Cells[2, 9].Value = totalIngresos.Value.ToString("N2");
                        sheet.Cells[3, 9].Value = totalSubsidio.Value.ToString("N2");

                        if (resumen.Count > 0)
                        {
                            resumen.Reverse();
                            foreach (var recurso in resumen)
                            {
                                sheet.InsertRow(7, 1, 6);
                                sheet.Cells[7, 1].Value = recurso.Codigo;
                                sheet.Cells[7, 2].Value = recurso.Descripcion;
                                sheet.Cells[7, 3].Value = recurso.SOS;
                                sheet.Cells[7, 4].Value = recurso.RRFF;
                                sheet.Cells[7, 5].Value = recurso.GOB;
                                sheet.Cells[7, 6].Value = recurso.MUN;
                                sheet.Cells[7, 7].Value = recurso.COM;
                                sheet.Cells[7, 8].Value = recurso.IPD;
                                sheet.Cells[7, 9].Value = recurso.Total;
                            }
                            sheet.DeleteRow(6);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception exception)
            {
                // Log Exception
                return null;
            }
        }

        public List<Recurso> GetReportePresupuestoGeneral(int presupuestoId, int? ciudadId, int? facilityId)
        {
            var consultaResultado = dbContext.Recursos.Where(r => r.Activo && r.PresupuestoId == presupuestoId);
            if (ciudadId != null)
            {
                consultaResultado = consultaResultado.Where(r => r.CiudadId == ciudadId.Value);
            }

            if (facilityId != null)
            {
                consultaResultado = consultaResultado.Where(r => r.FacilityId == facilityId.Value);
            }

            var resultado = consultaResultado.ToList();


            MapperManager.GetInstance();

            var reporte = new List<Recurso>();
            resultado.ForEach(r => reporte.Add(Mapper.Map<DataAccess.Models.Recurso, Recurso>(r)));

            return reporte;
        }

        public MemoryStream ExportReportePresupuestoGeneral(string templateDocument, List<Recurso> recursos)
        {
            try
            {

                // Results Output
                MemoryStream output = new MemoryStream();

                // Read Template
                using (FileStream templateDocumentStream = File.OpenRead(templateDocument))
                {
                    // Create Excel EPPlus Package based on template stream
                    using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                    {
                        // Grab the sheet with the template, sheet name is "BOL".
                        ExcelWorksheet sheet = package.Workbook.Worksheets["ReporteGeneral"];

                        //Se agrega Fecha Actual en Título
                        sheet.Cells[1, 2].Value = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                        sheet.Cells[2, 8].Formula = $"=SUM(H3:H{recursos.Count + 2})";
                        sheet.Cells[2, 9].Formula = $"=SUM(I3:I{recursos.Count + 2})";
                        sheet.Cells[2, 10].Formula = $"=SUM(J3:J{recursos.Count + 2})";

                        if (recursos.Count > 0)
                        {
                            foreach (var recurso in recursos)
                            {
                                sheet.InsertRow(4, 1, 3);
                                sheet.Cells[4, 1].Value = recurso.Ciudad.Nombre;
                                sheet.Cells[4, 2].Value = recurso.Facility.NombreDespliegue;
                                sheet.Cells[4, 3].Value = recurso.CuentaContable.NombreDespliegue;
                                sheet.Cells[4, 4].Value = recurso.PlanProgramatico.NombreDespliegue;
                                sheet.Cells[4, 5].Value = recurso.CodigosAuditoria != null ? recurso.CodigosAuditoria.Codigo : string.Empty;
                                sheet.Cells[4, 6].Value = recurso.CodigoProgramatico;
                                sheet.Cells[4, 7].Value = recurso.Descripcion;
                                sheet.Cells[4, 8].Value = recurso.Cobertura;
                                sheet.Cells[4, 9].Value = recurso.IndiceTransferencia;
                                sheet.Cells[4, 10].Value = recurso.Monto;
                            }
                            sheet.DeleteRow(3);
                        }

                        sheet.Cells[2, 8].Calculate();
                        sheet.Cells[2, 9].Calculate();
                        sheet.Cells[2, 10].Calculate();

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception exception)
            {
                // Log Exception
                return null;
            }
        }

        public MemoryStream ExportReporteGeneralExtendido(string templateDocument, List<Recurso> recursos)
        {
            try
            {

                // Results Output
                MemoryStream output = new MemoryStream();

                // Read Template
                using (FileStream templateDocumentStream = File.OpenRead(templateDocument))
                {
                    // Create Excel EPPlus Package based on template stream
                    using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                    {
                        // Grab the sheet with the template, sheet name is "BOL".
                        ExcelWorksheet sheet = package.Workbook.Worksheets["ReporteGeneral"];

                        //Se agrega Fecha Actual en Título
                        sheet.Cells[1, 2].Value = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                        sheet.Cells[2, 14].Formula = $"=SUM(N3:N{recursos.Count + 2})";
                        sheet.Cells[2, 15].Formula = $"=SUM(O3:O{recursos.Count + 2})";
                        sheet.Cells[2, 16].Formula = $"=SUM(P3:P{recursos.Count + 2})";

                        if (recursos.Count > 0)
                        {
                            foreach (var recurso in recursos)
                            {
                                sheet.InsertRow(4, 1, 3);
                                sheet.Cells[4, 1].Value = recurso.Ciudad.Nombre;
                                sheet.Cells[4, 2].Value = recurso.Facility.NombreDespliegue;
                                sheet.Cells[4, 3].Value = recurso.CuentaContable.NombreDespliegue;
                                sheet.Cells[4, 4].Value = recurso.PlanProgramatico.NombreDespliegue;
                                sheet.Cells[4, 5].Value = recurso.CodigosAuditoria != null ? recurso.CodigosAuditoria.Codigo : string.Empty;
                                sheet.Cells[4, 6].Value = recurso.CodigoProgramatico;
                                sheet.Cells[4, 7].Value = recurso.Ciudad.Codigo;
                                sheet.Cells[4, 8].Value = recurso.Territorio.Codigo;
                                sheet.Cells[4, 9].Value = recurso.Contraparte.Codigo;
                                sheet.Cells[4, 10].Value = recurso.AccionesNacionale != null ? recurso.AccionesNacionale.Codigo : "";
                                sheet.Cells[4, 11].Value = recurso.CodigoMarcoLogico != null ? recurso.CodigoMarcoLogico.Codigo : "";
                                sheet.Cells[4, 12].Value = recurso.NotasAdicionales;
                                sheet.Cells[4, 13].Value = recurso.Descripcion;
                                sheet.Cells[4, 14].Value = recurso.Cobertura;
                                sheet.Cells[4, 15].Value = recurso.IndiceTransferencia;
                                sheet.Cells[4, 16].Value = recurso.Monto;
                            }
                            sheet.DeleteRow(3);
                        }

                        sheet.Cells[2, 14].Calculate();
                        sheet.Cells[2, 15].Calculate();
                        sheet.Cells[2, 16].Calculate();

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception exception)
            {
                // Log Exception
                return null;
            }
        }

        public List<RecursoMes> GetReportePresupuestoMensual(int presupuestoId, int? ciudadId, int? facilityId)
        {
            var consultaResultado = dbContext.RecursosMeses.Where(r => r.Activo && r.Recurso.Activo && r.Recurso.PresupuestoId == presupuestoId);
            if (ciudadId != null)
            {
                consultaResultado = consultaResultado.Where(r => r.Recurso.CiudadId == ciudadId.Value);
            }

            if (facilityId != null)
            {
                consultaResultado = consultaResultado.Where(r => r.Recurso.FacilityId == facilityId.Value);
            }

            var resultado = consultaResultado.ToList();


            MapperManager.GetInstance();

            var reporte = new List<RecursoMes>();
            resultado.ForEach(r => reporte.Add(Mapper.Map<DataAccess.Models.RecursoMes, RecursoMes>(r)));

            return reporte;
        }

        public MemoryStream ExportReportePresupuestoMensual(string templateDocument, List<RecursoMes> recursos)
        {
            try
            {

                // Results Output
                MemoryStream output = new MemoryStream();

                // Read Template
                using (FileStream templateDocumentStream = File.OpenRead(templateDocument))
                {
                    // Create Excel EPPlus Package based on template stream
                    using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                    {
                        // Grab the sheet with the template, sheet name is "BOL".
                        ExcelWorksheet sheet = package.Workbook.Worksheets["ReporteGeneral"];

                        //Se agrega Fecha Actual en Título
                        sheet.Cells[1, 2].Value = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                        sheet.Cells[2, 8].Formula = $"=SUM(H3:H{recursos.Count + 2})";
                        sheet.Cells[2, 9].Formula = $"=SUM(I3:I{recursos.Count + 2})";
                        sheet.Cells[2, 10].Formula = $"=SUM(J3:J{recursos.Count + 2})";
                        sheet.Cells[2, 11].Formula = $"=SUM(K3:K{recursos.Count + 2})";
                        sheet.Cells[2, 12].Formula = $"=SUM(L3:L{recursos.Count + 2})";
                        sheet.Cells[2, 13].Formula = $"=SUM(M3:M{recursos.Count + 2})";
                        sheet.Cells[2, 14].Formula = $"=SUM(N3:N{recursos.Count + 2})";
                        sheet.Cells[2, 15].Formula = $"=SUM(O3:O{recursos.Count + 2})";
                        sheet.Cells[2, 16].Formula = $"=SUM(P3:P{recursos.Count + 2})";
                        sheet.Cells[2, 17].Formula = $"=SUM(Q3:Q{recursos.Count + 2})";
                        sheet.Cells[2, 18].Formula = $"=SUM(R3:R{recursos.Count + 2})";
                        sheet.Cells[2, 19].Formula = $"=SUM(S3:S{recursos.Count + 2})";
                        sheet.Cells[2, 20].Formula = $"=SUM(T3:T{recursos.Count + 2})";
                        sheet.Cells[2, 21].Formula = $"=SUM(U3:U{recursos.Count + 2})";
                        sheet.Cells[2, 22].Formula = $"=SUM(V3:V{recursos.Count + 2})";

                        if (recursos.Count > 0)
                        {
                            foreach (var recursoMes in recursos)
                            {
                                sheet.InsertRow(4, 1, 3);
                                sheet.Cells[4, 1].Value = recursoMes.Recurso.Ciudad.Nombre;
                                sheet.Cells[4, 2].Value = recursoMes.Recurso.Facility.NombreDespliegue;
                                sheet.Cells[4, 3].Value = recursoMes.Recurso.CuentaContable.NombreDespliegue;
                                sheet.Cells[4, 4].Value = recursoMes.Recurso.PlanProgramatico.NombreDespliegue;
                                sheet.Cells[4, 5].Value = recursoMes.Recurso.CodigosAuditoria != null ? recursoMes.Recurso.CodigosAuditoria.Codigo : string.Empty;
                                sheet.Cells[4, 6].Value = recursoMes.Recurso.CodigoProgramatico;
                                sheet.Cells[4, 7].Value = recursoMes.Recurso.Descripcion;
                                sheet.Cells[4, 8].Value = recursoMes.Recurso.Cobertura;
                                sheet.Cells[4, 9].Value = recursoMes.Recurso.IndiceTransferencia.HasValue ? Decimal.Round(recursoMes.Recurso.IndiceTransferencia.Value, 2) : 0;
                                sheet.Cells[4, 10].Value = Decimal.Round(recursoMes.Recurso.Monto, 2);
                                sheet.Cells[4, 11].Value = recursoMes.Enero.HasValue ? Decimal.Round(recursoMes.Enero.Value, 2) : 0;
                                sheet.Cells[4, 12].Value = recursoMes.Febrero.HasValue ? Decimal.Round(recursoMes.Febrero.Value, 2) : 0;
                                sheet.Cells[4, 13].Value = recursoMes.Marzo.HasValue ? Decimal.Round(recursoMes.Marzo.Value, 2) : 0;
                                sheet.Cells[4, 14].Value = recursoMes.Abril.HasValue ? Decimal.Round(recursoMes.Abril.Value, 2) : 0;
                                sheet.Cells[4, 15].Value = recursoMes.Mayo.HasValue ? Decimal.Round(recursoMes.Mayo.Value, 2) : 0;
                                sheet.Cells[4, 16].Value = recursoMes.Junio.HasValue ? Decimal.Round(recursoMes.Junio.Value, 2) : 0;
                                sheet.Cells[4, 17].Value = recursoMes.Julio.HasValue ? Decimal.Round(recursoMes.Julio.Value, 2) : 0;
                                sheet.Cells[4, 18].Value = recursoMes.Agosto.HasValue ? Decimal.Round(recursoMes.Agosto.Value, 2) : 0;
                                sheet.Cells[4, 19].Value = recursoMes.Septiembre.HasValue ? Decimal.Round(recursoMes.Septiembre.Value, 2) : 0;
                                sheet.Cells[4, 20].Value = recursoMes.Octubre.HasValue ? Decimal.Round(recursoMes.Octubre.Value, 2) : 0;
                                sheet.Cells[4, 21].Value = recursoMes.Noviembre.HasValue ? Decimal.Round(recursoMes.Noviembre.Value, 2) : 0;
                                sheet.Cells[4, 22].Value = recursoMes.Diciembre.HasValue ? Decimal.Round(recursoMes.Diciembre.Value, 2) : 0;
                            }
                            sheet.DeleteRow(3);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception exception)
            {
                // Log Exception
                return null;
            }
        }

        public MemoryStream GetReporteBet(string templateDocument, int presupuestoId)
        {
            try
            {

                // Results Output
                MemoryStream output = new MemoryStream();

                // Read Template
                using (FileStream templateDocumentStream = File.OpenRead(templateDocument))
                {
                    // Create Excel EPPlus Package based on template stream
                    using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                    {
                        // Grab the sheet with the template, sheet name is "BOL".
                        ExcelWorksheet sheet = package.Workbook.Worksheets["ReporteBET"];

                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var recursosMeses = dbContext.RecursosMeses.Where(r => r.Activo && r.Recurso.Activo && r.Recurso.PresupuestoId == presupuestoId).ToList();
                        var recurso = new DataAccess.Models.Recurso();
                        if (recursosMeses.Count > 0)
                        {
                            foreach (var recursoMes in recursosMeses)
                            {
                                recurso = recursoMes.Recurso;
                                sheet.InsertRow(5, 1, 4);
                                sheet.Cells[5, 1].Value = recursoMes.Recurso.CuentaContable.Numero;
                                var descripcionFiltrada = recurso.Descripcion.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
                                sheet.Cells[5, 2].Value = new String(descripcionFiltrada);
                                sheet.Cells[5, 3].Value = recurso.CodigosAuditoria != null ? recurso.CodigosAuditoria.Codigo : string.Empty;
                                var codigoProgramatico = $"{recurso.Ciudad.Codigo}{recurso.PlanProgramatico.Codigo}{recurso.Territorio.Codigo}{recurso.Contraparte.Codigo}-/{(recurso.AccionesNacionale != null ? recurso.AccionesNacionale.Codigo : "")}*{(recurso.CodigoMarcoLogico != null ? recurso.CodigoMarcoLogico.Codigo : "")}:{recurso.NotasAdicionales}";
                                var codigoProgramaticoFiltrado = codigoProgramatico.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
                                sheet.Cells[5, 4].Value = new String(codigoProgramaticoFiltrado);
                                sheet.Cells[5, 5].Value = Decimal.Round(recursoMes.Recurso.Monto, 2);

                                sheet.Cells[5, 6].Value = recursoMes.Enero.HasValue ? Decimal.Round(recursoMes.Enero.Value, 2) : 0;
                                sheet.Cells[5, 7].Value = recursoMes.Febrero.HasValue ? Decimal.Round(recursoMes.Febrero.Value, 2) : 0;
                                sheet.Cells[5, 8].Value = recursoMes.Marzo.HasValue ? Decimal.Round(recursoMes.Marzo.Value, 2) : 0;
                                sheet.Cells[5, 9].Value = recursoMes.Abril.HasValue ? Decimal.Round(recursoMes.Abril.Value, 2) : 0;
                                sheet.Cells[5, 10].Value = recursoMes.Mayo.HasValue ? Decimal.Round(recursoMes.Mayo.Value, 2) : 0;
                                sheet.Cells[5, 11].Value = recursoMes.Junio.HasValue ? Decimal.Round(recursoMes.Junio.Value, 2) : 0;
                                sheet.Cells[5, 12].Value = recursoMes.Julio.HasValue ? Decimal.Round(recursoMes.Julio.Value, 2) : 0;
                                sheet.Cells[5, 13].Value = recursoMes.Agosto.HasValue ? Decimal.Round(recursoMes.Agosto.Value, 2) : 0;
                                sheet.Cells[5, 14].Value = recursoMes.Septiembre.HasValue ? Decimal.Round(recursoMes.Septiembre.Value, 2) : 0;
                                sheet.Cells[5, 15].Value = recursoMes.Octubre.HasValue ? Decimal.Round(recursoMes.Octubre.Value, 2) : 0;
                                sheet.Cells[5, 16].Value = recursoMes.Noviembre.HasValue ? Decimal.Round(recursoMes.Noviembre.Value, 2) : 0;
                                sheet.Cells[5, 17].Value = recursoMes.Diciembre.HasValue ? Decimal.Round(recursoMes.Diciembre.Value, 2) : 0;
                                sheet.Cells[5, 18].Value = recursoMes.Recurso.Facility.Codigo;
                            }
                            sheet.DeleteRow(4);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception exception)
            {
                // Log Exception
                return null;
            }
        }

        #endregion

        #region Reportes Contabilidad
        public List<ReporteComprobante> GetReporteComprobante(int comprobanteId)
        {
            var resultado = dbContext.VComprobantes.Where(a => a.IdComprobante == comprobanteId).ToList();

            MapperManager.GetInstance();

            var reporte = new List<ReporteComprobante>();
            resultado.ForEach(r => reporte.Add(Mapper.Map<VComprobante, ReporteComprobante>(r)));

            return reporte;
        }

        public List<ReporteSumasSaldos> GetReporteSumasSaldos()
        {
            var resultado = dbContext.SumasSaldos("2018-03-20", "2018-05-28").ToList();
            MapperManager.GetInstance();

            var reporte = new List<ReporteSumasSaldos>();
            resultado.ForEach(r => reporte.Add(Mapper.Map<SumasSaldos_Result, ReporteSumasSaldos>(r)));

            return reporte;
        }

        public List<ReporteMayores> GetReporteMayores(int numeroCuenta)
        {
            var resultado = dbContext.Mayores(DateTime.ParseExact("2018-01-01", "yyyy-MM-dd", null), DateTime.ParseExact("2018-12-31", "yyyy-MM-dd", null), numeroCuenta.ToString()).ToList();

            MapperManager.GetInstance();

            var reporte = new List<ReporteMayores>();
            resultado.ForEach(r => reporte.Add(Mapper.Map<Mayores_Result, ReporteMayores>(r)));

            return reporte;
        }

        public List<ReporteBalanceComprobacion> GetReporteEstadoCuentas(string numeroCuentaInicio, string numeroCuentaFin, DateTime fechaInicio, DateTime fechaFin)
        {
            var resultado = dbContext.BalanceRangoCuentas(fechaInicio, fechaFin, numeroCuentaInicio, numeroCuentaFin).ToList();

            MapperManager.GetInstance();

            var reporte = new List<ReporteBalanceComprobacion>();
            resultado.ForEach(r => reporte.Add(Mapper.Map<BalanceRangoCuentas_Result, ReporteBalanceComprobacion>(r)));

            return reporte;
        }

        public List<ReporteLibroCompras> GetReporteLibroCompras(int mes, int gestion)
        {
            var resultado = dbContext.LibroCompras(mes, gestion).ToList();

            MapperManager.GetInstance();

            var reporte = new List<ReporteLibroCompras>();
            resultado.ForEach(r => reporte.Add(Mapper.Map<LibroCompras_Result, ReporteLibroCompras>(r)));

            return reporte;
        }

        public List<ReporteLibroVentas> GetReporteLibroVentas(int mes, int gestion)
        {
            var resultado = dbContext.LibroVentas(mes, gestion).ToList();

            MapperManager.GetInstance();

            var reporte = new List<ReporteLibroVentas>();
            resultado.ForEach(r => reporte.Add(Mapper.Map<LibroVentas_Result, ReporteLibroVentas>(r)));

            return reporte;
        }


        public List<ReporteBalanceComprobacion> GetReporteBalanceComprobacion(DateTime fechaInicio, DateTime fechaFin)
        {
            var resultado = dbContext.BalanceComprobacion(fechaInicio, fechaFin).ToList();

            MapperManager.GetInstance();

            var reporte = new List<ReporteBalanceComprobacion>();
            resultado.ForEach(r => reporte.Add(Mapper.Map<BalanceComprobacion_Result, ReporteBalanceComprobacion>(r)));

            return reporte;
        }


        public ReporteBalanceGeneral GetReporteBalance()
        {
            var cuentasQuery =
                dbContext.CuentasAsientos.Where(
                    c =>
                        c.Activo && c.Comprobante.FechaComprobante <= DateTime.Now);

            var reporte = new ReporteBalanceGeneral();

            // Activos No Corrientes
            var activosIntangibles = new List<string> { "02100", "02120", "02125", "02130", "02140", "02150", "02160", "02200" };
            reporte.Intangibles = cuentasQuery.Where(c => activosIntangibles.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var bienesEquipamiento = new List<string> { "01110", "01120", "01130", "01140", "01150", "01160", "01210", "01220", "01230", "01240", "01250", "01260", "01310", "01320", "01330", "01340", "01350", "01360", "01410", "01420", "01430", "01440", "01450", "01460", "01510", "01511", "01512", "01513", "0152", "01530", "01540", "01550", "01560", "01610", "01620", "01630", "01640", "01650", "01660", "01670", "01680", "01690", "01698", "01710", "01730", "01740", "01750", "01760" };
            reporte.BienesEquipamiento = cuentasQuery.Where(c => bienesEquipamiento.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var propiedadInversion = new List<string> { "00000" };
            reporte.PropiedadInversion = cuentasQuery.Where(c => propiedadInversion.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var otrosActivosLargoPlazo = new List<string> { "00000" };
            reporte.OtrosActivosLargoPlazo = cuentasQuery.Where(c => otrosActivosLargoPlazo.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var inversionesEmpresas = new List<string> { "00000" };
            reporte.InversionesEmpresas = cuentasQuery.Where(c => inversionesEmpresas.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var biologicos = new List<string> { "00000" };
            reporte.Biologicos = cuentasQuery.Where(c => biologicos.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            // Activos Corrientes
            var cuentasInventarios = new List<string> {"05100"};
            reporte.Inventarios = cuentasQuery.Where(c => cuentasInventarios.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var cuentasPorCobrar = new List<string> { "06110", "16210", "06620", "06910", "06920", "06930", "06940", "06950", "06960", "06970", "06990" };
            reporte.CuentasPorCobrar = cuentasQuery.Where(c => cuentasPorCobrar.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var efectivo = new List<string> { "11100", "11200", "11300", "11400", "11500", "11600", "12100", "12200", "12300" };
            reporte.Efectivo = cuentasQuery.Where(c => efectivo.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe) - cuentasQuery.Where(c => efectivo.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Haber );

            var otrosActivosCortoPlazo = new List<string> { "00000" };
            reporte.OtrosActivosCortoPlazo = cuentasQuery.Where(c => otrosActivosCortoPlazo.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            // Fondo acumulado
            var fondosNoRestringidos = new List<string> { "00000" };
            reporte.FondosNoRestringidos= cuentasQuery.Where(c => fondosNoRestringidos.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var fondosRestringidos = new List<string> { "00000" };
            reporte.FondosRestringidos= cuentasQuery.Where(c => fondosRestringidos.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var resultadoAnteriorGestion = new List<string> { "00000" };
            reporte.ResultadoAnteriorGestion = cuentasQuery.Where(c => resultadoAnteriorGestion.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var resultadoGestionActual = new List<string> { "00000" };
            reporte.ResultadoGestionActual = cuentasQuery.Where(c => resultadoGestionActual.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            // Pasivos no corrientes
            var fondosAdministracionFiduciaria = new List<string> { "92110" };
            reporte.FondosAdministracionFiduciaria = cuentasQuery.Where(c => fondosAdministracionFiduciaria.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var provisionesLargoPlazo = new List<string> { "92220" };
            reporte.ProvisionesLargoPlazo = cuentasQuery.Where(c => provisionesLargoPlazo.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            //Pasivos corrientes
            var otrosPasivosCortoPlazo = new List<string> { "92610" };
            reporte.OtrosPasivosCortoPlazo = cuentasQuery.Where(c => otrosPasivosCortoPlazo.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var ingresoDiferido = new List<string> { "00000" };
            reporte.IngresoDiferido = cuentasQuery.Where(c => ingresoDiferido.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var pasivoFondosRestringidos = new List<string> { "00000" };
            reporte.PasivoFondosRestringidos = cuentasQuery.Where(c => pasivoFondosRestringidos.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            //Transferencias internas
            var transferenciasInternas = new List<string> { "00000" };
            reporte.TransferenciasInternas = cuentasQuery.Where(c => transferenciasInternas.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            return reporte;
        }

        public ReporteEstadoResultados GetReporteResultados()
        {
            var cuentasQuery =
                            dbContext.CuentasAsientos.Where(
                                c =>
                                    c.Activo && c.Comprobante.FechaComprobante <= DateTime.Now);

            var reporte = new ReporteEstadoResultados();

            // Ingresos
            var ingresoGastosManutencion = new List<string> { "21100", "21200", "22110", "22120", "22130", "22140", "22150", "22160", "22170", "22180", "22190", "22210", "22220", "22230", "22240", "22250", "22260", "22270", "22280", "22290", "22310", "22320", "22330", "22910", "22920", "23100", "24110", "24120", "24130", "24140", "24150", "24160", "24170", "24180", "24190", "24210", "24220", "24230", "24240", "24250", "24260", "24270", "24280", "24290", "24310", "24320", "24330", "24910", "24920", "26100", "27110", "27120", "27130", "27140", "27150", "27160", "27170", "27180", "27190", "27210", "27220", "27230", "27240", "27250", "27260", "27270", "27280", "27290", "27310", "27320", "27330", "27601", "27602", "27603", "27604", "27605", "27606", "27607", "27608", "27609", "27610", "27611", "27612", "27613", "27614", "27615", "27616", "27617", "27618", "27619", "27620", "27621", "27622", "27623", "27910", "27920", "27950" };
            reporte.IngresoGastosManutencion = cuentasQuery.Where(c => ingresoGastosManutencion.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var ingresoGc = new List<string> { "00000" };
            reporte.IngresoGc = cuentasQuery.Where(c => ingresoGc.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var ingresoRecaudacionFondos = new List<string> { "31110", "31115", "31120", "31121", "31122", "31123", "31124", "31125", "31129", "31130", "31131", "31132", "31133", "31134", "31140", "31150", "31160", "31170", "31171", "31173", "31174", "31180", "31181", "31190", "31995" };
            reporte.IngresoRecaudacionFondos = cuentasQuery.Where(c => ingresoRecaudacionFondos.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var ingresoSubsidios = new List<string> { "32110", "32111", "32112", "32120", "32121", "32122" };
            reporte.IngresoSubsidios = cuentasQuery.Where(c => ingresoSubsidios.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var ingresoOperativo = new List<string> { "33100", "33200", "33300" };
            reporte.IngresoOperativo = cuentasQuery.Where(c => ingresoOperativo.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var otroIngreso = new List<string> { "34200", "34900", "34910" };
            reporte.OtroIngreso = cuentasQuery.Where(c => otroIngreso.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            // Gastos
            var gastosPrograma = new List<string> { "00000" };
            reporte.GastosPrograma = cuentasQuery.Where(c => gastosPrograma.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var gastosAdministrativos = new List<string> { "00000" };
            reporte.GastosAdministrativos = cuentasQuery.Where(c => gastosAdministrativos.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var gastosRecaudacionFondos = new List<string> { "00000" };
            reporte.GastosRecaudacionFondos = cuentasQuery.Where(c => gastosRecaudacionFondos.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var otrosGastos = new List<string> { "81100", "83900" }; // Revisar, se debe restar el saldo del primero menos el segundo
            reporte.OtrosGastos = cuentasQuery.Where(c => otrosGastos.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            // Excedentes/déficit
            var ingresoIntereses = new List<string> { "34300" };
            reporte.IngresoIntereses = cuentasQuery.Where(c => ingresoIntereses.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var gastoIntereses = new List<string> { "71500" };
            reporte.GastoIntereses = cuentasQuery.Where(c => gastoIntereses.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var otroIngresoFinanciero = new List<string> { "83100", "83200" }; // Revisar, se debe restar el primero menos el segundo
            reporte.OtroIngresoFinanciero = cuentasQuery.Where(c => otroIngresoFinanciero.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            var gastoPorImpuesto = new List<string> { "00000" }; // De donde se obtiene?
            reporte.GastoPorImpuesto = cuentasQuery.Where(c => gastoPorImpuesto.Contains(c.CuentaContable.Numero)).ToList().Sum(c => c.Debe);

            return reporte;
        }

        #endregion

    }
}
