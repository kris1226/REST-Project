using iAgentDataTool.Models.Remix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.ScriptHelpers.Interfaces
{
    public interface IPayerResponseRepository
    {
        Task<IEnumerable<PayerResponseMap>> GetByIPRKeyAsync(string iprkey);
        Task<IEnumerable<PayerResponseMap>> GetAllResponseMapIPRKeys();
    }
}
