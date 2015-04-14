using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.SmartAgentModels
{
    public class CriteriaDetails
    {
        [Key]
        public Guid CriteriaDetailKey { get; set; }
        public Guid CriteriaSetKey { get; set; }
        public Guid FieldKey { get; set; }
        public int FieldPostion { get; set; }
        public string CompareValue { get; set; }
        public string DeviceId { get; set; }
    }
}
