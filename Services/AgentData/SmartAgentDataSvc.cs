using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Models.Remix;
using iAgentDataTool.AsyncRepositories.Common;
using iAgentDataTool.Repositories.SmartAgentRepos;
using iAgentDataTool.Helpers;
using System.Data.SqlClient;
using System.Configuration;
using Ninject;
using AgentDataServices.Modules;
using iAgentDataTool.Helpers.Interfaces;

namespace AgentDataServices
{
    public class SmartAgentDataSvc
    {
        public async Task<IEnumerable<CriteriaSets>> FindCriteria(string term, string source) {
            return await Disposable.Using(
                () => new SqlConnection(ConfigurationManager.ConnectionStrings[source].ConnectionString),
                async connection => {
                    IKernel kernel = new StandardKernel(new AgentDataModule(connection));
                    var repo = kernel.Get<IAsyncRepository<CriteriaSets>>();
                    return await repo.FindByName(term);
                });
        }
        public Dictionary<string, Guid> GetCollectionItemsMap()
        {
            var collectionItemsMap = new Dictionary<string, Guid>();
            collectionItemsMap.Add("WebsiteDomain", new Guid("96F51FD2-6539-49BD-A1C8-1F8DDE73CE1E"));
            collectionItemsMap.Add("Username", new Guid("D8BB9D68-DC56-E011-B21D-001E4F27A50B"));
            collectionItemsMap.Add("websitePassword", new Guid("D9BB9D68-DC56-E011-B21D-001E4F27A50B"));
            collectionItemsMap.Add("MemberID", new Guid("AAC881E8-ECB6-4B9B-835E-A56C89E5973F"));
            collectionItemsMap.Add("PatientDOB", new Guid("B6147C25-8F9A-E411-82F5-000C29729DFF"));
            collectionItemsMap.Add("PatientLastName", new Guid("D19691B4-9E92-E411-82F5-000C29729DFF"));
            collectionItemsMap.Add("PatientFirstName", new Guid("D09691B4-9E92-E411-82F5-000C29729DFF"));
            collectionItemsMap.Add("ToDate", new Guid("A17CC0FD-170A-4F75-AB50-44BA8AA064F5"));
            collectionItemsMap.Add("ServiceDate", new Guid("2917715D-C7F5-E011-ABE5-000C29729DFF"));
            collectionItemsMap.Add("SearchType", new Guid("c75bcc73-5356-e011-b21d-001e4f27a50b"));
            collectionItemsMap.Add("3PartDate", new Guid("62593b00-4777-e011-b21d-001e4f27a50b"));
            collectionItemsMap.Add("Health Plan", new Guid("b59302d2-226c-e011-b21d-001e4f27a50b"));
            collectionItemsMap.Add("HighmarkRequestingProvider", new Guid("44F4842D-88D3-E211-81D2-000C29729D00"));
            collectionItemsMap.Add("UHCRadRequestingProvider", new Guid("8c379ceb-11c8-e011-a8c4-000c29729dff"));
            collectionItemsMap.Add("AetnaRequestingProvider", new Guid("2e9c25ae-d25b-e011-b21d-001e4f27a50b"));
            collectionItemsMap.Add("State", new Guid("7B466BB8-D0A6-49DC-876A-2B237ADAC07C"));
            collectionItemsMap.Add("UserContactName", new Guid("6A3E2FF8-07F5-4657-ACDC-3AABEE967A21"));
            collectionItemsMap.Add("UserContactPhone", new Guid("f3fbbdfe-e84c-4dea-b471-0883a443ca0f"));
            collectionItemsMap.Add("TextEntry", new Guid("154f0a0c-f1a4-e511-96c2-000c29729dff"));
            collectionItemsMap.Add("ProviderFax", new Guid("1043b0fa-5f3d-e111-a475-000c29729dff"));
            return collectionItemsMap;
        }
    }
}
