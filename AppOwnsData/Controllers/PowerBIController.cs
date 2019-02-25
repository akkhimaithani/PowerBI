using AppOwnsData.Common;
using AppOwnsData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppOwnsData.Controllers
{
    public class PowerBIController : Controller
    {
        private EmbedSettingModel embedSetting;
        private ConfigErrorHandler configError;
        HttpClient authclient = new HttpClient();
        HttpClient powerBiClient = new HttpClient();
        
        public PowerBIController(IOptions<EmbedSettingModel> options, ConfigErrorHandler error)
        {
            embedSetting = options.Value;
            configError = error;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// For getting default report i.e report id is and group id will be staticaly bind 
        /// </summary>
        /// <returns></returns>
        [Route("/getDefaultReport")]
        public async Task<JsonResult> GetDefaultReport()
        {

            var authContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", embedSetting.PbiUserName),
                new KeyValuePair<string, string>("password", embedSetting.PbiPassword),
                new KeyValuePair<string, string>("client_id", embedSetting.ApplicationId),
                new KeyValuePair<string, string>("resource", embedSetting.ResourceUrl)
            });

            var accessToken = await authclient.PostAsync(embedSetting.TokenEndPoint, authContent).ContinueWith<string>((response) =>
            {
                AzureAdTokenResponseModel tokenRes =
                JsonConvert.DeserializeObject<AzureAdTokenResponseModel>(response.Result.Content.ReadAsStringAsync().Result);
                return tokenRes?.AccessToken;
            });


            // Get PowerBi report url and embed token
            powerBiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var embedUrl =
               await powerBiClient.GetAsync($"https://api.powerbi.com/v1.0/myorg/groups/{embedSetting.GroupId}/reports/{embedSetting.ReportId}")
               .ContinueWith<string>((response) =>
               {

                   PowerBiReportModel report =
                   JsonConvert.DeserializeObject<PowerBiReportModel>(response.Result.Content.ReadAsStringAsync().Result);
                   return report?.EmbedUrl;
               });


            var tokenContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("accessLevel", "view")
            });

            var embedToken =
                await powerBiClient.PostAsync($"https://api.powerbi.com/v1.0/myorg/groups/{embedSetting.GroupId}/reports/{embedSetting.ReportId}/GenerateToken", tokenContent)
                .ContinueWith<string>((response) =>
                {

                    PowerBiEmbedTokenModel powerBiEmbedToken =
                    JsonConvert.DeserializeObject<PowerBiEmbedTokenModel>(response.Result.Content.ReadAsStringAsync().Result);
                    return powerBiEmbedToken?.Token;
                });

            // JSON Response
            EmbedContentModel data = new EmbedContentModel
            {
                EmbedToken = embedToken,
                EmbedUrl = embedUrl,
            };
            return new JsonResult(data);
        }

        /// <summary>
        /// Get all the reports 
        /// </summary>
        /// <returns></returns>
        [Route("/getReportList")]
        public async Task<JsonResult> GetReportList()
        {
            var authContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", embedSetting.PbiUserName),
                new KeyValuePair<string, string>("password", embedSetting.PbiPassword),
                new KeyValuePair<string, string>("client_id", embedSetting.ApplicationId),
                new KeyValuePair<string, string>("resource", embedSetting.ResourceUrl)
            });

            var accessToken = await authclient.PostAsync(embedSetting.TokenEndPoint, authContent).ContinueWith<string>((result) =>
            {
                AzureAdTokenResponseModel tokenRes =
                JsonConvert.DeserializeObject<AzureAdTokenResponseModel>(result.Result.Content.ReadAsStringAsync().Result);
                return tokenRes?.AccessToken;
            });

            // Get PowerBi report url and embed token
            powerBiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var response = await powerBiClient.GetAsync($"https://api.powerbi.com/v1.0/myorg/groups/{embedSetting.GroupId}/reports");
            string responseData = response.Content.ReadAsStringAsync().Result;
            var responseObj = JsonConvert.DeserializeObject<PowerBiReportModel>(responseData);
            List<PowerBiReportModel> listOfReport = new List<PowerBiReportModel>();
            listOfReport = responseObj.value;
            return Json(listOfReport);

        }

        /// <summary>
        /// Get report by report key 
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [Route("/getReportByReportId")]
        public async Task<JsonResult> GetReportByReportId(string reportId)
        {
            var accessToken = string.Empty;
            var authContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", embedSetting.PbiUserName),
                new KeyValuePair<string, string>("password", embedSetting.PbiPassword),
                new KeyValuePair<string, string>("client_id", embedSetting.ApplicationId),
                new KeyValuePair<string, string>("resource", embedSetting.ResourceUrl)
            });

            accessToken = await authclient.PostAsync(embedSetting.TokenEndPoint, authContent).ContinueWith<string>((response) =>
            {
                AzureAdTokenResponseModel tokenRes =
                JsonConvert.DeserializeObject<AzureAdTokenResponseModel>(response.Result.Content.ReadAsStringAsync().Result);
                return tokenRes?.AccessToken;
            });


            // Get PowerBi report url and embed token
            powerBiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var embedUrl =
               await powerBiClient.GetAsync($"https://api.powerbi.com/v1.0/myorg/groups/{embedSetting.GroupId}/reports/{reportId}")
               .ContinueWith<string>((response) =>
               {

                   PowerBiReportModel report =
                   JsonConvert.DeserializeObject<PowerBiReportModel>(response.Result.Content.ReadAsStringAsync().Result);
                   return report?.EmbedUrl;
               });


            var tokenContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("accessLevel", "view")
            });

            var embedToken =
                await powerBiClient.PostAsync($"https://api.powerbi.com/v1.0/myorg/groups/{embedSetting.GroupId}/reports/{reportId}/GenerateToken", tokenContent)
                .ContinueWith<string>((response) =>
                {

                    PowerBiEmbedTokenModel powerBiEmbedToken =
                    JsonConvert.DeserializeObject<PowerBiEmbedTokenModel>(response.Result.Content.ReadAsStringAsync().Result);
                    return powerBiEmbedToken?.Token;
                });

            // JSON Response
            EmbedContentModel data = new EmbedContentModel
            {
                EmbedToken = embedToken,
                EmbedUrl = embedUrl.Replace("https://app", "https://msit"),
            };

            return new JsonResult(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("/getDashboardList")]
        public async Task<JsonResult> GetDashboardList()
        {
            var authContent = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", embedSetting.PbiUserName),
                new KeyValuePair<string, string>("password", embedSetting.PbiPassword),
                new KeyValuePair<string, string>("client_id", embedSetting.ApplicationId),
                new KeyValuePair<string, string>("resource", embedSetting.ResourceUrl)
            });

            var accessToken = await authclient.PostAsync(embedSetting.TokenEndPoint, authContent).ContinueWith<string>((result) =>
            {
                AzureAdTokenResponseModel tokenRes =
                JsonConvert.DeserializeObject<AzureAdTokenResponseModel>(result.Result.Content.ReadAsStringAsync().Result);
                return tokenRes?.AccessToken;
            });

            // Get PowerBi report url and embed token
            powerBiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var response = await powerBiClient.GetAsync($"https://api.powerbi.com/v1.0/myorg/groups/{embedSetting.GroupId}/dashboards");
            string responseData = response.Content.ReadAsStringAsync().Result;
            var responseObj = JsonConvert.DeserializeObject<PowerBiDashboardModel>(responseData);
            List<PowerBiDashboardModel> listOfReport = new List<PowerBiDashboardModel>();
            listOfReport = responseObj.value;
            return Json(listOfReport);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dashboardId"></param>
        /// <returns></returns>
        [Route("/getDashboardById")]
        public async Task<JsonResult> GetDashboardById(string dashboardId)
        {
            var authContent = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", embedSetting.PbiUserName),
                new KeyValuePair<string, string>("password", embedSetting.PbiPassword),
                new KeyValuePair<string, string>("client_id", embedSetting.ApplicationId),
                new KeyValuePair<string, string>("resource", embedSetting.ResourceUrl)
            });

            var accessToken = await authclient.PostAsync(embedSetting.TokenEndPoint, authContent).ContinueWith<string>((result) =>
            {
                AzureAdTokenResponseModel tokenRes =
                JsonConvert.DeserializeObject<AzureAdTokenResponseModel>(result.Result.Content.ReadAsStringAsync().Result);
                return tokenRes?.AccessToken;
            });

            // Get PowerBi report url and embed token
            powerBiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var embedUrl =
               await powerBiClient.GetAsync($"https://api.powerbi.com/v1.0/myorg/groups/{embedSetting.GroupId}/dashboards/{dashboardId}")
               .ContinueWith<string>((response) =>
               {

                   PowerBiDashboardModel report =
                   JsonConvert.DeserializeObject<PowerBiDashboardModel>(response.Result.Content.ReadAsStringAsync().Result);
                   return report?.EmbedUrl;
               });
            var tokenContent = new FormUrlEncodedContent(new[]
          {
                new KeyValuePair<string, string>("accessLevel", "view")
            });

            var embedToken =
               await powerBiClient.PostAsync($"https://api.powerbi.com/v1.0/myorg/groups/{embedSetting.GroupId}/dashboards/{dashboardId}/GenerateToken", tokenContent)
               .ContinueWith<string>((response) =>
               {

                   PowerBiEmbedTokenModel powerBiEmbedToken =
                   JsonConvert.DeserializeObject<PowerBiEmbedTokenModel>(response.Result.Content.ReadAsStringAsync().Result);
                   return powerBiEmbedToken?.Token;
               });
            // JSON Response
            EmbedContentModel data = new EmbedContentModel
            {
                EmbedToken = embedToken,
                EmbedUrl = embedUrl.Replace("https://app", "https://msit"),
                DashboadId = dashboardId
            };

            return new JsonResult(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dashboardId"></param>
        /// <returns></returns>
        [Route("/getTilesByDashboardId")]
        public async Task<JsonResult> GetTilesByDashboardId(string dashboardId)
        {
            var authContent = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", embedSetting.PbiUserName),
                new KeyValuePair<string, string>("password", embedSetting.PbiPassword),
                new KeyValuePair<string, string>("client_id", embedSetting.ApplicationId),
                new KeyValuePair<string, string>("resource", embedSetting.ResourceUrl)
            });

            var accessToken = await authclient.PostAsync(embedSetting.TokenEndPoint, authContent).ContinueWith<string>((result) =>
            {
                AzureAdTokenResponseModel tokenRes =
                JsonConvert.DeserializeObject<AzureAdTokenResponseModel>(result.Result.Content.ReadAsStringAsync().Result);
                return tokenRes?.AccessToken;
            });

            // Get PowerBi report url and embed token
            powerBiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var response = await powerBiClient.GetAsync($"https://api.powerbi.com/v1.0/myorg/groups/{embedSetting.GroupId}/dashboards/{dashboardId}/tiles");
            string responseData = response.Content.ReadAsStringAsync().Result;
            var responseObj = JsonConvert.DeserializeObject<PowerBITileModel>(responseData);
            List<PowerBITileModel> listOfReport = new List<PowerBITileModel>();
            listOfReport = responseObj.value;
            return Json(listOfReport);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileKey"></param>
        /// <param name="dashboardKey"></param>
        /// <returns></returns>
        [Route("/getTileByTileKey")]
        public async Task<JsonResult> GetTileByTileKey(string tileKey, string dashboardKey)
        {
            var authContent = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", embedSetting.PbiUserName),
                new KeyValuePair<string, string>("password", embedSetting.PbiPassword),
                new KeyValuePair<string, string>("client_id", embedSetting.ApplicationId),
                new KeyValuePair<string, string>("resource", embedSetting.ResourceUrl)
            });

            var accessToken = await authclient.PostAsync(embedSetting.TokenEndPoint, authContent).ContinueWith<string>((result) =>
            {
                AzureAdTokenResponseModel tokenRes =
                JsonConvert.DeserializeObject<AzureAdTokenResponseModel>(result.Result.Content.ReadAsStringAsync().Result);
                return tokenRes?.AccessToken;
            });

            // Get PowerBi report url and embed token
            powerBiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var embedUrl =
               await powerBiClient.GetAsync($"https://api.powerbi.com/v1.0/myorg/groups/{embedSetting.GroupId}/dashboards/{dashboardKey}/tiles/{tileKey}")
               .ContinueWith<string>((response) =>
               {

                   PowerBITileModel report =
                   JsonConvert.DeserializeObject<PowerBITileModel>(response.Result.Content.ReadAsStringAsync().Result);
                   return report?.EmbedUrl;
               });
            var tokenContent = new FormUrlEncodedContent(new[]
          {
                new KeyValuePair<string, string>("accessLevel", "view")
            });

            var embedToken =
               await powerBiClient.PostAsync($"https://api.powerbi.com/v1.0/myorg/groups/{embedSetting.GroupId}/dashboards/{dashboardKey}/tiles/{tileKey}/GenerateToken", tokenContent)
               .ContinueWith<string>((response) =>
               {

                   PowerBiEmbedTokenModel powerBiEmbedToken =
                   JsonConvert.DeserializeObject<PowerBiEmbedTokenModel>(response.Result.Content.ReadAsStringAsync().Result);
                   return powerBiEmbedToken?.Token;
               });
            // JSON Response
            EmbedContentModel data = new EmbedContentModel
            {
                EmbedToken = embedToken,
                EmbedUrl = embedUrl.Replace("https://app", "https://msit"),
                DashboadId = dashboardKey
            };

            return new JsonResult(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [Route("/getDatasetList")]
        public async Task<JsonResult> GetDatasetList()
        {
            var authContent = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", embedSetting.PbiUserName),
                new KeyValuePair<string, string>("password", embedSetting.PbiPassword),
                new KeyValuePair<string, string>("client_id", embedSetting.ApplicationId),
                new KeyValuePair<string, string>("resource", embedSetting.ResourceUrl)
            });

            var accessToken = await authclient.PostAsync(embedSetting.TokenEndPoint, authContent).ContinueWith<string>((result) =>
            {
                AzureAdTokenResponseModel tokenRes =
                JsonConvert.DeserializeObject<AzureAdTokenResponseModel>(result.Result.Content.ReadAsStringAsync().Result);
                return tokenRes?.AccessToken;
            });

            // Get PowerBi report url and embed token
            powerBiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            var response = await powerBiClient.GetAsync($"https://api.powerbi.com/v1.0/myorg/datasets");
            string responseData = response.Content.ReadAsStringAsync().Result;
            var responseObj = JsonConvert.DeserializeObject<PowerBIDatasetModel>(responseData);
            List<PowerBIDatasetModel> listOfReport = new List<PowerBIDatasetModel>();
            listOfReport = responseObj.value;
            return Json(listOfReport);
        }


       

            
        

    }
}