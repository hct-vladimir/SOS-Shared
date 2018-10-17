using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using OfficeOpenXml;
using Presupuesto = Cerberus.Sos.Accounting.BusinessLogic.Entities.Presupuesto;
using PresupuestoCiudad = Cerberus.Sos.Accounting.BusinessLogic.Entities.PresupuestoCiudad;
using PresupuestoFacility = Cerberus.Sos.Accounting.BusinessLogic.Entities.PresupuestoFacility;
using PresupuestoFacilityCompartido = Cerberus.Sos.Accounting.BusinessLogic.Entities.PresupuestoFacilityCompartido;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class PresupuestosManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();


        public List<Presupuesto> GetAllPresupuestos()
        {
            MapperManager.GetInstance();

            var presupuestosDb =
                dbContext.Presupuestos.Where(p => p.Activo).ToList();

            var presupuestos = new List<Presupuesto>();

            presupuestosDb.ForEach(p => presupuestos.Add(Mapper.Map<DataAccess.Models.Presupuesto, Presupuesto>(p)));

            return presupuestos;
        }

        public List<PresupuestoCiudad> GetPresupuestosCiudades(int presupuestoId)
        {
            MapperManager.GetInstance();

            var presupuestosDb = dbContext.PresupuestosCiudades.Where(p => p.PresupuestoId == presupuestoId).ToList();

            var presupuestos = new List<PresupuestoCiudad>();

            presupuestosDb.ForEach(p => presupuestos.Add(Mapper.Map<DataAccess.Models.PresupuestoCiudad, PresupuestoCiudad>(p)));

            return presupuestos;
        }

        public List<PresupuestoCiudad> GetPresupuestosCiudadesByUsuario(int presupuestoId, int usuarioId)
        {
            MapperManager.GetInstance();
            var presupuestos = new List<PresupuestoCiudad>();

            var ciudadesIds = dbContext.UsuariosCiudades.Where(uc => uc.UsuarioId == usuarioId).Select(uc => uc.CiudadId).ToList();

            if (ciudadesIds.Count > 0)
            {
                var presupuestosDb = dbContext.PresupuestosCiudades.Where(p => p.PresupuestoId == presupuestoId && ciudadesIds.Contains(p.CiudadId)).ToList();

                presupuestosDb.ForEach(p => presupuestos.Add(Mapper.Map<DataAccess.Models.PresupuestoCiudad, PresupuestoCiudad>(p)));

            }

            return presupuestos;
        }

        public List<PresupuestoFacility> GetPresupuestosFacilities(int presupuestoId, int ciudadId)
        {
            MapperManager.GetInstance();
            var presupuestos = new List<PresupuestoFacility>();

            var facilitiesIds = dbContext.Facilities.Where(f => f.CiudadId == ciudadId).Select(f => f.Id).ToList();

            if (facilitiesIds.Count > 0)
            {
                var presupuestosDb = dbContext.PresupuestosFacilities.Where(p => p.PresupuestoId == presupuestoId && facilitiesIds.Contains(p.FacilityId)).ToList();

                presupuestosDb.ForEach(p => presupuestos.Add(Mapper.Map<DataAccess.Models.PresupuestoFacility, PresupuestoFacility>(p)));
            }

            return presupuestos;
        }

        public PresupuestoFacility GetPresupuestosFacilityActual(int presupuestoId, int facilityId)
        {
            MapperManager.GetInstance();
            var presupuesto = new PresupuestoFacility();

            var presupuestoDb =
                dbContext.PresupuestosFacilities.FirstOrDefault(
                    pf => pf.PresupuestoId == presupuestoId && pf.FacilityId == facilityId);

                presupuesto = Mapper.Map<DataAccess.Models.PresupuestoFacility, PresupuestoFacility>(presupuestoDb);

            return presupuesto;
        }

        public PresupuestoFacilityCompartido GetPresupuestoCompartidoActual(int presupuestoId, int facilityId, int ciudadId)
        {
            MapperManager.GetInstance();
            var presupuesto = new PresupuestoFacilityCompartido();

            var presupuestoDb =
                dbContext.PresupuestosFacilitiesCompartidos.FirstOrDefault(
                    pf => pf.PresupuestosFacility.PresupuestoId == presupuestoId && pf.PresupuestosFacility.Facility.Id == facilityId && pf.CiudadId == ciudadId);

            presupuesto = Mapper.Map<DataAccess.Models.PresupuestoFacilityCompartido, PresupuestoFacilityCompartido>(presupuestoDb);

            return presupuesto;
        }


        public List<Presupuesto> GetPresupuestoByGestion(int gestion)
        {
            MapperManager.GetInstance();

            var presupuestosDb =
                dbContext.Presupuestos.Where(p => p.Gestion == gestion).ToList();

            var presupuestos = new List<Presupuesto>();

            presupuestosDb.ForEach(p => presupuestos.Add(Mapper.Map<DataAccess.Models.Presupuesto, Presupuesto>(p)));

            return presupuestos;
        }

        public Presupuesto GetPresupuestoActual()
        {
            MapperManager.GetInstance();
            // El Presupuesto actual debe tener el estado EN ELABORACION (2)
            var presupuestoDb = dbContext.Presupuestos.FirstOrDefault(p => p.EstadoPresupuestoId == 2 && p.Activo);

            var presupuesto = Mapper.Map<DataAccess.Models.Presupuesto, Presupuesto>(presupuestoDb);

            return presupuesto;
        }

        ////public List<VPresupuestoCiudad> GetPresupuestosCiudades(int gestion)
        ////{
        ////    return GetPresupuestosCiudades(null, gestion);
        ////}

        ////public List<VPresupuestoCiudad> GetPresupuestosCiudades(List<int> ciudadesIds, int gestion)
        ////{
        ////    MapperManager.GetInstance();
        ////    var presupuestoCiudadDb = new List<VPresupuesto>();

        ////    if (ciudadesIds != null)
        ////    {
        ////        presupuestoCiudadDb = dbContext.VPresupuestos.Where(p => ciudadesIds.Contains(p.CiudadId) && p.Gestion == gestion).ToList();
        ////    }
        ////    else
        ////    {
        ////        presupuestoCiudadDb = dbContext.VPresupuestos.Where(p => p.Gestion == gestion).ToList();
        ////    }

        ////    var presupuestos = new List<VPresupuestoCiudad>();

        ////    presupuestoCiudadDb.ForEach(p => presupuestos.Add(Mapper.Map<VPresupuesto, VPresupuestoCiudad>(p)));

        ////    return presupuestos;
        ////}

        public Resultado InsertPresupuesto(Presupuesto presupuesto)
        {
            MapperManager.GetInstance();

            try
            {
                var presupuestoDb = Mapper.Map<Presupuesto, DataAccess.Models.Presupuesto>(presupuesto);

                presupuestoDb.EstadoPresupuestoId = 2;
                presupuestoDb.FechaCreacion = DateTime.Now;
                presupuestoDb.FechaModificacion = DateTime.Now;
                dbContext.Presupuestos.Add(presupuestoDb);
                dbContext.SaveChanges();

                //Crear Presupuestos por ciudades
                var presupuestoCiudad = new DataAccess.Models.PresupuestoCiudad();
                var ciudades = dbContext.Ciudades.Where(c => c.Activo);
                foreach (var ciudad in ciudades)
                {
                    presupuestoCiudad = new DataAccess.Models.PresupuestoCiudad
                    {
                        PresupuestoId = presupuestoDb.Id,
                        CiudadId = ciudad.Id,
                        EstadoPresupuestoId = 2,
                        Monto = 0,
                        UsuarioCreacion = presupuestoDb.UsuarioCreacion,
                        FechaCreacion = DateTime.Now,
                        UsuarioModificacion = presupuestoDb.UsuarioModificacion,
                        FechaModificacion = DateTime.Now
                    };
                    dbContext.PresupuestosCiudades.Add(presupuestoCiudad);
                }
                dbContext.SaveChanges();

                //Crear Presupuestos por facilities
                var presupuestoFacility = new DataAccess.Models.PresupuestoFacility();
                var facilities = dbContext.Facilities.Where(c => c.Activo);
                foreach (var facility in facilities)
                {
                    presupuestoFacility = new DataAccess.Models.PresupuestoFacility
                    {
                        PresupuestoId = presupuestoDb.Id,
                        FacilityId = facility.Id,
                        EstadoPresupuestoId = 2,
                        Monto = 0,
                        UsuarioCreacion = presupuestoDb.UsuarioCreacion,
                        FechaCreacion = DateTime.Now,
                        UsuarioModificacion = presupuestoDb.UsuarioModificacion,
                        FechaModificacion = DateTime.Now
                    };
                    dbContext.PresupuestosFacilities.Add(presupuestoFacility);
                }
                dbContext.SaveChanges();
                
                return new Resultado("El Presupuesto se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }


        public Resultado UpdatePresupuesto(Presupuesto presupuesto)
        {
            try
            {
                var presupuestoDb = dbContext.Presupuestos.FirstOrDefault(p => p.Id == presupuesto.Id);
                if (presupuestoDb != null)
                {
                    presupuestoDb.EstadoPresupuestoId = presupuesto.EstadoPresupuestoId;
                    presupuestoDb.UsuarioModificacion = presupuesto.UsuarioModificacion;
                    presupuestoDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(presupuestoDb).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return new Resultado("El registro se guardó correctamente.");
                }
                return new Resultado("No se encontró el presupuesto específicado");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado HabilitarGestion(int gestion)
        {
            try
            {
                var presupuestos = dbContext.Presupuestos.Where(p => p.Gestion == gestion);
                foreach (var presupuesto in presupuestos)
                {
                    presupuesto.EstadoPresupuestoId = 2;
                    dbContext.Entry(presupuesto).State = EntityState.Modified;
                }
                dbContext.SaveChanges();

                return new Resultado("Se habilitaron los presupuestos para la gestión solicitada");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Resultado CerrarGestion(int gestion)
        {
            try
            {
                var presupuestos = dbContext.Presupuestos.Where(p => p.Gestion == gestion);
                foreach (var presupuesto in presupuestos)
                {
                    presupuesto.EstadoPresupuestoId = 6;
                    dbContext.Entry(presupuesto).State = EntityState.Modified;
                }
                dbContext.SaveChanges();

                return new Resultado("Se cerraron los presupuestos para la gestión solicitada");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Resultado CambiarEstado(int gestion, int version, short estadoId)
        {
            try
            {
                var presupuestoDb = dbContext.Presupuestos.FirstOrDefault(p => p.Gestion == gestion && p.Version == version && p.Activo);
                if (presupuestoDb != null)
                {
                    presupuestoDb.EstadoPresupuestoId = estadoId;
                    presupuestoDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(presupuestoDb).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return new Resultado("El registro se guardó correctamente.");
                }

                return new Resultado("El presupuesto no existe.");
            }
            catch (Exception exception)
            {
                
                throw;
            }
        }

        public Resultado CambiarEstadoCiudad(int presupuestoId, int ciudadId, short estadoId)
        {
            try
            {
                var presupuestoDb = dbContext.PresupuestosCiudades.FirstOrDefault(p => p.PresupuestoId == presupuestoId && p.CiudadId == ciudadId);
                if (presupuestoDb != null)
                {
                    presupuestoDb.EstadoPresupuestoId = estadoId;
                    presupuestoDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(presupuestoDb).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return new Resultado("El registro se guardó correctamente.");
                }

                return new Resultado("El presupuesto no existe.");
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        public Resultado CambiarEstadoFacility(int presupuestoId, int facilityId, short estadoId, string usuarioActual)
        {
            try
            {
                var presupuestoDb = dbContext.PresupuestosFacilities.FirstOrDefault(p => p.PresupuestoId == presupuestoId && p.FacilityId == facilityId);
                if (presupuestoDb != null)
                {
                    presupuestoDb.EstadoPresupuestoId = estadoId;
                    presupuestoDb.UsuarioModificacion = usuarioActual;
                    presupuestoDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(presupuestoDb).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return new Resultado("El registro se guardó correctamente.");
                }

                return new Resultado("El presupuesto no existe.");
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        public Resultado CambiarEstadoFacilityCompartido(int presupuestoCompartidoId, int ciudadId, short estadoId)
        {
            try
            {
                var presupuestoDb =
                    dbContext.PresupuestosFacilitiesCompartidos.FirstOrDefault(pfc => pfc.Id == presupuestoCompartidoId && pfc.CiudadId == ciudadId);
                if (presupuestoDb != null)
                {
                    presupuestoDb.EstadoPresupuestoId = estadoId;
                    presupuestoDb.FechaModificacion = DateTime.Now;

                    dbContext.Entry(presupuestoDb).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return new Resultado("El registro se guardó correctamente.");
                }

                return new Resultado("El presupuesto no existe.");
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public Resultado ImportarPresupuesto(int presupuestoId, string rutaArchivoImportado, string nombreArchivo, int tamanioArchivo, string usuarioActual)
        {
            try
            {
                // Se guardan datos de Cabecera del Lote
                var lote = new RecursosExcelLote
                {
                    NombreArchivo = nombreArchivo,
                    TamanioArchivo = tamanioArchivo,
                    Activo = true,
                    UsuarioCreacion = usuarioActual,
                    FechaCreacion = DateTime.Now,
                    UsuarioModificacion = usuarioActual,
                    FechaModificacion = DateTime.Now
                };

                dbContext.RecursosExcelLotes.Add(lote);
                dbContext.SaveChanges();

                // Se guarda el detalle
                var startColumn = 1;
                var startRow = 2;

                using (ExcelPackage package = new ExcelPackage(new FileInfo(rutaArchivoImportado)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    var cell = worksheet.Cells[startRow, startColumn] != null && worksheet.Cells[startRow, startColumn].Value != null ? worksheet.Cells[startRow, startColumn].Value.ToString() : string.Empty;
                    var item = new RecursosExcel();
                    var recurso = new DataAccess.Models.Recurso();
                    while (!string.IsNullOrEmpty(cell))
                    {
                        item = new RecursosExcel
                        {
                            RecursoExcelLoteId = lote.Id,
                            PresupuestoId = presupuestoId,
                            Ciudad = worksheet.Cells[startRow, startColumn].Value.ToString(),
                            Facility = worksheet.Cells[startRow, startColumn + 1].Value.ToString(),
                            CuentaContable = worksheet.Cells[startRow, startColumn + 2].Value.ToString(),
                            PlanProgramatico = worksheet.Cells[startRow, startColumn + 3].Value.ToString(),

                            Descripcion = worksheet.Cells[startRow, startColumn + 4].Value.ToString(),
                            NotasAdicionales = worksheet.Cells[startRow, startColumn + 5].Value != null ? worksheet.Cells[startRow, startColumn + 5].Value.ToString() : string.Empty,

                            Cobertura = worksheet.Cells[startRow, startColumn + 6].Value != null ? Convert.ToInt32(worksheet.Cells[startRow, startColumn + 6].Value.ToString()) : 0,
                            IndiceTransferencia = worksheet.Cells[startRow, startColumn + 7].Value != null ? Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 7].Value.ToString()) : 0,
                            MontoAnual = worksheet.Cells[startRow, startColumn + 8].Value != null ? Convert.ToDecimal(worksheet.Cells[startRow, startColumn + 8].Value.ToString()) : 0,

                            Territorio = worksheet.Cells[startRow, startColumn + 9].Value.ToString(),
                            Contraparte = worksheet.Cells[startRow, startColumn + 10].Value.ToString(),
                            CodigoAuditoria = worksheet.Cells[startRow, startColumn + 11] != null && worksheet.Cells[startRow, startColumn + 11].Value != null ? worksheet.Cells[startRow, startColumn + 11].Value.ToString() : "0",
                            AccionNacional = worksheet.Cells[startRow, startColumn + 12] != null && worksheet.Cells[startRow, startColumn + 12].Value != null ? worksheet.Cells[startRow, startColumn + 12].Value.ToString() : "0",
                            MarcoLogico = worksheet.Cells[startRow, startColumn + 13] != null && worksheet.Cells[startRow, startColumn + 13].Value != null ? worksheet.Cells[startRow, startColumn + 13].Value.ToString() : "0",

                            Activo = true,
                            UsuarioCreacion = usuarioActual,
                            FechaCreacion = DateTime.Now,
                            UsuarioModificacion = usuarioActual,
                            FechaModificacion = DateTime.Now
                        };
                        dbContext.RecursosExcels.Add(item);
                        startRow++;
                        cell = worksheet.Cells[startRow, startColumn] != null && worksheet.Cells[startRow, startColumn].Value != null ? worksheet.Cells[startRow, startColumn].Value.ToString() : string.Empty;
                    }

                    dbContext.SaveChanges();
                }

                return new Resultado("El Recurso se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }
    }
}
