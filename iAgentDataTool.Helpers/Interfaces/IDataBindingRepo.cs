using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Helpers.Interfaces
{
    public interface IDataBindingRepo
    {
        void GetAllData();
        Task<DataTable> GetData(Guid websiteKey);
    }
}
