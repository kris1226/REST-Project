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
    public class ClientMaster {

        private Guid _clientKey;
        private string _clientName;
        private string _howToDeliver;
        private string _deviceId;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid ClientKey { get { return _clientKey; } }
        public string HowToDeliver { get { return _howToDeliver;  } }
        public string ClientName { get { return _clientName; } }
        public string DeviceId { get { return _deviceId; } }

        public ClientMaster(Guid clientKey, string clientName = "NoclientName", string howToDeliver = "OCSVC", string deviceId="nodata")
        {
            _clientName = clientName;
            _clientKey = clientKey;
            _howToDeliver = howToDeliver;
            _deviceId = deviceId;
        }
        public override string ToString()
        {
            return string.Join(" | ", new string[] {
                string.Format("ClientKey: {0}", _clientKey),
                string.Format("{0}", _clientName),
                string.Format("How to Deliver: {0}", _howToDeliver),
                string.Format("{0}", _deviceId)
            });
        }
    }

}
