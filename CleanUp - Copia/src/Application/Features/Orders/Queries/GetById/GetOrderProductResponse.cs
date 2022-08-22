using ErbertPranzi.Application.Features.Products.Queries.GetAllPaged;
using System;
using System.Collections.Generic;

namespace ErbertPranzi.Application.Features.Orders.Queries.GetById
{
    public class GetOrderProductResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Tax { get; set; }
        public double Price { get; set; }
        public double FinalPrice { get; set; }
        public double Weight { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public int? MenuId { get; set; }
    }
}