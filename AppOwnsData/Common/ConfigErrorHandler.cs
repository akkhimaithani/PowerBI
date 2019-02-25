using AppOwnsData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppOwnsData.Common
{
    public class ConfigErrorHandler
    {
        HttpClient authclient = new HttpClient();
        private EmbedSettingModel _embedSetting;
        public ConfigErrorHandler(IOptions<EmbedSettingModel> options)
        {
            _embedSetting = options.Value;
        }
        public string GetWebConfigErrors()
        {
            // Application Id must have a value.
            if (string.IsNullOrWhiteSpace(_embedSetting.ApplicationId))
            {
                return "ApplicationId is empty. please register your application as Native app in https://dev.powerbi.com/apps and fill client Id in web.config.";
            }

            // Application Id must be a Guid object.
            Guid result;
            if (!Guid.TryParse(_embedSetting.ApplicationId, out result))
            {
                return "ApplicationId must be a Guid object. please register your application as Native app in https://dev.powerbi.com/apps and fill application Id in web.config.";
            }

            // Workspace Id must have a value.
            if (string.IsNullOrWhiteSpace(_embedSetting.WorkspaceId))
            {
                return "WorkspaceId is empty. Please select a group you own and fill its Id in web.config";
            }

            // Workspace Id must be a Guid object.
            if (!Guid.TryParse(_embedSetting.WorkspaceId, out result))
            {
                return "WorkspaceId must be a Guid object. Please select a workspace you own and fill its Id in web.config";
            }

            // Username must have a value.
            if (string.IsNullOrWhiteSpace(_embedSetting.PbiUserName))
            {
                return "Username is empty. Please fill Power BI username in web.config";
            }

            // Password must have a value.
            if (string.IsNullOrWhiteSpace(_embedSetting.PbiPassword))
            {
                return "Password is empty. Please fill password of Power BI username in web.config";
            }

            return null;
        }


        
    }
}
