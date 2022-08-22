using ErbertPranzi.Application.Specifications.Base;
using ErbertPranzi.Domain.Entities.Catalog;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ErbertPranzi.Application.Specifications.Catalog
{
    public class OrderProductFilterSpecification : HeroSpecification<OrderProduct>
    {
        public OrderProductFilterSpecification(string searchString, int? orderId = null)
        {
            Includes.Add(x => x.Product);
            Criteria = p => true;

            if (orderId.HasValue)
            {
                Criteria = p => p.OrderId == orderId;
            }


            if (!string.IsNullOrEmpty(searchString))
            {
                Expression<Func<OrderProduct, bool>> seachStringExpression = (p => p.Product.Code != null && (p.Product.Name.Contains(searchString) || p.Product.Code.Contains(searchString)));
                if (orderId.HasValue)
                {
                    Criteria = Expression.Lambda<Func<OrderProduct, bool>>(Expression.And(Criteria, seachStringExpression));
                }
                else
                {
                    Criteria = seachStringExpression;
                }
                
            }
        }
    }
}