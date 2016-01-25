using iAgentDataTool.Helpers.Interfaces;
using iAgentDataTool.Models.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Net;
using System.Net.Http;

namespace SmartAgent.Web.Controllers
{
    [EnableCors("*", "*", "GET")]
    public class UpwController : ApiController
    {
        IAsyncRepository<Upw> _repo;
        IUpwAsyncRepository _upwRepo;
        public UpwController(IAsyncRepository<Upw> repo, IUpwAsyncRepository upwRepo)
        {
            this._upwRepo = upwRepo;
            this._repo = repo;
        }
        [HttpGet]
        [Route("api/upw/{term}")]
        public async Task<IHttpActionResult> Get(string term)
        {
            IEnumerable<Upw> records = null;
            if (IsEntKey(term))
            {
                records = await _upwRepo.FindWithEntKey(term);
                if (RecordsFound(records))
                {
                    return Ok(records);
                }
                else
                {
                    return NotFound();
                }
            }
            records = await _repo.FindByName(term);
            if (RecordsFound(records))
            {
                return Ok(records);
            }
            return NotFound();
        }

        bool IsEntKey(string term)
        {
            if (term.Contains("000"))
            {
                return true;
            }
            return false;            
        }

        bool RecordsFound<T>(IEnumerable<T> records)
        {
            if (records.Any())
            {
                return true;
            }
            return false;
        }

    }
}