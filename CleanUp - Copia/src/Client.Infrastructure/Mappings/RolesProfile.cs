using AutoMapper;
using ErbertPranzi.Application.Requests.Identity;
using ErbertPranzi.Application.Responses.Identity;

namespace ErbertPranzi.Client.Infrastructure.Mappings
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