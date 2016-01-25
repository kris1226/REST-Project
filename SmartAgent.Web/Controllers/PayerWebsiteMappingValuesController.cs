using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using iAgentDataTool.Models.Common;
using iAgentDataTool.Helpers.Interfaces;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace SmartAgent.Web.Controllers
{
    public class PayerWebsiteMappingValuesController : ApiController
    {
        [Inject]
        IAsyncRepository<PayerWebsiteMappingValue> _payerWebsiteMappingValuesRepo;
        public PayerWebsiteMappingValuesController(IAsyncRepository<PayerWebsiteMappingValue> payerWebsiteMappingValuesRepo)
        {
            this._payerWebsiteMappingValuesRepo = payerWebsiteMappingValuesRepo;
        }
        [HttpGet]
        [Route("api/payerWebsiteMappingValues/{clientKey}/{clientLocationKey}")]
        [ResponseType(typeof(PayerWebsiteMappingValue))]
        public async Task<IHttpActionResult> GetPayerWebsiteMappingValues(Guid clientKey, Guid clientLocationKey)         
        {
            if (clientKey == null || clientLocationKey == null)
            {
                return NotFound();
            }
            var payerWebsiteMappingValues = await _payerWebsiteMappingValuesRepo.FindWith2GuidsAsync(clientKey, clientLocationKey);
            if (payerWebsiteMappingValues.Any())
            {
                return Ok(payerWebsiteMappingValues);
            }
            return NotFound();
            
        }
        [HttpPost]
        [Route("api/payerWebsiteMappingValues/{clientKey}/{clientLocationKey}")]
        [ResponseType(typeof(PayerWebsiteMappingValue))]
        public async Task<IHttpActionResult> PostPayerWebsiteMappingValues(string clientKey, string clientLocationKey)
        {
            if (clientKey == null || clientLocationKey == null)
            {
                return NotFound();
            }
            var newRecord = Initilize(clientKey, clientLocationKey);
            var key = await _payerWebsiteMappingValuesRepo.AddAsync(newRecord);
            var badGuid = new Guid("00000000-0000-0000-0000-000000000000");
            if (key != null && key != badGuid)
            {
                return Ok(key);
            }
            return NotFound();
        }

        private static IEnumerable<PayerWebsiteMappingValue> Initilize(string clientKey, string clientLocationKey)
        {
            var mappingValues = new List<PayerWebsiteMappingValue>();
            var newRecord = new PayerWebsiteMappingValue();
            newRecord.ClientKey = clientKey;
            newRecord.ClientLocationKey = clientLocationKey;
            mappingValues.Add(newRecord);
            return mappingValues;
        }
    }
}
