using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Helpers.Interfaces
{
    public interface IPortalUserRepo
    {
        Task UpdateUsername(string username, int Id);
        Task UpdatePassword(string password, int Id);
        Task UpdatePortalId(int portalId, int Id);
    }
}
