using CleanUp.Client.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CleanUp.Client.Models.Api;
using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Requests;
using CleanUp.WebApi.Sdk.Requests.User;
using CleanUp.WebApi.Sdk.Endpoints;

namespace CleanUp.Client.Managers.Identity.Users
{
    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;

        public UserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<User>> GetAsync()
        {
            var response = await _httpClient.GetAsync(UserEndpoints.Get);
            return await response.ToResult<User>();
        }

        public async Task<ApiResult<User>> RegisterUserAsync(RegisterUserRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(UserEndpoints.Register, request);
            return await response.ToResult<User>();
        }

        public async Task<ApiResult> ConfirmEmail(string userId, string code)
        {
            var response = await _httpClient.GetAsync(UserEndpoints.ConfirmEmail(userId, code));
            return await response.ToResult();
        }


        public async Task<ApiResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(UserEndpoints.ForgotPassword, request);
            return await response.ToResult();
        }

        public async Task<ApiResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(UserEndpoints.ResetPassword, request);
            return await response.ToResult();
        }

        public async Task<ApiResult> ConfirmPrivacy()
        {
            var response = await _httpClient.PostAsJsonAsync(UserEndpoints.ConfirmPrivacy, new { });
            return await response.ToResult();
        }

        public async Task<ApiResult> UpdateDefaultAddress(UpdateDefaultAddressRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(UserEndpoints.UpdateDefaultAddress, request);
            return await response.ToResult();
        }
    }
}