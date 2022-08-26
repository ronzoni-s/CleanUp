using AutoMapper;
using CleanUp.Application.Features.Products.Commands.AddEdit;
using CleanUp.Domain.Entities.Catalog;

namespace CleanUp.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddEditProductCommand, Product>().ReverseMap();
        }
    }
}