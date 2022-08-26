using AutoMapper;
using CleanUp.Infrastructure.Models.Identity;
using CleanUp.Application.Responses.Identity;

namespace CleanUp.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, BlazorHeroUser>().ReverseMap();
        }
    }
}