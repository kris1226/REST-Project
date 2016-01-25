using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Common;
using System.Web.Http.Description;
using System.Threading.Tasks;


namespace SmartAgent.Web.Controllers
{
    public class ClientLocationsController : ApiController
    {
        [Inject]
        IAsyncRepository<ClientLocations> _clientLocationsRepo;

        [Inject]
        public ClientLocationsController(IAsyncRepository<ClientLocations> clientLocationsRepo)
        {
            this._clientLocationsRepo = clientLocationsRepo;
        }

        [HttpGet]
        [Route("api/locations")]
        [ResponseType(typeof(ClientLocations))]
        public async Task<IHttpActionResult> GetLocations()
        {
            var locations = await _clientLocationsRepo.GetAllAsync();
            return LocationsResponse(locations);            
        }

        private IHttpActionResult LocationsResponse(IEnumerable<ClientLocations> locations)
        {
            if (locations.Any())
                return this.Ok(locations);
            else
                return this.NotFound();
        }

        [HttpGet]
        [Route("api/locations/{clientKey}")]
        [ResponseType(typeof(ClientLocations))]
        public async Task<IHttpActionResult> GetLocation(Guid clientKey)
        {
            var location = await _clientLocationsRepo.FindWithGuidAsync(clientKey);
            return LocationResponse(location);       
        }

        private IHttpActionResult LocationResponse(IEnumerable<ClientLocations> client)
        {
            if (client.Any())
                return Ok(client.FirstOrDefault());
            else
                return NotFound();
        }
    }
}
