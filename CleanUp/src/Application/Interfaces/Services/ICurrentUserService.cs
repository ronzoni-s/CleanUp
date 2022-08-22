using CleanUp.Application.Interfaces.Common;

namespace CleanUp.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}