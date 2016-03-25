using iAgentDataTool.ScriptHelpers.Interfaces;
using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.SmartAgentRepos;
using SmartAgent.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web;

namespace SmartAgent.Web.Controllers
{
    [EnableCors("*", "*", "GET", "POST")]
    public class CriteriaController : ApiController
    {
        IAsyncRepository<CriteriaSets> _criteriaSetsRepo;
        ISmartAgentRepo _smartAgentRepo;

        public CriteriaController(IAsyncRepository<CriteriaSets> criteriaSetsRepo, ISmartAgentRepo smartAgentRepo)
        {
            this._criteriaSetsRepo = criteriaSetsRepo;
            _smartAgentRepo = smartAgentRepo;
        }

        [HttpGet]
        [Route("api/criteria/{criteriaSetName}")]
        public async Task<IHttpActionResult> GetCriteriaSets(string criteriaSetName)
        {
            if (criteriaSetName.IsValid())
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
            return BadRequest();
        }
        [HttpGet]
        [Route("api/criteria/{criteriaSetKey:Guid}")]
        public async Task<IHttpActionResult> GetCriteriSets(Guid criteriaSetKey)
        {            
            var criteriaSets = await _criteriaSetsRepo.FindWithGuidAsync(criteriaSetKey);
            if (criteriaSets.Any())
            {
                return Ok(criteriaSets);
            }
            return NotFound();       
        }
        [HttpPost]
        [Route("api/criteria")]
        public async Task<IHttpActionResult> Post([FromBody]Criteria criteriaSet)
        {
            var newCriteraSet = Criteria.CreateCriteria(criteriaSet);

            var result = await _smartAgentRepo.CreateCriteraRecord(criteriaSet);
            if (result.Any())
            {
                return Created<Criteria>(Request.RequestUri + "/" + newCriteraSet.CriteriaSetKey.ToString(), newCriteraSet);
            }
            return BadRequest();
        }
    }
}
