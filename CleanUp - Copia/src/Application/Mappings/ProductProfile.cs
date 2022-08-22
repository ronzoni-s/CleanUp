using AutoMapper;
using ErbertPranzi.Application.Features.Products.Commands.AddEdit;
using ErbertPranzi.Domain.Entities.Catalog;

namespace ErbertPranzi.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddEditProductCommand, Product>().ReverseMap();
        }
    }
}