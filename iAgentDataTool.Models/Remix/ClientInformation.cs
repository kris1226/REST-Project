using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using iAgentDataTool.Models.Common;

namespace iAgentDataTool.Models
{
    public class ClientInformation
    {
        public string ClientName { get; set; }
        public Guid ClientKey { get; set; }
        public Guid ClientLocationKey { get; set; }
        public Guid FacilityKey { get; set; }
        public string TradingPartnerId { get; set; }
        public string ClientId { get; set; }
        public string FacilityId { get; set; }
        public int ClientLocationId { get; set; }
        public string ParentId { get; set; }
        public string HowToDeliver { get; set; }
        public List<PayerWebsiteMappingValue> PayerWebsiteMappingValues { get; set; }
        public List<ClientMappingMaster> ClientMappingMaster { get; set; }
        public List<ClientMappingValue> ClientMappingValues { get; set; }

        public override string ToString()
        {
            return string.Join(", ", new string[] 
            {
                string.Format("{0}", this.ClientName),
                string.Format(" {0}", this.ClientKey),
                string.Format(" {0}", this.ClientLocationKey),
                string.Format(" {0}", this.TradingPartnerId),
                string.Format(" {0}", this.FacilityId),
                string.Format(" {0}", this.ClientLocationId),
                string.Format(" {0}", this.ParentId)
            });
        }
        public void LoadClientInformation(SqlDataReader reader)
        {
            ClientKey = (Guid)reader["Clientkey"];
            ClientLocationKey = (Guid)reader["Clientlocationkey"];
            TradingPartnerId = reader["TpId"].ToString();
            FacilityId = reader["FacilityId"].ToString();
            ClientId = reader["ClientId"].ToString();
            FacilityKey = (Guid)reader["FacilityKey"];
            ClientLocationId = (int)reader["Id"];
            ParentId = reader["Parent_Id"].ToString();
        }
    }
}
