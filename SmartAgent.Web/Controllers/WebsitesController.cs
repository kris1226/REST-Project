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
using System.Web.Http.Description;

namespace SmartAgent.Web.Controllers
{
    public class WebsitesController : ApiController
    {
        [Inject]
        IAsyncRepository<WebsiteMaster> _websiteRepo;

        [Inject]
        public WebsitesController(IAsyncRepository<WebsiteMaster> websiteRepo)
        {
            this._websiteRepo = websiteRepo;
        }

        [HttpGet]
        [Route("api/websites")]
        [ResponseType(typeof(WebsiteMaster))]
        public async Task<IHttpActionResult> GetAllWebsites()
        {
            var websites = await _websiteRepo.GetAllAsync();
            if (websites.Any())
            {
                return this.Ok(websites);
            }
            else
            {
                return this.NotFound();
            }
        }
        [HttpGet]
        [Route("api/websites/{websiteDescription}")]
        [ResponseType(typeof(WebsiteMaster))]
        public async Task<IHttpActionResult> GetWebsite(string wildCard)
        {
            var websites = await _websiteRepo.FindByName(wildCard);
            if (websites.Any())
            {
                return Ok(websites);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        [Route("api/websites")]
        [ResponseType(typeof(WebsiteMaster))]
        public async Task<IHttpActionResult> PostWebsite(IEnumerable<WebsiteMaster> newWebsite)
        {
            foreach (var item in newWebsite)
            {
                var response = Request.CreateResponse<WebsiteMaster>(HttpStatusCode.Created, item);
                if (item.WebsiteDescription != null)
                {
                    var key = await _websiteRepo.AddAsync(newWebsite);
                    string uri = Url.Link("DefaultApi", new { key });
                    response.Headers.Location = new Uri(uri);
                    return this.Ok();
                }
                return this.BadRequest();
            }
            return this.BadRequest();
        }
    }
}
