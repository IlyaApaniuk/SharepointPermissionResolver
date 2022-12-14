using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharePointPermissionsResolver.Models;
using SharePointPermissionsResolver.Services.AuthWrapper;

namespace SharePointPermissionsResolver.Services.SharePointService
{
    public class SharePointService : ISharePointService
    {
        private IAuthWrapper authWrapper;

        public SharePointService(IAuthWrapper wrapper)
        {
            this.authWrapper = wrapper;
        }

        public async Task<ApiResponse> GetListItems(Request request)
        {
            try
            {
                var token = await this.authWrapper.GetToken(request.SpfxToken);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var siteId = await this.GetSiteId(token, request.RootPath, request.ServerRelativePath);
                var url = request.ApiUrl.Replace("{siteId}", siteId);

                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<GraphEntityWrapper>(content);
                var fields = json.Value.Select(data => data.Fields).ToList();
                var listItems = new { value = fields };

                return new ApiResponse { IsSuccessed = response.IsSuccessStatusCode, Content = JsonConvert.SerializeObject(listItems) };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> CreateListItem(Request request)
        {
            try
            {
                var token = await this.authWrapper.GetToken(request.SpfxToken);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var requestData = new StringContent(request.Data, Encoding.UTF8, "application/json");

                var siteId = await this.GetSiteId(token, request.RootPath, request.ServerRelativePath);
                var url = request.ApiUrl.Replace("{siteId}", siteId);

                var response = await httpClient.PostAsync(url, requestData);
                var content = await response.Content.ReadAsStringAsync();

                return new ApiResponse { IsSuccessed = response.IsSuccessStatusCode, Content = content };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateListItem(Request request)
        {
            try
            {
                var token = await this.authWrapper.GetToken(request.SpfxToken);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var requestData = new StringContent(request.Data, Encoding.UTF8, "application/json");

                var siteId = await this.GetSiteId(token, request.RootPath, request.ServerRelativePath);
                var url = request.ApiUrl.Replace("{siteId}", siteId);

                var response = await httpClient.PatchAsync(url, requestData);
                var content = await response.Content.ReadAsStringAsync();

                return new ApiResponse { IsSuccessed = response.IsSuccessStatusCode, Content = content };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> GetDriveItems(Request request)
        {
            try
            {
                var token = await this.authWrapper.GetToken(request.SpfxToken);
                var siteId = await this.GetSiteId(token, request.RootPath, request.ServerRelativePath);
                var driveId = await this.GetDriveId(token, siteId, request.DriveName);

                var url = request.ApiUrl.Replace("{siteId}", siteId).Replace("{driveId}", driveId);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                return new ApiResponse { IsSuccessed = response.IsSuccessStatusCode, Content = content };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> UploadDriveItem(Request request, IFormFile file)
        {
            try
            {
                var token = await this.authWrapper.GetToken(request.SpfxToken);
                var siteId = await this.GetSiteId(token, request.RootPath, request.ServerRelativePath);
                var driveId = await this.GetDriveId(token, siteId, request.DriveName);

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "audio/webm;codecs=opus");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var fileStreamContent = new StreamContent(file.OpenReadStream());

                var url = request.ApiUrl.Replace("{driveId}", driveId);

                var response = await httpClient.PutAsync(url, fileStreamContent);
                var content = await response.Content.ReadAsStringAsync();

                return new ApiResponse { IsSuccessed = response.IsSuccessStatusCode, Content = content };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse> PerformSearch(Request request)
        {
            try
            {
                var token = await this.authWrapper.GetToken(request.SpfxToken, request.RootPath, false);
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var response = await httpClient.GetAsync(request.ApiUrl);
                var content = await response.Content.ReadAsStringAsync();

                return new ApiResponse { IsSuccessed = response.IsSuccessStatusCode, Content = content };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<string> GetSiteId(string token, string rootPath, string serverRelativePath)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var response = await httpClient.GetAsync($"https://graph.microsoft.com/v1.0/sites/{rootPath}:{serverRelativePath}");

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<GraphEntity>(content);

                return json.Id.Split(",")[1];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<string> GetDriveId(string token, string siteId, string driveName)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                
                var response = await httpClient.GetAsync($"https://graph.microsoft.com/v1.0/sites/{siteId}/drives");

                var content = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<GraphEntityWrapper>(content);

                var drive = json.Value.Where(s => s.Name == driveName).FirstOrDefault();

                return drive.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

