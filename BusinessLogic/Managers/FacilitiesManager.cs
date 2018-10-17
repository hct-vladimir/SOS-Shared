using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using Cerberus.Sos.Accounting.Security.Entities;
using Facility = Cerberus.Sos.Accounting.BusinessLogic.Entities.Facility;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class FacilitiesManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<Facility> GetAllFacilities()
        {
            var facilityDb = dbContext.Facilities.Where(f => f.Activo).OrderBy(f => f.Codigo).ToList();
            MapperManager.GetInstance();

            var facility = new List<Facility>();
            facilityDb.ForEach(p => facility.Add(Mapper.Map<DataAccess.Models.Facility, Facility>(p)));

            return facility;
        }

        public List<Facility> GetFacilitiesPorCiudad(int ciudadId)
        {
            var facilitiesDb = dbContext.Facilities.Where(f => f.CiudadId == ciudadId && f.Activo).OrderBy(f => f.Codigo).ToList();

            //Si existen facilities compartidos para esta ciudad agregarlos
            var presupuestoActualDb = dbContext.Presupuestos.FirstOrDefault(p => p.EstadoPresupuestoId == 2 && p.Activo);

            var presupuestoFacilityCompartido =
                dbContext.PresupuestosFacilitiesCompartidos.FirstOrDefault(
                    pfc => pfc.PresupuestosFacility.PresupuestoId == presupuestoActualDb.Id && pfc.CiudadId == ciudadId);

            if (presupuestoFacilityCompartido != null)
            {
                facilitiesDb.Add(presupuestoFacilityCompartido.PresupuestosFacility.Facility);
            }

            MapperManager.GetInstance();

            var facility = new List<Facility>();
            facilitiesDb.ForEach(p => facility.Add(Mapper.Map<DataAccess.Models.Facility, Facility>(p)));

            return facility;
        }

        public List<Facility> GetFacilitiesRecursosPorCiudad(int presupuestoId, int ciudadId, bool esUsuarioCoordinador, bool esUsuarioRrff)
        {
            //El facility 57 (12) se utiliza para todas las ciudades
            var facilitiesTotales = dbContext.Recursos.Where(r => r.CiudadId == ciudadId && r.PresupuestoId == presupuestoId && r.Activo && r.Facility.Activo)
                .GroupBy(g => g.Facility).Select(f => new Facility
                {
                    Id = f.Key.Id,
                    Codigo = f.Key.Codigo,
                    Nombre = f.Key.Nombre,
                    TotalRecursos = f.Sum(r => r.Monto)
                }).ToList();

            var facilitiesCiudades = dbContext.PresupuestosFacilities.Where(pf => pf.Facility.Activo).Where(pf => pf.PresupuestoId == presupuestoId && pf.Facility.CiudadId == ciudadId).ToList();

            var result = facilitiesCiudades.GroupJoin(facilitiesTotales,
                fc => fc.Facility.Id,
                ft => ft.Id,
                (fc, ft) =>
                    new { FacilitiesCiudades = fc, FacilitiesTotales = ft.DefaultIfEmpty() })
                .SelectMany(r => r.FacilitiesTotales.Select(f => new { r.FacilitiesCiudades, FacilitiesTotales = f })).ToList();

            var facilities = result.Select(r => new Facility
            {
                Id = r.FacilitiesCiudades.Facility.Id,
                CiudadId = r.FacilitiesCiudades.Facility.CiudadId.Value,
                Codigo = r.FacilitiesCiudades.Facility.Codigo,
                Nombre = r.FacilitiesCiudades.Facility.Nombre,
                Descripcion = r.FacilitiesCiudades.Facility.Descripcion,
                EstadoFacilityNombre = r.FacilitiesCiudades.EstadoPresupuesto.Nombre,
                TotalRecursos = r.FacilitiesTotales != null ? r.FacilitiesTotales.TotalRecursos : 0
            }).ToList();

            // Si es usuario coordinador y no es de RRFF, entonces no se agregan los Facilities Compartidos puesto que solo el coordinador RRFF puede revisarlos.
            if (esUsuarioCoordinador && !esUsuarioRrff)
            {
                return facilities;
            }

            #region Facilities Compartidos

            // Facilty(es) compartido(es) según algún criterio por definir en siguientes versiones, en esta versión el Facility compartido es RRFF
            var presupuestoFacilityCompartido =
                dbContext.PresupuestosFacilities.FirstOrDefault(
                    pf => pf.PresupuestoId == presupuestoId && pf.Facility.Codigo == "R0031557");

            // Se busca si existe un compartido para la ciudad actual
            var facilitiesCompartidos = dbContext.PresupuestosFacilitiesCompartidos
                .Where(
                    pfc => pfc.PresupuestoFacilityId == presupuestoFacilityCompartido.Id && pfc.CiudadId == ciudadId)
                .Select(p => new Facility
                {
                    Id = p.PresupuestosFacility.Facility.Id,
                    CiudadId = p.PresupuestosFacility.Facility.CiudadId.Value,
                    Codigo = p.PresupuestosFacility.Facility.Codigo,
                    Nombre = p.PresupuestosFacility.Facility.Nombre + " (" + p.Ciudad.Nombre + ")",
                    Descripcion = p.PresupuestosFacility.Facility.Descripcion,
                    EstadoFacilityNombre = p.EstadosPresupuesto.Nombre,
                    Compartido = true,
                    CiudadCompartidaActualId = p.CiudadId,
                    TotalRecursos = dbContext.Recursos.Where(
                        r => r.Activo &&
                             r.PresupuestoId == presupuestoId &&
                             r.CiudadId == ciudadId &&
                             r.FacilityId == p.PresupuestosFacility.Facility.Id).Sum(r => r.Monto)
                }).ToList();

            //Si existe, se agrega a la lista de facilities de la ciudad con su correspondiente estado
            if (facilitiesCompartidos.Any())
            {
                facilities.AddRange(facilitiesCompartidos);
            }

            #endregion

            return facilities;
        }

        public List<Facility> GetFacilitiesRecursosCompartidos(int presupuestoId, int ciudadId)
        {
            // Facilty(es) compartido(es) según algún criterio por definir en siguientes versiones, en esta versión el Facility compartido es RRFF
            var presupuestoFacilityCompartido = dbContext.PresupuestosFacilities.FirstOrDefault(pf => pf.PresupuestoId == presupuestoId && pf.Facility.Codigo == "R0031557");

            // Se busca si existe un compartido para la ciudad actual
            var facilitiesCompartidos = dbContext.PresupuestosFacilitiesCompartidos
                .Where(pfc => pfc.PresupuestoFacilityId == presupuestoFacilityCompartido.Id && pfc.CiudadId != ciudadId)
                .Select(p => new Facility
                {
                    Id = p.PresupuestosFacility.Facility.Id,
                    CiudadId = p.PresupuestosFacility.Facility.CiudadId.Value,
                    Codigo = p.PresupuestosFacility.Facility.Codigo,
                    Nombre = p.PresupuestosFacility.Facility.Nombre + " (" + p.Ciudad.Nombre + ")",
                    Descripcion = p.PresupuestosFacility.Facility.Descripcion,
                    EstadoFacilityNombre = p.EstadosPresupuesto.Nombre,
                    Compartido = true,
                    CiudadCompartidaActualId = p.CiudadId,
                    TotalRecursos = dbContext.Recursos.Where(
                        r => r.Activo &&
                        r.PresupuestoId == presupuestoId &&
                        r.CiudadId == p.CiudadId &&
                        r.FacilityId == p.PresupuestosFacility.Facility.Id).Sum(r => r.Monto)
                }).ToList();

            return facilitiesCompartidos;
        }


        public List<Facility> GetFacilitiesComprobantesPorCiudad(int ciudadId, int cierreContableId)
        {
            //El facility 57 (12) se utiliza para todas las ciudades
            var facilitiesTotales = dbContext.Comprobantes.Where(c => (c.Facility.CiudadId == ciudadId || c.Facility.CiudadId == 12) && c.CierreContableId == cierreContableId && c.Activo && c.Facility.Activo)
                .GroupBy(g => g.Facility).Select(f => new Facility
                {
                    Id = f.Key.Id,
                    Codigo = f.Key.Codigo,
                    Nombre = f.Key.Nombre,
                    TotalComprobantes = f.Count(c => c.Activo)
                }).ToList();

            //El facility 57 (12) se utiliza para todas las ciudades
            var facilitiesCiudades = dbContext.Facilities.Where(f => f.Activo).Where(f => f.CiudadId == ciudadId || f.CiudadId == 12).ToList();

            var result = facilitiesCiudades.GroupJoin(facilitiesTotales,
                fc => fc.Id,
                ft => ft.Id,
                (fc, ft) =>
                    new { FacilitiesCiudades = fc, FacilitiesTotales = ft.DefaultIfEmpty() })
                .SelectMany(r => r.FacilitiesTotales.Select(f => new { r.FacilitiesCiudades, FacilitiesTotales = f })).ToList();

            var facilities = result.Select(r => new Facility
            {
                Id = r.FacilitiesCiudades.Id,
                Codigo = r.FacilitiesCiudades.Codigo,
                Nombre = r.FacilitiesCiudades.Nombre,
                TotalComprobantes = r.FacilitiesTotales != null ? r.FacilitiesTotales.TotalComprobantes : 0
            }).ToList();

            return facilities;
        }

        public Facility GetFacility(int facilityId)
        {
            var facilityDb = dbContext.Facilities.Find(facilityId);
            MapperManager.GetInstance();

            var facility = Mapper.Map<DataAccess.Models.Facility, Facility>(facilityDb);

            return facility;
        }

        public Facility GetFacilityByCodigo(string codigoFacility)
        {
            var facilityDb = dbContext.Facilities.FirstOrDefault(f => f.Codigo == codigoFacility);
            MapperManager.GetInstance();

            var facility = Mapper.Map<DataAccess.Models.Facility, Facility>(facilityDb);

            return facility;
        }

        public Resultado InsertFacility(Facility facility)
        {
            MapperManager.GetInstance();

            try
            {
                var facilityDb = Mapper.Map<Facility, DataAccess.Models.Facility>(facility);
                dbContext.Facilities.Add(facilityDb);
                dbContext.SaveChanges();
                return new Resultado("El territorio se guardó correctamente.");
            }
            catch (Exception excepcion)
            {
                LogHelper.RegisterError(excepcion.Message);
                return new Resultado("Ocurrio un error. Favor contactarse con el administrador.");
            }
        }

        public Resultado UpdateFacility(Facility facility)
        {
            MapperManager.GetInstance();

            try
            {
                var facilityDb = Mapper.Map<Facility, DataAccess.Models.Facility>(facility);
                dbContext.Entry(facilityDb).State = EntityState.Modified;
                dbContext.SaveChanges();
                return new Resultado("El facility se guardó correctamente.");
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
