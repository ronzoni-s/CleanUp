using ErbertPranzi.Domain.Contracts;

namespace ErbertPranzi.Domain.Entities.Catalog
{
    public class Parameter : AuditableEntity<int>
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}