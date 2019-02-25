using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppOwnsData.Models
{
    public class PowerBiDashboardModel
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "displayName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "embedUrl")]
        public string EmbedUrl { get; set; }

        public List<PowerBiDashboardModel> value { get; set; }
    }
}
