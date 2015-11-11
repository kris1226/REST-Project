using iAgentDataTool.Models.Common;
using iAgentDataTool.Models.SmartAgentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Repositories.SmartAgentRepos
{
    public interface ICreatSmartAgentClient
    {
        Task CreateSmartAgentClient(SmartAgentClient newClient);
        Task<Guid> CreateClientLocation(SmartAgentClient newClient);
        Task CreatePayerWebsiteMappingValues(Guid clientkey, Guid clientLocationKey);
    }
}
