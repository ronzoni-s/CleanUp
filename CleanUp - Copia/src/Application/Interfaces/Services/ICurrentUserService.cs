using ErbertPranzi.Application.Interfaces.Common;

namespace ErbertPranzi.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}