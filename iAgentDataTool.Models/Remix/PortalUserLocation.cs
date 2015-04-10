using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models
{
    public class PortalUserLocation
    {
        public int PortalUserId { get; set; }
        public int LocationId { get; set; }

        public override string ToString()
        {
            return string.Join(", ", new string[] {
                string.Format("{0}",  this.PortalUserId)
            });
            
        }
    }  
}
