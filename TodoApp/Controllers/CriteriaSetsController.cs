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

namespace TodoApp.Controllers
{
    public class CriteriaSetsController : ApiController
    {
        IAsyncRepository<CriteriaSets> _criteriaSetsRepo;

        public CriteriaSetsController(IAsyncRepository<CriteriaSets> criteriaSetsRepo)
        {
            this._criteriaSetsRepo = criteriaSetsRepo;
        }

        [HttpGet]
        [Route("api/criteriaSets/{criteriaSetName}")]
        public async Task<IHttpActionResult> GetCriteriSets(string criteriaSetName)
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
    }
}
