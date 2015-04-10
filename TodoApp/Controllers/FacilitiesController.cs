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

namespace TodoApp.Controllers
{
    public class FacilitiesController : ApiController
    {
        [Inject]
        IAsyncRepository<FacilityMaster> _facilityRepo;
        [Inject]
        IAsyncRepository<FacilityDetail> _facilityDetialsRepo;

        public FacilitiesController(IAsyncRepository<FacilityMaster> facilityRepo, IAsyncRepository<FacilityDetail> facilityDetialsRepo)
	    {
            this._facilityRepo = facilityRepo;
            this._facilityDetialsRepo = facilityDetialsRepo;
	    }

        [HttpGet]
        [Route("api/facilites")]
        public async Task<IHttpActionResult> GetFacilites()
        {
            var facilites = await _facilityRepo.GetAllAsync();
            if (facilites.Any())
            {
                return Ok(facilites);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
