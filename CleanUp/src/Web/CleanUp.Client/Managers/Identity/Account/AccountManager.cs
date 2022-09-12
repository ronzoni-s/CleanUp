using CleanUp.WebApi.Sdk.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers.Identity.Account
{
    public class AccountManager : IManager
    {
        private readonly HttpClient _httpClient;

        public AccountManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<ApiResult> ChangePasswordAsync(ChangePasswordRequest model)
        //{
        //    var response = await _httpClient.PutAsJsonAsync(Routes.AccountEndpoints.ChangePassword, model);
        //    return await response.ToResult();
        //}

        //public async Task<ApiResult> UpdateProfileAsync(UpdateProfileRequest model)
        //{
        //    var response = await _httpClient.PutAsJsonAsync(Routes.AccountEndpoints.UpdateProfile, model);
        //    return await response.ToResult();
        //}
    }
}