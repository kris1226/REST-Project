using iAgentDataTool.Models.Remix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Helpers.Interfaces
{
    public interface ITransactionRepo
    {
        Task<IEnumerable<AgentSchedule>> FindInAgentScheduleAsync(Guid transId);
        void DeleteFromAgentRequestTable(Guid transId);
        void UpdateAgentSchedule(Guid transId);
        Task<int> ScheduleTransaction(Guid transId);
        Task<int> UpdateAgentTransResponses(Guid transId);
        Task<int> UpdateAgentResponseTableAsync(Guid transId);
        void UpdateAgentTransResponses(List<Guid> transIds);
        void UpdateAgentResponseTableAsync(List<Guid> transIds);
        //void DeleteFromAgentRequestTable(Guid transId);
    }
}
