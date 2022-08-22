using ErbertPranzi.Domain.Contracts;
using System;

namespace ErbertPranzi.Domain.Entities.Catalog
{
    public class OrderMenu : AuditableEntity<int>
    {
        public int OrderId { get; set; }
        public int MenuId { get; set; }
        public double Price { get; set; }

        public Order Order { get; set; }
    }
}