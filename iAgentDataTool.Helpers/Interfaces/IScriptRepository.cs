using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartAgent.Models;

namespace iAgentDataTool.Helpers.Interfaces
{
    public interface IScriptRepository : IDisposable
    {
        IEnumerable<CriteriaSets> GetCriteriaSets();
        CriteriaSets GetCriteriaSetByGuid(Guid criteriaSetKey);
        void InsertCriteriaSet(CriteriaSets criteriaSet);
        void DeleteCriteriaSet(Guid criteriaSetKey);
        void UpdateCriteriaSet(CriteriaSets criteriaSet);
        void Save();      
    }
}
