using AutoMapper;
using ErbertPranzi.Infrastructure.Models.Identity;
using ErbertPranzi.Application.Responses.Identity;

namespace ErbertPranzi.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, BlazorHeroRole>().ReverseMap();
        }
    }
}