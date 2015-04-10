using iAgentDataTool.Models;
using iAgentDataTool.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Helpers.Interfaces
{
    public interface IClientRepository : IDisposable
    {
        IEnumerable<ClientMaster> GetClients();
        ClientMaster GetClientByGuid(Guid clientKey);
        void InsertClient(ClientMaster client);
        void UpdateClinet(ClientMaster client);
        void DeleteClient(Guid clientkey);
        void Save();
    }
}
