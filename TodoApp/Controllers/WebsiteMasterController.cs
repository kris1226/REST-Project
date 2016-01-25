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
    public class WebsiteMasterController : ApiController
    {
        [Inject]
        IAsyncRepository<WebsiteMaster> _websiteRepo;

        public WebsiteMasterController(IAsyncRepository<WebsiteMaster> websiteRepo)
        {
            this._websiteRepo = websiteRepo;
        }

        [HttpGet]
        [Route("api/websites")]
        public async Task<IHttpActionResult> GetWebsites()
        {
            var websites = await _websiteRepo.GetAllAsync();
            if (websites.Any())
            {
                return Ok(websites);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
