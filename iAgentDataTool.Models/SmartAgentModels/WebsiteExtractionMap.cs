using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.SmartAgentModels
{
    public class WebsiteExtractionMap
    {
        public int RecKey { get; set; }
        public Guid ExtractKey { get; set; }
        public Guid WebsiteKey { get; set; }
        public string DataName { get; set; }
        public string DocumentLocation{ get; set; }
        public string LocationType { get; set; }
        public string LocationValue { get; set; }
        public string ValueFunction { get; set; }
        public string FormatFunction { get; set; }
        public int Priority { get; set; }
    }
}
