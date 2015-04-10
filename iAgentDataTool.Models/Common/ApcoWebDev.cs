using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class ApcoWebDev
    {
        public string KopSiteName { get; set; }
        public string Tpid { get; set; }
        public string ClientId { get; set; }
        public string FacilityId { get; set; }
        public string EntKey { get; set; }
        public string SiteKey { get; set; }
        public bool IsRemixClient { get; set; }

        public override string ToString()
        {
            return string.Format(" | ",new string [] {
               string.Format("KOP Site Name:{0}", this.KopSiteName),
               string.Format("Trading Partner Id {0}", this.Tpid),
               string.Format("Client Id {0}", this.ClientId),
               string.Format("Facility Id {0}", this.FacilityId),
               string.Format("Ent Key {0}", this.EntKey),
               string.Format("Site Key {0}", this.SiteKey),
               string.Format("Is Remix client?: {0}", this.IsRemixClient)
            });
        }
    }
}
