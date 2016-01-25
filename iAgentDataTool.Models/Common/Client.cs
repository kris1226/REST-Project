using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class Client
    {
        public string ClientName { get; set; }
        public string ClientLocationName { get; set; }
        public string DeviceId { get; set; }
        public string HowToDeliver { get; set; }
        public string Tpid { get; set; }
        public string ClientId { get; set; }
        public string FacilityId { get; set; }
        public Guid ClientKey { get; set; }
        public Guid ClientLocationKey { get; set; }
        public Guid FacilityKey { get; set; }

        public Client(Client client)
        {
            ClientName = client.ClientName;
            ClientLocationName = client.ClientLocationName;
            DeviceId = client.DeviceId;
            HowToDeliver = client.HowToDeliver;

        }

        public static Client CreateClient(Client client)
        {
           return new Client(client);
        }
    }
}
