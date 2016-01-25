using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Common;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace TodoApp.Controllers
{
    [EnableCors("*", "*", "GET")]
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
        [Route("api/client/{clientKey:Guid}")]
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
        [Route("api/client/{clientName}")]
        public async Task<IHttpActionResult> Get(string clientName)
        {
            if (IsValid(clientName))
            {
                var records = await _clientRepo.FindByName(clientName);
                if (RecordsExits(records))
                {
                    return Ok(records);
                }
                return NotFound();
            }
            return NotFound();
        }

        private bool RecordsExits<T>(IEnumerable<T> records)
        {
            if (records.Any())
            {
                return true;
            }
            return false;
        }

        bool IsValid(string value)
        {
            if (value != null || value != "")
            {
                return true;
            }
            return false;
        }
        //public async Task<IHttpActionResult> Post([FromBody]string clientName, Guid clientKey, string howToDeliver = "OCSVC")
        //{
        //    var newCriteraSet = ClientMaster.CreateClientMaster(clientName, clientKey, howToDeliver);
        //    return BadRequest();
        //}
    }
}
