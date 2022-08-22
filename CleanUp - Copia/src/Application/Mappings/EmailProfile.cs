using AutoMapper;
using ErbertPranzi.Application.Features.Emails.Queries.GetById;
using ErbertPranzi.Domain.Entities;

namespace ErbertPranzi.Application.Mappings
{
    public class EmailProfile : Profile
    {
        public EmailProfile()
        {
            CreateMap<GetEmailConfigByIdResponse, EmailConfig>().ReverseMap();
        }
    }
}