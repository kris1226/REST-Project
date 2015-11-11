using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.SmartAgentModels
{
    public class ClientApps
    {
        [Key]
        public Guid ClientLocationKey { get; set; }
        public Guid AppKey 
        { 
            get 
            { 
                return AppKey;
            } 
            set 
            { 
                new Guid("7B1580D7-DD22-47F5-A369-B7C47B9132EA"); 
            } 
        } 
    }
}
