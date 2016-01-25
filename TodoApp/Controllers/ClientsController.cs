using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Common;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace TodoApp.Controllers
{
    public class ClientsController : ApiController
    {
        [Inject]
        IAsyncRepository<ClientMaster> _clientRepo;

        [Inject]
        public ClientsController(IAsyncRepository<ClientMaster> clientRepo)
        {
            this._clientRepo = clientRepo;
        }

        [HttpGet]
        [Route("api/clients")]
        [ResponseType(typeof(ClientMaster))]
        public async Task<IHttpActionResult> GetClients()
        {
            var clients = await _clientRepo.GetAllAsync();
            if (clients.Any())
            {
                return this.Ok(clients);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpGet]
        [Route("api/clients/{clientKey}")]
        public async Task<IHttpActionResult> GetClient(Guid clientKey)
        {
            var client = await _clientRepo.FindWithGuidAsync(clientKey);
            if (client.Any())
            {
                return Ok(client.SingleOrDefault());
            }
            else
            {
                return NotFound();
            }
        }
    }
}
