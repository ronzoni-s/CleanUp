using AutoMapper;
using CleanUp.Application.Features.Orders.Commands.AddEdit;
using CleanUp.Application.Features.Orders.Queries.GetAllPaged;
using CleanUp.Application.Features.Orders.Queries.GetById;
using CleanUp.Domain.Entities.Catalog;

namespace CleanUp.Application.Mappings
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