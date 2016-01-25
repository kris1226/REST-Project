using iAgentDataTool.Models.Remix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Repositories.RemixRepositories
{
    public interface IRemixRepository
    {
        Task<AgentConfiguration> ConfigureAgent(AgentConfiguration agent);
    }
}
