using CleanUp.Client.Extensions;
using CleanUp.WebApi.Sdk.Endpoints;
using CleanUp.WebApi.Sdk.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers.Identity.Users
{
    public class UserManager : IManager
    {
        private readonly HttpClient _httpClient;

        public UserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<User>> GetAsync(string userId)
        {
            var response = await _httpClient.GetAsync(UserEndpoints.Get(userId));
            return await response.ToResult<User>();
        }

        public async Task<ApiResult<List<User>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(UserEndpoints.GetAll);
            return await response.ToResult<List<User>>();
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

        //public async Task<ApiResult> UpdateRolesAsync(UpdateUserRolesRequest request)
        //{
        //    var response = await _httpClient.PutAsJsonAsync(Routes.UserEndpoints.GetUserRoles(request.UserId), request);
        //    return await response.ToResult<UserRolesResponse>();
        //}

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