using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iAgentDataTool.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace iAgentDataTool.Models.Common
{
    public class ClientMaster
    {
        public string ClientName { get; private set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid ClientKey { get; private set; }

        public string HowToDeliver { get; private set; }
        public string DeviceId { get; private set; }

        public ClientMaster(string clientName, Guid clientKey, string howToDeliver)
        {
            this.ClientName = clientName;
            this.ClientKey = clientKey;
            this.HowToDeliver = howToDeliver;
            this.DeviceId = clientName.Split(' ').FirstOrDefault();
        }
        public ClientMaster()
        {
        }
        public static ClientMaster CreateClientMaster(string clientName, Guid clientKey, string howToDeliver="OCSVC")
        {
            return new ClientMaster(clientName, clientKey, howToDeliver);
        }
        public override string ToString()
        {
            return string.Join(" | ", new string[] {
                string.Format("ClientKey: {0}", this.ClientKey),
                string.Format("Client Name: {0}", this.ClientName),
                string.Format("How to Deliver: {0}", this.HowToDeliver),
                string.Format("Device Id: {0}", this.DeviceId)
            });
        }
    }

}
