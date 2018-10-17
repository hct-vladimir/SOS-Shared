using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using OfficeOpenXml;
using Mapper = AutoMapper.Mapper;
using RecursoMes = Cerberus.Sos.Accounting.BusinessLogic.Entities.RecursoMes;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class RecursosMesesManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<RecursoMes> GetAllRecursosMeses()
        {
            var recursosMesesDb = dbContext.RecursosMeses.ToList();
            MapperManager.GetInstance();

            var recursosMeses = new List<RecursoMes>();
            recursosMesesDb.ForEach(p => recursosMeses.Add(Mapper.Map<DataAccess.Models.RecursoMes, RecursoMes>(p)));

            return recursosMeses;
        }

        public List<RecursoMes> GetRecursosMesesPorFacility(int facilityId)
        {
            var recursosMesesDb = dbContext.RecursosMeses.Where(r=>r.Recurso.FacilityId == facilityId && r.Recurso.Activo && r.Activo).ToList();
            MapperManager.GetInstance();

            var recursosMeses = new List<RecursoMes>();
            recursosMesesDb.ForEach(p => recursosMeses.Add(Mapper.Map<DataAccess.Models.RecursoMes, RecursoMes>(p)));

            return recursosMeses;
        }

        public List<RecursoMes> GetRecursosMesesPorFacilityCiudad(int? facilityId, int ciudadId)
        {
            var recursosMesesDb = facilityId != null ? 
                dbContext.RecursosMeses.Where(r =>r.Recurso.FacilityId == facilityId.Value && r.Recurso.CiudadId == ciudadId && r.Recurso.Activo &&r.Activo).ToList() : 
                dbContext.RecursosMeses.Where(r => r.Recurso.CiudadId == ciudadId && r.Recurso.Activo && r.Activo).ToList();

            MapperManager.GetInstance();

            var recursosMeses = new List<RecursoMes>();
            recursosMesesDb.ForEach(p => recursosMeses.Add(Mapper.Map<DataAccess.Models.RecursoMes, RecursoMes>(p)));

            return recursosMeses;
        }

        public RecursoMes GetRecursoMes(int recursoMesId)
        {
            var recursoMesDb = dbContext.RecursosMeses.Find(recursoMesId);
            MapperManager.GetInstance();

            var recursoMes = Mapper.Map<DataAccess.Models.RecursoMes, RecursoMes>(recursoMesDb);

            return recursoMes;
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
                        var recursosMeses = dbContext.RecursosMeses.Where(r => r.Activo && r.Recurso.Activo && r.Recurso.FacilityId == facilityId && r.Recurso.CiudadId == ciudadId).ToList();
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
                                sheet.Cells[5, 5].Value = recursoMes.Recurso.Monto;

                                sheet.Cells[5, 6].Value = recursoMes.Enero;
                                sheet.Cells[5, 7].Value = recursoMes.Febrero;
                                sheet.Cells[5, 8].Value = recursoMes.Marzo;
                                sheet.Cells[5, 9].Value = recursoMes.Abril;
                                sheet.Cells[5, 10].Value = recursoMes.Mayo;
                                sheet.Cells[5, 11].Value = recursoMes.Junio;
                                sheet.Cells[5, 12].Value = recursoMes.Julio;
                                sheet.Cells[5, 13].Value = recursoMes.Agosto;
                                sheet.Cells[5, 14].Value = recursoMes.Septiembre;
                                sheet.Cells[5, 15].Value = recursoMes.Octubre;
                                sheet.Cells[5, 16].Value = recursoMes.Noviembre;
                                sheet.Cells[5, 17].Value = recursoMes.Diciembre;
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

        public Resultado InsertRecursoMes(RecursoMes recursoMes)
        {
            MapperManager.GetInstance();

            try
            {
                var recursoMesDb = Mapper.Map<RecursoMes, DataAccess.Models.RecursoMes>(recursoMes);
                recursoMesDb.Activo = true;
                recursoMesDb.UsuarioCreacion = "dbo";
                recursoMesDb.FechaCreacion = DateTime.Now;
                recursoMesDb.UsuarioModificacion = "dbo";
                recursoMesDb.FechaModificacion = DateTime.Now;
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

        public Resultado UpdateRecursoMes(RecursoMes recursoMes)
        {
            MapperManager.GetInstance();

            try
            {
                var recursoMesDb = dbContext.RecursosMeses.Find(recursoMes.Id);
                if (recursoMesDb != null)
                {
                    recursoMesDb.Enero = recursoMes.Enero;
                    recursoMesDb.Febrero = recursoMes.Febrero;
                    recursoMesDb.Marzo = recursoMes.Marzo;
                    recursoMesDb.Abril = recursoMes.Abril;
                    recursoMesDb.Mayo = recursoMes.Mayo;
                    recursoMesDb.Junio = recursoMes.Junio;
                    recursoMesDb.Julio = recursoMes.Julio;
                    recursoMesDb.Agosto = recursoMes.Agosto;
                    recursoMesDb.Septiembre = recursoMes.Septiembre;
                    recursoMesDb.Octubre = recursoMes.Octubre;
                    recursoMesDb.Noviembre = recursoMes.Noviembre;
                    recursoMesDb.Diciembre = recursoMes.Diciembre;

                    recursoMesDb.UsuarioModificacion = recursoMes.UsuarioModificacion;
                    recursoMesDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(recursoMesDb).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return new Resultado("El Recurso se guardó correctamente.");
                }
                return new Resultado("No se encontró el Recurso especificado.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateCoberturaRecursoMes(RecursoMes recursoMes)
        {
            MapperManager.GetInstance();

            try
            {
                var recursoMesDb = dbContext.RecursosMeses.Find(recursoMes.Id);
                if (recursoMesDb != null)
                {
                    recursoMesDb.CoberturaEnero = recursoMes.CoberturaEnero;
                    recursoMesDb.CoberturaFebrero = recursoMes.CoberturaFebrero;
                    recursoMesDb.CoberturaMarzo = recursoMes.CoberturaMarzo;
                    recursoMesDb.CoberturaAbril = recursoMes.CoberturaAbril;
                    recursoMesDb.CoberturaMayo = recursoMes.CoberturaMayo;
                    recursoMesDb.CoberturaJunio = recursoMes.CoberturaJunio;
                    recursoMesDb.CoberturaJulio = recursoMes.CoberturaJulio;
                    recursoMesDb.CoberturaAgosto = recursoMes.CoberturaAgosto;
                    recursoMesDb.CoberturaSeptiembre = recursoMes.CoberturaSeptiembre;
                    recursoMesDb.CoberturaOctubre = recursoMes.CoberturaOctubre;
                    recursoMesDb.CoberturaNoviembre = recursoMes.CoberturaNoviembre;
                    recursoMesDb.CoberturaDiciembre = recursoMes.CoberturaDiciembre;

                    var indiceTransferencia = recursoMesDb.Recurso.IndiceTransferencia ?? 0M;

                    recursoMesDb.Enero = indiceTransferencia * recursoMes.CoberturaEnero;
                    recursoMesDb.Febrero = indiceTransferencia * recursoMes.CoberturaFebrero;
                    recursoMesDb.Marzo = indiceTransferencia * recursoMes.CoberturaMarzo;
                    recursoMesDb.Abril = indiceTransferencia * recursoMes.CoberturaAbril;
                    recursoMesDb.Mayo = indiceTransferencia * recursoMes.CoberturaMayo;
                    recursoMesDb.Junio = indiceTransferencia * recursoMes.CoberturaJunio;
                    recursoMesDb.Julio = indiceTransferencia * recursoMes.CoberturaJulio;
                    recursoMesDb.Agosto = indiceTransferencia * recursoMes.CoberturaAgosto;
                    recursoMesDb.Septiembre = indiceTransferencia * recursoMes.CoberturaSeptiembre;
                    recursoMesDb.Octubre = indiceTransferencia * recursoMes.CoberturaOctubre;
                    recursoMesDb.Noviembre = indiceTransferencia * recursoMes.CoberturaNoviembre;
                    recursoMesDb.Diciembre = indiceTransferencia * recursoMes.CoberturaDiciembre;

                    recursoMesDb.UsuarioModificacion = recursoMes.UsuarioModificacion;
                    recursoMesDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(recursoMesDb).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return new Resultado("El Recurso se guardó correctamente.");
                }
                return new Resultado("No se encontró el Recurso especificado.");
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
