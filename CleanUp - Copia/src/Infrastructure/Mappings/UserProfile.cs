using AutoMapper;
using ErbertPranzi.Infrastructure.Models.Identity;
using ErbertPranzi.Application.Responses.Identity;

namespace ErbertPranzi.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, BlazorHeroUser>().ReverseMap();
        }
    }
}