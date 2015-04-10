using iAgentDataTool.Models.Remix;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Helpers.Interfaces
{
    public interface ITransactionService
    {
        Task<int> RerunTransactionAsync(Guid transId);
        void ReRunTransactionsMultiple(IEnumerable<Guid> transIds);
        void ReparseMultipleTransactionsAsync(List<Guid> transIds);
        void ReparseTransactionAsync(Guid transId);
    }
}
