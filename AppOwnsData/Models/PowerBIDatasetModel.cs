using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppOwnsData.Models
{
    public class PowerBIDatasetModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "addRowsAPIEnabled")]
        public bool AddRowsAPIEnabled { get; set; }

        [JsonProperty(PropertyName = "configuredBy")]
        public string ConfiguredBy { get; set; }

        [JsonProperty(PropertyName = "isRefreshable")]
        public bool IsRefreshable { get; set; }

        [JsonProperty(PropertyName = "isEffectiveIdentityRequired")]
        public bool IsEffectiveIdentityRequired { get; set; }

        [JsonProperty(PropertyName = "isEffectiveIdentityRolesRequired")]
        public bool IsEffectiveIdentityRolesRequired { get; set; }

        [JsonProperty(PropertyName = "isOnPremGatewayRequired")]
        public bool IsOnPremGatewayRequired { get; set; }

        public List<PowerBIDatasetModel> value { get; set; }
    }
}
