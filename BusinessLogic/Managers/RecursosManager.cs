using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using OfficeOpenXml;
using Recurso = Cerberus.Sos.Accounting.BusinessLogic.Entities.Recurso;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class RecursosManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Recurso> GetAllRecursos()
        {
            var recursosDb = dbContext.Recursos.Where(r => r.Activo).ToList();
            MapperManager.GetInstance();

            var recursos = new List<Recurso>();
            recursosDb.ForEach(p => recursos.Add(Mapper.Map<DataAccess.Models.Recurso, Recurso>(p)));

            return recursos;
        }

        public List<Recurso> GetRecursosPorFacility(int facilityId)
        {
            var recursosDb = dbContext.Recursos.Where(r => r.Activo && r.FacilityId == facilityId).ToList();
            MapperManager.GetInstance();

            var recursos = new List<Recurso>();
            recursosDb.ForEach(p => recursos.Add(Mapper.Map<DataAccess.Models.Recurso, Recurso>(p)));

            return recursos;
        }

        public List<Recurso> GetRecursosPorFacilityCiudad(int facilityId, int ciudadId)
        {
            var recursosDb = dbContext.Recursos.Where(r => r.Activo && r.FacilityId == facilityId && r.CiudadId == ciudadId).ToList();
            MapperManager.GetInstance();

            var recursos = new List<Recurso>();
            recursosDb.ForEach(p => recursos.Add(Mapper.Map<DataAccess.Models.Recurso, Recurso>(p)));

            return recursos;
        }

        public MemoryStream GetReporteAnual(string templateDocument)
        {
            try
            {

                // Results Output
                MemoryStream output = new MemoryStream();

                // Read Template
                using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
                {
                    // Create Excel EPPlus Package based on template stream
                    using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                    {
                        // Grab the sheet with the template, sheet name is "BOL".
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Reporte"];

                        // SubSonic 2x - Data Acces, load shipment header data.
                        // Shipment shipment = Shipment.FetchByID(id);

                        //// Insert BOL header data into template
                        sheet.Cells[1, 1].Value = string.Format("R1 - Presupuesto Anual {0}", DateTime.Now.Year);
                        sheet.Cells[4, 2].Value = "LA PAZ";

                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var recursos = dbContext.Recursos.Where(r => r.PlanProgramatico.NivelProgramaticoId == 4).ToList();


                        var totalGastosServicios = recursos.Where(r => r.PlanProgramatico.PlanProgramaticoParent.PlanProgramaticoParent.PlanProgramaticoParent.Codigo == "10000").Sum(r => r.Monto);
                        var totalGastoPersonal = recursos.Where(r => r.PlanProgramatico.PlanProgramaticoParent.PlanProgramaticoParent.PlanProgramaticoParent.Codigo == "20000").Sum(r => r.Monto);
                        var totalGastoOfAdm = recursos.Where(r => r.PlanProgramatico.PlanProgramaticoParent.PlanProgramaticoParent.PlanProgramaticoParent.Codigo == "30000").Sum(r => r.Monto);
                        var totalGastoFinanciamiento = recursos.Where(r => r.PlanProgramatico.PlanProgramaticoParent.PlanProgramaticoParent.PlanProgramaticoParent.Codigo == "40000").Sum(r => r.Monto);
                        var totalIngresoFinanciamiento = recursos.Where(r => r.PlanProgramatico.PlanProgramaticoParent.PlanProgramaticoParent.PlanProgramaticoParent.Codigo == "50000").Sum(r => r.Monto);
                        var totalSubsidioRequerido = (totalGastosServicios + totalGastoPersonal + totalGastoOfAdm + totalGastoFinanciamiento) - totalIngresoFinanciamiento;

                        sheet.Cells[7, 2].Value = totalGastosServicios;
                        sheet.Cells[8, 2].Value = totalGastoPersonal;
                        sheet.Cells[9, 2].Value = totalGastoOfAdm;
                        sheet.Cells[10, 2].Value = totalGastoFinanciamiento;
                        ////sheet.InsertRow(11, 1, 10);
                        ////sheet.Cells[11, 2].Value = 5000;

                        sheet.Cells[12, 2].Value = totalIngresoFinanciamiento;
                        sheet.Cells[13, 2].Value = totalSubsidioRequerido > 0 ? totalSubsidioRequerido : 0;

                        sheet.Cells[6, 2].Calculate();
                        sheet.Cells[11, 2].Calculate();

                        ////sheet.Cells[3, 3].Value = string.Format("{0} ( {1}-{2} )",
                        ////    shipment.CustomerName,
                        ////    shipment.CustomerCode,
                        ////    shipment.DelToCode);
                        ////sheet.Cells[6, 6].Value = shipment.DestinationAddress1;
                        ////sheet.Cells[7, 6].Value = shipment.DestinationAddress2;
                        ////sheet.Cells[8, 6].Value = shipment.DestinationAddress3;
                        ////sheet.Cells[9, 6].Value = shipment.DestinationAddress4;

                        // Start Row for Detail Rows
                        int rowIndex = 12;

                        // SubSonic 2x - Data Access, load shipment details
                        ////using (IDataReader reader = SPs.RohmPortal_GetShipmentOrderStatistics(id).GetReader())
                        ////{
                        ////    while (reader.Read())
                        ////    {
                        ////        // Cell 1, Carton Count
                        ////        sheet.Cells[rowIndex, 1].Value = Int32.Parse(reader["Cartons"].ToString());
                        ////        // Cell 2, Order Number (Outline around columns 2-7 make it look like 1 column)
                        ////        sheet.Cells[rowIndex, 2].Value = reader["DNNO"].ToString().Trim();
                        ////        // Cell 8, Weight in LBS (convert KG to LBS, and rounding to whole number)
                        ////        sheet.Cells[rowIndex, 8].Value =
                        ////            Math.Round(decimal.Parse(reader["GrossWeight"].ToString()) * 2.2m);

                        ////        // Increment Row Counter
                        ////        rowIndex++;
                        ////    }
                        ////}

                        // Force formula to compute, or formulas are not updated when you convert XLSX / PDF.
                        sheet.Cells[402, 1].Calculate();
                        sheet.Cells[402, 8].Calculate();

                        // Logic to keep up to row 35 when the data is short (pretty 1 page).
                        if (rowIndex < 35)
                        {
                            sheet.DeleteRow(36, 400 - 36);
                        }
                        else if (rowIndex < 400)
                        {
                            sheet.DeleteRow(rowIndex, 400 - rowIndex);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception e)
            {
                // Log Exception
                return null;
            }
        }

        public MemoryStream GetReporteMensual(string templateDocument)
        {
            try
            {

                // Results Output
                MemoryStream output = new MemoryStream();

                // Read Template
                using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
                {
                    // Create Excel EPPlus Package based on template stream
                    using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                    {
                        // Grab the sheet with the template, sheet name is "BOL".
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Reporte"];

                        // SubSonic 2x - Data Acces, load shipment header data.
                        // Shipment shipment = Shipment.FetchByID(id);

                        //// Insert BOL header data into template
                        sheet.Cells[1, 1].Value = string.Format("R2 - Presupuesto Mensual {0}", DateTime.Now.Year);
                        sheet.Cells[4, 2].Value = "LA PAZ";

                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var recursos = dbContext.Recursos.Where(r => r.PlanProgramatico.NivelProgramaticoId == 4).ToList();

                        var recursosGastosServicios = recursos.Where(r =>r.PlanProgramatico.PlanProgramaticoParent.PlanProgramaticoParent.PlanProgramaticoParent.Codigo == "10000").ToList();
                        var totalGastosServicios = recursosGastosServicios.Sum(r => r.Monto);
                        var recursosGastosServiciosIds = recursosGastosServicios.Select(r => r.Id).ToList();
                        var totalGSEnero = dbContext.RecursosMeses.Where(rm => recursosGastosServiciosIds.Contains(rm.RecursoId) && rm.Activo).Sum(rm => rm.Enero);
                        var totalGSFebrero = dbContext.RecursosMeses.Where(rm => recursosGastosServiciosIds.Contains(rm.RecursoId) && rm.Activo).Sum(rm => rm.Febrero);
                        var totalGSMarzo = dbContext.RecursosMeses.Where(rm => recursosGastosServiciosIds.Contains(rm.RecursoId) && rm.Activo).Sum(rm => rm.Marzo);
                        var totalGSAbril = dbContext.RecursosMeses.Where(rm => recursosGastosServiciosIds.Contains(rm.RecursoId) && rm.Activo).Sum(rm => rm.Abril);
                        var totalGSMayo = dbContext.RecursosMeses.Where(rm => recursosGastosServiciosIds.Contains(rm.RecursoId) && rm.Activo).Sum(rm => rm.Mayo);
                        var totalGSJunio = dbContext.RecursosMeses.Where(rm => recursosGastosServiciosIds.Contains(rm.RecursoId) && rm.Activo).Sum(rm => rm.Junio);
                        var totalGSJulio = dbContext.RecursosMeses.Where(rm => recursosGastosServiciosIds.Contains(rm.RecursoId) && rm.Activo).Sum(rm => rm.Julio);
                        var totalGSAgosto = dbContext.RecursosMeses.Where(rm => recursosGastosServiciosIds.Contains(rm.RecursoId) && rm.Activo).Sum(rm => rm.Agosto);
                        var totalGSSeptiembre = dbContext.RecursosMeses.Where(rm => recursosGastosServiciosIds.Contains(rm.RecursoId) && rm.Activo).Sum(rm => rm.Septiembre);
                        var totalGSOctubre = dbContext.RecursosMeses.Where(rm => recursosGastosServiciosIds.Contains(rm.RecursoId) && rm.Activo).Sum(rm => rm.Octubre);
                        var totalGSNoviembre = dbContext.RecursosMeses.Where(rm => recursosGastosServiciosIds.Contains(rm.RecursoId) && rm.Activo).Sum(rm => rm.Noviembre);
                        var totalGSDiciembre = dbContext.RecursosMeses.Where(rm => recursosGastosServiciosIds.Contains(rm.RecursoId) && rm.Activo).Sum(rm => rm.Diciembre);

                        var totalGastoPersonal = recursos.Where(r => r.PlanProgramatico.PlanProgramaticoParent.PlanProgramaticoParent.PlanProgramaticoParent.Codigo == "20000").Sum(r => r.Monto);
                        var totalGastoOfAdm = recursos.Where(r => r.PlanProgramatico.PlanProgramaticoParent.PlanProgramaticoParent.PlanProgramaticoParent.Codigo == "30000").Sum(r => r.Monto);
                        var totalGastoFinanciamiento = recursos.Where(r => r.PlanProgramatico.PlanProgramaticoParent.PlanProgramaticoParent.PlanProgramaticoParent.Codigo == "40000").Sum(r => r.Monto);
                        var totalIngresoFinanciamiento = recursos.Where(r => r.PlanProgramatico.PlanProgramaticoParent.PlanProgramaticoParent.PlanProgramaticoParent.Codigo == "50000").Sum(r => r.Monto);
                        var totalSubsidioRequerido = (totalGastosServicios + totalGastoPersonal + totalGastoOfAdm + totalGastoFinanciamiento) - totalIngresoFinanciamiento;

                        sheet.Cells[7, 2].Value = totalGastosServicios;
                        sheet.Cells[7, 3].Value = totalGSEnero;
                        sheet.Cells[7, 4].Value = totalGSFebrero;
                        sheet.Cells[7, 5].Value = totalGSMarzo;
                        sheet.Cells[7, 6].Value = totalGSAbril;
                        sheet.Cells[7, 7].Value = totalGSMayo;
                        sheet.Cells[7, 8].Value = totalGSJunio;
                        sheet.Cells[7, 9].Value = totalGSJulio;
                        sheet.Cells[7, 10].Value = totalGSAgosto;
                        sheet.Cells[7, 11].Value = totalGSSeptiembre;
                        sheet.Cells[7, 12].Value = totalGSOctubre;
                        sheet.Cells[7, 13].Value = totalGSNoviembre;
                        sheet.Cells[7, 14].Value = totalGSDiciembre;


                        sheet.Cells[8, 2].Value = totalGastoPersonal;
                        sheet.Cells[9, 2].Value = totalGastoOfAdm;
                        sheet.Cells[10, 2].Value = totalGastoFinanciamiento;
                        ////sheet.InsertRow(11, 1, 10);
                        ////sheet.Cells[11, 2].Value = 5000;

                        sheet.Cells[12, 2].Value = totalIngresoFinanciamiento;
                        sheet.Cells[13, 2].Value = totalSubsidioRequerido > 0 ? totalSubsidioRequerido : 0;

                        sheet.Cells[6, 2].Calculate();
                        sheet.Cells[11, 2].Calculate();

                        ////sheet.Cells[3, 3].Value = string.Format("{0} ( {1}-{2} )",
                        ////    shipment.CustomerName,
                        ////    shipment.CustomerCode,
                        ////    shipment.DelToCode);
                        ////sheet.Cells[6, 6].Value = shipment.DestinationAddress1;
                        ////sheet.Cells[7, 6].Value = shipment.DestinationAddress2;
                        ////sheet.Cells[8, 6].Value = shipment.DestinationAddress3;
                        ////sheet.Cells[9, 6].Value = shipment.DestinationAddress4;

                        // Start Row for Detail Rows
                        int rowIndex = 12;

                        // SubSonic 2x - Data Access, load shipment details
                        ////using (IDataReader reader = SPs.RohmPortal_GetShipmentOrderStatistics(id).GetReader())
                        ////{
                        ////    while (reader.Read())
                        ////    {
                        ////        // Cell 1, Carton Count
                        ////        sheet.Cells[rowIndex, 1].Value = Int32.Parse(reader["Cartons"].ToString());
                        ////        // Cell 2, Order Number (Outline around columns 2-7 make it look like 1 column)
                        ////        sheet.Cells[rowIndex, 2].Value = reader["DNNO"].ToString().Trim();
                        ////        // Cell 8, Weight in LBS (convert KG to LBS, and rounding to whole number)
                        ////        sheet.Cells[rowIndex, 8].Value =
                        ////            Math.Round(decimal.Parse(reader["GrossWeight"].ToString()) * 2.2m);

                        ////        // Increment Row Counter
                        ////        rowIndex++;
                        ////    }
                        ////}

                        // Force formula to compute, or formulas are not updated when you convert XLSX / PDF.
                        sheet.Cells[402, 1].Calculate();
                        sheet.Cells[402, 8].Calculate();

                        // Logic to keep up to row 35 when the data is short (pretty 1 page).
                        if (rowIndex < 35)
                        {
                            sheet.DeleteRow(36, 400 - 36);
                        }
                        else if (rowIndex < 400)
                        {
                            sheet.DeleteRow(rowIndex, 400 - rowIndex);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception e)
            {
                // Log Exception
                return null;
            }
        }

        public MemoryStream GetReporteBet(string templateDocument, int facilityId, int ciudadId)
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

                        var facility = dbContext.Facilities.Find(facilityId);
                        var ciudad = dbContext.Ciudades.Find(ciudadId);

                        sheet.Cells[1, 1].Value = $"Ciudad: {ciudad.Codigo} - {ciudad.Nombre}";
                        sheet.Cells[2, 1].Value = $"Unidad de Programa: {facility.Codigo} - {facility.Nombre}";

                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var recursos = dbContext.Recursos.Where(r => r.Activo && r.FacilityId == facilityId && r.CiudadId == ciudadId).ToList();
                        if (recursos.Count > 0)
                        {
                            foreach (var recurso in recursos)
                            {
                                sheet.InsertRow(5, 1, 4);
                                sheet.Cells[5, 1].Value = string.Format("{0} - {1}", recurso.CuentaContable.Numero, recurso.CuentaContable.Nombre);
                                var descripcionFiltrada = recurso.Descripcion.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
                                sheet.Cells[5, 2].Value = new String(descripcionFiltrada);
                                sheet.Cells[5, 3].Value = recurso.CodigosAuditoria != null ? recurso.CodigosAuditoria.Codigo : string.Empty;
                                var codigoProgramatico = $"{recurso.Ciudad.Codigo}{recurso.PlanProgramatico.Codigo}{recurso.Territorio.Codigo}{recurso.Contraparte.Codigo}-/{(recurso.AccionesNacionale != null ? recurso.AccionesNacionale.Codigo : "")}*{recurso.NotasAdicionales}";
                                var codigoProgramaticoFiltrado = codigoProgramatico.Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
                                sheet.Cells[5, 4].Value = new String(codigoProgramaticoFiltrado);
                                sheet.Cells[5, 5].Value = recurso.Monto;
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

        public Recurso GetRecurso(int recursoId)
        {
            var recursoDb = dbContext.Recursos.Find(recursoId);
            MapperManager.GetInstance();

            var recurso = Mapper.Map<DataAccess.Models.Recurso, Recurso>(recursoDb);

            return recurso;
        }

        public Resultado InsertRecurso(Recurso recurso)
        {
            MapperManager.GetInstance();

            try
            {
                var recursoDb = Mapper.Map<Recurso, DataAccess.Models.Recurso>(recurso);
                recursoDb.Activo = true;
                recursoDb.FechaCreacion = DateTime.Now;
                recursoDb.FechaModificacion = DateTime.Now;
                dbContext.Recursos.Add(recursoDb);
                dbContext.SaveChanges();

                var recursoMesDb = new DataAccess.Models.RecursoMes
                {
                    RecursoId = recursoDb.Id,
                    Enero = 0,
                    Febrero = 0,
                    Marzo = 0,
                    Abril = 0,
                    Mayo = 0,
                    Junio = 0,
                    Julio = 0,
                    Agosto = 0,
                    Septiembre = 0,
                    Octubre = 0,
                    Noviembre = 0,
                    Diciembre = 0,
                    TipoPresupuesto = "P",
                    Activo = true,
                    UsuarioCreacion = recurso.UsuarioCreacion,
                    FechaCreacion = DateTime.Now,
                    UsuarioModificacion = recurso.UsuarioCreacion,
                    FechaModificacion = DateTime.Now
                };
                dbContext.RecursosMeses.Add(recursoMesDb);
                dbContext.SaveChanges();

                return new Resultado("El Recurso se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateRecurso(Recurso recurso)
        {
            MapperManager.GetInstance();

            try
            {
                var recursoDb = dbContext.Recursos.Find(recurso.Id);
                if (recursoDb != null)
                {
                    recursoDb.AccionNacionalId = recurso.AccionNacionalId;
                    recursoDb.CodigoAuditoriaId = recurso.CodigoAuditoriaId;
                    recursoDb.ContraparteId = recurso.ContraparteId;
                    recursoDb.CuentaContableId = recurso.CuentaContableId;
                    recursoDb.PlanProgramaticoId = recurso.PlanProgramaticoId;
                    recursoDb.TerritorioId = recurso.TerritorioId;
                    recursoDb.MarcoLogicoId = recurso.MarcoLogicoId;

                    recursoDb.NotasAdicionales = recurso.NotasAdicionales;
                    recursoDb.Descripcion = recurso.Descripcion;
                    recursoDb.Monto = recurso.Monto;
                    recursoDb.Cobertura = recurso.Cobertura;
                    recursoDb.IndiceTransferencia = recurso.IndiceTransferencia;

                    recursoDb.UsuarioModificacion = recurso.UsuarioModificacion;
                    recursoDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(recursoDb).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return new Resultado("El Recurso se guardó correctamente.");
                }
                return new Resultado("No se encontró el recurso especificado.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado DeleteRecurso(int id, string usuarioActual)
        {
            MapperManager.GetInstance();

            try
            {
                var recursoDb = dbContext.Recursos.Find(id);
                recursoDb.Activo = false;
                recursoDb.UsuarioModificacion = usuarioActual;
                recursoDb.FechaModificacion = DateTime.Now;
                dbContext.Entry(recursoDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El registro se eliminó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
