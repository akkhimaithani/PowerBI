using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppOwnsData.Models
{
    public class AzureAdTokenResponseModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
