using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject;
using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.Models.Common;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace SmartAgent.Web.Controllers
{
    public class FacilityDetailsController : ApiController
    {
        [Inject]
        IAsyncRepository<FacilityDetail> _facilityDetailsRepo;
        public FacilityDetailsController(IAsyncRepository<FacilityDetail> facilityDetailsRepo)
        {
            this._facilityDetailsRepo = facilityDetailsRepo;
        }
        [HttpGet]
        [Route("api/facilityDetails/{facilityKey}")]
        [ResponseType(typeof(FacilityDetail))]
        public async Task<IHttpActionResult> GetFacilityDetail(Guid facilityKey)
        {
            if (facilityKey == null)
            {
                return NotFound();
            }
            var facilityDetails = await _facilityDetailsRepo.FindWithGuidAsync(facilityKey);
            return Ok(facilityDetails);
        }
    }

}
