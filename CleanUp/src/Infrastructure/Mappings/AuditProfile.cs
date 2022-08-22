using AutoMapper;
using CleanUp.Infrastructure.Models.Audit;
using CleanUp.Application.Responses.Audit;

namespace CleanUp.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}