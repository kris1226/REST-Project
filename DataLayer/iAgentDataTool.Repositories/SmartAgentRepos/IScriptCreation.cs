using iAgentDataTool.Models;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Models.SmartAgentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Repositories.SmartAgentRepos
{
    public interface IScriptCreation
    {
        Task<Guid> CreateScritp(Script script);
        Task CreateReturnValues(ScriptReturnValue returnValues);
        Task<ScriptCollectionItem> CreateCollectionItems(ScriptCollectionItem collectionItem);

        Task<WebsiteExtractionMap> CreateExtractionMap(WebsiteExtractionMap extractMap);

        Task<IEnumerable<ScriptMaster>> GetScripts(Guid websiteKey);

        Task UpdateScriptCode(string scriptCode, Guid scriptKey);
    }
}
