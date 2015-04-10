using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Remix
{
    public class Portal
    {
        public int Id { get; set; }
        public int Portal_Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public Guid WebsiteKey { get; set; }

        public override string ToString()
        {
            return string.Join(", ", new string[] {
                string.Format("{0}",  this.Portal_Id)
            });
        }
    }

}
