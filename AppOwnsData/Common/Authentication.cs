using AppOwnsData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppOwnsData.Common
{
    public class Authentication
    {
        HttpClient authclient = new HttpClient();
        private EmbedSettingModel _embedSetting;
        public Authentication(IOptions<EmbedSettingModel> options)
        {
            _embedSetting = options.Value;
        }
        public async Task<string> GetAccessToken()
        {
            var authContent = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", _embedSetting.PbiUserName),
                new KeyValuePair<string, string>("password", _embedSetting.PbiPassword),
                new KeyValuePair<string, string>("client_id", _embedSetting.ApplicationId),
                new KeyValuePair<string, string>("resource", _embedSetting.ResourceUrl)
            });

            var accessToken = await authclient.PostAsync(_embedSetting.TokenEndPoint, authContent).ContinueWith<string>((response) =>
            {
                AzureAdTokenResponseModel tokenRes =
                JsonConvert.DeserializeObject<AzureAdTokenResponseModel>(response.Result.Content.ReadAsStringAsync().Result);
                return tokenRes?.AccessToken;
            });

            return accessToken ;
        }
    }
}
