using AutoMapper;
using ErbertPranzi.Infrastructure.Models.Audit;
using ErbertPranzi.Application.Responses.Audit;

namespace ErbertPranzi.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}