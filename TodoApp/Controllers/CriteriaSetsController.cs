using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Common;
using Ninject;
using iAgentDataTool.Models.SmartAgentModels;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace TodoApp.Controllers.CriteriaRecords
{
    [Route("api/[controller]")]
    public class CriteriaSetsController : ApiController
    {
        IAsyncRepository<CriteriaSets> _criteriaSetsRepo;

        public CriteriaSetsController(IAsyncRepository<CriteriaSets> criteriaSetsRepo)
        {
            this._criteriaSetsRepo = criteriaSetsRepo;
        }

        [HttpGet]
        //[Route("api/criteriaSets/{criteriaSetName}")]
        //[ResponseType(typeof(CriteriaSets))]
        public async Task<IHttpActionResult> GetCriteriaSetsByName(string criteriaSetName)
        {
            var criteriaSets = await _criteriaSetsRepo.FindByName(criteriaSetName);
            if (criteriaSets.Any())
            {
                return Ok(criteriaSets);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        [Route("api/criteriaSets/{criteriaSetKey:Guid}")]
        public async Task<IHttpActionResult> GetCriteriSetsWithGuid(Guid criteriaSetKey)
        {
            var criteriaSets = await _criteriaSetsRepo.FindWithGuidAsync(criteriaSetKey);
            if (criteriaSets.Any())
            {
                return Ok(criteriaSets);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
