using CleanUp.Domain.Contracts;
using System.Collections.Generic;

namespace CleanUp.Domain.Entities.Catalog
{
    public class Customer : AuditableEntity<int>
    {
        public string Name { get; set; }

        public IList<CustomerAddress> CustomerAddresss { get; set; }
    }
}