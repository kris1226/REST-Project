using iAgentDataTool.Models.Common;
using iAgentDataTool.Models.SmartAgentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Repositories.SmartAgentRepos
{
    public interface ISmartAgentRepo
    {
        Task<Guid> CreateFacilityMasterRecord(SmartAgentClient client);
        Task<Guid> CreateWebsiteMasterRecord(WebsiteMaster website);
        Task<IEnumerable<Criteria>> CreateCriteraRecord(Criteria newRecord);
        Task<WebsiteMaster> FindWebsiteRecord(Guid websiteKey);
        Task<WebsiteMaster> FindWebsiteRecord(string term);
        Task<IEnumerable<Criteria>> CreateCriteraRecords(Criteria record);
        Task DeleteCritera(string criteraSetName);
        Task<int> AddPayerWebsiteMappingValue(IEnumerable<PayerWebsiteMappingValue> payerWebsiteMappingValue);
    }
}
