using CleanUp.Domain.Contracts;
using System;

namespace CleanUp.Domain.Entities.Catalog
{
    public class OrderMenu : AuditableEntity<int>
    {
        public int OrderId { get; set; }
        public int MenuId { get; set; }
        public double Price { get; set; }

        public Order Order { get; set; }
    }
}