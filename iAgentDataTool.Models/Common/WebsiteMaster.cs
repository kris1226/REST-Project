using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class WebsiteMaster
    {
        public string WebsiteDescription { get;  private set; }
        public Guid WebsiteKey { get; private set; }
        public string WebsiteDomain { get; private set; }
        public int Portal_Id { get; private set; }
        public string DeviceId { get; private set; }

        public WebsiteMaster()
        {
        }

        private WebsiteMaster(Guid websiteKey, 
                              string websiteDesription = "no desctiption found", 
                              string websiteDoman = "no domain found", 
                              string deviceId = "no device id found", 
                              int portalId = 0)
        {
            WebsiteDescription = websiteDesription;
            WebsiteDomain = websiteDoman;
            Portal_Id = portalId;
            DeviceId = deviceId;
            WebsiteKey = websiteKey;
        }

        public static WebsiteMaster CreateWebsiteMaster(string websiteDesription, string websiteDoman, string deviceId, Guid websiteKey, int portalId = 0)
        {
            return new WebsiteMaster(
                websiteKey: websiteKey, 
                websiteDoman: websiteDoman, 
                websiteDesription: websiteDesription, 
                deviceId: deviceId, 
                portalId: portalId
            );
        }

        public override string ToString()
        {
            return string.Join(" | ", new string[] {
                string.Format("{0}", this.WebsiteDescription),
                string.Format("{0}", this.WebsiteKey),
                string.Format("{0}", this.WebsiteDomain),
                string.Format("{0}", this.DeviceId),
                string.Format("{0}", this.Portal_Id)
            });
        }
    }
}
