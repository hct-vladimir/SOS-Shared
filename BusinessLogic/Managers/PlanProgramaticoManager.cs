using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cerberus.Sos.Accounting.BusinessLogic.Entities;
using Cerberus.Sos.Accounting.DataAccess.Models;
using Cerberus.Sos.Accounting.Log;
using PlanProgramatico = Cerberus.Sos.Accounting.BusinessLogic.Entities.PlanProgramatico;

namespace Cerberus.Sos.Accounting.BusinessLogic.Managers
{
    public class PlanProgramaticoManager : IDisposable
    {
        private AccountingSosDBEntities dbContext = new AccountingSosDBEntities();

        public List<PlanProgramatico> GetAllPlan()
        {
            var planesDb = dbContext.PlanProgramaticos.OrderBy(p => p.Codigo).ToList();
            MapperManager.GetInstance();

            var planes = new List<PlanProgramatico>();
            planesDb.ForEach(p => planes.Add(Mapper.Map<DataAccess.Models.PlanProgramatico, PlanProgramatico>(p)));

            return planes;
        }

        public List<PlanProgramatico> GetPlanByFacility(int facilityId)
        {
            var planesDb = dbContext.FacilitiesPlanProgramaticos.Where(fp => fp.FacilityId == facilityId).Select(fp => fp.PlanProgramatico).OrderBy(p => p.Codigo).ToList();
            MapperManager.GetInstance();

            var planes = new List<PlanProgramatico>();
            planesDb.ForEach(p => planes.Add(Mapper.Map<DataAccess.Models.PlanProgramatico, PlanProgramatico>(p)));

            return planes;
        }

        public List<int> GetParentsPlanIds()
        {
            var planesDb = dbContext.PlanProgramaticos.Where(p => !p.Seleccionable) .ToList();
            MapperManager.GetInstance();

            var planes = new List<PlanProgramatico>();
            planesDb.ForEach(p => planes.Add(Mapper.Map<DataAccess.Models.PlanProgramatico, PlanProgramatico>(p)));

            return planes.Select(p => p.Id).ToList();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
