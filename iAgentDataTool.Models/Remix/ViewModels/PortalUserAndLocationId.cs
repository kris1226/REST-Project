using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Remix.ViewModels
{
    public class PortalUserAndLocationId
    {
        public string Description { get; set; }
        public int PortalUser_Id { get; set; }
        public int Portal_Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsExpired { get; set; }
        public DateTime DateExpired { get; set; }
        public bool IsEnabled { get; set; }
        public bool AllowsConcurrent { get; set; }
        public int Location_Id { get; set; }

        public override string ToString()
        {
            return string.Join(" | ", new string[] {
                string.Format("Portal User Id: {0}",  this.PortalUser_Id),
                string.Format("Portal Id {0}", this.Portal_Id),
                string.Format("Location Id: {0}", this.Location_Id),
                string.Format("Username {0}", this.Username),
                string.Format("Password {0}", this.Password),
                string.Format("Is Expired? {0}", this.IsExpired),
                string.Format("Date Expired {0}", this.DateExpired),
                string.Format("Is Enabled? {0}", this.IsEnabled),
                string.Format("Allow Concurrent? {0}", this.AllowsConcurrent)
            });
        }
    }

}
