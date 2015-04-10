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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ClientKey { get; set; }
        public string ClientName { get; set; }
        public string HowToDeliver { get; set; }

        public override string ToString()
        {
            return string.Join("   ", new string[] {
                string.Format("{0}", this.ClientKey),
                string.Format("{0}", this.ClientName),
                string.Format("{0}", this.HowToDeliver)
            });
        }
    }

}
