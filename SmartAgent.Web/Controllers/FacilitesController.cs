using iAgentDataTool.ScriptHelpers.Interfaces;
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

namespace SmartAgent.Web.Controllers
{
    public class FacilitesController : ApiController
    {
        [Inject]
        IAsyncRepository<FacilityMaster> _facilitesRepo;

        [Inject]
        public FacilitesController(IAsyncRepository<FacilityMaster> facilitesRepo)
        {
            this._facilitesRepo = facilitesRepo;
        }

        [HttpGet]
        [Route("api/facilites")]
        [ResponseType(typeof(FacilityMaster))]
        public async Task<IHttpActionResult> GetFacilites()
        {
            var facilites = await _facilitesRepo.GetAllAsync();
            return FacilitesResponse(facilites);            
        }
        

        [HttpGet]
        [Route("api/facilites/{clientKey}")]
        [ResponseType(typeof(FacilityMaster))]
        public async Task<IHttpActionResult> GetFacility(Guid clientKey)
        {
            Func<IEnumerable<FacilityMaster>, IHttpActionResult> Response = (value) => 
            {
                if (value.Any())
                    return Ok(value.FirstOrDefault());
                else
                    return NotFound();
            };

            var facility = await _facilitesRepo.FindWithGuidAsync(clientKey);
            return Response(facility);       
        }
        private IHttpActionResult FacilitesResponse(IEnumerable<FacilityMaster> facilites)
        {
            if (facilites.Any())
                return this.Ok(facilites);
            else
                return this.NotFound();
        }
    }
}
