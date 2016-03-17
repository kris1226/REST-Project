using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.ScriptHelpers.Interfaces
{
    public interface IDataBindingRepo
    {
        void GetAllData();
        Task<DataTable> GetData(Guid websiteKey);
    }
}
