using CleanUp.Application.Features.Products.Queries.GetAllPaged;
using System;
using System.Collections.Generic;

namespace CleanUp.Application.Features.Orders.Queries.GetAllPaged
{
    public class GetAllPagedOrdersResponse
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public int CustomerAddressId { get; set; }

        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string ContactName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public double FinalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public int? Bags { get; set; }
        public DateTime? CompletionDateTime { get; set; }
        public DateTime? CancellationDateTime { get; set; }

    }
}