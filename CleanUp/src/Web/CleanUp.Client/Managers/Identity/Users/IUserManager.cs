using CleanUp.WebApi.Sdk.Models;
using CleanUp.WebApi.Sdk.Requests;
using CleanUp.WebApi.Sdk.Requests.User;
using CleanUp.Client.Models.Api;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Client.Managers.Identity.Users
{
    public interface IUserManager : IManager
    {
        Task<ApiResult<User>> GetAsync();

        Task<ApiResult<User>> RegisterUserAsync(RegisterUserRequest request);

        Task<ApiResult> ConfirmEmail(string userId, string code);

        Task<ApiResult> ForgotPasswordAsync(ForgotPasswordRequest request);

        Task<ApiResult> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ApiResult> ConfirmPrivacy();
        Task<ApiResult> UpdateDefaultAddress(UpdateDefaultAddressRequest request);
    }
}