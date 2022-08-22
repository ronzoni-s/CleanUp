using ErbertPranzi.Application.Features.Products.Queries.GetAllPaged;
using ErbertPranzi.Application.Models;
using System;
using System.Collections.Generic;

namespace ErbertPranzi.Application.Features.Orders.Queries.GetById
{
    public class GetOrderByIdResponse
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public int CustomerAddressId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public double WeightPerBag { get; set; }
        public double FinalPrice { get; set; }
        public double Weight { get; set; }
        public int BagsPerPolibox { get; set; }
        public int Bags { get; set; }
        public int? UsedBags { get; set; }
        public DateTime? CompletionDateTime { get; set; }
        public DateTime? CancellationDateTime { get; set; }

        public List<GetOrderProductResponse> OrderProducts { get; set; }
        public List<OrderPolibox> OrderPoliboxs { get; set; }
    }
}