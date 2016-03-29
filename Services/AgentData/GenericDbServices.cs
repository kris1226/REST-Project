using AgentDataServices.Modules;
using iAgentDataTool.ScriptHelpers.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentData
{
    public class GenericDbServices<T> where T : class
    {
        public async Task<IEnumerable<T>> GetAll(string source)
        {
            return await Disposable.UsingAsync(
                () => new SqlConnection(ConfigurationManager.ConnectionStrings[source].ConnectionString),
                async connection =>
                {
                    var kernel = new StandardKernel(new AgentDataModule(connection));
                    var websiteRepo = kernel.Get<IAsyncRepository<T>>();
                    return await websiteRepo.GetAllAsync();
                });
        }
    }
}
