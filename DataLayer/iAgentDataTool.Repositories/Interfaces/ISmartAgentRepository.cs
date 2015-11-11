using iAgentDataTool.Models;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Models.SmartAgentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Repositories.Interfaces
{
    public interface ISmartAgentRepository
    {
        Task<IEnumerable<ClientApps>> FindClientAppsRecords(Guid clientLocationKey);

        Task<int> CreateClientMappings(Guid clientKey);
        Task AddClientAppsRecord(IEnumerable<ClientApps> clientApps);

        Task<ClientMaster> FindClientMasterRecord(Guid clientKey);
        Task<IEnumerable<ClientMaster>> FindClientMasterRecord(string term);
        Task<Guid> AddClientMasterRecord(ClientMaster clientMasterRecord);

        Task<IEnumerable<ClientMappingMaster>> FindClientMappingMasterRecords(Guid clientKey);
        Task AddClientMappingMasterRecords(Guid clientKey);

        Task<IEnumerable<ClientMappingValue>> FindClientMappingValues(Guid clientKey);
        Task AddClientMappingValues(Guid clientKey);

        Task<IEnumerable<ClientLocations>> FindClientLocationRecords(Guid clientKey);
        Task<IEnumerable<ClientLocations>> FindClientLocationRecords(string term);
        Task AddClientLocationRecords(IEnumerable<ClientLocations> clientLocations);
        Task AddCriteriaRecord(Criteria criteria);

        Task<IEnumerable<PayerWebsiteMappingValue>> FindPayerWebsiteMappingValues(Guid clientKey);

        Task<IEnumerable<PayerWebsiteMappingValue>> FindPayerWebsiteMappingValues(Guid clientKey, Guid clientLocationKey);
        Task AddPayerWebsiteMappingValues(IEnumerable<PayerWebsiteMappingValue> payerWebsiteMappingValues);

        Task<IEnumerable<FacilityMaster>> FindFacilityMasterRecords(Guid clientKey);

        Task<IEnumerable<FacilityMaster>> FindFacilityMasterRecords(string term);
        Task AddFacilityMasterRecord(IEnumerable<FacilityMaster> facilites);

        Task<IEnumerable<FacilityDetail>> FindFacilityDetailsRecords(IEnumerable<Guid> facilityKeys);
        Task<IEnumerable<FacilityDetail>> FindFacilityDetailsRecords(Guid facilityKey);
        Task AddFacilityDetailsRecords(IEnumerable<FacilityDetail> facilityDetails);

        Task<IEnumerable<CriteriaDetails>> FindCriteriaWithClientKey(Guid clientKey);
        Task<IEnumerable<CriteriaSets>> FindCriteriaSetsWithCriteriaSetKeys(IEnumerable<Guid> criteriaSetKeys);

        Task<CriteriaSets> FindCriteriaSetRecord(Guid criteriaSetKey);
        Task<IEnumerable<Criteria>> AddCriteriaSetRecord(CriteriaSets criteriaSetRecord);

        Task<IEnumerable<CriteriaDetails>> FindCriteriaDetailRecords(Guid criteriaSetKey);
        Task<IEnumerable<CriteriaDetails>> FindCriteriaDetailRecords(IEnumerable<Guid> criteriaSetKeys);
        Task<IEnumerable<CriteriaDetails>> AddCriteriaDetailRecords(IEnumerable<CriteriaDetails> criteriaDetailRecords);

        Task<IEnumerable<CriteriaDetails>> FindWithCriteriaSetKeys(IEnumerable<Guid> criteriaSetKeys);
        Task<IEnumerable<CriteriaDetails>> FindCriteriaDetails(string wildCard);
        Task<IEnumerable<CriteriaSets>> FindCriteriaSetRecords(string wildCard);

        Task AddCriterias(IEnumerable<CriteriaSets> criteriaSets);

        Task AddCriteriaDetails(IEnumerable<CriteriaDetails> criteriaDetials);

        Task<IEnumerable<WebsiteExtractionMap>> FindExtractions(Guid websiteKey);

        Task<IEnumerable<ScriptMaster>> FindScripts(Guid websiteKey);
        Task<IEnumerable<ScriptReturnValue>> FindScriptReturnValues(Guid websiteKey);
        Task<IEnumerable<ScriptCollectionItem>> FindCollectionItems(Guid websiteKey);

        Task<IEnumerable<ScriptMaster>> AddScripts(IEnumerable<ScriptMaster> scripts);
        Task AddScriptReturnValues(IEnumerable<ScriptReturnValue> returnValues);
        Task<bool> AddScriptCollectionItems(IEnumerable<ScriptCollectionItem> collectionItems);

        Task<Guid> GetFirstScriptKey(Criteria critera);
    }
}
