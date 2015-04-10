using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Data.Linq;
using System.ComponentModel.DataAnnotations;

namespace SmartAgent.Models
{
    public class CriteriaSets : IEntity
    {
        [Key]
        public virtual Guid CriteriaSetKey { get; set; }
        public virtual String CriteriaSetName { get; set; }
        public virtual Guid StartingScriptKey { get; set; }
        public virtual string DeviceId { get; set; }

        public Guid Key
        {
            get { throw new NotImplementedException(); }
        }
    }
}
