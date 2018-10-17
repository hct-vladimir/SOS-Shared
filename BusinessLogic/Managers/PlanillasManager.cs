using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using OfficeOpenXml;
using PlanillaSueldo = Cerberus.Sos.Accounting.BusinessLogic.Entities.PlanillaSueldo;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class PlanillasManager : IDisposable
    {
        private PlanillasSosDBEntities dbContext = new PlanillasSosDBEntities();

        public List<RptPlanillaSueldos> GetPlanillaSueldos(int mes, int gestion)
        {
            var planillaSueldosDb = dbContext.VPLANILLA_SUELDOS.Where(p => p.MES == mes && p.GESTION == gestion.ToString()).OrderBy(p => p.ITEM).ToList();

            MapperManager.GetInstance();

            var planillas = new List<RptPlanillaSueldos>();
            planillaSueldosDb.ForEach(p => planillas.Add(Mapper.Map<VPLANILLA_SUELDOS, RptPlanillaSueldos>(p)));

            return planillas;
        }

        public PlanillaSueldo GetPlanillaSueldosByItem(int item, int mes, int gestion)
        {
            var planillaDb = dbContext.PLANILLA_SUELDOS.FirstOrDefault(p => p.ITEM == item && p.MES == mes && p.GESTION == gestion.ToString());

            MapperManager.GetInstance();

            var planilla = Mapper.Map<PLANILLA_SUELDOS, PlanillaSueldo>(planillaDb);

            return planilla;
        }

        public Resultado UpdatePlanillaSueldo(PlanillaSueldo planilla)
        {
            MapperManager.GetInstance();

            try
            {
                var planillaDb = dbContext.PLANILLA_SUELDOS.FirstOrDefault(p => p.ITEM == planilla.ITEM && p.MES == planilla.MES && p.GESTION == planilla.GESTION);

                planillaDb.MOD_RC_IVA = planilla.MOD_RC_IVA;
                planillaDb.MOD_COMISION = planilla.MOD_COMISION;
                planillaDb.MOD_AMIGOSOS = planilla.MOD_AMIGOSOS;
                planillaDb.MOD_PRESTAMOS = planilla.MOD_PRESTAMOS;
                planillaDb.MOD_OTRS_DESCUENTOS = planilla.MOD_OTRS_DESCUENTOS;
                planillaDb.MOD_OTROS_INGRESOS = planilla.MOD_OTROS_INGRESOS;

                dbContext.Entry(planillaDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El Recurso se guardó correctamente.");

            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado ImportarPlanillaSueldo(int mes, int gestion, string rutaArchivoImportado)
        {
            try
            {
                var startColumn = 1;
                var startRow = 2;

                using (ExcelPackage package = new ExcelPackage(new FileInfo(rutaArchivoImportado)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    var cell = worksheet.Cells[startRow, startColumn].Value;
                    while (cell != null)
                    {
                        var itemArchivo = Int32.Parse(cell.ToString());
                        var itemDb = dbContext.PLANILLA_SUELDOS.First(t => t.ITEM == itemArchivo && t.GESTION == gestion.ToString() && t.MES == mes);
                        if (itemDb != null)
                        {
                            itemDb.GESTION = gestion.ToString();
                            itemDb.MES = mes;
                            if (Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 1].Value) > 0)
                                itemDb.MOD_RC_IVA = Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 1].Value);

                            if (Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 2].Value) > 0)
                                itemDb.MOD_COMISION = Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 2].Value);

                            if (Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 3].Value) > 0)
                                itemDb.MOD_AMIGOSOS = Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 3].Value);

                            if (Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 4].Value) > 0)
                                itemDb.MOD_PRESTAMOS = Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 4].Value);

                            if (Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 5].Value) > 0)
                                itemDb.MOD_OTRS_DESCUENTOS = Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 5].Value);

                            if (Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 6].Value) > 0)
                                itemDb.MOD_OTROS_INGRESOS = Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 6].Value);

                            if (Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 6].Value) > 0)
                                itemDb.MOD_MONTO_NETO = Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 7].Value);

                            if (Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 7].Value) > 0)
                                itemDb.MOD_ASIGSOS = Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 8].Value);

                            dbContext.Entry(itemDb).State = EntityState.Modified;
                            dbContext.SaveChanges();
                        }
                        startRow++;
                        cell = worksheet.Cells[startRow, startColumn].Value;
                    }
                }
                return new Resultado("El Recurso se guardó correctamente.");

            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Resultado GenerarPlanillaSueldo()
        {
            try
            {
                var resultado = dbContext.GENERA_PLANILLA(1);

                return new Resultado("Se generó correctamente");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Resultado ProcesarPlanillaSueldo()
        {
            try
            {
                var resultado = dbContext.GENERA_PLANILLA(2);

                return new Resultado("Se generó correctamente");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Resultado GenerarPlanillaRcIva()
        {
            try
            {
                var resultado = dbContext.GENERA_PLANILLA_RCIVA(1);

                return new Resultado("Se procesó correctamente");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Resultado ProcesarPlanillaRcIva()
        {
            try
            {
                var resultado = dbContext.GENERA_PLANILLA_RCIVA(2);

                return new Resultado("Se procesó correctamente");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado SetFechaProceso(DateTime fechaProceso)
        {
            var parametroDb = dbContext.PARAMETROS.FirstOrDefault(p => p.COD_PARAMETRO == "FECHA_PROCESO");
            try
            {
                parametroDb.DES_PARAMETRO = fechaProceso.ToString("yyyyMMdd");

                dbContext.Entry(parametroDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("Se actualizó correctamente");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }


        public DateTime GetFechaProceso()
        {
            var parametroDb = dbContext.PARAMETROS.FirstOrDefault(p => p.COD_PARAMETRO == "FECHA_PROCESO");

            MapperManager.GetInstance();

            var parametro = Mapper.Map<PARAMETRO, PlanillaParametro>(parametroDb);
            var fechaProceso = DateTime.ParseExact(parametro.DES_PARAMETRO, "yyyyMMdd", null);
            return fechaProceso;
        }

        public MemoryStream GetReportePlanillaSueldosGeneral(string templateDocument, int mes, int gestion)
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
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Sueldos"];


                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var rptPlanillaSueldos = dbContext.VPLANILLA_SUELDOS_GENERAL.Where(p => p.MES == mes && p.GESTION == gestion.ToString()).OrderByDescending(p => p.ITEM).ToList();
                        if (rptPlanillaSueldos.Count > 0)
                        {
                            var fila = rptPlanillaSueldos.Count;
                            foreach (var planilla in rptPlanillaSueldos)
                            {
                                sheet.InsertRow(3, 1, 2);
                                sheet.Cells[3, 1].Value = fila--;
                                sheet.Cells[3, 2].Value = planilla.ITEM;
                                sheet.Cells[3, 3].Value = planilla.FECHA_NACIMIENTO;
                                sheet.Cells[3, 4].Value = planilla.SEXO;
                                sheet.Cells[3, 5].Value = planilla.MES;
                                sheet.Cells[3, 6].Value = planilla.GESTION;
                                sheet.Cells[3, 7].Value = planilla.NOMBRE;
                                sheet.Cells[3, 8].Value = planilla.CARGO;
                                sheet.Cells[3, 9].Value = planilla.NUMERO_DOC;
                                sheet.Cells[3, 10].Value = planilla.EXPEDIDO;
                                sheet.Cells[3, 11].Value = planilla.FECHA_INGRESO;
                                sheet.Cells[3, 12].Value = planilla.DIAS_PAG;
                                sheet.Cells[3, 13].Value = planilla.NUMERO_CUENTA;
                                sheet.Cells[3, 14].Value = planilla.HABER_BASICO;
                                sheet.Cells[3, 15].Value = planilla.BASICO_GANADO;
                                sheet.Cells[3, 16].Value = planilla.COD_FILIAL;
                                sheet.Cells[3, 17].Value = planilla.FILIAL;
                                sheet.Cells[3, 18].Value = planilla.PROGRAMA;
                                sheet.Cells[3, 19].Value = planilla.COD_SECCION;
                                sheet.Cells[3, 20].Value = planilla.SECCION;
                                sheet.Cells[3, 21].Value = planilla.CAL_BONO_ANTIGUEDAD;
                                sheet.Cells[3, 22].Value = planilla.MOD_RC_IVA;
                                sheet.Cells[3, 23].Value = planilla.MOD_COMISION;
                                sheet.Cells[3, 24].Value = planilla.MOD_AMIGOSOS;
                                sheet.Cells[3, 25].Value = planilla.MOD_PRESTAMOS;
                                sheet.Cells[3, 26].Value = planilla.MOD_OTRS_DESCUENTOS;
                                sheet.Cells[3, 27].Value = planilla.CAL_TOTAL_GANADO;
                                sheet.Cells[3, 28].Value = planilla.CAL_APONALSOL;
                                sheet.Cells[3, 29].Value = planilla.CAL_AFP;
                                sheet.Cells[3, 30].Value = planilla.CAL_APO_SOL_ASEG;
                                sheet.Cells[3, 31].Value = planilla.COD_ESCALA_SAL;
                                sheet.Cells[3, 32].Value = planilla.CAL_IMPUESTO_RETENIDO;
                                sheet.Cells[3, 33].Value = planilla.CODIGO_FACILITY;
                                sheet.Cells[3, 34].Value = planilla.NOM_FACILITY;
                                sheet.Cells[3, 35].Value = planilla.NOM_FACPROYECTO;
                                sheet.Cells[3, 36].Value = planilla.AREA;
                                sheet.Cells[3, 37].Value = planilla.CAL_APONACSOL_1;
                                sheet.Cells[3, 38].Value = planilla.CAL_APONACSOL_2;
                                sheet.Cells[3, 39].Value = planilla.CAL_APONACSOL_3;
                                sheet.Cells[3, 40].Value = planilla.ITEM_2;
                                sheet.Cells[3, 41].Value = planilla.NACIONALIDAD;
                                sheet.Cells[3, 42].Value = planilla.TIPO_SEGURO;
                                sheet.Cells[3, 43].Value = planilla.COTIZANTE;
                                sheet.Cells[3, 44].Value = planilla.TIPO_AFP;

                            }
                            sheet.DeleteRow(2);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                throw;
            }
        }


        public MemoryStream GetReportePlanillaSueldos(string templateDocument, int mes, int gestion)
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
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Sueldos"];


                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var rptPlanillaSueldos = dbContext.VPLANILLA_SUELDOS.Where(p => p.MES == mes && p.GESTION == gestion.ToString()).OrderByDescending(p => p.ITEM).ToList();
                        if (rptPlanillaSueldos.Count > 0)
                        {
                            var fila = rptPlanillaSueldos.Count;
                            foreach (var planilla in rptPlanillaSueldos)
                            {
                                sheet.InsertRow(3, 1, 2);
                                sheet.Cells[3, 1].Value = fila--;
                                sheet.Cells[3, 2].Value = planilla.ITEM;
                                sheet.Cells[3, 3].Value = planilla.NOMBRE;
                                sheet.Cells[3, 4].Value = planilla.CARGO;
                                sheet.Cells[3, 5].Value = planilla.FECHA_INGRESO;
                                sheet.Cells[3, 6].Value = planilla.DIAS_PAG;
                                sheet.Cells[3, 7].Value = planilla.HABER_BASICO;
                                sheet.Cells[3, 8].Value = planilla.BASICO_GANADO;
                                sheet.Cells[3, 9].Value = planilla.CAL_BONO_ANTIGUEDAD;
                                sheet.Cells[3, 10].Value = planilla.MOD_ASIGSOS;
                                sheet.Cells[3, 11].Value = planilla.MOD_COMISION;
                                sheet.Cells[3, 12].Value = planilla.CAL_TOTAL_GANADO;
                                sheet.Cells[3, 13].Value = planilla.MOD_RC_IVA;
                                sheet.Cells[3, 14].Value = planilla.CAL_AFP;
                                sheet.Cells[3, 15].Value = planilla.CAL_APO_SOL_ASEG;
                                sheet.Cells[3, 16].Value = planilla.CAL_APONALSOL;
                                sheet.Cells[3, 17].Value = planilla.MOD_PRESTAMOS;
                                sheet.Cells[3, 18].Value = planilla.MOD_OTRS_DESCUENTOS;
                                sheet.Cells[3, 19].Value = planilla.MOD_AMIGOSOS;
                                sheet.Cells[3, 20].Value = planilla.CAL_TOTAL_DESCUENTO;
                                sheet.Cells[3, 21].Value = planilla.CAL_LIQ_PAGABLE;
                                sheet.Cells[3, 22].Value = planilla.FILIAL;
                                sheet.Cells[3, 23].Value = planilla.PROGRAMA;
                                sheet.Cells[3, 24].Value = planilla.NUMERO_DOC;
                                sheet.Cells[3, 25].Value = planilla.EXPEDIDO;
                                sheet.Cells[3, 26].Value = planilla.COD_ESCALA_SAL;
                                sheet.Cells[3, 27].Value = planilla.NUMERO_CUENTA;

                            }
                            sheet.DeleteRow(2);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                throw;
            }
        }

        // ECC 13/10/18 obtencion de datos de planillaafpRcIva
        public List<RptPlanillaRcIva> GetPlanillaRcIva(string mesCierre)
        {
            var planillaAfpDb = dbContext.VPLANILLA_RCIVA.Where(r => r.MES_CIERRE == mesCierre).OrderByDescending(r => r.ITEM).ToList();
            MapperManager.GetInstance();
            var planillas = new List<RptPlanillaRcIva>();
            planillaAfpDb.ForEach(p => planillas.Add(Mapper.Map<VPLANILLA_RCIVA, RptPlanillaRcIva>(p)));
            return planillas;
        }
        public MemoryStream GetReportePlanillaRcIva(string templateDocument, string mesCierre)
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
                        ExcelWorksheet sheet = package.Workbook.Worksheets["RC-IVA"];


                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var rptPlanillaRcIva = dbContext.VPLANILLA_RCIVA.Where(r => r.MES_CIERRE == mesCierre).OrderByDescending(r => r.ITEM).ToList();
                        if (rptPlanillaRcIva.Count > 0)
                        {
                            var fila = rptPlanillaRcIva.Count;
                            foreach (var planilla in rptPlanillaRcIva)
                            {
                                sheet.InsertRow(3, 1, 2);
                                sheet.Cells[3, 1].Value = fila--;
                                sheet.Cells[3, 2].Value = planilla.ITEM;
                                sheet.Cells[3, 3].Value = planilla.NOMBRE;
                                sheet.Cells[3, 4].Value = planilla.CAL_SUELDO_NETO;
                                sheet.Cells[3, 5].Value = planilla.MOD_OTR_INGRESOS;
                                sheet.Cells[3, 6].Value = planilla.CAL_SUELDO_TOTAL_NETO;
                                sheet.Cells[3, 7].Value = planilla.CAL_MIN_NO_IMPONIBLE;
                                sheet.Cells[3, 8].Value = planilla.CAL_SUJETO_IMP;
                                sheet.Cells[3, 9].Value = planilla.CAL_IMPUESTO;
                                sheet.Cells[3, 10].Value = planilla.MOD_COMPUTO_IVA;
                                sheet.Cells[3, 11].Value = planilla.CAL_POR_NRO_SMN;
                                sheet.Cells[3, 12].Value = planilla.CAL_SALDO_FISCO;
                                sheet.Cells[3, 13].Value = planilla.CAL_A_FAVOR_DEP;
                                sheet.Cells[3, 14].Value = planilla.CAL_SLD_MES_ANT;
                                sheet.Cells[3, 15].Value = planilla.CAL_ACT_MES_ANT;
                                sheet.Cells[3, 16].Value = planilla.CAL_VALOR_ACT;
                                sheet.Cells[3, 17].Value = planilla.CAL_SLD_TOTAL_DEP;
                                sheet.Cells[3, 18].Value = planilla.CAL_SLD_UTILIZADO;
                                sheet.Cells[3, 19].Value = planilla.CAL_IMP_RETENIDO;
                                sheet.Cells[3, 20].Value = planilla.CAL_SLD_FAVOR_PROX_MES;
                                sheet.Cells[3, 21].Value = planilla.TOTAL_GANADO;
                                sheet.Cells[3, 22].Value = planilla.CI;
                                sheet.Cells[3, 23].Value = planilla.EXTENDIDO;
                                sheet.Cells[3, 24].Value = planilla.NROCUENTA;
                                sheet.Cells[3, 25].Value = planilla.FILIAL;
                                sheet.Cells[3, 26].Value = planilla.PROGRAMA;
                                sheet.Cells[3, 27].Value = planilla.SECCION;
                            }
                            sheet.DeleteRow(2);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                throw;
            }
        }

        //GetReportePlanillaAfpFuturo
        public MemoryStream GetReportePlanillaAfpFuturo(string templateDocument, string mesCierre)
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
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Futuro"];

                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var rptPlanillaSueldos = dbContext.VPLANILLA_AFP_FUTURO.Where(p => p.MES_CIERRE == mesCierre).OrderByDescending(p => p.ITEM).ToList();
                        if (rptPlanillaSueldos.Count > 0)
                        {
                            var fila = rptPlanillaSueldos.Count;
                            foreach (var planilla in rptPlanillaSueldos)
                            {
                                sheet.InsertRow(3, 1, 2);
                                sheet.Cells[3, 1].Value = fila--;
                                sheet.Cells[3, 2].Value = planilla.CI;
                                sheet.Cells[3, 3].Value = planilla.EXT;
                                sheet.Cells[3, 4].Value = planilla.NOMBRE;
                                sheet.Cells[3, 5].Value = planilla.CARGO;
                                sheet.Cells[3, 6].Value = planilla.DIAS_TRAB;
                                sheet.Cells[3, 7].Value = planilla.FECHA_INGRESO;
                                sheet.Cells[3, 8].Value = planilla.TOTAL_GANADO;
                                sheet.Cells[3, 9].Value = planilla.NOVEDAD;
                                sheet.Cells[3, 10].Value = planilla.FECHANOVEDAD;
                                sheet.Cells[3, 11].Value = planilla.AFP_12_21;
                                sheet.Cells[3, 12].Value = planilla.AFP_SOL_5;
                                sheet.Cells[3, 13].Value = planilla.APO_SOL_1;
                                sheet.Cells[3, 14].Value = planilla.APO_SOL_5;
                                sheet.Cells[3, 15].Value = planilla.APO_SOL_10;
                                sheet.Cells[3, 16].Value = planilla.APO_PAT;
                                sheet.Cells[3, 17].Value = planilla.APO_PRO_VIV;
                                sheet.Cells[3, 18].Value = planilla.APO_PAT_SOL;
                                sheet.Cells[3, 19].Value = planilla.APELLIDO_PATERNO;
                                sheet.Cells[3, 20].Value = planilla.APELLIDO_MATERNO;
                                sheet.Cells[3, 21].Value = planilla.APELLIDO_CASADA;
                                sheet.Cells[3, 22].Value = planilla.NOMBRES;
                                sheet.Cells[3, 23].Value = planilla.NUA;
                                sheet.Cells[3, 24].Value = planilla.FILIAL;
                                sheet.Cells[3, 25].Value = planilla.TIPO_DOC;
                                sheet.Cells[3, 26].Value = planilla.PROGRAMA;
                            }
                            sheet.DeleteRow(2);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                throw;
            }
        }

        // ECC 13/10/18 obtencion de datos de planillaafpfuturo
        public List<RptPlanillaAfpFuturo> GetPlanillaAfpFuturo(int mes, int gestion)
        {
            var planillaAfpDb = dbContext.VPLANILLA_AFP_FUTURO.Where(p => p.MES == mes && p.GESTION == gestion.ToString()).OrderBy(p => p.ITEM).ToList();
            MapperManager.GetInstance();
            var planillas = new List<RptPlanillaAfpFuturo>();
            planillaAfpDb.ForEach(p => planillas.Add(Mapper.Map<VPLANILLA_AFP_FUTURO, RptPlanillaAfpFuturo>(p)));
            return planillas;
        }

        public MemoryStream GetReportePlanillaAfpPrevision(string templateDocument, string mesCierre)
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
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Prevision"];

                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var rptPlanillaSueldos = dbContext.VPLANILLA_AFP_PREVISION.Where(p => p.MES_CIERRE == mesCierre).OrderByDescending(p => p.ITEM).ToList();
                        if (rptPlanillaSueldos.Count > 0)
                        {
                            var fila = rptPlanillaSueldos.Count;
                            foreach (var planilla in rptPlanillaSueldos)
                            {
                                sheet.InsertRow(3, 1, 2);
                                sheet.Cells[3, 1].Value = fila--;
                                sheet.Cells[3, 2].Value = planilla.CI;
                                sheet.Cells[3, 3].Value = planilla.EXT;
                                sheet.Cells[3, 4].Value = planilla.NOMBRE;
                                sheet.Cells[3, 5].Value = planilla.CARGO;
                                sheet.Cells[3, 6].Value = planilla.DIAS_TRAB;
                                sheet.Cells[3, 7].Value = planilla.FECHA_INGRESO;
                                sheet.Cells[3, 8].Value = planilla.TOTAL_GANADO;
                                sheet.Cells[3, 9].Value = planilla.NOVEDAD;
                                sheet.Cells[3, 10].Value = planilla.FECHANOVEDAD;
                                sheet.Cells[3, 11].Value = planilla.AFP_12_21;
                                sheet.Cells[3, 12].Value = planilla.AFP_SOL_5;
                                sheet.Cells[3, 13].Value = planilla.APO_SOL_1;
                                sheet.Cells[3, 14].Value = planilla.APO_SOL_5;
                                sheet.Cells[3, 15].Value = planilla.APO_SOL_10;
                                sheet.Cells[3, 16].Value = planilla.APO_PAT;
                                sheet.Cells[3, 17].Value = planilla.APO_PRO_VIV;
                                sheet.Cells[3, 18].Value = planilla.APO_PAT_SOL;
                                sheet.Cells[3, 19].Value = planilla.APELLIDO_PATENRO;
                                sheet.Cells[3, 20].Value = planilla.APELLIDO_MATERNO;
                                sheet.Cells[3, 21].Value = planilla.APELLIDO_CASADA;
                                sheet.Cells[3, 22].Value = planilla.NOMBRES;
                                sheet.Cells[3, 23].Value = planilla.NUA;
                                sheet.Cells[3, 24].Value = planilla.FILIAL;
                                sheet.Cells[3, 25].Value = planilla.TIPO_DOC;
                                sheet.Cells[3, 26].Value = planilla.PROG_PROGRAMA;
                            }
                            sheet.DeleteRow(2);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                throw;
            }
        }

        // ECC 13/10/18 obtencion de datos de planillaafpprevision
        public List<RptPlanillaAfpPrevision> GetPlanillaAfpPrevision(int mes, int gestion)
        {
            var planillaAfpDb = dbContext.VPLANILLA_AFP_PREVISION.Where(p => p.MES == mes && p.GESTION == gestion.ToString()).OrderBy(p => p.ITEM).ToList();
            MapperManager.GetInstance();
            var planillas = new List<RptPlanillaAfpPrevision>();
            planillaAfpDb.ForEach(p => planillas.Add(Mapper.Map<VPLANILLA_AFP_PREVISION, RptPlanillaAfpPrevision>(p)));
            return planillas;
        }

        /// <summary>
        /// Planilla de Aportes de Salud Cochabamba.
        /// </summary>
        /// <param name="templateDocument"></param>
        /// <param name="mes"></param>
        /// <param name="gestion"></param>
        /// <returns></returns>
        public MemoryStream GetReportePlanillaSalud(string templateDocument, int mes, int gestion)
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
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Salud"];

                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var rptPlanillaSalud = dbContext.VPLANILLA_APORTES_SALUD
                            .Where(p => p.MES == mes && p.GESTION == gestion.ToString() && p.FILIAL == "Cochabamba")
                            .OrderByDescending(p => p.ITEM).ToList();
                        if (rptPlanillaSalud.Count > 0)
                        {
                            var fila = rptPlanillaSalud.Count;
                            foreach (var planilla in rptPlanillaSalud)
                            {
                                sheet.InsertRow(3, 1, 2);
                                sheet.Cells[3, 1].Value = fila--;
                                sheet.Cells[3, 2].Value = planilla.ITEM;
                                sheet.Cells[3, 3].Value = planilla.NUMERO_DOC;
                                sheet.Cells[3, 4].Value = planilla.EXT;
                                sheet.Cells[3, 5].Value = planilla.APELLIDO_PATERNO;
                                sheet.Cells[3, 6].Value = planilla.APELLIDO_MATERNO;
                                sheet.Cells[3, 7].Value = planilla.NOMBRES;
                                sheet.Cells[3, 8].Value = planilla.CARGO;
                                sheet.Cells[3, 9].Value = planilla.FECHA_INGRESO;
                                sheet.Cells[3, 10].Value = planilla.DIAS_TRAB;
                                sheet.Cells[3, 11].Value = planilla.SUELDO_BASICO;
                                sheet.Cells[3, 12].Value = planilla.BONO_ANTIGUEDAD;
                                sheet.Cells[3, 13].Value = planilla.ASIGNACION_SOS;
                                sheet.Cells[3, 14].Value = planilla.COMISION;
                                sheet.Cells[3, 15].Value = planilla.TOTAL_GANADO;
                                sheet.Cells[3, 16].Value = planilla.RC_IVA;
                                sheet.Cells[3, 17].Value = planilla.AFP;
                                sheet.Cells[3, 18].Value = planilla.SOLIDARIO;
                                sheet.Cells[3, 19].Value = planilla.AMIGO_SOS;
                                sheet.Cells[3, 20].Value = planilla.TOTAL_DESCUENTO;
                                sheet.Cells[3, 21].Value = planilla.LIQUIDO_PAGABLE;
                                sheet.Cells[3, 22].Value = planilla.FECHA_NAC;
                                sheet.Cells[3, 23].Value = planilla.SEXO;
                                sheet.Cells[3, 24].Value = planilla.NUMERO_CUENTA;
                                sheet.Cells[3, 25].Value = planilla.FILIAL;

                            }
                            sheet.DeleteRow(2);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                throw;
            }
        }

        // ECC 16/10/18 obtencion de datos de planillasaludla paz
        public List<RptPlanillaAportesSalud> GetReportePlanillaSaludLaPaz(int mes, int gestion)
        {
            var planillaSalud = dbContext.VPLANILLA_APORTES_SALUD
                .Where(p => p.MES == mes && p.GESTION == gestion.ToString() && (p.FILIAL == "La Paz" || p.FILIAL == "El Alto"))
                .OrderByDescending(p => p.ITEM).ToList();
            MapperManager.GetInstance();
            var planillas = new List<RptPlanillaAportesSalud>();
            planillaSalud.ForEach(p => planillas.Add(Mapper.Map<VPLANILLA_APORTES_SALUD, RptPlanillaAportesSalud>(p)));
            return planillas;
        }
        public MemoryStream GetReportePlanillaSaludLp(string templateDocument, int mes, int gestion)
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
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Salud"];

                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var rptPlanillaSalud = dbContext.VPLANILLA_APORTES_SALUD
                            .Where(p => p.MES == mes && p.GESTION == gestion.ToString() && (p.FILIAL == "La Paz" || p.FILIAL == "El Alto"))
                            .OrderByDescending(p => p.ITEM).ToList();
                        if (rptPlanillaSalud.Count > 0)
                        {
                            var fila = rptPlanillaSalud.Count;
                            foreach (var planilla in rptPlanillaSalud)
                            {
                                sheet.InsertRow(3, 1, 2);
                                sheet.Cells[3, 1].Value = fila--;
                                sheet.Cells[3, 2].Value = planilla.ITEM;
                                sheet.Cells[3, 3].Value = planilla.NUMERO_DOC;
                                sheet.Cells[3, 4].Value = planilla.EXT;
                                sheet.Cells[3, 5].Value = planilla.APELLIDO_PATERNO;
                                sheet.Cells[3, 6].Value = planilla.APELLIDO_MATERNO;
                                sheet.Cells[3, 7].Value = planilla.NOMBRES;
                                sheet.Cells[3, 8].Value = planilla.CARGO;
                                sheet.Cells[3, 9].Value = planilla.FECHA_INGRESO;
                                sheet.Cells[3, 10].Value = planilla.DIAS_TRAB;
                                sheet.Cells[3, 11].Value = planilla.SUELDO_BASICO;
                                sheet.Cells[3, 12].Value = planilla.BONO_ANTIGUEDAD;
                                sheet.Cells[3, 13].Value = planilla.ASIGNACION_SOS;
                                sheet.Cells[3, 14].Value = planilla.COMISION;
                                sheet.Cells[3, 15].Value = planilla.TOTAL_GANADO;
                                sheet.Cells[3, 16].Value = planilla.RC_IVA;
                                sheet.Cells[3, 17].Value = planilla.AFP;
                                sheet.Cells[3, 18].Value = planilla.SOLIDARIO;
                                sheet.Cells[3, 19].Value = planilla.AMIGO_SOS;
                                sheet.Cells[3, 20].Value = planilla.TOTAL_DESCUENTO;
                                sheet.Cells[3, 21].Value = planilla.LIQUIDO_PAGABLE;
                                sheet.Cells[3, 22].Value = planilla.FECHA_NAC;
                                sheet.Cells[3, 23].Value = planilla.SEXO;
                                sheet.Cells[3, 24].Value = planilla.NUMERO_CUENTA;
                                sheet.Cells[3, 25].Value = planilla.FILIAL;

                            }
                            sheet.DeleteRow(2);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                throw;
            }
        }

        // ECC 13/10/18 obtencion de datos de planillasaludoruro
        public List<RptPlanillaAportesSalud> GetReportePlanillaSaludOruro(int mes, int gestion)
        {
            var planillaSalud = dbContext.VPLANILLA_APORTES_SALUD
                .Where(p => p.MES == mes && p.GESTION == gestion.ToString() && p.FILIAL == "Oruro")
                .OrderByDescending(p => p.ITEM).ToList();
            MapperManager.GetInstance();
            var planillas = new List<RptPlanillaAportesSalud>();
            planillaSalud.ForEach(p => planillas.Add(Mapper.Map<VPLANILLA_APORTES_SALUD, RptPlanillaAportesSalud>(p)));
            return planillas;
        }
        public MemoryStream GetReportePlanillaSaludOr(string templateDocument, int mes, int gestion)
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
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Salud"];

                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var rptPlanillaSalud = dbContext.VPLANILLA_APORTES_SALUD
                            .Where(p => p.MES == mes && p.GESTION == gestion.ToString() && p.FILIAL == "Oruro")
                            .OrderByDescending(p => p.ITEM).ToList();
                        if (rptPlanillaSalud.Count > 0)
                        {
                            var fila = rptPlanillaSalud.Count;
                            foreach (var planilla in rptPlanillaSalud)
                            {
                                sheet.InsertRow(3, 1, 2);
                                sheet.Cells[3, 1].Value = fila--;
                                sheet.Cells[3, 2].Value = planilla.ITEM;
                                sheet.Cells[3, 3].Value = planilla.NUMERO_DOC;
                                sheet.Cells[3, 4].Value = planilla.EXT;
                                sheet.Cells[3, 5].Value = $"{planilla.APELLIDO_PATERNO} {planilla.APELLIDO_MATERNO} {planilla.NOMBRES}";
                                sheet.Cells[3, 6].Value = planilla.CARGO;
                                sheet.Cells[3, 7].Value = planilla.FECHA_INGRESO;
                                sheet.Cells[3, 8].Value = planilla.DIAS_TRAB;
                                sheet.Cells[3, 9].Value = planilla.SUELDO_BASICO;
                                sheet.Cells[3, 10].Value = planilla.BONO_ANTIGUEDAD;
                                sheet.Cells[3, 11].Value = planilla.ASIGNACION_SOS;
                                sheet.Cells[3, 12].Value = planilla.COMISION;
                                sheet.Cells[3, 13].Value = planilla.TOTAL_GANADO;
                                sheet.Cells[3, 14].Value = planilla.RC_IVA;
                                sheet.Cells[3, 15].Value = planilla.AFP;
                                sheet.Cells[3, 16].Value = planilla.SOLIDARIO;
                                sheet.Cells[3, 17].Value = planilla.AMIGO_SOS;
                                sheet.Cells[3, 18].Value = planilla.TOTAL_DESCUENTO;
                                sheet.Cells[3, 19].Value = planilla.LIQUIDO_PAGABLE;
                                sheet.Cells[3, 20].Value = planilla.FECHA_NAC;
                                sheet.Cells[3, 21].Value = planilla.NUMERO_CUENTA;
                            }
                            sheet.DeleteRow(2);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                throw;
            }
        }

        // ECC 16/10/18 obtencion de datos de planillasalud
        public List<RptPlanillaAportesSalud> GetReportePlanillaSaludSantaCruz(int mes, int gestion)
        {
            var planillaSalud = dbContext.VPLANILLA_APORTES_SALUD
                .Where(p => p.MES == mes && p.GESTION == gestion.ToString() && p.FILIAL == "Santa Cruz")
                .OrderByDescending(p => p.ITEM).ToList();
            MapperManager.GetInstance();
            var planillas = new List<RptPlanillaAportesSalud>();
            planillaSalud.ForEach(p => planillas.Add(Mapper.Map<VPLANILLA_APORTES_SALUD, RptPlanillaAportesSalud>(p)));
            return planillas;
        }
        public MemoryStream GetReportePlanillaSaludSc(string templateDocument, int mes, int gestion)
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
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Salud"];

                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var rptPlanillaSalud = dbContext.VPLANILLA_APORTES_SALUD
                            .Where(p => p.MES == mes && p.GESTION == gestion.ToString() && p.FILIAL == "Santa Cruz")
                            .OrderByDescending(p => p.ITEM).ToList();
                        if (rptPlanillaSalud.Count > 0)
                        {
                            var fila = rptPlanillaSalud.Count;
                            foreach (var planilla in rptPlanillaSalud)
                            {
                                sheet.InsertRow(3, 1, 2);
                                sheet.Cells[3, 1].Value = fila--;
                                sheet.Cells[3, 2].Value = planilla.ITEM;
                                sheet.Cells[3, 3].Value = planilla.NUMERO_DOC;
                                sheet.Cells[3, 4].Value = planilla.EXT;
                                sheet.Cells[3, 5].Value = $"{planilla.APELLIDO_PATERNO} {planilla.APELLIDO_MATERNO} {planilla.NOMBRES}";
                                sheet.Cells[3, 6].Value = planilla.CARGO;
                                sheet.Cells[3, 7].Value = planilla.FECHA_INGRESO;
                                sheet.Cells[3, 8].Value = planilla.DIAS_TRAB;
                                sheet.Cells[3, 9].Value = planilla.SUELDO_BASICO;
                                sheet.Cells[3, 10].Value = planilla.BASICO_GANADO;
                                sheet.Cells[3, 11].Value = planilla.BONO_ANTIGUEDAD;
                                sheet.Cells[3, 12].Value = planilla.ASIGNACION_SOS;
                                sheet.Cells[3, 13].Value = planilla.COMISION;
                                sheet.Cells[3, 14].Value = planilla.TOTAL_GANADO;
                                sheet.Cells[3, 15].Value = planilla.RC_IVA;
                                sheet.Cells[3, 16].Value = planilla.AFP_1221;
                                sheet.Cells[3, 17].Value = planilla.AFP_050;
                                sheet.Cells[3, 18].Value = planilla.SOLIDARIO;
                                sheet.Cells[3, 19].Value = planilla.OTROS_DESCUENTOS;
                                sheet.Cells[3, 20].Value = planilla.AMIGO_SOS;
                                sheet.Cells[3, 21].Value = planilla.TOTAL_DESCUENTO;
                                sheet.Cells[3, 22].Value = planilla.LIQUIDO_PAGABLE;
                                sheet.Cells[3, 23].Value = planilla.NUMERO_CUENTA;
                            }
                            sheet.DeleteRow(2);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                throw;
            }
        }

        // ECC 16/10/18 obtencion de datos de planillasalud
        public List<RptPlanillaAportesSalud> GetReportePlanillaSaludSucre(int mes, int gestion)
        {
            var planillaSalud = dbContext.VPLANILLA_APORTES_SALUD
                .Where(p => p.MES == mes && p.GESTION == gestion.ToString() && p.FILIAL == "Sucre")
                .OrderByDescending(p => p.ITEM).ToList();
            MapperManager.GetInstance();
            var planillas = new List<RptPlanillaAportesSalud>();
            planillaSalud.ForEach(p => planillas.Add(Mapper.Map<VPLANILLA_APORTES_SALUD, RptPlanillaAportesSalud>(p)));
            return planillas;
        }
        public MemoryStream GetReportePlanillaSaludSr(string templateDocument, int mes, int gestion)
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
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Salud"];

                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var rptPlanillaSalud = dbContext.VPLANILLA_APORTES_SALUD
                            .Where(p => p.MES == mes && p.GESTION == gestion.ToString() && p.FILIAL == "Sucre")
                            .OrderByDescending(p => p.ITEM).ToList();
                        if (rptPlanillaSalud.Count > 0)
                        {
                            var fila = rptPlanillaSalud.Count;
                            foreach (var planilla in rptPlanillaSalud)
                            {
                                sheet.InsertRow(3, 1, 2);
                                sheet.Cells[3, 1].Value = fila--;
                                sheet.Cells[3, 2].Value = planilla.ITEM;
                                sheet.Cells[3, 3].Value = planilla.NUMERO_DOC;
                                sheet.Cells[3, 4].Value = planilla.EXT;
                                sheet.Cells[3, 5].Value = $"{planilla.APELLIDO_PATERNO} {planilla.APELLIDO_MATERNO} {planilla.NOMBRES}";
                                sheet.Cells[3, 6].Value = planilla.CARGO;
                                sheet.Cells[3, 7].Value = planilla.FECHA_INGRESO;
                                sheet.Cells[3, 8].Value = planilla.DIAS_TRAB;
                                sheet.Cells[3, 9].Value = planilla.SUELDO_BASICO;
                                sheet.Cells[3, 10].Value = planilla.BONO_ANTIGUEDAD;
                                sheet.Cells[3, 11].Value = planilla.ASIGNACION_SOS;
                                sheet.Cells[3, 12].Value = planilla.COMISION;
                                sheet.Cells[3, 13].Value = planilla.TOTAL_GANADO;
                                sheet.Cells[3, 14].Value = planilla.RC_IVA;
                                sheet.Cells[3, 15].Value = planilla.AFP;
                                sheet.Cells[3, 16].Value = planilla.SOLIDARIO;
                                sheet.Cells[3, 17].Value = planilla.AMIGO_SOS;
                                sheet.Cells[3, 18].Value = planilla.TOTAL_DESCUENTO;
                                sheet.Cells[3, 19].Value = planilla.LIQUIDO_PAGABLE;
                                sheet.Cells[3, 20].Value = planilla.FECHA_NAC;
                                sheet.Cells[3, 21].Value = planilla.NUMERO_CUENTA;
                            }
                            sheet.DeleteRow(2);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                throw;
            }
        }

        // ECC 16/10/18 obtencion de datos de planillasalud
        public List<RptPlanillaAportesSalud> GetReportePlanillaSaludTarija(int mes, int gestion)
        {
            var planillaSalud = dbContext.VPLANILLA_APORTES_SALUD
                .Where(p => p.MES == mes && p.GESTION == gestion.ToString() && p.FILIAL == "Tarija")
                .OrderByDescending(p => p.ITEM).ToList();
            MapperManager.GetInstance();
            var planillas = new List<RptPlanillaAportesSalud>();
            planillaSalud.ForEach(p => planillas.Add(Mapper.Map<VPLANILLA_APORTES_SALUD, RptPlanillaAportesSalud>(p)));
            return planillas;
        }

        public MemoryStream GetReportePlanillaSaludTj(string templateDocument, int mes, int gestion)
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
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Salud"];

                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var rptPlanillaSalud = dbContext.VPLANILLA_APORTES_SALUD
                            .Where(p => p.MES == mes && p.GESTION == gestion.ToString() && p.FILIAL == "Tarija")
                            .OrderByDescending(p => p.ITEM).ToList();
                        if (rptPlanillaSalud.Count > 0)
                        {
                            var fila = rptPlanillaSalud.Count;
                            foreach (var planilla in rptPlanillaSalud)
                            {
                                sheet.InsertRow(3, 1, 2);
                                sheet.Cells[3, 1].Value = fila--;
                                sheet.Cells[3, 2].Value = planilla.ITEM;
                                sheet.Cells[3, 3].Value = planilla.NUMERO_DOC;
                                sheet.Cells[3, 4].Value = planilla.EXT;
                                sheet.Cells[3, 5].Value = $"{planilla.APELLIDO_PATERNO} {planilla.APELLIDO_MATERNO} {planilla.NOMBRES}";
                                sheet.Cells[3, 6].Value = planilla.FECHA_NAC;
                                sheet.Cells[3, 7].Value = planilla.SEXO;
                                sheet.Cells[3, 8].Value = planilla.CARGO;
                                sheet.Cells[3, 9].Value = planilla.FECHA_INGRESO;
                                sheet.Cells[3, 10].Value = planilla.DIAS_TRAB;
                                sheet.Cells[3, 11].Value = planilla.HORA_PAG;
                                sheet.Cells[3, 12].Value = planilla.SUELDO_BASICO;
                                sheet.Cells[3, 13].Value = planilla.BONO_ANTIGUEDAD;
                                sheet.Cells[3, 14].Value = planilla.ASIGNACION_SOS;
                                sheet.Cells[3, 15].Value = planilla.HORAS_EXTRAS;
                                sheet.Cells[3, 16].Value = planilla.MONTO_PAGADO;
                                sheet.Cells[3, 17].Value = planilla.BONO_PRODUCCION;
                                sheet.Cells[3, 18].Value = planilla.OTROS_BONOS;
                                sheet.Cells[3, 19].Value = planilla.NRO_DIAS_DOMINICALES;
                                sheet.Cells[3, 20].Value = planilla.MONTO_DOMINICALES;
                                sheet.Cells[3, 21].Value = planilla.TOTAL_GANADO;
                                sheet.Cells[3, 22].Value = planilla.AFP;
                                sheet.Cells[3, 23].Value = planilla.SOLIDARIO;
                                sheet.Cells[3, 24].Value = planilla.RC_IVA;
                                sheet.Cells[3, 25].Value = planilla.DESCUENTOS;
                                sheet.Cells[3, 26].Value = planilla.OTROS_DESCUENTOS;
                                sheet.Cells[3, 27].Value = planilla.TOTAL_DESCUENTO;
                                sheet.Cells[3, 28].Value = planilla.LIQUIDO_PAGABLE;
                                sheet.Cells[3, 29].Value = planilla.NUMERO_CUENTA;
                            }
                            sheet.DeleteRow(2);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                throw;
            }
        }

        // ECC 13/10/18 obtencion de datos de aportes salud
        public List<RptPlanillaAportesSalud> GetPlanillaAporteMensualSalud(int mes, int gestion)
        {
            var planillaAporteSaludDb = dbContext.VPLANILLA_APORTES_SALUD.Where(p => p.MES == mes && p.GESTION == gestion.ToString()).OrderBy(p => p.ITEM).ToList();
            MapperManager.GetInstance();
            var planillas = new List<RptPlanillaAportesSalud>();
            planillaAporteSaludDb.ForEach(p => planillas.Add(Mapper.Map<VPLANILLA_APORTES_SALUD, RptPlanillaAportesSalud>(p)));
            return planillas;
        }

        public MemoryStream GetReportePlanillaMinisterio(string templateDocument, int mes, int gestion)
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
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Ministerio"];


                        //sheet.Cells[6, 2].Formula = "SUMA(B7:B11)";
                        var rptPlanillaMinisterio = dbContext.VPLANILLA_MINISTERIO.Where(p => p.MES == mes && p.GESTION == gestion.ToString()).OrderBy(p => p.APELLIDO_PATERNO).ToList();
                        if (rptPlanillaMinisterio.Count > 0)
                        {
                            var fila = rptPlanillaMinisterio.Count;
                            foreach (var planilla in rptPlanillaMinisterio)
                            {
                                sheet.InsertRow(3, 1, 2);
                                sheet.Cells[3, 1].Value = fila--;
                                sheet.Cells[3, 2].Value = planilla.NUMERO_DOC;
                                sheet.Cells[3, 3].Value = planilla.EXPEDIDO;
                                sheet.Cells[3, 4].Value = planilla.AFP_APORTA;
                                sheet.Cells[3, 5].Value = planilla.NUA;
                                sheet.Cells[3, 6].Value = planilla.APELLIDO_PATERNO;
                                sheet.Cells[3, 7].Value = planilla.APELLIDO_MATERNO;
                                sheet.Cells[3, 8].Value = planilla.APELLIDO_CASADA;
                                sheet.Cells[3, 9].Value = planilla.NOMBRES;
                                sheet.Cells[3, 10].Value = planilla.NACIONALIDAD;
                                sheet.Cells[3, 11].Value = planilla.FECHA_NACIMIENTO;
                                sheet.Cells[3, 12].Value = planilla.SEXO;
                                sheet.Cells[3, 13].Value = planilla.CARGO;
                                sheet.Cells[3, 14].Value = planilla.FECHA_INGRESO;
                                sheet.Cells[3, 15].Value = planilla.TIPO_CONTRATO;
                                sheet.Cells[3, 16].Value = planilla.FECHA_RETIRO;
                                sheet.Cells[3, 17].Value = planilla.DIAS_TRAB;
                                sheet.Cells[3, 18].Value = planilla.CARGA_HORARIA;
                                sheet.Cells[3, 19].Value = planilla.HABER_BASICO;
                                sheet.Cells[3, 20].Value = planilla.BONO_ANTIGUEDAD;
                                sheet.Cells[3, 21].Value = planilla.ASIG_SOS;
                                sheet.Cells[3, 22].Value = planilla.NUMERO;
                                sheet.Cells[3, 23].Value = planilla.MONTO_PAGADO;
                                sheet.Cells[3, 24].Value = planilla.BONO_PRODUCCION;
                                sheet.Cells[3, 25].Value = planilla.OTROS_BONOS;
                                sheet.Cells[3, 26].Value = planilla.NUMERO_DOMINICAL;
                                sheet.Cells[3, 27].Value = planilla.DOMINICAL;
                                sheet.Cells[3, 28].Value = planilla.TOTAL_GANADO;
                                sheet.Cells[3, 29].Value = planilla.AFP;
                                sheet.Cells[3, 30].Value = planilla.RC_IVA;
                                sheet.Cells[3, 31].Value = planilla.OTROS_DESCUENTOS;
                                sheet.Cells[3, 32].Value = planilla.TOTAL_DESCUENTO;
                                sheet.Cells[3, 33].Value = planilla.LIQ_PAGABLE;
                                sheet.Cells[3, 34].Value = planilla.FILIAL;
                                sheet.Cells[3, 35].Value = planilla.PROGRAMA;
                                sheet.Cells[3, 36].Value = planilla.SECCION;
                                sheet.Cells[3, 37].Value = planilla.ESCALA_SALARIAL;
                            }
                            sheet.DeleteRow(2);
                        }

                        package.SaveAs(output);
                    }
                    return output;
                }
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                throw;
            }
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
