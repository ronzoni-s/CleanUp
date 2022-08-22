using AutoMapper;
using ErbertPranzi.Application.Features.Orders.Commands.AddEdit;
using ErbertPranzi.Application.Features.Orders.Queries.GetAllPaged;
using ErbertPranzi.Application.Features.Orders.Queries.GetById;
using ErbertPranzi.Domain.Entities.Catalog;

namespace ErbertPranzi.Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddOrderCommand, Order>().ReverseMap();
            CreateMap<GetOrderByIdResponse, Order>().ReverseMap();
            CreateMap<GetAllPagedOrdersResponse, Order>().ReverseMap();
        }
    }
}