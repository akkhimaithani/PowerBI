using AppOwnsData.Common;
using AppOwnsData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppOwnsData.Pages.Embed
{
    public class DefaultReportModel : PageModel
    {
        private EmbedSettingModel _embedSetting;
        private ConfigErrorHandler _configError;
        HttpClient powerBiClient = new HttpClient();

        public DefaultReportModel(IOptions<EmbedSettingModel> options, ConfigErrorHandler error)
        {
            _embedSetting = options.Value;
            _configError = error;
        }

        public void OnGet()
        {
          

        }

     


    }
}
