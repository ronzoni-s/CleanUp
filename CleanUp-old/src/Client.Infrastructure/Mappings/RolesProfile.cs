using AutoMapper;
using CleanUp.Application.Requests.Identity;
using CleanUp.Application.Responses.Identity;

namespace CleanUp.Client.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<PermissionResponse, PermissionRequest>().ReverseMap();
            CreateMap<RoleClaimResponse, RoleClaimRequest>().ReverseMap();
        }
    }
}