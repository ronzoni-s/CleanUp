using AutoMapper;
using CleanUp.Application.Features.Emails.Queries.GetById;
using CleanUp.Domain.Entities;

namespace CleanUp.Application.Mappings
{
    public class EmailProfile : Profile
    {
        public EmailProfile()
        {
            CreateMap<GetEmailConfigByIdResponse, EmailConfig>().ReverseMap();
        }
    }
}