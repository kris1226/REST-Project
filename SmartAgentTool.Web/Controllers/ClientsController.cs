using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SmartAgentTool.Web.Controllers
{
    public class ClientsController : ApiController
    {
        IAsyncRepository<ClientMaster> _clientRepo;
        
        public ClientsController(IAsyncRepository<ClientMaster> clientRepo)
        {
            this._clientRepo = clientRepo;
        }

        [HttpGet]
        [Route("/api/clients/search")]
        [ResponseType(typeof(IEnumerable<ClientMaster>))]
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
        [Route("/api/clients/{clientKey:Guid}")]
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
        [HttpGet]
        [Route("/api/clients")]
        public async Task<IHttpActionResult> FindClient(string clientName = "")
        {
            var client = await _clientRepo.FindByName(clientName);
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
