using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Helpers.Interfaces
{
    public interface IClientLocaionRepository
    {
        Task UpdateTpid(string tpid);
        Task UpdateClientId(string clientId);
        Task UpdateFacilityId(string facilityId);
    }
}
