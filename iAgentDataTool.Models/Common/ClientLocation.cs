using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iAgentDataTool.Models.Common;

namespace iAgentDataTool.Models.Common
{
    public class ClientLocations : AgentClient
    {
        public string ClientLocationName { get; set; }
        public int Id { get; set; }
        public int Parent_Id { get; set; }
        public virtual Guid ClientLocationKey { get; set; }
        public string ClientId { get; set; }
        public string TpId { get; set; }
        public string FacilityId { get; set; }
        public string DeviceId { get; set; }
        public string LastUserId { get; set; }

        public ClientLocations(){}
        private ClientLocations(string clientLocationName, Guid clientKey, Guid clientLocationKey, string clientId, string tpid, string facilityId)
        {
            this.ClientLocationName = clientLocationName;
            this.ClientKey = clientKey;
            this.ClientId = clientId;
            this.TpId = tpid;
            this.FacilityId = facilityId;
            this.DeviceId = clientLocationName.Split(' ').FirstOrDefault();
        }
        public static ClientLocations CreateClientLocation(string clientLocationName, Guid clientKey, Guid clientLocationKey, string clientId, string tpid, string facilityId)
        {
            return new ClientLocations(clientLocationName, clientKey, clientLocationKey, clientId, tpid, facilityId);
        }
        public override string ToString()
        {
            return string.Join(" | ", new string[] 
            {
                string.Format("{0}", this.ClientLocationName),
                string.Format("Client Key: {0}", this.ClientKey),
                string.Format("Client Location Key: {0}", this.ClientLocationKey),
                string.Format("Tpid: {0}", this.TpId),
                string.Format("Client Id: {0}", this.ClientId),
                string.Format("Facility Id: {0}", this.FacilityId),
                string.Format("Client Location Id: {0}", this.Id),
                string.Format("Parent Id: {0}", this.Parent_Id),
                string.Format("Device Id: {0}", this.DeviceId)
            });
        }
    }
}
