using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class dsa_clientMaster
    {
        public Nullable<System.DateTime> dateAdded { get; set; }
        public Nullable<System.DateTime> dateChanged { get; set; }
        public string lastUserID { get; set; }
        public string deviceID { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid clientKey { get; set; }
        public string clientName { get; set; }
        public string clientAddress { get; set; }
        public string clientAddress1 { get; set; }
        public string clientCity { get; set; }
        public string clientState { get; set; }
        public string clientZip { get; set; }
        public string clientPhone { get; set; }
        public string clientFax { get; set; }
        public string clientContactName { get; set; }
        public string clientContacteMail { get; set; }
        public string clientContactPhone { get; set; }
        public string clientContactPhoneType { get; set; }
        public byte ImageDelay { get; set; }
        public string HowToDeliver { get; set; }
        public string AutoRunUser { get; set; }
    }
}
