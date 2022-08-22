using ErbertPranzi.Domain.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErbertPranzi.Domain.Entities.Catalog
{
    public class Product : AuditableEntity<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
        public int Tax { get; set; }
        public bool IsActive { get; set; }

        public IList<OrderProduct> OrderProducts { get; set; }
    }
}