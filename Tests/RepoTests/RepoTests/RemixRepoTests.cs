using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Practices.Unity;
using iAgentDataTool.Models.Remix;
using iAgentDataTool.Repositories.RemixRepositories;
using NUnit.Framework;

namespace RepoTests
{
    [TestFixture]
    public class RemixRepoTests
    {
        private readonly string _prodAppConfigName = "RemixDb";
        private Action<object> write = w => Console.WriteLine(w);

        [Test]
        public async Task Add_Agent_Config_Test()
        {
            var agentConfiguration = new AgentConfiguration();
            agentConfiguration.AgentId = 93;
            agentConfiguration.ParentId = 72;

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[_prodAppConfigName].ConnectionString))
            {
                
                var container = new UnityContainer();
                container.RegisterType<IRemixRepository, RemixRepository>(new InjectionConstructor(db));
                container.RegisterType<IDbConnection, SqlConnection>();
                var repo = container.Resolve<IRemixRepository>();
                var agentConfig = await repo.ConfigureAgent(agentConfiguration);
                write(agentConfig);
            }
        }
    }
}
