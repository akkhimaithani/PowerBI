using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppOwnsData.Models
{
    public class PowerBiReportModel
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "webUrl")]
        public string WebUrl { get; set; }

        [JsonProperty(PropertyName = "embedUrl")]
        public string EmbedUrl { get; set; }

        [JsonProperty(PropertyName = "datasetId")]
        public string DatasetId { get; set; }

        public List<PowerBiReportModel> value { get; set; }
    }
}
