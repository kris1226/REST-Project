using iAgentDataTool.Models.Remix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.ScriptHelpers.Interfaces
{
    public interface IDapperAsyncRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetById(long id);
    }
}
