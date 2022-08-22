using ErbertPranzi.Application.Interfaces.Common;
using ErbertPranzi.Application.Requests.Identity;
using ErbertPranzi.Shared.Wrapper;
using System.Threading.Tasks;

namespace ErbertPranzi.Application.Interfaces.Services.Account
{
    public interface IAccountService : IService
    {
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId);

        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
    }
}