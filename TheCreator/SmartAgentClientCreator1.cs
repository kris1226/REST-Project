using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Repositories.SmartAgentRepos;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCreator.Modules;

namespace TheCreator
{
    public class SmartAgentClientCreator1 : IClientCreator
    {
        IKernel _kernel;
        public SmartAgentClientCreator1(IDbConnection db)
        {
            _kernel = new StandardKernel(new ClientCreatorModule(db));
        }
        public async Task CreateClients(IList<ClientMaster> clients)
        {
            var clientRepo = _kernel.Get<IAsyncRepository<ClientMaster>>();
            var locationsRepo = _kernel.Get<IAsyncRepository<ClientLocations>>();
            try
            {
                var clientKey = await CreateRecord<ClientMaster>(clientRepo, clients);

                foreach (var client in clients)
	            {
		          //  var clientLocation = ClientLocations.CreateClientLocation(client.ClientName, clientKey, Guid.NewGuid(), )
	            }

               // var clientLocationKey = await CreateRecord<ClientLocations>(locationsRepo, )
            }
            catch (Exception)
            {                
                throw;
            }            
        }
        public async Task<Guid> CreateRecord<T>(IAsyncRepository<T> repo, IList<T> enitites) where T : class
        {
            return await repo.AddAsync(enitites);
        }
    }
}
