using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Models.Common
{
    public class FacilityMaster 
    {
        public string FacilityName { get; private set; }
        [Key]
        public  int RecordKey { get; set; }
        public string Ordermap { get; set; }
        public  virtual Guid FacilityKey { get; private set; }
        public  virtual string ClientKey { get; private set; }
        public  virtual string ClientLocationKey { get; private set; }

        public FacilityMaster(){}

        private FacilityMaster(string facilityName, Guid facilityKey, string orderMap, Guid clientKey, Guid clientLocationKey)
        {
            this.FacilityName = facilityName;
            this.FacilityKey = FacilityKey;
            this.Ordermap = orderMap;
            this.ClientKey = clientKey.ToString();
            this.ClientLocationKey = clientLocationKey.ToString();
        }

        public static FacilityMaster CreateFaciltiy(string facilityName, Guid facilityKey, string orderMap, Guid clientKey, Guid clientLocationKey)
        {
            return new FacilityMaster(facilityName, facilityKey, orderMap, clientKey, clientLocationKey);
        }
        public override string ToString()
        {
            return string.Join(" | ", new string[] {
                string.Format("Record Key {0}: ", this.RecordKey),
                string.Format("Facility Name: {0}", this.FacilityName),
                string.Format("Order Map: {0}", this.Ordermap),
                string.Format("Facility Key {0}", this.FacilityKey),
                string.Format("Client Key {0}", this.ClientKey),
                string.Format("Client Location Key {0}", this.ClientLocationKey)
            });
        }
    }
}
