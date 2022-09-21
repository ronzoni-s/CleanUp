using CleanUp.Client.Extensions;
using CleanUp.WebApi.Sdk.Endpoints;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Models.Events;
using CleanUp.WebApi.Sdk.Models.User;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers
{
    public class EventManager : IManager
    {
        private readonly HttpClient _httpClient;

        public EventManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<Event>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync(EventEndpoints.Get(id));
            return await response.ToResult<Event>();
        }

        public async Task<ApiResult<List<Event>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(EventEndpoints.GetAll);
            return await response.ToResult<List<Event>>();
        }

        //public async Task<ApiResult> RegisterUserAsync(RegisterRequest request)
        //{
        //    var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.Register, request);
        //    return await response.ToResult();
        //}

        //public async Task<ApiResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        //{
        //    var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.ToggleUserStatus, request);
        //    return await response.ToResult();
        //}

        //public async Task<ApiResult<UserRolesResponse>> GetRolesAsync(string userId)
        //{
        //    var response = await _httpClient.GetAsync(Routes.UserEndpoints.GetUserRoles(userId));
        //    return await response.ToResult<UserRolesResponse>();
        //}

        public async Task<ApiResult> UploadAsync(MultipartFormDataContent content)
        {
            var response = await _httpClient.PostAsync(EventEndpoints.Upload, content);
            return await response.ToResult();
        }

        //public async Task<ApiResult> ForgotPasswordAsync(ForgotPasswordRequest model)
        //{
        //    var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.ForgotPassword, model);
        //    return await response.ToResult();
        //}

        //public async Task<ApiResult> ResetPasswordAsync(ResetPasswordRequest request)
        //{
        //    var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoints.ResetPassword, request);
        //    return await response.ToResult();
        //}

        //public async Task<string> ExportToExcelAsync(string searchString = "")
        //{
        //    var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
        //        ? Routes.UserEndpoints.Export
        //        : Routes.UserEndpoints.ExportFiltered(searchString));
        //    var data = await response.Content.ReadAsStringAsync();
        //    return data;
        //}
    }
}