using AutoMapper;

namespace CleanUp.Application.Mappings
{
    public class ExtendedAttributeProfile : Profile
    {
        public ExtendedAttributeProfile()
        {
            //CreateMap(typeof(AddEditExtendedAttributeCommand<,,,>), typeof(DocumentExtendedAttribute))
            //    .ForMember(nameof(DocumentExtendedAttribute.Entity), opt => opt.Ignore())
            //    .ForMember(nameof(DocumentExtendedAttribute.CreatedBy), opt => opt.Ignore())
            //    .ForMember(nameof(DocumentExtendedAttribute.Created), opt => opt.Ignore())
            //    .ForMember(nameof(DocumentExtendedAttribute.LastModifiedBy), opt => opt.Ignore())
            //    .ForMember(nameof(DocumentExtendedAttribute.LastModified), opt => opt.Ignore());

            //CreateMap(typeof(GetExtendedAttributeByIdResponse<,>), typeof(DocumentExtendedAttribute)).ReverseMap();
            //CreateMap(typeof(GetAllExtendedAttributesResponse<,>), typeof(DocumentExtendedAttribute)).ReverseMap();
            //CreateMap(typeof(GetAllExtendedAttributesByEntityIdResponse<,>), typeof(DocumentExtendedAttribute)).ReverseMap();
        }
    }
}