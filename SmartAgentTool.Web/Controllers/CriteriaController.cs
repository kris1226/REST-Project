using iAgentDataTool.Models.SmartAgentModels;
using iAgentDataTool.Repositories.SmartAgentRepos;
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
    public class CriteriaController : ApiController
    {
        ISmartAgentRepo _smartAgentRepo;
        public CriteriaController(ISmartAgentRepo smartAgentRepo)
        {
            this._smartAgentRepo = smartAgentRepo;
        }
        [HttpPost]
        [Route("api/criteria")]
        [ResponseType(typeof(Criteria))]
        public async Task<IHttpActionResult> PostCriteria(Criteria criteria)
        {
            var response = Request.CreateResponse<Criteria>(HttpStatusCode.Created, criteria);
            if (criteria.CriteriaSetName != null)
            {
                var records = await _smartAgentRepo.CreateCriteraRecords(criteria);
                string uri = Url.Link("DefaultApi", new { values = records });
                response.Headers.Location = new Uri(uri);
                return this.Ok();
            }
            return this.BadRequest();
        }
    }
}
