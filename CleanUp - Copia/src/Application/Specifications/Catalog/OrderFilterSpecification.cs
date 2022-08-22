using ErbertPranzi.Application.Specifications.Base;
using ErbertPranzi.Domain.Entities.Catalog;

namespace ErbertPranzi.Application.Specifications.Catalog
{
    public class OrderFilterSpecification : HeroSpecification<Order>
    {
        public OrderFilterSpecification(string searchString, bool hideCompleted = false, bool hideVoided = false)
        {
            Includes.Add(a => a.CustomerAddress);
            Includes.Add(a => a.CustomerAddress.Customer);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.OrderNumber.ToString().Contains(searchString) 
                                    || (!string.IsNullOrEmpty(p.ContactName) && p.ContactName.Contains(searchString) )
                                    || (!string.IsNullOrEmpty(p.ContactPhoneNumber) && p.ContactPhoneNumber.Contains(searchString))
                                    || (p.Bags.ToString().Contains(searchString))
                                    || (!string.IsNullOrEmpty(p.CustomerAddress.Address) && p.CustomerAddress.Address.Contains(searchString))
                                    || (!string.IsNullOrEmpty(p.CustomerAddress.Customer.Name) && p.CustomerAddress.Customer.Name.Contains(searchString)))
                                    && (!hideCompleted || p.CompletionDateTime == null)
                                    && (!hideVoided || p.CancellationDateTime == null)
                                    ;
            }
            else
            {
                
                Criteria = p => (!hideCompleted || p.CompletionDateTime == null)
                                && (!hideVoided || p.CancellationDateTime == null)
                                && true;
            }
        }
    }
}