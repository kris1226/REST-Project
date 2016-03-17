using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.ScriptHelpers.Interfaces
{
    public  interface IAddMultipleEntitesToDbAsync<T> where T : class
    {
        Task AddAll(IEnumerable<T> entities);
    }
}
