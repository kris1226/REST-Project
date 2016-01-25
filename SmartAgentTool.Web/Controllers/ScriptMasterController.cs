using iAgentDataTool.Models.Common;
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
    public class ScriptMasterController : ApiController
    {
        IScriptCreation _scriptRepo;
        public ScriptMasterController(IScriptCreation scriptRepo)
        {
            this._scriptRepo = scriptRepo;
        }
        [HttpGet]
        [Route("api/Scripts/{websiteKey}")]
        [ResponseType(typeof(Script))]
        public async Task<IHttpActionResult> GetScripts(Guid websiteKey)
        {
                var results = await _scriptRepo.GetScripts(websiteKey);
                if (results.Any())
                {
                    return Ok(results);
                }
                return NotFound();
        }

        [HttpPost]
        [Route("api/Script")]
        [ResponseType(typeof(Script))]
        public async Task<IHttpActionResult> PostScript(Script script)
        {
            var response = Request.CreateResponse<Script>(HttpStatusCode.Created, script);
            if (script.Code != null)
            {
                var scriptKey = await _scriptRepo.CreateScritp(script);
                return this.Ok();
            }
            else
            {
                return this.BadRequest();
            }
        }
    }
}
