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
    public class CriteriaDetailsController : ApiController
    {
        IAsyncRepository<CriteriaDetails> _criteriaDetialsRepo;

        public CriteriaDetailsController(IAsyncRepository<CriteriaDetails> criteriaDetialsRepo)
        {
            this._criteriaDetialsRepo = criteriaDetialsRepo;
        }

        [HttpGet]
        [Route("api/criteriaDetails/{criteriaSetName}")]
        public async Task<IHttpActionResult> GetCriteriaDetailsWithCriteriaSetname(string criteriaSetname)
        {
            var criteriaDetials = await _criteriaDetialsRepo.FindByName(criteriaSetname);
            if (criteriaDetials.Any())
            {
                return Ok(criteriaDetials);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
