using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Remix
{
    public class ExtractionMap
    {
        public int ExtractionMapId { get; set; }
        public int PayerResponseId { get; set; }
        public string LocationType { get; set; }
        public string Location { get; set; }
        public string DataItem { get; set; }
        public string PageType { get; set; }
        public string NormalizeFunction { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string AttributeName { get; set; }
    }
}
