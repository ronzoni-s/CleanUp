using ErbertPranzi.Domain.Contracts;
using System.Collections.Generic;

namespace ErbertPranzi.Domain.Entities.Catalog
{
    public class CustomerAddress : AuditableEntity<int>
    {
        public int CustomerId { get; set; }
        public string Address { get; set; }

        public Customer Customer { get; set; }
        public IList<Order> Orders { get; set; }
    }
}