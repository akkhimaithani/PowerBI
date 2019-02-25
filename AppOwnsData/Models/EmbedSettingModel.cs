using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppOwnsData.Models
{
    public class EmbedSettingModel
    {
        public string PbiUserName { get; set; }
        public string PbiPassword { get; set; }
        public string AuthorityUrl { get; set; }
        public string ResourceUrl { get; set; }
        public string ApplicationId { get; set; }
        public string ApiUrl { get; set; }
        public string WorkspaceId { get; set; }
        public string ReportId { get; set; }
        public string TokenEndPoint { get; set; }
        public string GroupId { get; set; }
    }
}
