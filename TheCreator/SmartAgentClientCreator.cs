using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.Models.Common;
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
    public class SmartAgentClientCreator
    {
        private IDbConnection _db;
        private IKernel _kernel;
       // private IAsyncRepository<ClientMaster> _clientRepo;

        public SmartAgentClientCreator(IDbConnection db)
        {
            this._db = db;
            _kernel = new StandardKernel(new ClientCreatorModule(_db));
        }
        public async Task<Guid> Create<T>(IEnumerable<T> newRecord) where T : class
        {
            foreach (var item in newRecord)
            {
                if (item != null)
                {
                    var repo = _kernel.Get<IAsyncRepository<T>>();

                    var isExits = await DoesClientExits<T>(repo, item);
                    if (isExits == true)
                    {
                        return new Guid("00000000-0000-0000-0000-000000000000");
                    }
                    else
                    {
                        var key = await repo.AddAsync(newRecord);
                        return key;
                    }
                }
            }
            return new Guid("00000000-0000-0000-0000-000000000000");
        }
        public async Task<bool> DoesClientExits<T>(IAsyncRepository<T> repo, T value) where T : class
        {
            if (value != null)
            {
                try
                {
                    var property = typeof(T)
                        .GetProperties()
                        .FirstOrDefault()
                        .GetValue(value);

                    if (property.ToString() != null)
                    {
                        var result = await repo.FindByName(property.ToString());
                        if (result.Any())
                        {
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return false;
        }
    }
}
