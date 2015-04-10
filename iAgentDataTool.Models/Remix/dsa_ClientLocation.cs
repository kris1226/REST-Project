using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iAgentDataTool.Models.Common;

namespace iAgentDataTool.Models.Common
{
    public class ClientLocations : AgentClient
    {
        public int Id { get; set; }
        public int Parent_Id { get; set; }
        public string ClientLocationName { get; set; }
        public Guid ClientLocationKey { get; set; }
        public string ClientId { get; set; }
        public string TpId { get; set; }
        public string FacilityId { get; set; }
    
        public override string ToString()
        {
            return string.Join(", ", new string[] 
            {
                string.Format("{0}", this.ClientLocationName),
                string.Format(" {0}", this.ClientKey),
                string.Format(" {0}", this.ClientLocationKey),
                string.Format(" {0}", this.TpId),
                string.Format(" {0}", this.ClientId),
                string.Format(" {0}", this.FacilityId),
                string.Format(" {0}", this.Id),
                string.Format(" {0}", this.Parent_Id)
            });
        }
    }
}
