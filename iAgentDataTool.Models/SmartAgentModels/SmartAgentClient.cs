using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.SmartAgentModels
{
    public class SmartAgentClient
    {
        public Guid ClientKey { get; set; }
        public Guid  ClientLocationKey { get; set; }
        public Guid  FacilityKey { get; set; }
        public string ClientId { get; set; }
        public string TpId { get; set; }
        public string FacilityId { get; set; }
        public string ClientName { get; set; }
        public string ClientLocationName { get; set; }
        public string OrderMap { get; set; }
        public string HowToDeilver { get; set; }
        public string DeviceId { get; set; }
    }
}
