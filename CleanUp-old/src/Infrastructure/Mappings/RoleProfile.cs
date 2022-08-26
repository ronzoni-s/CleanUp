using AutoMapper;
using CleanUp.Infrastructure.Models.Identity;
using CleanUp.Application.Responses.Identity;

namespace CleanUp.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, BlazorHeroRole>().ReverseMap();
        }
    }
}