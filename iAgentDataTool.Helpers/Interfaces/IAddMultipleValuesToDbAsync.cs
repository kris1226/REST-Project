using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Helpers.Interfaces
{
    public  interface IAddMultipleEntitesToDbAsync<T> where T : class
    {
        Task AddAll(IEnumerable<T> entities);
    }
}
