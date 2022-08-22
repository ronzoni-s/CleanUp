using ErbertPranzi.Domain.Contracts;
using System;

namespace ErbertPranzi.Domain.Entities.Catalog
{
    public class OrderProduct : AuditableEntity<int>
    {
        public int OrderId { get; set; }
        public int? MenuId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double FinalPrice { get; set; }
        public int Tax { get; set; }
        public double ProductWeight { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}