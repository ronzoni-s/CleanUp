using ErbertPranzi.Domain.Contracts;
using System.Collections.Generic;

namespace ErbertPranzi.Domain.Entities.Catalog
{
    public class Customer : AuditableEntity<int>
    {
        public string Name { get; set; }

        public IList<CustomerAddress> CustomerAddresss { get; set; }
    }
}